namespace Fmat.Numerics

open System
open MatrixUtil
open Validation

///<summary>Boolean dense matrix object returned from elementwise matrix comparison. For internal use only.
///</summary>
///<remarks>Use BoolMatrix for bool matrix operations.
///</remarks>
type __BoolMatrix internal (size : seq<int>, data : matrixData<bool>) =
    let _data = data
    let _size = size
    member this.Data = _data
    member this.Size = _size

///<summary>Multidimensional generic matrix.
///</summary>
///<param name="size">Length of each dimension. There must be at least 2 dimensions.</param>
///<param name="data">Matrix data in column major order.</param>
///<typeparam name="T">Type of matrix elements</typeparam>
///<typeparam name="S">Type of matrix operations interface</typeparam>
///<remarks>Must have at least 2 dimensions. Scalar values are matrices 1x1, vectors are 1xN or Nx1
///</remarks>
type Matrix<'T, 'S when 'S: (new : unit -> 'S) and 'S:>IMatrixOps<'T>> (size : seq<int>, data : matrixData<'T>) =
    let mutable _size = size |> Seq.toArray
    let _data = data

    static let _matrixOps = new 'S()
    static let _genericOps = _matrixOps.GenericMatrixOps
    static let _compOps = _matrixOps.ComparableMatrixOps
    static let _boolOps = _matrixOps.BoolMatrixOps
    static let _numericOps = _matrixOps.NumericMatrixOps

    static let _empty = new Matrix<'T, 'S>([], Managed([||]))

    static let mutable _maxDisplaySize = [|10|]

    interface IFormattable with
        member this.ToString(format, provider) = ""

    ///<param name="size">Length of each dimension. There must be at least 2 dimensions.</param>
    ///<param name="data">Matrix data in column major order</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3;4], [1.0..24.0])
    ///</code>
    ///</example>
    new(size: seq<int>, data : seq<'T>) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="size">Length of each dimension</param>
    ///<param name="init">Scalar value to fill matrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3;4], 2.0)
    ///</code>
    ///</example>
    new(size : seq<int>, init : 'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">2D .NET array</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Array2D.create 2 3 1.0
    ///let y = new Matrix(x)
    ///</code>
    ///</example>
    new(data : 'T[,]) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">3D .NET array</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Array3D.create 2 3 2 1.0
    ///let y = Matrix(x)
    ///</code>
    ///</example>
    new(data : 'T[,,]) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">4D .NET array</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Array4D.create 2 3 2 2 1.0
    ///let y = Matrix(x)
    ///</code>
    ///</example>
    new(data : 'T[,,,]) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">Sequence of values</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = [1.0..3.0]
    ///let y = Matrix(x)  // returns matrix 1x3
    ///</code>
    ///</example>
    new(data : seq<'T>) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">Sequence of values in 2D matrix by rows</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = [ [1.0;2.0;3.0]
    ///          [2.0;3.0]
    ///        ]
    ///let y = Matrix(x)  // returns matrix 2x3
    ///</code>
    ///</example>
    ///<remarks>If rows have different lengths, missing values are set to generic zero
    ///</remarks>
    new (data : seq<'T list>) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">Sequence of values in 2D matrix by rows</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = [ {1.0..3.0}
    ///          {2.0..3.0}
    ///        ]
    ///let y = Matrix(x)  // returns matrix 2x3
    ///</code>
    ///</example>
    ///<remarks>If rows have different lengths, missing values are set to generic zero
    ///</remarks>
    new (data : seq<seq<'T>>) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="data">Sequence of values in 2D matrix by rows</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = [ [|1.0..3.0|]
    ///          [|2.0..3.0|]
    ///        ]
    ///let y = Matrix(x)  // returns matrix 2x3
    ///</code>
    ///</example>
    ///<remarks>If rows have different lengths, missing values are set to generic zero
    ///</remarks>
    new (data : seq<'T[]>) = new Matrix<'T, 'S>([], Managed([||]))
        
    ///<param name="data">Scalar value</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix(2.0)
    ///</code>
    ///</example>
    new (data:'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="boolMatrix">Boolean matrix object as returned from elementwise comparison</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x1 = Matrix([2;2], 3.0)
    ///let x2 = Matrix([2;2], 4.0)
    ///let y = Matrix(x1 .&lt; x2)
    ///</code>
    ///</example>
    ///<remarks>True is converted to generic 1, False to generic zero. Matrix comparison operators, e.g. ".&lt;" return boolean matrix object __BoolMatrix.
    ///</remarks>
    new (boolMatrix : __BoolMatrix) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="size">Length of each dimension</param>
    ///<param name="initializer">Function generating a value for each column major order index i.</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3;4], (fun i -&gt; float(i) + 2.0))
    ///</code>
    ///</example>
    new (size : seq<int>, initializer : int -> 'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="size">Length of each dimension. Must have 2 dimensions.</param>
    ///<param name="initializer">Function generating a value for each row and column index</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3], (fun i j -&gt; float(i) + float(j)))
    ///</code>
    ///</example>
    new (size : seq<int>, initializer : int -> int -> 'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="size">Length of each dimension. Must have 3 dimensions.</param>
    ///<param name="initializer">Function generating a value for each row, column and page index</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3;4], (fun i j k -&gt; float(i) + float(j) + float(k)))
    ///</code>
    ///</example>
    new (size : seq<int>, initializer : int -> int -> int -> 'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<param name="size">Length of each dimension. Must have 4 dimensions.</param>
    ///<param name="initializer">Function generating a value for dim0..dim3 index</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;3;4], (fun i j k l -&gt; float(i) + float(j) + float(k) + float(l)))
    ///</code>
    ///</example>
    new (size : seq<int>, initializer : int -> int -> int -> int -> 'T) = new Matrix<'T, 'S>([], Managed([||]))

    ///<summary>Returns matrix data in column major order
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([2;2], [1.0..4.0])
    ///let data = x.Data
    ///</code>
    ///</example>
    member this.Data with get() = _data

    ///<summary>Gets number of dimensions
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let dims = x.NDims //returns 3
    ///</code>
    ///</example>
    member this.NDims with get() = _size.Length

    ///<summary>Gets total number of elements/cells in matrix
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let len = x.Length //returns 24
    ///</code>
    ///</example>
    member this.Length with get() = _size |> Array.fold (*) 1

    ///<summary>Gets size of the matrix (each dimension length)
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let size = x.Size // returns [|2;3;4|]
    ///</code>
    ///</example>
    member this.Size with get() = _size |> Array.copy 

    ///<summary>Checks if matrix is 1xn or nx1
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [1;3]
    ///let x.IsVector // returns true
    ///</code>
    ///</example>
    member this.IsVector = true

    ///<summary>Checks if matrix is scalar 1x1
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [1;1]
    ///let x.IsScalar // returns true
    ///</code>
    ///</example>
    member this.IsScalar = true

    ///<summary>Get instance of matrix with zero elements
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix.Empty
    ///let y = x.Length // returns 0
    ///let y = x.Size // returns [|0;0|]
    ///</code>
    ///</example> 
    static member Empty = _empty

    ///<summary>Gets or sets format used to display matrix elements when ToString() is called. 
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///Matrix.DisplayFormat &lt;- "G5" // show 5 digits
    ///</code>
    ///</example>
    static member DisplayFormat
        with get() = ""
        and set(value : string) = ()

    ///<summary>Sets display format for given matrix type to Gn where n equals display digits 
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///Matrix.DisplayDigits &lt;- 5 // show 5 digits
    ///</code>
    ///</example>
    static member DisplayDigits
        with private get() = 0
        and set(digits : int) = ()

    ///<summary>Gets or sets maximum number of elements in each dimension to display when calling Matrix.ToString()
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.MatrixFunctions
    ///let x = ones [10;10]
    ///Matrix.MaxDisplaySize &lt;- [|2;3;4|] // show first 2 rows, 3 columns and 4 pages
    ///let s = x.ToString() 
    ///</code>
    ///</example>
    ///<remarks>Default value is 10 in each dimension, [|2;3;4|] is equivalent to [|2;3;4;4;4;4;4...|]
    ///</remarks>
    static member MaxDisplaySize 
        with get() = _maxDisplaySize
        and set(value) = _maxDisplaySize <- value

    ///<summary>Returns transposed matrix as new instance. Original instance is not affected. 
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.T // new instance matrix [3;2]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    member this.T = new Matrix<'T, 'S>([], Managed([||]))

    ///<summary>Reshapes current instance (in place operation). Dimensions can change but number of elements (length) must stay the same
    ///</summary>
    ///<param name="size">New dimensions</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///x.Reshape([6;4])
    ///let size = x.Size // returns [6;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when reshaped length does not equal original length.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when new dimensions are invalid, e.g. negative.</exception>
    member this.Reshape(size : seq<int>) = ()

    ///<summary>Transposes matrix in place
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///x.Transpose()
    ///let size = x.Size // returns [3;2]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    member this.Transpose() = ()

    ///<summary>Gets slice of matrix using linear indexing
    ///</summary>
    ///<param name="start">Start linear subscript. Zero if not specified</param>
    ///<param name="finish">End linear subscript. (Length-1) if not specified</param>
    ///<returns>Slice of matrix
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.[0..2] // returns first 3 elements
    ///let y = x.[3..] // returns last 3 elements
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or finish out of range.</exception>
    member this.GetSlice(start : option<int>, finish : option<int>) = Matrix<'T, 'S>.Empty
        
    ///<summary>Sets slice of matrix using linear indexing
    ///</summary>
    ///<param name="start">Start linear subscript. Zero if not specified</param>
    ///<param name="finish">End linear subscript. (Length-1) if not specified</param>
    ///<param name="value">Vector of new values</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///x.[0..2] &lt;- !![1;2;3] // sets first 3 elements to 1 2 3
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or finish out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when length of value not compatible with start and finish indices.</exception>
    member this.SetSlice(start : option<int>, finish : option<int>, value : Matrix<'T, 'S>) = ()

    ///<summary>Sets slice of matrix using linear indexing
    ///</summary>
    ///<param name="start">Start linear subscript. Zero if not specified</param>
    ///<param name="finish">End linear subscript. (Length-1) if not specified</param>
    ///<param name="value">New value</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///x.[0..2] &lt;- 1.0 // sets first 3 elements to 1
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or finish out of range.</exception>
    member this.SetSlice(start : option<int>, finish : option<int>, value : 'T) = ()

    ///<summary>Gets slice of 2D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<returns>Slice of matrix
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = x.[1..2, 2..3]  
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    member this.GetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Sets slice of 2D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="value">Matrix of new values</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6] : Matrix
    ///x.[1..2, 2..3] &lt;- ones [2;2] 
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value has wrong shape.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix or value not 2D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, value : Matrix<'T, 'S>) = ()

    ///<summary>Sets slice of 2D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="value">New value</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6] : Matrix
    ///x.[1..2, 2..3] &lt;- 1.0
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, value : 'T) = ()

    ///<summary>Gets slice of 3D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<returns>Slice of matrix
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4;5] : Matrix
    ///let y = x.[1..2, 2..3, 2..4]  
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 3D.</exception>
    member this.GetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Sets slice of 3D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<param name="value">Matrix of new values</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6;5] : Matrix
    ///x.[1..2, 2..3, 2..4] &lt;- ones [2;2;3] 
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value has wrong shape.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix or value not 3D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>, value : Matrix<'T, 'S>) = ()

    ///<summary>Sets slice of 3D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<param name="value">New value</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6;5] : Matrix
    ///x.[1..2, 2..3, 2..4] &lt;- 1.0
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 3D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>, value : 'T) = ()

    ///<summary>Gets slice of 4D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<param name="start3">Start subscript in 4th dimension. Zero if not specified</param>
    ///<param name="end3">End subscript in 4th dimension. (Length-1) if not specified</param>
    ///<returns>Slice of matrix
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4;5;6] : Matrix
    ///let y = x.[1..2, 2..3, 2..4, 2..5]  
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 4D.</exception>
    member this.GetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>, start3 : option<int>, end3 : option<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Sets slice of 4D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<param name="start3">Start subscript in 4th dimension. Zero if not specified</param>
    ///<param name="end3">End subscript in 4th dimension. (Length-1) if not specified</param>
    ///<param name="value">Matrix of new values</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6;5;4] : Matrix
    ///x.[1..2, 2..3, 2..4, 0..2] &lt;- ones [2;2;3;3] 
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value has wrong shape.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix or value not 4D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>, start3 : option<int>, end3 : option<int>, value : Matrix<'T, 'S>) = ()

    ///<summary>Sets slice of 4D matrix
    ///</summary>
    ///<param name="start0">Start row. Zero if not specified</param>
    ///<param name="end0">End row. (Length-1) if not specified</param>
    ///<param name="start1">Start column. Zero if not specified</param>
    ///<param name="end1">End column. (Length-1) if not specified</param>
    ///<param name="start2">Start page. Zero if not specified</param>
    ///<param name="end2">End page. (Length-1) if not specified</param>
    ///<param name="start3">Start subscript in 4th dimension. Zero if not specified</param>
    ///<param name="end3">End subscript in 4th dimension. (Length-1) if not specified</param>
    ///<param name="value">New value</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;6;5;4] : Matrix
    ///x.[1..2, 2..3, 2..4, 0..2] &lt;- 1.0
    ///</code>
    ///</example>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when start or end indices out of range.</exception>
    ///<exception cref="T:System.RankException">Thrown when matrix not 4D.</exception>
    member this.SetSlice(start0 : option<int>, end0 : option<int>, start1 : option<int>, end1 : option<int>, start2 : option<int>, end2 : option<int>, start3 : option<int>, end3 : option<int>, value : 'T) = ()

    ///<summary>Gets or sets matrix element
    ///</summary>
    ///<param name="i">Linear index in column major order.</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[1] // returns second element in matrix using column major order
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with get(i : int) = Unchecked.defaultof<'T>
        and set (i : int) (value : 'T) = ()

    ///<summary>Gets or sets matrix element
    ///</summary>
    ///<param name="indices">Array of subscripts, each for one matrix dimension</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.[ [|0; 1|] ] // returns element in first row and second column
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of indices.</exception>
    member this.Item
        with get(indices : int[]) = Unchecked.defaultof<'T>
        and set (indices : int[]) (value : 'T) = ()

    ///<summary>Gets or sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.[1, 1]
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with get(i : int, j : int) = Unchecked.defaultof<'T>

    ///<summary>Gets or sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<param name="k">Third subscript: page</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3] : Matrix
    ///let y = x.[1, 1, 2]
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with get(i: int, j : int, k : int) = Unchecked.defaultof<'T>

    ///<summary>Gets or sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<param name="k">Third subscript: page</param>
    ///<param name="k">4th dimension subscript</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3;4] : Matrix
    ///let y = x.[1, 1, 2, 3]
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with get(i : int, j : int, k : int, l : int) = Unchecked.defaultof<'T>

    ///<summary>Sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<param name="value">Value to set</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///x.[0, 1] &lt;- 0.0// sets element in first row and second column to zero
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with set (i : int, j : int) (value : 'T) = ()

    ///<summary>Sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<param name="k">Third subscript: page</param>
    ///<param name="value">Value to set</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3] : Matrix
    ///x.[0, 1, 2] &lt;- 0.0// sets element in first row second column and third page to zero
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with set (i : int, j : int, k : int) (value : 'T) = ()

    ///<summary>Sets matrix element
    ///</summary>
    ///<param name="i">First subscript: row</param>
    ///<param name="j">Second subscript: column</param>
    ///<param name="k">Third subscript: page</param>
    ///<param name="l">4th subscript</param>
    ///<param name="value">Value to set</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3;4] : Matrix
    ///x.[0, 1, 2, 3] &lt;- 0.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Item
        with set (i : int, j : int, k : int, l : int) (value : 'T) = ()

    ///<summary>Gets or sets submatrix using linear indexing in column major order
    ///</summary>
    ///<param name="indices">Sequence of subscripts</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3;4] : Matrix
    ///let y = x.[{0L..2..4}] // gets 1st, 3rd and 5th element as row vector
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when non matching length of value matrix.</exception>
    member this.Item
        with get(indices : seq<int>) = Matrix<'T, 'S>.Empty
        and set (indices : seq<int>) (value : Matrix<'T, 'S>) = ()

    ///<summary>Sets submatrix using linear indexing in column major order
    ///</summary>
    ///<param name="indices">Sequence of subscripts</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;3;4] : Matrix
    ///x.[{0..2..4}] &lt;- 3.0 // sets 1st, 3rd and 5th element to 3.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    member this.Set (indices : seq<int>, value : 'T) = ()

    ///<summary>Gets or sets submatrix
    ///</summary>
    ///<param name="indexSeqs">ParamArray of subscript sequences</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///let y = x.[[0;2],[1;3]] // gets elements in rows 0 and 2 and columns 1 and 3 
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value does not match submatrix shape.</exception>
    member this.Item
        with get([<ParamArray>] indexSeqs: seq<int>[]) = Matrix<'T, 'S>.Empty
        and set ([<ParamArray>] indexSeqs: seq<int>[]) (value : Matrix<'T, 'S>) = ()

    ///<summary>Sets submatrix to scalar value
    ///</summary>
    ///<param name="indexSeqs">Seq of subscript sequences. If only 1 sequence then linear indexing used</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///x.Set([ {0..2};{1..3} ] , 3.0) 
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (indexSeqs: seq<seq<int>>, value : 'T) = ()

    ///<summary>Sets submatrix to scalar value
    ///</summary>
    ///<param name="indexSeqs">Seq of subscript arrays. If only 1 sequence then linear indexing used</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///x.Set([ [|0..2|];[|1..3|] ] , 3.0) 
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (indexSeqs: seq<int[]>, value : 'T) = ()

    ///<summary>Sets submatrix to scalar value
    ///</summary>
    ///<param name="indexSeqs">Seq of subscript lists. If only 1 sequence then linear indexing used</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///x.Set([ [0..2];[1..3] ] , 3.0) 
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (indexSeqs: seq<int list>, value : 'T) = ()

    ///<summary>Gets or sets submatrix
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///x.[[0;2], [1;3]] &lt;- ones [2;2] // sets elements in rows 0 and 2 and columns 1 and 3 to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value does not match submatrix shape.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>) = Matrix<'T, 'S>.Empty
        and set (s0 : seq<int>, s1 : seq<int>) (value : Matrix<'T, 'S>) = ()

    ///<summary>Sets submatrix to scalar
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4] : Matrix
    ///x.Set([0;2], [1;3], 1.0) // sets elements in rows 0 and 2 and columns 1 and 3 to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (s0 : seq<int>, s1 : seq<int>, value : 'T) = ()

    ///<summary>Gets or sets submatrix
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="s2">Page subscript sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4;4] : Matrix
    ///x.[[0;2], [1;3], [0;1]] &lt;- ones [2;2;2] // sets elements in rows 0 and 2 and columns 1 and 3 and page 0 and 1 to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value does not match submatrix shape.</exception>
    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>, s2 : seq<int>) = Matrix<'T, 'S>.Empty
        and set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>) (value : Matrix<'T, 'S>) = ()

    ///<summary>Sets submatrix
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="s2">Page subscript sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4;4] 
    ///x.Set([0;2], [1;3], [0;1], 1.0) // sets elements in rows 0 and 2 and columns 1 and 3 and page 0 and 1 to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, value : 'T) = ()

    ///<summary>Gets or sets submatrix
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="s2">Page subscript sequence</param>
    ///<param name="s3">4th dimension sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4;4;4] 
    ///x.[[0;2], [1;3], [0;1], [2;3]] &lt;- ones [2;2;2;2] // sets specified elements to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when value does not match submatrix shape.</exception>
    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>) = Matrix<'T, 'S>.Empty
        and set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>) (value : Matrix<'T, 'S>) = ()

    ///<summary>Sets submatrix
    ///</summary>
    ///<param name="s0">Row subscript sequence</param>
    ///<param name="s1">Column subscript sequence</param>
    ///<param name="s2">Page subscript sequence</param>
    ///<param name="s3">4th dimension sequence</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [4;4;4;4] 
    ///x.Set([0;2], [1;3], [0;1], [2;3], 1.0) // sets specified elements to 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.IndexOutOfRangeException">Thrown when index out of range.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when wrong number of index sequences.</exception>
    member this.Set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>, value : 'T) = ()

    ///<summary>Gets or sets submatrix using boolean indexing
    ///</summary>
    ///<param name="boolMatrix">Boolean matrix. Specifies which elements to get or set</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///x.[x .&gt; 0.5] &lt;- 1.0 // sets elements to 1.0 where element value is greater than 0.5
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when boolean matrix dimensions do not match matrix dimensions.</exception>
    member this.Item
        with get(boolMatrix : __BoolMatrix) : Matrix<'T, 'S> = Matrix<'T, 'S>.Empty
        and set (boolMatrix: __BoolMatrix) (value : 'T) = ()

    ///<summary>Sets submatrix using boolean indexing
    ///</summary>
    ///<param name="boolMatrix">Boolean matrix. Specifies which elements to get or set</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = !![[1.;2.;3.]]
    ///let x2 = !![[3.;2.;1.]] 
    ///let bMatrix = x1 .&gt;= x2
    ///x1.Set(bMatrix, !![[0.;1.]]) // sets elements to 0 and 1 where x1 is greater or equal x2
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when boolean matrix dimensions do not match matrix dimensions.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when length of value matrix not equal number of true elements in boolean matrix.</exception>
    member this.Set (boolMatrix : __BoolMatrix, value : Matrix<'T, 'S>) = ()

    ///<summary>Gets or sets submatrix using boolean indexing based on given function
    ///</summary>
    ///<param name="predicate">Boolean function which specifies which elements to get or set</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///x.[fun x -&gt; x*x &lt; 2.0] &lt;- 1.0 // sets elements to 1.0 where square of element value is less than 2.0
    ///</code>
    ///</example>
    member this.Item
        with get(predicate : 'T -> bool) : Matrix<'T, 'S> = Matrix<'T, 'S>.Empty
        and set (predicate : 'T -> bool) (value : 'T) = ()

    ///<summary>Sets submatrix using boolean indexing based on given function
    ///</summary>
    ///<param name="predicate">Boolean function which specifies which elements to set</param>
    ///<param name="value">Value to set in submatrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix([1.;2.;3.])
    ///x.Set(fun x -&gt; x &gt; 1.0 , Matrix([0.;1.])) // sets elements to 0 and 1 where x is greater than 1.0
    ///</code>
    ///</example>
    ///<remarks>Subscripts are zero based
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when length of value matrix not equal number of true elements in boolean matrix.</exception>
    member this.Set (predicate : 'T -> bool, value : Matrix<'T, 'S>) = ()

    ///<summary>Converts matrix to string for display. 
    ///</summary>
    ///<returns>String representation of 2D or 3D matrix
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.ToString()
    ///</code>
    ///</example>
    ///<remarks>
    ///2D matrix will be displayed as rows/columns. 3D matrix will be displayed by page. For 4D and higher use matrix slicing.
    ///Use MatrixOptions.MaxDisplaySize to specify maximum number of columns/rows/pages to display.
    ///Use MatrixOptions.DisplayDigits to specify number of digits to display.
    ///</remarks>
    override this.ToString() = ""

    ///<summary>Gets matrix k-th diagonal as new matrix instance. If k not specified then main diag (k=0)
    ///</summary>
    ///<param name="k">Diagonal offset (optional)</param>
    ///<returns>k-th diagonal
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///let y = x.Diag(-1)
    ///let z = x.Diag()
    ///</code>
    ///</example>   
    ///<exception cref="T:System.RankException">Thrown when matrix is not 2D.</exception>   
    member this.Diag 
        with get(?k) = Matrix<'T, 'S>.Empty

    ///<summary>Converts matrix to IEnumerable sequence of elements in column major order
    ///</summary>
    ///<returns>Matrix data as sequence of elements in column major order
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3]
    ///let y = x.ToColMajorSeq() // returns seq of float
    ///</code>
    ///</example> 
    member this.ToColMajorSeq() = Array.zeroCreate<'T> 0 |> Array.toSeq

    ///<summary>Applies given function to each element of matrix in place
    ///</summary>
    ///<param name="f">Function to apply elementwise</param>
    ///<returns>(). Input matrix is modified.
    ///</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3] : Matrix
    ///x.ApplyFun(fun x -&gt; x * x) // squares each element
    ///</code>
    ///</example> 
    member this.ApplyFun(f : 'T -> 'T) = ()

    ///<summary>Creates diagonal matrix based on given vector (matrix 1xN or Nx1)
    ///</summary>
    ///<param name="matrix">Values to store in diagonal</param>
    ///<param name="offset">Offset. Positive to store values above main digonal</param>
    ///<returns>Matrix with k-th diagonal set to given vector</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let v = Matrix([1.;2.;3.])
    ///let x = Matrix.diag(x, 1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when offset too big.</exception>
    ///<exception cref="T:System.RankException">Thrown when not vector.</exception>
    static member diag(matrix: Matrix<'T, 'S>, offset : int) = Matrix<'T, 'S>.Empty

    ///<summary>Extracts lower triangular matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="offset">Offset. Specifies which diagonal should be included</param>
    ///<returns>Lower triangular matrix up to k-th diagonal</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = Matrix.triL(x, 1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when not 2D matrix.</exception>
    static member triL(matrix: Matrix<'T, 'S>, offset : int) = Matrix<'T, 'S>.Empty

    ///<summary>Extracts upper triangular matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="offset">Offset. Specifies which diagonal should be included</param>
    ///<returns>Upper triangular matrix down to k-th diagonal</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = Matrix.triU(x, -1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when not 2D matrix.</exception>
    static member triU(matrix: Matrix<'T, 'S>, offset : int) = Matrix<'T, 'S>.Empty

    ///<summary>Concatenates matrices along given dimension
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<param name="dim">Dimension</param>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [3;2;5] : Matrix
    ///let x2 = rand [3;3;5]
    ///let x3 = rand [3;4;5]
    ///let y = Matrix.concat([x1;x2;x3], 1) // returns matrix 3x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member concat(matrices: seq<Matrix<'T, 'S>>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Concatenates matrices along dimension 1
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [3;2;5] : Matrix
    ///let x2 = rand [3;3;5]
    ///let x3 = rand [3;4;5]
    ///let y = Matrix.horzConcat([x1;x2;x3]) // returns matrix 3x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member horzConcat(matrices: seq<Matrix<'T, 'S>>) = Matrix<'T, 'S>.Empty

    ///<summary>Concatenates matrices along dimension 0
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;2;5 : Matrix
    ///let x2 = rand [3;2;5]
    ///let x3 = rand [4;2;5]
    ///let y = Matrix.vertConcat([x1;x2;x3]) // returns matrix 9x2x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member vertConcat(matrices: seq<Matrix<'T, 'S>>) = Matrix<'T, 'S>.Empty

    ///<summary>Replicates matrix in each dimension
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="replicator">Array of replicators, one for each dimension</param>
    ///<returns>Replicated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;5] : Matrix
    ///let y = Matrix.repmat(x, [2;3;1] // returns matrix 4x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when replicator has less than 2 elements or negative element.</exception>
    static member repmat(matrix: Matrix<'T, 'S>, replicator : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Reshapes matrix. Number of elements must not change.
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="size">New size</param>
    ///<returns>Reshaped matrix. Input matrix is not changed</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = Matrix.reshape(x, [3;2;2]) // returns matrix 3x2x2
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when new size is invalid or length is different.</exception>
    static member reshape(matrix: Matrix<'T, 'S>, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Transposes matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<returns>Transposed matrix. Input matrix does not change</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = Matrix.transpose(x) // returns matrix 4x3
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    static member transpose(matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Casts matrix to underlying element type matrix is scalar 1x1
    ///</summary>
    static member op_Explicit(matrix : Matrix<'T, 'S>) = Unchecked.defaultof<'T>

    static member op_Explicit(matrix : Matrix<'T, 'S>) = Array.zeroCreate<'T> 0

    static member op_Explicit(matrix : Matrix<'T, 'S>) = Array2D.zeroCreate<'T> 0 0

    static member op_Explicit(matrix : Matrix<'T, 'S>) = Array3D.zeroCreate<'T> 0 0 0

    static member op_Explicit(matrix : Matrix<'T, 'S>) = Array4D.zeroCreate<'T> 0 0 0 0

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        matrix

    static member op_Explicit(data : 'T seq seq) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T list seq) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T array seq) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T seq list) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T list list) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T array list) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T seq array) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T list array) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T array array) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T seq) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T list) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T array) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T[,]) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T[,,]) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T[,,,]) = 
        new Matrix<'T,'S>(data)

    static member op_Explicit(data : 'T) = 
        new Matrix<'T,'S>(data)

    ///<summary>Creates a matrix with diagonal elements set to generic 1 and generic 0 otherwise
    ///</summary>
    ///<param name="rows">Number of rows</param>
    ///<param name="cols">Number of columns</param>
    ///<returns>Matrix with diagonal elements equal 1</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix.Identity(2,3) : Matrix
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when n or m &lt; 0.</exception>
    static member Identity(rows : int, cols : int) = Matrix<'T, 'S>.Empty

    ///<summary>Creates a matrix of given size and sets all elements to generic zero
    ///</summary>
    ///<param name="size">Size of matrix</param>
    ///<returns>Matrix with all elements equal zero</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix.zeros [2;3;4] : Matrix
    ///</code>
    ///</example>
    static member zeros(size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Creates a matrix of given size and sets all elements to generic one
    ///</summary>
    ///<param name="size">Size of matrix</param>
    ///<returns>Matrix with all elements equal one</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix.ones [2;3;4] : Matrix
    ///</code>
    ///</example>
    static member ones(size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Applies given function elementwise to a matrix. New matrix is returned. Input matrix is not modified.
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="f">Elementwise function</param>
    ///<returns>New matrix as elementwise transformation of input matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [3;4] : Matrix
    ///let y = Matrix.applyFun(x, fun x -&gt; x + 1.0)
    ///</code>
    ///</example>
    static member applyFun(matrix : Matrix<'T, 'S>, f : 'T -> 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Applies given function elementwise to 2 matrices. New matrix is returned. Input matrices are not modified and must have the same size.
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<param name="f">Elementwise function of 2 args</param>
    ///<returns>New matrix as elementwise transformation of 2 input matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [3;4] : Matrix
    ///let x2 = rand [3;4]
    ///let y = Matrix.applyFun2Arg(x1, x2, fun x1 x2 -&gt; x1 + x2)
    ///</code>
    ///</example>
    static member applyFun2Arg(matrix1 : Matrix<'T, 'S>, matrix2 : Matrix<'T, 'S>, f : 'T -> 'T -> 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Applies given function elementwise to 3 matrices. New matrix is returned. Input matrices are not modified and must have the same size.
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<param name="matrix3">Third matrix</param>
    ///<param name="f">Elementwise function of 3 args</param>
    ///<returns>New matrix as elementwise transformation of 3 input matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [3;4] : Matrix
    ///let x2 = rand [3;4]
    ///let x3 = rand [3;4]
    ///let y = Matrix.applyFun3Arg(x1, x2, x3, fun x1 x2 x3 -&gt; x1 + x2 + x3)
    ///</code>
    ///</example>
    static member applyFun3Arg(matrix1 : Matrix<'T, 'S>, matrix2 : Matrix<'T, 'S>, matrix3 : Matrix<'T, 'S>, f : 'T -> 'T -> 'T -> 'T) = Matrix<'T, 'S>.Empty

//***************************************************************************************************************************************
//********************COMPARABLE*********************************************************************************************************
//***************************************************************************************************************************************

    ///<summary>Checks if 2 matrix instances hold identical values
    ///</summary>
    ///<param name="matrix1">First matrix to compare</param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>True if matrices have the same size and the same values in all cells</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1 == x2 // returns boolean
    ///</code>
    ///</example>
    static member (==) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = true

    ///<summary>Checks if matrix is a scalar matrix equal to given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">scalar value</param>
    ///<returns>True if the same value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix(2.0) // scalar matrix
    ///let a = 2.0
    ///let y = x == a // returns true
    ///</code>
    ///</example>
    static member (==) (matrix: Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if matrix is a scalar matrix equal to given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">scalar value</param>
    ///<returns>True if the same value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix(2.0) // scalar matrix
    ///let a = 2.0
    ///let y = a == x // returns true
    ///</code>
    ///</example>
    static member (==) (a : 'T, matrix: Matrix<'T, 'S>) = true

    ///<summary>Checks if 2 matrix instances hold different value
    ///</summary>
    ///<param name="matrix1">First matrix to compare</param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>True if matrices have different size or at least one different value in a cell</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1 != x2 // returns boolean
    ///</code>
    ///</example>
    static member (!=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = true

    ///<summary>Checks if matrix is a scalar matrix not equal to given value
    ///</summary>
    ///<param name="a">scalar value</param>
    ///<param name="matrix">Matrix to compare</param>
    ///<returns>True if the same value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix(2.0) // scalar matrix
    ///let a = 1.0
    ///let y = x != a // returns true
    ///</code>
    ///</example>
    static member (!=) (matrix: Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if matrix is a scalar matrix not equal to given value
    ///</summary>
    ///<param name="a">scalar value</param>
    ///<param name="matrix">Matrix to compare</param>
    ///<returns>True if the same value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix(2.0) // scalar matrix
    ///let a = 1.0
    ///let y = a != x // returns true
    ///</code>
    ///</example>
    static member (!=) (a : 'T, matrix: Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;= 1.0 // returns true
    ///</code>
    ///</example>
    static member (&=) (matrix: Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are not equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;!= 2.0 // returns true
    ///</code>
    ///</example>
    static member (&!=) (matrix: Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are not equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;&lt;&gt; 2.0 // returns true
    ///</code>
    ///</example>
    static member (&<>) (matrix: Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are less than given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are less than given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;&lt; 2.0 // returns true
    ///</code>
    ///</example>
    static member (&<) (matrix : Matrix<'T, 'S>, a : 'T)  = true

    ///<summary>Checks if all matrix elements are less than equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are less than equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;&lt;= 1.0 // returns true
    ///</code>
    ///</example>
    static member (&<=) (matrix : Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are greater than given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are greater than given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;&gt; 1.0 // returns true
    ///</code>
    ///</example>
    static member (&>) (matrix : Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are greater than equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are greater than equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = x &amp;&gt;= 1.0 // returns true
    ///</code>
    ///</example>
    static member (&>=) (matrix : Matrix<'T, 'S>, a : 'T) = true

    ///<summary>Checks if all matrix elements are greater than given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are greater than given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = 2.0 &amp;&lt; x // returns false
    ///</code>
    ///</example>
    static member (&<) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are greater than equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are greater than equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;&lt;= x // returns true
    ///</code>
    ///</example>
    static member (&<=) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are less than given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are less than given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;&gt; x // returns false
    ///</code>
    ///</example>
    static member (&>) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are less than equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are less than equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;&gt;= x // returns true
    ///</code>
    ///</example>
    static member (&>=) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;= x // returns true
    ///</code>
    ///</example>
    static member (&=) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are not equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are not equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics.Math
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;!= x // returns true
    ///</code>
    ///</example>
    static member (&!=) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Checks if all matrix elements are not equal given value
    ///</summary>
    ///<param name="matrix">Matrix to compare</param>
    ///<param name="a">Value</param>
    ///<returns>True if all matrix elements are not equal given value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics.Math
    ///let x = ones [2;3]
    ///let y = 1.0 &amp;&lt;&gt; x // returns true
    ///</code>
    ///</example>
    static member (&<>) (a : 'T, matrix : Matrix<'T, 'S>) = true

    ///<summary>Applies elementwise "less than" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Value to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .&lt; 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<) (matrix: Matrix<'T, 'S>, a: 'T) = new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "less than or equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .&lt;= 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<=) (matrix: Matrix<'T, 'S>, a: 'T) = new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .&gt; 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than or equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .&gt;= 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>=) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .== 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.==) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .= 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.=) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .!= 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.!=) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to matrix and a value
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[x .&lt;&gt; 0.5] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<>) (matrix: Matrix<'T, 'S>, a: 'T) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "less than" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .&lt; x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "less than or equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .&lt;= x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<=) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .&gt; x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than or equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .&gt;= x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>=) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .== x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.==) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .= x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.=) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .!= x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.!=) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to a value and matrix
    ///</summary>
    ///<param name="matrix">Matrix to be compared</param>
    ///<param name="a">Number to compare matrix elements with</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x.[0.5 .&lt;&gt; x] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<>) (a : 'T, matrix: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "less than" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .&lt; x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<) ( matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "less than or equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .&lt;= x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .&gt; x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "greater than or equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .&gt;= x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.>=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .== x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.==) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .= x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .!= x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.!=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Applies elementwise "not equal" operator to matrices (including scalar matrices 1x1)
    ///</summary>
    ///<param name="matrix1">First matrix to compare. </param>
    ///<param name="matrix2">Second matrix to compare</param>
    ///<returns>Boolean matrix with result of elementwise comparison</returns>
    ///<remarks>You can use boolean matrix result in matrix indexing:
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1.[x1 .&lt;&gt; x2] // returns vector
    ///</code>
    ///</example>
    ///</remarks>
    static member (.<>) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =  new __BoolMatrix([], Managed([||]))

    ///<summary>Calculates elementwise minimum of matrices.
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Elementwise minimum of x and y</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = rand [2;3;4]
    ///let z = Matrix.minXY(x, y)
    ///</code>
    ///</example>
    static member minXY(matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise minimum of matrix and scalar
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Scalar argument</param>
    ///<returns>Elementwise minimum</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4]
    ///let a = 0.5
    ///let z = Matrix.minXY(x, a)
    ///</code>
    ///</example>
    static member minXY(matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise minimum of matrix and scalar
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Scalar argument</param>
    ///<returns>Elementwise minimum</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4]
    ///let a = 0.5
    ///let z = Matrix.minXY(a, x)
    ///</code>
    ///</example>
    static member minXY(a :'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise maximum of matrices.
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Elementwise minimum of x and y</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = rand [2;3;4]
    ///let z = Matrix.maxXY(x, y)
    ///</code>
    ///</example>
    static member maxXY(matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise maximum of matrix and scalar
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Scalar argument</param>
    ///<returns>Elementwise maximum</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4]
    ///let a = 0.5
    ///let z = Matrix.maxXY(x, a)
    ///</code>
    ///</example>
    static member maxXY(matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise maximum of matrix and scalar
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Scalar argument</param>
    ///<returns>Elementwise maximum</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4]
    ///let a = 0.5
    ///let z = Matrix.maxXY(a, x)
    ///</code>
    ///</example>
    static member maxXY(a :'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates minimum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which minimum will be calculated</param>
    ///<returns>Matrix with reduced data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.min(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member min(matrix: Matrix<'T, 'S>, dim:int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates minimum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which minimum will be calculated</param>
    ///<returns>Matrix with reduced data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.max(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member max(matrix: Matrix<'T, 'S>, dim:int) = Matrix<'T, 'S>.Empty

//***************************************************************************************************************************************
//********************BOOLEAN*********************************************************************************************************
//***************************************************************************************************************************************
  
    ///<summary>Calculates elementwise "AND" for bool matrices
    ///</summary>
    ///<param name="matrix1">First bool matrix</param>
    ///<param name="matrix2">Second bool matrix</param>
    ///<returns>Result of elementwise "AND"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let x = BoolMatrix([true;false;false])
    ///let z = x .&amp;&amp; y // returns bool matrix [true;false;false]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrices do not have matching sizes.</exception>
    static member (.&&) (matrix1: Matrix<'T,'S>, matrix2: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "AND" for bool matrix and scalar value
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<param name="a">Scalar value</param>
    ///<returns>Result of elementwise "AND"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let a = true
    ///let z = x .&amp;&amp; a // returns bool matrix [true;false;true]
    ///</code>
    ///</example>
    static member (.&&) (matrix: Matrix<'T,'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "AND" for bool matrix and scalar value
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<param name="a">Scalar value</param>
    ///<returns>Result of elementwise "AND"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let a = true
    ///let z = a .&amp;&amp; x // returns bool matrix [true;false;true]
    ///</code>
    ///</example>
    static member (.&&) (a : 'T, matrix: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "OR" for bool matrices
    ///</summary>
    ///<param name="matrix1">First bool matrix</param>
    ///<param name="matrix2">Second bool matrix</param>
    ///<returns>Result of elementwise "OR"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let x = BoolMatrix([true;false;false])
    ///let z = x .|| y // returns bool matrix [true;false;true]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrices do not have matching sizes.</exception>
    static member (.||) (matrix1: Matrix<'T,'S>, matrix2: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "AND" for bool matrix and scalar value
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<param name="a">Scalar value</param>
    ///<returns>Result of elementwise "AND"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let a = true
    ///let z = x .|| a // returns bool matrix [true;true;true]
    ///</code>
    ///</example>
    static member (.||) (matrix: Matrix<'T,'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "OR" for bool matrix and scalar value
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<param name="a">Scalar value</param>
    ///<returns>Result of elementwise "OR"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let a = true
    ///let z = a .&amp;&amp; x // returns bool matrix [true;true;true]
    ///</code>
    ///</example>
    static member (.||) (a : 'T, matrix: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "NOT" for bool matrix
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<returns>Result of elementwise "NOT"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let y = ~x // returns bool matrix [false;true;false]
    ///</code>
    ///</example>
    static member (~~) (matrix: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise "NOT" for bool matrix
    ///</summary>
    ///<param name="matrix">Bool matrix</param>
    ///<returns>Result of elementwise "NOT"</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = BoolMatrix([true;false;true])
    ///let y = ~x // returns bool matrix [false;true;false]
    ///</code>
    ///</example>
    static member not (matrix: Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

//***************************************************************************************************************************************
//********************NUMERIC OPERATORS*********************************************************************************************************
//***************************************************************************************************************************************

    ///<summary>Multiplies matrix by a scalar value
    ///</summary>
    ///<param name="a">Scalar argument</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix multiplied by a value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 * x 
    ///</code>
    ///</example>
    static member (*) (a: 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Multiplies matrix by a scalar value
    ///</summary>
    ///<param name="a">Scalar argument</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix multiplied by a value</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x * 2.0 
    ///</code>
    ///</example>
    static member (*) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Multiplies 2D matrix by 2D matrix
    ///</summary>
    ///<param name="matrix1">First 2D matrix</param>
    ///<param name="matrix2">Second 2D matrix</param>
    ///<returns>Matrix multiplied by matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3] : Matrix
    ///let x1 = rand [3;4]
    ///let y = x1 * x2 // return matrix 2x4 
    ///</code>
    ///</example>
    static member (*) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Multiplies scalar value by a matrix elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix multiplied by scalar elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 .* x
    ///</code>
    ///</example> 
    static member (.*) (a: 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Multiplies matrix by scalar value elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix multiplied by scalar elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x .* 2.0
    ///</code>
    ///</example> 
    static member (.*) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Multiplies matrices elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Multiplied matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x1 = rand [2;3;4]
    ///let y = x1 .* x2  
    ///</code>
    ///</example> 
    static member (.*) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Adds matrices elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Added matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x1 = rand [2;3;4]
    ///let y = x1 .+ x2  
    ///</code>
    ///</example>  
    static member (.+) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Adds scalar value to a matrix
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar added to a matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 .+ x  
    ///</code>
    ///</example> 
    static member (.+) (a : 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Adds scalar value to a matrix
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar added to a matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x .+ 2.0 
    ///</code>
    ///</example> 
    static member (.+) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Adds matrices elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Added matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x1 = rand [2;3;4]
    ///let y = x1 + x2  
    ///</code>
    ///</example>  
    static member (+) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Adds matrix to a scalar value elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar added to a matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 + x  
    ///</code>
    ///</example>  
    static member (+) (a : 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Adds matrix to a scalar value elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar added to a matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x + 2.0  
    ///</code>
    ///</example>  
    static member (+) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts matrix from a scalar elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix subtracted from scalar</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 - x
    ///</code>
    ///</example> 
    static member (-) (a: 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts scalar value from a matrix elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar subtracted from matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x - 2.0
    ///</code>
    ///</example> 
    static member (-) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts matrix from a matrix elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Matrix subtracted from matrix elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    /// let x2 = rand [2;3;4]
    ///let y = x1 - x2
    ///</code>
    ///</example> 
    static member (-) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts matrix from a scalar elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix subtracted from scalar</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 .- x
    ///</code>
    ///</example> 
    static member (.-) (a: 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts scalar value from a matrix elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar subtracted from matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x .- 2.0
    ///</code>
    ///</example> 
    static member (.-) (matrix: Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Subtracts matrix from a matrix elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Matrix subtracted from matrix elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x1 = rand [2;3;4] : Matrix
    /// let x2 = rand [2;3;4]
    ///let y = x1 .- x2
    ///</code>
    ///</example> 
    static member (.-) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates minus matrix
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix with "-" applied elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = -x
    ///</code>
    ///</example> 
    static member (~-) (matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Divides matrix by matrix elementwise
    ///</summary>
    ///<param name="matrix1">First matrix</param>
    ///<param name="matrix2">Second matrix</param>
    ///<returns>Matrix divided by another matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BAsicStat
    ///let x1 = rand [2;3;4] : Matrix
    ///let x2 = rand [2;3;4]
    ///let y = x1 ./ x2
    ///</code>
    ///</example> 
    static member (./) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Divides matrix by scalar value elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix divided by scalar</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x / 2.0
    ///</code>
    ///</example> 
    static member (/) (matrix : Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Divides matrix by scalar value elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Matrix divided by scalar</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x ./ 2.0
    ///</code>
    ///</example> 
    static member (./) (matrix : Matrix<'T, 'S>, a : 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Divides scalar by matrix elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar divided by matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 / x
    ///</code>
    ///</example> 
    static member (/) ( a : 'T, matrix : Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Divides scalar by matrix elementwise
    ///</summary>
    ///<param name="a">Scalar value</param>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Scalar divided by matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 ./ x
    ///</code>
    ///</example> 
    static member (./) ( a : 'T, matrix : Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise power of matrix
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Power</param>
    ///<returns>Elementwise power of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = x ** 2.0
    ///</code>
    ///</example> 
    static member Pow (matrix: Matrix<'T, 'S>, a: 'T) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise scalar power matrix
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="a">Power</param>
    ///<returns>Elementwise power of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = 2.0 .^ x
    ///</code>
    ///</example> 
    static member (.^) (a: 'T, matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise power of matrix
    ///</summary>
    ///<param name="matrix1">Matrix argument</param>
    ///<param name="matrix2">Power</param>
    ///<returns>Elementwise power of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = rand [2;3;4] : Matrix
    ///let z = x ** y
    ///</code>
    ///</example> 
    static member Pow (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

//***************************************************************************************************************************************
//********************NUMERIC VECTOR FUNCTIONS*********************************************************************************************************
//***************************************************************************************************************************************

    ///<summary>Calculates elementwise absolute value
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Absolute value of matrix elementwise</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] - 0.5
    ///let y = abs(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Abs(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise square root
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = sqrt(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Sqrt(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise sin
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = sin(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Sin(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise cos
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = cos(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Cos(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise tan
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = tan(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Tan(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise asin
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = asin(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Asin(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise acos
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = acos(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Acos(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise atan
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = atan(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Atan(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise sinh
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = sinh(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Sinh(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise cosh
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = cosh(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Cosh(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise tanh
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = tanh(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Tanh(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise exp
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = exp(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Exp(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise log
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = log(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Log(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise log10
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = log10(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Log10(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise round
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = round(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Round(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise ceil
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise sqrt of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = ceil(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Ceiling(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise error function
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.erf(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Erf(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise complementary error function
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise complementary error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.erfc(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Erfc(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise inverse error function
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise inverse error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.erfinv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Erfinv(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise inverse complementary error function
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise inverse complementary error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.erfcinv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Erfcinv(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise standard normal cumulative distribution
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise standard normal cumulative distribution of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.normcdf(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Normcdf(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates elementwise inverse standard normal cumulative distribution
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<returns>Elementwise inverse standard normal cumulative distribution function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = Matrix.norminv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    static member Norminv(matrix : Matrix<'T,'S>) = Matrix<'T, 'S>.Empty

//***************************************************************************************************************************************
//********************NUMERIC RANDOM*********************************************************************************************************
//***************************************************************************************************************************************

    ///<summary>Generates matrix with continuous uniform random numbers in [a, b] 
    ///</summary>
    ///<param name="a">Lower endpoint</param>
    ///<param name="b">Upper endpoint</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<returns>Matrix with random data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.unifRnd(0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or b &lt;= a.</exception>
    static member unifRnd(a : 'T, b : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with normal random numbers
    ///</summary>
    ///<param name="mean">Mean</param>
    ///<param name="sigma">Standard deviation</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.normRnd(0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or sigma &lt;= 0.</exception>
    static member normalRnd(mean : 'T, sigma : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with lognormal random numbers
    ///</summary>
    ///<param name="mean">Mean</param>
    ///<param name="sigma">Standard deviation</param>
    ///<param name="a">Displacement</param>
    ///<param name="scale">Scale</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.lognormRnd(0.0, 1.0, 0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or sigma &lt;= 0.</exception>
    static member lognormRnd(mean : 'T, sigma : 'T, a : 'T, scale : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with Bernoulli distributed random numbers
    ///</summary>
    ///<param name="p">Probability of success</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.bernRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or p not between 0 and 1.</exception>
    static member bernRnd(p : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with binomial distributed random numbers
    ///</summary>
    ///<param name="n">Number of trials</param>
    ///<param name="p">Probability of success</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.binomRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or p not between 0 and 1 or n &lt;0.</exception>
    static member binomRnd(n : int, p : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with Poisson distributed random numbers
    ///</summary>
    ///<param name="lambda">Lambda</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = Matrix.poissRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or lambda negative.</exception>
    static member poissRnd(lambda : 'T, size : seq<int>) = Matrix<'T, 'S>.Empty

    ///<summary>Generates matrix with multivariate normal distribution
    ///</summary>
    ///<param name="mean">Vector kx1 or 1xk of means</param>
    ///<param name="cov">Covariance matrix kxk</param>
    ///<param name="n">Number of k-dimensional vectors to generate. Vectors are returned in rows of result matrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let mn = !![2.345;2.345]
    ///let cv = !![ [1.;1.]
    ///             [1.;2.] ]
    ///let m = Matrix.mvNormRnd(mn, cv, 10) // returns matrix 10x2 
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or cov matrix not pos definite or incompatible size.</exception>
    static member mvNormRnd(mean : Matrix<'T, 'S>, cov : Matrix<'T, 'S>, n: int) = Matrix<'T, 'S>.Empty
 
//***************************************************************************************************************************************
//********************NUMERIC LINEAR ALGEBRA*********************************************************************************************************
//*************************************************************************************************************************************** 

    ///<summary>Performs cholesky factorization
    ///</summary>
    ///<param name="matrix">Input matrix. Must be positive definite.</param>
    ///<returns>Upper triangular matrix calculated in factorization</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///let x = Matrix([ [1.0;0.5]
    ///                 [0.5;1.0] ]
    ///let y = Matrix.chol(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix not symmetrical.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when matrix not positive definite.</exception>
    static member chol(matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Solves linear equation using chol factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must be positive definite.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let a = Matrix([ [1.0;0.5]
    ///                 [0.5;1.0] ]
    ///let b = rand [2;1]
    ///let x = Matrix.cholSolve(a, b) // ax=b
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not square or A and B have non compatible dimensions.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix not symmetrical.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when matrix A not positive definite.</exception>
    static member cholSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Performs LU factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(L, U, P) Lower/Upper matrices, P is a vector with row permutations</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [20;10]
    ///let (l, u, p) = Matrix.lu(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    static member lu(matrix: Matrix<'T, 'S>) = ( Matrix<'T, 'S>.Empty,  Matrix<'T, 'S>.Empty, Array.zeroCreate<int> 0)

    ///<summary>Solves linear equation using LU factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = Matrix.luSolve(a, b) // ax=b
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D square or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found.</exception>
    static member luSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Performs QR factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(Q, R) matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [20;10]
    ///let (q, r) = Matrix.qr(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when factorization failed.</exception>
    static member qr(matrix: Matrix<'T, 'S>) = (Matrix<'T, 'S>.Empty,  Matrix<'T, 'S>.Empty)

    ///<summary>Finds least squares solution of linear equation using QR factorization assuming full rank
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must have full rank</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Least squares solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = Matrix.qrSolveFull(a, b)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Matrix does not have full rank</exception>
    static member qrSolveFull(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Finds least squares solution of linear equation using singular value factorization with given tolerance
    ///</summary>
    ///<param name="a">A in equation Ax=b.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<param name="tol">Tolerance to determine rank of A.</param>
    ///<returns>Least squares solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = Matrix.svdSolve(a, b, 1e-10)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Try different tolerance.</exception>
    static member svdSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>, tol : 'T) = (Matrix<'T, 'S>.Empty, 0)

    ///<summary>Performs singular value factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(U, S, Vt) matrices, S is vector of singular values, Vt is transposed V </returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [20;10]
    ///let (u, s, vt) = Matrix.svd(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when algorithm did not converge.</exception>
    static member svd(matrix: Matrix<'T, 'S>) = (Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty)

//***************************************************************************************************************************************
//********************NUMERIC BASIC STATS*********************************************************************************************************
//*************************************************************************************************************************************** 

    ///<summary>Calculates sum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which sum will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.sum(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member sum(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates cumulative sum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which cumulative sum will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.cumsum(x, 1) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member cumsum(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates product of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which product will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.prod(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member prod(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates cumulative product of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which cumulative product will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.cumprod(x, 1) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    ///<exception cref="T:System.OutOfMemoryException">Thrown when not enough memory available.</exception>
    ///<exception cref="T:System.ObjectDisposedException">Thrown when matrix has been disposed with Dispose().</exception>
    static member cumprod(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates mean of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which mean will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.mean(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member mean(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates variance of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which variance will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.var(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member var(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates skewness of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which skewness will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.skewness(x, 1) // returns Matrix with size [2;1;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member skewness(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates kurtosis of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which kurtosis will be calculated</param>
    ///<returns>matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] : Matrix
    ///let y = Matrix.kurtosis(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    static member kurtosis(matrix: Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates quantiles of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="q">Quantiles vector:  Matrix 1xn or nx1</param>
    ///<param name="dim">Dimension along which quantiles will be calculated</param>
    ///<returns>matrix with quantiles</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;30;4] : Matrix
    ///let q = Matrix([0.05;0.95])
    ///let y = Matrix.quantile(x, q, 1) // returns Matrix with size [2;2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified or quantile q not in 0&lt;=q&lt;=1.</exception>
    ///<exception cref="T:System.RankException">Thrown when quantiles not a vector 1xn or nx1.</exception>
    static member quantile(matrix: Matrix<'T, 'S>, q : Matrix<'T, 'S>, dim : int) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates correlation between 2D matrix columns
    ///</summary>
    ///<param name="matrix">2D Matrix argument [nxp], with n observations and p variables</param>
    ///<returns>Correlation matrix pxp</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [10;3] //10 observations, 3 variables
    ///let y = Matrix.corr(x) // returns Matrix with size [3;3]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    static member corr(matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

    ///<summary>Calculates covariance between 2D matrix columns
    ///</summary>
    ///<param name="matrix">2D Matrix argument [nxp], with n observations and p variables</param>
    ///<returns>Covariance matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [10;3] //10 observations, 3 variables
    ///let y = Matrix.cov(x) // returns Matrix with size [3;3]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    static member cov(matrix: Matrix<'T, 'S>) = Matrix<'T, 'S>.Empty

