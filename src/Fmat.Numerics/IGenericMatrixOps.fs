namespace Fmat.Numerics

open System

type IGenericMatrixOps<'T> = 
    abstract member CreateMatrixData : seq<'T> -> matrixData<'T> * int
    abstract member CreateMatrixData : seq<int> * 'T -> matrixData<'T>
    abstract member CreateMatrixData : 'T[,] -> matrixData<'T> * int[]
    abstract member CreateMatrixData : 'T -> matrixData<'T>
    abstract member CreateMatrixData : 'T[,,] -> matrixData<'T> * int[]
    abstract member CreateMatrixData : 'T[,,,] -> matrixData<'T> * int[]
    abstract member CreateMatrixData : matrixData<bool> -> matrixData<'T>
    abstract member CreateMatrixData : seq<seq<'T>> -> matrixData<'T> * int[]
    abstract member CreateMatrixData : int * (int -> 'T) -> matrixData<'T>
    abstract member CreateMatrixData : int[] * (int -> int -> 'T) -> matrixData<'T>
    abstract member CreateMatrixData : int[] * (int -> int -> int -> 'T) -> matrixData<'T>
    abstract member CreateMatrixData : int[] * (int -> int -> int -> int -> 'T) -> matrixData<'T>
    abstract member Transpose : matrixData<'T> * int[] -> matrixData<'T>
    abstract member CloneMatrixData : matrixData<'T> -> matrixData<'T>
    abstract member ConvertToArray : matrixData<'T> -> 'T[]
    abstract member ConvertToArray2D : matrixData<'T> * int -> 'T[,]
    abstract member ConvertToArray3D : matrixData<'T> * int * int -> 'T[,,]
    abstract member ConvertToArray4D : matrixData<'T> * int * int * int -> 'T[,,,]
    abstract member ConvertToColMajorSeq : matrixData<'T> -> seq<'T>
    abstract member TransposeInPlace : matrixData<'T> * int[] -> unit
    abstract member GetLinearSlice : matrixData<'T> * int * int -> matrixData<'T>
    abstract member SetLinearSlice : matrixData<'T> * int * int * 'T -> unit
    abstract member SetLinearSlice : matrixData<'T> * int * int * matrixData<'T> -> unit
    abstract member GetSlice : matrixData<'T> * int[] * (int * int)[] -> matrixData<'T>
    abstract member SetSlice : matrixData<'T> * int[] * (int * int)[] * matrixData<'T> -> unit
    abstract member SetSlice : matrixData<'T> * int[] * (int * int)[] * 'T -> unit
    abstract member Item : matrixData<'T> * int -> 'T with get, set
    abstract member Item : matrixData<'T> * int[] * int[] -> 'T with get, set
    abstract member Item : matrixData<'T> * seq<int> -> matrixData<'T> with get, set
    abstract member Item : int[] * matrixData<'T> * seq<int>[] -> matrixData<'T> * int[] with get
    abstract member Item : int[] * matrixData<'T> * seq<int>[] -> matrixData<'T> with set
    abstract member Item : matrixData<'T> * matrixData<bool> -> matrixData<'T> with get, set
    abstract member Item : matrixData<'T> * ('T -> bool) -> matrixData<'T> with get, set
    abstract member Set : matrixData<'T> * seq<int> * 'T -> unit
    abstract member Set : matrixData<'T> * int[] * seq<int>[] * 'T -> unit
    abstract member Set : matrixData<'T> * matrixData<bool> * 'T -> unit
    abstract member Set : matrixData<'T> * ('T -> bool) * 'T -> unit
    abstract member Diag : matrixData<'T> * int[] * int -> matrixData<'T>
    abstract member ToString : matrixData<'T> * int[] * string * int[] -> string
    abstract member Concat : seq<matrixData<'T> * int[]> * int -> matrixData<'T> * int[]
    abstract member SetDiagonal : matrixData<'T> * int -> matrixData<'T> * int[]
    abstract member TriLower : int * int * matrixData<'T> * int -> matrixData<'T>
    abstract member TriUpper : int * int * matrixData<'T> * int -> matrixData<'T>
    abstract member Repmat : int[] * matrixData<'T> * int[] -> matrixData<'T> * int[]
    abstract member ApplyFunInPlace : matrixData<'T> * ('T -> 'T) -> unit
    abstract member ApplyFun : matrixData<'T> * ('T -> 'T) -> matrixData<'T>
    abstract member ApplyFun : matrixData<'T> * matrixData<'T> * ('T -> 'T -> 'T) -> matrixData<'T>
    abstract member ApplyFun : matrixData<'T> * matrixData<'T> * matrixData<'T> * ('T -> 'T -> 'T -> 'T) -> matrixData<'T>




