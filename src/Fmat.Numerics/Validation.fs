namespace Fmat.Numerics

module internal Validation =
    open System
    open Fmat.Numerics.MatrixUtil

    let validateMatrixSize (s : seq<int>) = 
        let n = s |> Seq.length
        if n < 2 then raise (new ArgumentException("Matrix must have at least 2 dimensions"))
        s |> Seq.iter (fun i -> if i < 0 then raise (new ArgumentException("Each matrix dimension must be nonnegative")))

    let validateLengthAndSizeMatch length size =
         let n = size |> Seq.fold (*) 1
         if n <> length then raise (new ArgumentException("Data length does not match matrix size"))

    let validate2D (s : int[]) = 
        let dims = s.Length
        if (dims <> 2) then raise (new RankException("Matrix must be 2 dimensional"))

    let validateSameLengths (s1 : int[]) (s2 : int[]) =
        let n1 = s1 |> Array.fold (*) 1
        let n2 = s2 |> Array.fold (*) 1
        if n1 <> n2 then raise (new ArgumentException("Matrix lengths (number of elements) must be equal"))

    let validateIndices (ind : int[]) (size : int[]) = 
       let n = ind.Length
       let m = size.Length
       if n <> m then raise (new ArgumentException("Number of subscripts incompatible with matrix size"))
       ind |> Array.iteri (fun i k -> if k < 0 || k >= size.[i] then raise (new IndexOutOfRangeException()))

    let validateSeqIndices (ind : seq<int>) (len : int) =
        ind |> Seq.iter (fun i -> if i < 0 || i >= len then raise (new IndexOutOfRangeException()))

    let validateSeqLength (s : seq<int>) (len : int) =
        let  n = s |> Seq.length
        if n <> len then raise (new ArgumentException("Length mismatch"))

    let validateIndexRangeSeq (ind : seq<int>[]) (size : int[]) =
        let n = ind.Length
        let m = size.Length
        if n <> m then raise (new ArgumentException("Index ranges incompatible with matrix size"))
        ind |> Array.iteri (fun dim x -> x |> Seq.iter (fun y -> if y < 0 || y >= size.[dim] then raise (new IndexOutOfRangeException())))

    let validateIndexRangeMatchesSize (ind : seq<int>[]) (size : int[]) =
        let n = ind.Length
        let m = size.Length
        if n <> m then raise (new ArgumentException("Index ranges incompatible with matrix size"))
        ind |> Array.iteri (fun dim x -> let len = x |> Seq.length
                                         if len <> size.[dim] then raise (new ArgumentException("Index ranges incompatible with matrix size")))

    let validateMatrixDataLengthsMatch (x : matrixData<'a>) (y : matrixData<'b>) =
        let n = getMatrixDataLength x
        let m = getMatrixDataLength y
        if n <> m then raise (new ArgumentException("Matrix lengths (number of elements) not compatible"))

    let validateSizesMatch (s1: int[]) (s2 : int[]) =
        let n = s1.Length
        let m = s2.Length
        let len1 = s1 |> Array.fold (*) 1
        let len2 = s2 |> Array.fold (*) 1
        if len1 <> len2 then raise (new ArgumentException("Matrix sizes do not match"))
        if len1 > 0 then
            if n <> m then raise (new ArgumentException("Matrix sizes do not match"))
            else s1 |> Array.iteri (fun dim s -> if s <> s2.[dim] then raise (new ArgumentException("Matrix sizes do not match")))

    let areSizesSame (s1: int[]) (s2 : int[]) =
        let n = s1.Length
        let m = s2.Length
        let len1 = s1 |> Array.fold (*) 1
        let len2 = s2 |> Array.fold (*) 1
        let res = ref true
        if len1 <> len2 then res := false
        if len1 > 0 then
            if n <> m then res := false
            else s1 |> Array.iteri (fun dim s -> if s <> s2.[dim] then res := false)
        !res

    let validateCanMultiply (size1 : int[]) (size2 : int[]) =
        validate2D size1
        validate2D size2
        if size1.[1] <> size2.[0] then raise (new ArgumentException("Matrix dimensions do not agree"))

    let validateVector (size : int[]) =
        let dims = size.Length
        if (dims <> 2) then raise (new ArgumentException("Matrix is not vector 1xN or Nx1"))
        let n = size.[0]
        let m = size.[1]
        if n <> 1 && m <> 1 && n <> 0 && m <> 0 then raise (new ArgumentException("Matrix is not vector 1xN or Nx1"))

    let validateCanConcat (sizes : seq<int[]>) (dim : int) =
        if dim < 0 then raise (new ArgumentException("Dimension must be nonnegative"))
        let s0 = sizes |> Seq.nth 0
        let dim0 = s0.Length
        sizes |> Seq.iter (fun s -> s |> Array.iteri (fun i d -> let k = if i < s0.Length then s0.[i] else 1 
                                                                 if i <> dim && d <> k then raise (new ArgumentException("Dimensions mismatch in matrix concatenation"))))

    let validateSquare2D (size : int[]) =
        validate2D size
        let n = size.[0]
        let m = size.[1]
        if n <> m then raise (new ArgumentException("Matrix is not square"))

    let validateLinSolveSize (sizeA : int[]) (sizeB : int[]) =
        validate2D sizeA
        validate2D sizeB
        let n = sizeA.[0]
        let m = sizeB.[0]
        if n <> m then raise (new ArgumentException("Matrix dimensions mismatch"))

    let validateDimAgainstSize (size: int[]) (dim : int) =
        if dim < 0 || dim >= size.Length then raise (new ArgumentException("Invalid dimension"))

    let validateIndex (i : int) (len : int) = 
        if i < 0 || i >= len then raise (new IndexOutOfRangeException())

