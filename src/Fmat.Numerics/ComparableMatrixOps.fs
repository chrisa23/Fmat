namespace Fmat.Numerics

open System
open MatrixUtil

type ComparableMatrixOpsRec<'T> =
    {
        AreEqual : matrixData<'T> -> matrixData<'T> -> bool
        AllEqual : matrixData<'T> -> 'T -> bool
        AllNotEqual : matrixData<'T> -> 'T -> bool
        AllLessThan : matrixData<'T> -> 'T -> bool
        AllLessThanEqual : matrixData<'T> -> 'T -> bool
        AllGreaterThan : matrixData<'T> -> 'T -> bool
        AllGreaterThanEqual : matrixData<'T> -> 'T -> bool
        LessThanScalar : matrixData<'T> -> 'T -> matrixData<bool>
        LessThanEqualScalar : matrixData<'T> -> 'T -> matrixData<bool>
        GreaterThanScalar : matrixData<'T> -> 'T -> matrixData<bool>
        GreaterThanEqualScalar : matrixData<'T> -> 'T -> matrixData<bool>
        EqualElementwiseScalar : matrixData<'T> -> 'T -> matrixData<bool>
        NotEqualElementwiseScalar : matrixData<'T> -> 'T -> matrixData<bool>
        LessThan : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        LessThanEqual : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        GreaterThan : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        GreaterThanEqual : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        EqualElementwise : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        NotEqualElementwise : matrixData<'T> -> matrixData<'T> -> matrixData<bool>
        MinXY : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        MaxXY : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        MinXa : matrixData<'T> -> 'T -> matrixData<'T>
        MaxXa : matrixData<'T> -> 'T -> matrixData<'T>
        Min : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        Max : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
    }

