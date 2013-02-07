namespace Fmat.Numerics

open System

type IComparableMatrixOps<'T> = 
    abstract member AreEqual : matrixData<'T> * matrixData<'T> -> bool
    abstract member AllEqual : matrixData<'T> * 'T -> bool
    abstract member AllNotEqual : matrixData<'T> * 'T -> bool
    abstract member AllLessThan : matrixData<'T> * 'T -> bool
    abstract member AllLessThanEqual : matrixData<'T> * 'T -> bool
    abstract member AllGreaterThan : matrixData<'T> * 'T -> bool
    abstract member AllGreaterThanEqual : matrixData<'T> * 'T -> bool
    abstract member LessThan : matrixData<'T> * 'T -> matrixData<bool>
    abstract member LessThanEqual : matrixData<'T> * 'T -> matrixData<bool>
    abstract member GreaterThan : matrixData<'T> * 'T -> matrixData<bool>
    abstract member GreaterThanEqual : matrixData<'T> * 'T -> matrixData<bool>
    abstract member EqualElementwise : matrixData<'T> * 'T -> matrixData<bool>
    abstract member NotEqualElementwise : matrixData<'T> * 'T -> matrixData<bool>
    abstract member LessThan : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member LessThanEqual : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member GreaterThan : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member GreaterThanEqual : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member EqualElementwise : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member NotEqualElementwise : matrixData<'T> * matrixData<'T> -> matrixData<bool>
    abstract member MinXY : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member MaxXY : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member MinXa : matrixData<'T> * 'T -> matrixData<'T>
    abstract member MaxXa : matrixData<'T> * 'T -> matrixData<'T>
    abstract member Min : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member Max : matrixData<'T> * int[] * int -> matrixData<'T> * int[]

