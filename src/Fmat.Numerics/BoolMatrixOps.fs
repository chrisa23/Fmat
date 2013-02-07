namespace Fmat.Numerics

open System
open MatrixUtil

type BoolMatrixOpsRec<'T> =
    {
        And : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        Or : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        AndScalar : matrixData<'T> -> 'T -> matrixData<'T>
        OrScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Not : matrixData<'T> -> matrixData<'T>
    }

module BoolMatrixOps =

    let boolMatrixOpsRec =
        {
            And = funMatrixMatrix (&&)
            Or = funMatrixMatrix (||)
            AndScalar = funMatrixScalar (&&)
            OrScalar = funMatrixScalar (||)
            Not = funMatrix (not)
        }

    let noneBoolMatrixOpsRec<'T> =
        {
            And = (invalidOp2Arg : matrixData<'T> -> matrixData<'T> -> matrixData<'T>)
            Or = invalidOp2Arg
            AndScalar = invalidOp2Arg
            OrScalar = invalidOp2Arg
            Not = invalidOp1Arg
        }

    let createBoolMatrixOps (boolOpsRec : BoolMatrixOpsRec<'T>) =
        {new IBoolMatrixOps<'T> with
            member this.And(matrixData1, matrixData2) = boolOpsRec.And matrixData1 matrixData2
            member this.Or(matrixData1, matrixData2) = boolOpsRec.Or matrixData1 matrixData2
            member this.And(matrixData, a) = boolOpsRec.AndScalar matrixData a
            member this.Or(matrixData, a) = boolOpsRec.OrScalar matrixData a
            member this.Not(matrixData) = boolOpsRec.Not matrixData
        }


