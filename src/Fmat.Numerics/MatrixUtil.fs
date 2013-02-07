namespace Fmat.Numerics

module MatrixUtil =
    open System
    open System.Collections.Generic

    let getLinSliceStartEnd (start : Option<int>) (finish : Option<int>) length =
        let s = defaultArg start 0
        let e = defaultArg finish (length - 1)
        if s < 0 || s >= length then raise (new IndexOutOfRangeException())
        if e < 0 || e >= length then raise (new IndexOutOfRangeException())
        if s > e then raise (new IndexOutOfRangeException())
        (s,e)

    let getSliceStartEnd (slice : (Option<int>*Option<int>)[]) (size : int[]) =
        if slice.Length <> size.Length then raise (new ArgumentException())
        let s = slice |> Array.mapi (fun i (x,y) -> (defaultArg x 0, defaultArg y (size.[i] - 1)))
        s |> Array.iteri (fun i (x,y) -> if x < 0 || x >= size.[i] || y < 0 || y >= size.[i] || x > y then raise (new IndexOutOfRangeException()))
        let sliceSize = s |> Array.map (fun (x,y) -> y - x + 1 )
        (s, sliceSize)

    let getMatrixDataLength matrixData =
        match matrixData with
            | Managed(arr) -> arr.Length

    let matrixDataToSeq m =
        match m with
            | Managed arr -> Seq.ofArray arr

    let funMatrixScalar f matrixData a = Managed(matrixData |> matrixDataToSeq |> Seq.map (fun x -> f x  a) |> Seq.toArray)

    let funScalarMatrix f a matrixData = Managed(matrixData |> matrixDataToSeq |> Seq.map (fun x -> f a  x) |> Seq.toArray)

    let funMatrixMatrix f matrixData1 matrixData2=
        let seq1 = matrixData1 |> matrixDataToSeq
        let seq2 = matrixData2 |> matrixDataToSeq
        Managed(seq2 |> Seq.zip seq1 |> Seq.map (fun (x,y) -> f x  y) |> Seq.toArray)

    let funMatrixMatrixMatrix f matrixData1 matrixData2 matrixData3 =
        let seq1 = matrixData1 |> matrixDataToSeq
        let seq2 = matrixData2 |> matrixDataToSeq
        let seq3 = matrixData3 |> matrixDataToSeq
        Managed(Seq.zip3 seq1 seq2 seq3 |> Seq.map (fun (x,y,z) -> f x  y z) |> Seq.toArray)

    let funMatrix (f : 'T -> 'S) matrixData= Managed(matrixData |> matrixDataToSeq |> Seq.map (fun x -> f x) |> Seq.toArray)

    let invalidOp0Arg () = raise (new InvalidOperationException())

    let invalidOp1Arg x = raise (new InvalidOperationException())

    let invalidOp2Arg x y = raise (new InvalidOperationException())

    let invalidOp3Arg x y z = raise (new InvalidOperationException())

    let invalidOp4Arg x y z u = raise (new InvalidOperationException())

    let invalidOp5Arg x y z u t = raise (new InvalidOperationException())

    let cartesian (x : seq<seq<'T>>) =
        let len = x |> Seq.fold (fun state item -> state * Seq.length(item)) 1
        let enumerators = x |> Seq.map (fun item -> item.GetEnumerator()) |> Seq.toArray
        let rec moveEnumerators i = 
            if (enumerators.[i].MoveNext()) then ()
            else
                enumerators.[i].Reset()
                enumerators.[i].MoveNext() |> ignore
                moveEnumerators (i+1)
        seq{
            enumerators |> Seq.iter (fun e -> e.Reset()
                                              e.MoveNext() |> ignore)
            for i in 0..len-1 do
               if (i = 0) then 
                   yield enumerators |> Seq.map (fun e -> e.Current) |> Seq.toArray
               else 
                   moveEnumerators 0
                   yield enumerators |> Seq.map (fun e -> e.Current) |> Seq.toArray
            }

    let getDimProd (size : int[]) =
        let dimProd = Array.create size.Length 1
        dimProd.[0] <- 1
        let rec calcProd i =
            if (i = size.Length) then ()
            else dimProd.[i] <- dimProd.[i - 1] * size.[i-1]
                 calcProd (i + 1)
        calcProd 1
        dimProd

    let sub2ind (dimProd : int[]) (subscripts : int[]) =
        subscripts |> Array.mapi (fun i x -> x * dimProd.[i]) |> Array.fold (+) 0

    let ind2sub (index : int) (size : int[]) =
        let len = size |> Array.fold (*) 1
        let n = size.Length
        let dimProd = getDimProd size
        let mutable ind = index
        let res = Array.create n 0
        for i in (n-1) .. -1 .. 1 do
            res.[i] <- ind / dimProd.[i]
            ind <- ind % dimProd.[i]
        res.[0] <- ind
        res

    let getMatrixDataItem (dimProd : int[]) (subscripts : int[]) matrixData =
        let linearIndex = if subscripts.Length = 1 then subscripts.[0] else sub2ind dimProd subscripts
        match matrixData with
          | Managed arr -> arr.[linearIndex]

    let cloneMatrixData matrixData =
        match matrixData with
          | Managed x -> Managed (x |> Array.copy)

    let setMatrixDataItem (dimProd : int[]) (subscripts : int[]) matrixData value =
        let linearIndex = sub2ind dimProd subscripts
        match matrixData with
          | Managed arr -> arr.[linearIndex] <- value

    let getSqueezedSize (size : seq<int>) =
        let size = size |> Seq.toArray
        let (|First|Second|Both|None|) (s : int[])=
            if (s.[0] = 1) then 
                if (s.[1] = 1) then Both
                else First
            else
                if (s.[1] = 1) then Second
                else None
        let tail = size |> Seq.skip 2 |> Seq.filter (fun x -> x <> 1)
        let tailLen = tail |> Seq.length
        if (tailLen = 0) then [|size.[0];size.[1]|]
        else
            match size with
            | First -> [Seq.singleton(size.[1]);tail] |> Seq.concat |> Seq.toArray
            | Second -> [Seq.singleton(size.[0]);tail] |> Seq.concat |> Seq.toArray
            | None -> let first2 = size |> Seq.take 2
                      [first2;tail] |> Seq.concat |> Seq.toArray
            | Both -> if (tailLen = 1) then [Seq.singleton(1);tail] |> Seq.concat |> Seq.toArray
                      else tail |> Seq.toArray


