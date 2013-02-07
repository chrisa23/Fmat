namespace Fmat.Numerics

open System

type IMatrixOps<'T> =
    abstract member GenericMatrixOps : IGenericMatrixOps<'T> with get
    abstract member ComparableMatrixOps : IComparableMatrixOps<'T> with get
    abstract member BoolMatrixOps : IBoolMatrixOps<'T> with get
    abstract member NumericMatrixOps : INumericMatrixOps<'T> with get

