namespace Fmat.Numerics

open System

type IBoolMatrixOps<'T> =
    abstract member And : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Or : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member And : matrixData<'T> * 'T -> matrixData<'T>
    abstract member Or : matrixData<'T> * 'T -> matrixData<'T>
    abstract member Not : matrixData<'T> -> matrixData<'T>