module ComparableMatrixOps =

    let allMeetCondition (cond : 'T -> 'T -> bool) matrixData a =
        let seq = matrixDataToSeq matrixData
        match seq |> Seq.tryFind (fun x -> not (cond x a)) with
            | Some x -> false
            | None -> true  

    let compareElementwiseScalar (cond : 'T -> 'T -> bool) matrixData a=
        Managed(matrixData |> matrixDataToSeq |> Seq.map (fun x -> cond x a) |> Seq.toArray)

    let compareElementwise (cond : 'T -> 'T -> bool) matrixData1 matrixData2=
        let seq1 = matrixData1 |> matrixDataToSeq
        let seq2 = matrixData2 |> matrixDataToSeq
        Managed(seq2 |> Seq.zip seq1 |> Seq.map (fun (x,y) -> cond x y) |> Seq.toArray)

    let areEqual (comp : 'T -> 'T -> bool) matrixData1 matrixData2 =
        let seq1 = matrixDataToSeq matrixData1
        let seq2 = matrixDataToSeq matrixData2
        match seq2 |> Seq.zip seq1 |> Seq.tryFind (fun (x,y) -> not (comp x y)) with
            | Some x -> false
            | None -> true

    let foldNdim (comp : 'T -> 'T -> 'T) (matrixData : matrixData<'T>) (size : int[]) (dim : int) : matrixData<'T>*int[] =
        let len = size |> Array.fold (*) 1
        let K = size |> Array.toSeq |> Seq.take dim |> Seq.fold (*) 1
        let N = size.[dim]
        let M = len / (K * N)
        let res = Managed (Array.zeroCreate<'T> (K*M))
        let dimProd = getDimProd [|K;M|]
        let resSize = size |> Array.mapi (fun i n -> if i = dim then 1 else n) 
        match matrixData with
          | Managed arr ->
                for i in 0..M-1 do
                    for j in 0..K-1 do
                        let sample = [|0..N-1|] |> Array.map (fun k -> arr.[i*K*N+k*K+j])
                        let init = sample.[0]
                        let v = sample |> Array.fold comp init
                        setMatrixDataItem dimProd [|j;i|] res v
        (res, resSize)

    let noneComparableMatrixOpsRec<'T> =
        {
            AreEqual = (invalidOp2Arg  : matrixData<'T> -> matrixData<'T> -> bool)
            AllEqual = invalidOp2Arg
            AllNotEqual = invalidOp2Arg
            AllLessThan = invalidOp2Arg
            AllLessThanEqual = invalidOp2Arg
            AllGreaterThan = invalidOp2Arg
            AllGreaterThanEqual = invalidOp2Arg
            LessThanScalar = invalidOp2Arg
            LessThanEqualScalar = invalidOp2Arg
            GreaterThanScalar = invalidOp2Arg
            GreaterThanEqualScalar = invalidOp2Arg
            EqualElementwiseScalar = invalidOp2Arg
            NotEqualElementwiseScalar = invalidOp2Arg
            LessThan = invalidOp2Arg
            LessThanEqual = invalidOp2Arg
            GreaterThan = invalidOp2Arg
            GreaterThanEqual = invalidOp2Arg
            EqualElementwise = invalidOp2Arg
            NotEqualElementwise = invalidOp2Arg
            MinXY = invalidOp2Arg
            MaxXY = invalidOp2Arg
            MinXa = invalidOp2Arg
            MaxXa = invalidOp2Arg
            Min = invalidOp2Arg
            Max = invalidOp2Arg
        }

    let inline compMatrixOpsRec<'T when 'T : comparison> = 
        {
            AreEqual = (areEqual (=) : matrixData<'T> -> matrixData<'T> -> bool)
            AllEqual = allMeetCondition (=)
            AllNotEqual = allMeetCondition (<>)
            AllLessThan = allMeetCondition (<)
            AllLessThanEqual = allMeetCondition (<=)
            AllGreaterThan = allMeetCondition (>)
            AllGreaterThanEqual = allMeetCondition (>=)
            LessThanScalar = compareElementwiseScalar (<)
            LessThanEqualScalar = compareElementwiseScalar (<=)
            GreaterThanScalar = compareElementwiseScalar (>)
            GreaterThanEqualScalar = compareElementwiseScalar (>=)
            EqualElementwiseScalar = compareElementwiseScalar (=)
            NotEqualElementwiseScalar = compareElementwiseScalar (<>)
            LessThan = compareElementwise (<)
            LessThanEqual = compareElementwise (<=)
            GreaterThan = compareElementwise (>)
            GreaterThanEqual = compareElementwise (>=)
            EqualElementwise = compareElementwise (=)
            NotEqualElementwise = compareElementwise (<>)
            MinXY = funMatrixMatrix min
            MaxXY = funMatrixMatrix max
            MinXa = funMatrixScalar min
            MaxXa = funMatrixScalar max
            Min = foldNdim min
            Max = foldNdim max
        }
        
    let createComparableMatrixOps (compOpsRec : ComparableMatrixOpsRec<'T>) : IComparableMatrixOps<'T> = 
        {new IComparableMatrixOps<'T> with
             member this.AreEqual(matrixData1, matrixData2) = compOpsRec.AreEqual matrixData1 matrixData2
             member this.AllEqual(matrixData, scalar) = compOpsRec.AllEqual matrixData scalar
             member this.AllNotEqual(matrixData, scalar) = compOpsRec.AllNotEqual matrixData scalar
             member this.AllLessThan(matrixData, scalar) = compOpsRec.AllLessThan matrixData scalar
             member this.AllLessThanEqual(matrixData, scalar) = compOpsRec.AllLessThanEqual matrixData scalar
             member this.AllGreaterThan(matrixData, scalar) = compOpsRec.AllGreaterThan matrixData scalar
             member this.AllGreaterThanEqual(matrixData, scalar) = compOpsRec.AllGreaterThanEqual matrixData scalar
             member this.LessThan(matrixData, scalar) = compOpsRec.LessThanScalar matrixData scalar
             member this.LessThanEqual(matrixData, scalar) = compOpsRec.LessThanEqualScalar matrixData scalar
             member this.GreaterThan(matrixData, scalar) = compOpsRec.GreaterThanScalar matrixData scalar
             member this.GreaterThanEqual(matrixData, scalar) = compOpsRec.GreaterThanEqualScalar matrixData scalar
             member this.EqualElementwise(matrixData, scalar) = compOpsRec.EqualElementwiseScalar matrixData scalar
             member this.NotEqualElementwise(matrixData, scalar) = compOpsRec.NotEqualElementwiseScalar matrixData scalar
             member this.LessThan(matrixData1, matrixData2) = compOpsRec.LessThan matrixData1 matrixData2
             member this.LessThanEqual(matrixData1, matrixData2) = compOpsRec.LessThanEqual matrixData1 matrixData2
             member this.GreaterThan(matrixData1, matrixData2) = compOpsRec.GreaterThan matrixData1 matrixData2
             member this.GreaterThanEqual(matrixData1, matrixData2) = compOpsRec.GreaterThanEqual matrixData1 matrixData2
             member this.EqualElementwise(matrixData1, matrixData2) = compOpsRec.EqualElementwise matrixData1 matrixData2
             member this.NotEqualElementwise(matrixData1, matrixData2) = compOpsRec.NotEqualElementwise matrixData1 matrixData2
             member this.MinXY(matrixData1, matrixData2) = compOpsRec.MinXY matrixData1 matrixData2
             member this.MaxXY(matrixData1, matrixData2) = compOpsRec.MaxXY matrixData1 matrixData2
             member this.MinXa(matrixData, a) = compOpsRec.MinXa matrixData a
             member this.MaxXa(matrixData, a) = compOpsRec.MaxXa matrixData a 
             member this.Min(matrixData, size, dim) = compOpsRec.Min matrixData size dim
             member this.Max(matrixData, size, dim) = compOpsRec.Max matrixData size dim
        } 
