namespace Fmat.Numerics

open System
open MatrixUtil
open Validation
open GenericFormatting

type __BoolMatrix(size : seq<int>, data : matrixData<bool>) =
    let _data = data
    let _size = 
        validateMatrixSize size
        validateLengthAndSizeMatch (getMatrixDataLength(data)) size
        getSqueezedSize size

    member this.Data = _data
    member this.Size = _size

///<summary>Multidimensional generic matrix
///</summary>
///<param name="size">Size of each dimension. There must be at least 2 dimensions.</param>
///<param name="data">Matrix data in column major order.</param>
///<typeparam name="T">Type of matrix elements</typeparam>
///<typeparam name="S">Type of matrix operations interface</typeparam>
///<remarks>Must have at least 2 dimensions. Scalar values are matrices 1x1, vectors are 1xn or nx1
///</remarks>
type Matrix<'T, 'S when 'S: (new : unit -> 'S) and 'S:>IMatrixOps<'T>> (size : seq<int>, data : matrixData<'T>) =
    let mutable _size =
        validateMatrixSize size
        validateLengthAndSizeMatch (getMatrixDataLength(data)) size
        getSqueezedSize size

    let _data = data

    static let _matrixOps = new 'S()
    static let _genericOps = _matrixOps.GenericMatrixOps
    static let _compOps = _matrixOps.ComparableMatrixOps
    static let _boolOps = _matrixOps.BoolMatrixOps
    static let _numericOps = _matrixOps.NumericMatrixOps

    static let _empty = let size = Array.create 2 0
                        let matrixData = Managed(Array.zeroCreate<'T> 0)
                        new Matrix<'T, 'S>(size, matrixData)

    static let mutable _maxDisplaySize = [|10|]

    interface IFormattable with
        member this.ToString(format, provider) =
            _genericOps.ToString(_data, _size, format, Matrix<'T,'S>.MaxDisplaySize)

    new(size: seq<int>, data : seq<'T>) =
        validateMatrixSize size
        let matrixData, len = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData) 

    new(size : seq<int>, init : 'T) =
        validateMatrixSize size
        let matrixData = _genericOps.CreateMatrixData(size, init)
        new Matrix<'T, 'S>(size, matrixData) 
    new(data : 'T[,]) =
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData) 

    new(data : 'T[,,]) =
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData) 

    new(data : 'T[,,,]) =
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData) 

    new(data : seq<'T>) =
        let matrixData, len = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>([1;len], matrixData)

    new (data : seq<'T list>) =
        let data = data |> Seq.map (fun x -> x:>seq<'T>)
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData)

    new (data : seq<seq<'T>>) =
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData)

    new (data : seq<'T[]>) =
        let data = data |> Seq.map (fun x -> x:>seq<'T>)
        let matrixData, size = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData)
        
    new (data:'T) =
        let size = [1;1] 
        let matrixData = _genericOps.CreateMatrixData(data)
        new Matrix<'T, 'S>(size, matrixData)

    new (boolMatrix : __BoolMatrix) =
        let matrixData = _genericOps.CreateMatrixData(boolMatrix.Data)
        new Matrix<'T,'S>(boolMatrix.Size, matrixData)

    new (size : seq<int>, initializer : int -> 'T) =
        validateMatrixSize size
        let n = size |> Seq.fold (*) 1
        let matrixData = _genericOps.CreateMatrixData(n, initializer)
        new Matrix<'T,'S>(size, matrixData)

    new (size : seq<int>, initializer : int -> int -> 'T) =
        validateMatrixSize size
        let size = size |> Seq.toArray
        let matrixData = _genericOps.CreateMatrixData(size, initializer)
        new Matrix<'T,'S>(size, matrixData)

    new (size : seq<int>, initializer : int -> int -> int -> 'T) =
        validateMatrixSize size
        let size = size |> Seq.toArray
        let matrixData = _genericOps.CreateMatrixData(size, initializer)
        new Matrix<'T,'S>(size, matrixData)

    new (size : seq<int>, initializer : int -> int -> int -> int -> 'T) =
        validateMatrixSize size
        let size = size |> Seq.toArray
        let matrixData = _genericOps.CreateMatrixData(size, initializer)
        new Matrix<'T,'S>(size, matrixData)

    member this.Data with get() = _data
    member this.NDims with get() = _size.Length
    member this.Length with get() = _size |> Array.fold (*) 1
    member this.Size with get() = _size |> Array.copy 

    member this.IsVector =
        let n = this.Size.[0]
        let m = this.Size.[1]
        this.NDims = 2 && (n = 1 || m = 1)

    member this.IsScalar =
        this.Length = 1

    static member Empty = _empty

    static member DisplayFormat
        with get() = GenericFormat.Instance.GetFormat<'T>() Unchecked.defaultof<'T>
        and set(value) = GenericFormat.Instance.SetFormat(fun (x : 'T) -> value) 

    static member DisplayDigits
        with set(digits : int) = GenericFormat.Instance.SetFormat(fun (x : 'T) -> String.Format("G{0}", digits)) 

    static member MaxDisplaySize 
        with get() = _maxDisplaySize
        and set(value) = _maxDisplaySize <- value

    member this.T = 
        validate2D _size
        let size = [_size.[1]; _size.[0]]
        let matrixData = _genericOps.Transpose(_data, _size)
        new Matrix<'T, 'S>(size, matrixData)

    member this.Reshape(size : seq<int>) =
        validateMatrixSize size
        validateLengthAndSizeMatch this.Length size
        let s = size |> getSqueezedSize
        _size <- s

    member this.Transpose() =
        validate2D _size
        let n = _size.[0]
        let m = _size.[1]
        _genericOps.TransposeInPlace(_data, _size)
        _size.[0] <- m
        _size.[1] <- n

    member this.GetSlice(start, finish : option<int>) =
        let (s,e) = getLinSliceStartEnd start finish this.Length
        let len = e - s + 1
        let size = if this.NDims = 2 && _size.[1] = 1 then [len;1] else [1;len]
        let matrixData = _genericOps.GetLinearSlice(_data, s, e)
        new Matrix<'T, 'S>(size, matrixData)  
        
    member this.SetSlice(start, finish : option<int>, value : Matrix<'T, 'S>) =
        let (s,e) = getLinSliceStartEnd start finish this.Length
        if e - s + 1 <> value.Length then raise (new ArgumentException())
        _genericOps.SetLinearSlice(_data, s, e, value.Data)    

    member this.SetSlice(start, finish : option<int>, value : 'T) =
        let (s,e) = getLinSliceStartEnd start finish this.Length
        _genericOps.SetLinearSlice(_data, s, e, value) 

    member this.GetSlice(start0, end0, start1, end1) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1)|] _size
        let matrixData = _genericOps.GetSlice(_data, _size, slice)
        new Matrix<'T, 'S>(size, matrixData) 

    member this.SetSlice(start0, end0, start1, end1, value : Matrix<'T, 'S>) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1)|] _size
        validateSizesMatch size value.Size
        _genericOps.SetSlice(_data, _size, slice, value.Data)

    member this.SetSlice(start0, end0, start1, end1, value : 'T) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1)|] _size
        _genericOps.SetSlice(_data, _size, slice, value)

    member this.GetSlice(start0, end0, start1, end1, start2, end2) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2)|] _size
        let matrixData = _genericOps.GetSlice(_data, _size, slice)
        new Matrix<'T, 'S>(size, matrixData) 

    member this.SetSlice(start0, end0, start1, end1, start2, end2, value : Matrix<'T, 'S>) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2)|] _size
        validateSizesMatch size value.Size
        _genericOps.SetSlice(_data, _size, slice, value.Data)

    member this.SetSlice(start0, end0, start1, end1, start2, end2, value : 'T) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2)|] _size
        _genericOps.SetSlice(_data, _size, slice, value)

    member this.GetSlice(start0, end0, start1, end1, start2, end2, start3, end3) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2);(start3,end3)|] _size
        let matrixData = _genericOps.GetSlice(_data, _size, slice)
        new Matrix<'T, 'S>(size, matrixData) 

    member this.SetSlice(start0, end0, start1, end1, start2, end2, start3, end3, value : Matrix<'T, 'S>) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2);(start3,end3)|] _size
        validateSizesMatch size value.Size
        _genericOps.SetSlice(_data, _size, slice, value.Data)

    member this.SetSlice(start0, end0, start1, end1, start2, end2, start3, end3, value : 'T) =
        let (slice, size) = getSliceStartEnd [|(start0, end0);(start1,end1);(start2,end2);(start3,end3)|] _size
        _genericOps.SetSlice(_data, _size, slice, value)

    member this.Item
        with get(i : int) =
            validateIndex i this.Length
            _genericOps.[_data, i]
        and set (i : int) value =
            validateIndex i this.Length
            _genericOps.[_data, i] <- value

    member this.Item
        with get(indices : int[]) =
            validateIndices indices _size
            _genericOps.[_data, _size, indices]
        and set (indices : int[]) value =
            validateIndices indices _size
            _genericOps.[_data, _size, indices] <- value

    member this.Item
        with get(i, j) =
            validateIndices [|i;j|] _size
            _genericOps.[_data, _size, [|i;j|]]

    member this.Item
        with get(i, j, k) =
            validateIndices [|i;j;k|] _size
            _genericOps.[_data, _size, [|i;j;k|]]

    member this.Item
        with get(i, j, k, l) =
            validateIndices [|i;j;k;l|] _size
            _genericOps.[_data, _size, [|i;j;k;l|]]

    member this.Item
        with set (i, j) value =
            validateIndices [|i;j|] _size
            _genericOps.[_data, _size, [|i;j|]] <- value

    member this.Item
        with set (i, j, k) value =
            validateIndices [|i;j;k|] _size
            _genericOps.[_data, _size, [|i;j;k|]] <- value

    member this.Item
        with set (i, j, k, l) value =
            validateIndices [|i;j;k;l|] _size
            _genericOps.[_data, _size, [|i;j;k;l|]] <- value

    member this.Item
        with get(indices : seq<int>) = validateSeqIndices indices this.Length
                                       let matrixData = _genericOps.[_data, indices]
                                       let n = getMatrixDataLength matrixData
                                       let size = if this.NDims = 2 && _size.[1] = 1 then [|n;1|] else [|1;n|]
                                       new Matrix<'T, 'S>(size, matrixData)
        and set (indices : seq<int>) (value : Matrix<'T, 'S>) = 
                                    validateSeqIndices indices this.Length
                                    validateSeqLength indices value.Length
                                    _genericOps.[_data, indices] <- value.Data

    member this.Set (indices : seq<int>, value : 'T) = validateSeqIndices indices this.Length
                                                       _genericOps.Set(_data, indices, value)

    member this.Item
        with get([<ParamArray>] indexSeqs: seq<int>[]) =
            let n = indexSeqs |> Seq.length
            if (n = 1) then
                this.[indexSeqs.[0]]
            else
                validateIndexRangeSeq indexSeqs _size 
                let (matrixData, size) = _genericOps.[_size, _data, indexSeqs]
                new Matrix<'T, 'S>(size, matrixData)
        and set ([<ParamArray>] indexSeqs: seq<int>[]) (value : Matrix<'T, 'S>) = 
            let n = indexSeqs |> Seq.length
            if (n = 1) then
                this.[indexSeqs.[0]] <- value
            else
                validateIndexRangeSeq indexSeqs _size 
                validateIndexRangeMatchesSize indexSeqs value.Size
                _genericOps.[_size, _data, indexSeqs] <- value.Data

    member this.Set (indexSeqs: seq<seq<int>>, value : 'T) = 
        let n = indexSeqs |> Seq.length
        if n = 0 then raise (new ArgumentException())
        if (n = 1) then
            _genericOps.Set(_data, indexSeqs |> Seq.head, value)
        else
            let indexSeqs = indexSeqs |> Seq.toArray
            validateIndexRangeSeq indexSeqs _size 
            _genericOps.Set(_data, _size, indexSeqs, value)

    member this.Set (indexSeqs: seq<int[]>, value : 'T) = 
        let indexSeqs = indexSeqs |> Seq.map (fun x -> x |> Array.toSeq)
        this.Set(indexSeqs, value)

    member this.Set (indexSeqs: seq<int list>, value : 'T) = 
        let indexSeqs = indexSeqs |> Seq.map (fun x -> x |> List.toSeq)
        this.Set(indexSeqs, value)

    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>) = this.[[|s0;s1|]]
        and set (s0 : seq<int>, s1 : seq<int>) (value : Matrix<'T, 'S>) = this.[[|s0;s1|]] <- value

    member this.Set (s0 : seq<int>, s1 : seq<int>, value : 'T) = this.Set( [|s0;s1|], value)

    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>, s2 : seq<int>) = this.[[|s0;s1;s2|]]
        and set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>) (value : Matrix<'T, 'S>) = this.[ [|s0;s1;s2|]] <- value

    member this.Set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, value : 'T) = this.Set( [|s0;s1;s2|], value)

    member this.Item
        with get(s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>) = this.[[|s0;s1;s2;s3|]]
        and set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>) (value : Matrix<'T, 'S>) = this.[ [|s0;s1;s2;s3|]] <- value

    member this.Set (s0 : seq<int>, s1 : seq<int>, s2 : seq<int>, s3 : seq<int>, value : 'T) = this.Set( [|s0;s1;s2;s3|], value)

    member this.Item
        with get(boolMatrix : __BoolMatrix) : Matrix<'T, 'S> = 
                validateSizesMatch _size boolMatrix.Size
                let matrixData = _genericOps.[_data, boolMatrix.Data]
                let n = getMatrixDataLength matrixData
                let size = [|n;1|]
                new Matrix<'T, 'S>(size, matrixData)
        and set (boolMatrix: __BoolMatrix) (value : 'T) = 
                validateSizesMatch _size boolMatrix.Size
                _genericOps.Set(_data, boolMatrix.Data, value)

    member this.Set (boolMatrix : __BoolMatrix, value : Matrix<'T, 'S>) = 
            validateSizesMatch _size boolMatrix.Size
            _genericOps.[_data, boolMatrix.Data] <- value.Data

    member this.Item
        with get(predicate : 'T -> bool) : Matrix<'T, 'S> = 
            let matrixData = _genericOps.[_data, predicate]
            let n = getMatrixDataLength matrixData
            let size = [|n;1|]
            new Matrix<'T, 'S>(size, matrixData)
        and set (predicate : 'T -> bool) (value : 'T) = 
            _genericOps.Set(_data, predicate, value)

    member this.Set (predicate : 'T -> bool, value : Matrix<'T, 'S>) = 
        _genericOps.[_data, predicate] <- value.Data

    override this.ToString() = 
        (this:>IFormattable).ToString(Matrix<'T,'S>.DisplayFormat, null)

    member this.Diag 
        with get(?k) =
            validate2D _size
            let k = defaultArg k 0
            let matrixData = (_data, _size, k) |> _genericOps.Diag
            let n = matrixData |> getMatrixDataLength
            let size = [|n;1|]
            new Matrix<'T, 'S>(size, matrixData)

    member this.ToColMajorSeq() = 
        _genericOps.ConvertToColMajorSeq(_data)

    member this.ApplyFun(f : 'T -> 'T) =
        _genericOps.ApplyFunInPlace(_data, f)

    static member diag(matrix: Matrix<'T, 'S>, offset : int) =
        validateVector matrix.Size
        let (m, s) = _genericOps.SetDiagonal(matrix.Data, offset)
        new Matrix<'T, 'S>(s, m)

    static member triL(matrix: Matrix<'T, 'S>, offset : int) =
        validate2D matrix.Size
        let rows = matrix.Size.[0]
        let cols = matrix.Size.[1]
        let m = _genericOps.TriLower(rows, cols, matrix.Data, offset)
        new Matrix<'T, 'S>(matrix.Size, m)

    static member triU(matrix: Matrix<'T, 'S>, offset : int) =
        validate2D matrix.Size
        let rows = matrix.Size.[0]
        let cols = matrix.Size.[1]
        let m = _genericOps.TriUpper(rows, cols, matrix.Data, offset)
        new Matrix<'T, 'S>(matrix.Size, m)

    static member concat(matrices: seq<Matrix<'T, 'S>>, dim : int) =
        let sizes = matrices |> Seq.map (fun x -> x.Size)
        validateCanConcat sizes dim
        let mats = matrices |> Seq.map (fun x -> (x.Data, x.Size))
        let (m, s) = _genericOps.Concat(mats, dim)
        new Matrix<'T, 'S>(s, m)

    static member horzConcat(matrices: seq<Matrix<'T, 'S>>) =
        Matrix<'T, 'S>.concat(matrices, 1)

    static member vertConcat(matrices: seq<Matrix<'T, 'S>>) =
        Matrix<'T, 'S>.concat(matrices, 0)

    static member repmat(matrix: Matrix<'T, 'S>, replicator : seq<int>) =
        validateMatrixSize replicator
        let size = matrix.Size
        let rep = replicator |> Seq.toArray
        let dims = if size.Length > rep.Length then size.Length else rep.Length
        let expandedSize = Array.create dims 1
        let expandedRepl = Array.create dims 1
        Array.Copy(size, expandedSize, size.Length)
        Array.Copy(rep, expandedRepl, rep.Length)
        let (m, s) = _genericOps.Repmat(expandedSize, matrix.Data, expandedRepl)
        new Matrix<'T, 'S>(s, m)

    static member reshape(matrix: Matrix<'T, 'S>, size : seq<int>) =
        validateMatrixSize size
        let newSize = size |> Seq.toArray
        validateLengthAndSizeMatch matrix.Length newSize
        let m = _genericOps.CloneMatrixData(matrix.Data)
        new Matrix<'T, 'S>(size, m)

    static member transpose(matrix: Matrix<'T, 'S>) =
        let size = matrix.Size
        validate2D size
        let m = _genericOps.Transpose(matrix.Data, matrix.Size)
        new Matrix<'T, 'S>([|size.[1];size.[0]|], m)

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        if (matrix.Length = 1) then matrix.[0]
        else raise (InvalidCastException())

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        if matrix.IsVector then
            _genericOps.ConvertToArray(matrix.Data)
        else raise (InvalidCastException())

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        if matrix.NDims = 2 then
            _genericOps.ConvertToArray2D(matrix.Data, matrix.Size.[0])
        else raise (InvalidCastException())

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        if matrix.NDims = 3 then
            _genericOps.ConvertToArray3D(matrix.Data, matrix.Size.[0], matrix.Size.[1])
        else raise (InvalidCastException())

    static member op_Explicit(matrix : Matrix<'T, 'S>) = 
        if matrix.NDims = 4 then
            _genericOps.ConvertToArray4D(matrix.Data, matrix.Size.[0], matrix.Size.[1], matrix.Size.[2])
        else raise (InvalidCastException())

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

    static member Identity(rows : int, cols : int) =
        let size = [|rows;cols|]
        validateMatrixSize size
        validate2D size
        let m = _numericOps.Identity(rows, cols)
        new Matrix<'T, 'S>(size, m)

    static member zeros(size : seq<int>) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.Zeros(s)
        new Matrix<'T, 'S>(size, m)

    static member ones(size : seq<int>) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.Ones(s)
        new Matrix<'T, 'S>(size, m)

    static member applyFun(matrix : Matrix<'T, 'S>, f) =
        let matrixData = _genericOps.ApplyFun(matrix.Data, f)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member applyFun2Arg(matrix1 : Matrix<'T, 'S>, matrix2 : Matrix<'T, 'S>, f) =
        validateSizesMatch matrix1.Size matrix2.Size
        let matrixData = _genericOps.ApplyFun(matrix1.Data, matrix2.Data, f)
        new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member applyFun3Arg(matrix1 : Matrix<'T, 'S>, matrix2 : Matrix<'T, 'S>, matrix3 : Matrix<'T, 'S>, f) =
        validateSizesMatch matrix1.Size matrix2.Size
        validateSizesMatch matrix2.Size matrix3.Size
        let matrixData = _genericOps.ApplyFun(matrix1.Data, matrix2.Data, matrix3.Data, f)
        new Matrix<'T, 'S>(matrix1.Size, matrixData)

//***************************************************************************************************************************************
//********************COMPARABLE*********************************************************************************************************
//***************************************************************************************************************************************

    static member (==) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        _compOps.AreEqual(matrix1.Data, matrix2.Data)

    static member (==) (matrix: Matrix<'T, 'S>, a : 'T) =
       _compOps.AllEqual(matrix.Data, a)

    static member (==) (a : 'T, matrix: Matrix<'T, 'S>) =
        _compOps.AllEqual(matrix.Data, a)

    static member (!=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        not (matrix1 == matrix2)

    static member (!=) (matrix: Matrix<'T, 'S>, a : 'T) =
        not (_compOps.AllEqual(matrix.Data, a))

    static member (!=) (a : 'T, matrix: Matrix<'T, 'S>) =
        not (_compOps.AllEqual(matrix.Data, a))

    static member (&=) (matrix: Matrix<'T, 'S>, a : 'T) =
        _compOps.AllEqual(matrix.Data, a)

    static member (&!=) (matrix: Matrix<'T, 'S>, a : 'T) =
        _compOps.AllNotEqual(matrix.Data, a)

    static member (&<>) (matrix: Matrix<'T, 'S>, a : 'T) =
        _compOps.AllNotEqual(matrix.Data, a)

    static member (&<) (matrix : Matrix<'T, 'S>, a : 'T) =
        _compOps.AllLessThan(matrix.Data, a)

    static member (&<=) (matrix : Matrix<'T, 'S>, a : 'T) =
        _compOps.AllLessThanEqual(matrix.Data, a)

    static member (&>) (matrix : Matrix<'T, 'S>, a : 'T) =
        _compOps.AllGreaterThan(matrix.Data, a)

    static member (&>=) (matrix : Matrix<'T, 'S>, a : 'T) =
        _compOps.AllGreaterThanEqual(matrix.Data, a)

    static member (&<) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllGreaterThan(matrix.Data, a)

    static member (&<=) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllGreaterThanEqual(matrix.Data, a)

    static member (&>) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllLessThan(matrix.Data, a)

    static member (&>=) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllLessThanEqual(matrix.Data, a)

    static member (&=) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllEqual(matrix.Data, a)

    static member (&!=) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllNotEqual(matrix.Data, a)

    static member (&<>) (a : 'T, matrix : Matrix<'T, 'S>) =
        _compOps.AllNotEqual(matrix.Data, a)

    static member (.<) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.LessThan(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<=) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.LessThanEqual(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.>) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.GreaterThan(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.>=) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.GreaterThanEqual(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.==) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.EqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.=) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.EqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.!=) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.NotEqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<>) (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _compOps.NotEqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.GreaterThan(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<=) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.GreaterThanEqual(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.>) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.LessThan(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.>=) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.LessThanEqual(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.==) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.EqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.=) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.EqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.!=) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.NotEqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<>) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.NotEqualElementwise(matrix.Data, a)
        __BoolMatrix(matrix.Size, matrixData)

    static member (.<) ( matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.GreaterThan(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.LessThan(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.LessThan(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.<=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.GreaterThanEqual(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.LessThanEqual(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.LessThanEqual(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.>) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.LessThan(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.GreaterThan(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.GreaterThan(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.>=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.LessThanEqual(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.GreaterThanEqual(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.GreaterThanEqual(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.==) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.EqualElementwise(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.EqualElementwise(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.EqualElementwise(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.EqualElementwise(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.EqualElementwise(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.EqualElementwise(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.!=) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.NotEqualElementwise(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.NotEqualElementwise(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.NotEqualElementwise(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member (.<>) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.NotEqualElementwise(matrix2.Data, a)
            __BoolMatrix(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.NotEqualElementwise(matrix1.Data, a)
            __BoolMatrix(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.NotEqualElementwise(matrix1.Data, matrix2.Data)
            __BoolMatrix(matrix1.Size, matrixData)

    static member minXY(matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.MinXa(matrix2.Data, a)
            new Matrix<'T,'S>(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.MinXa(matrix1.Data, a)
            new Matrix<'T,'S>(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.MinXY(matrix1.Data, matrix2.Data)
            new Matrix<'T,'S>(matrix1.Size, matrixData)

    static member minXY(matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _compOps.MinXa(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member minXY(a :'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.MinXa(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member maxXY(matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _compOps.MaxXa(matrix2.Data, a)
            new Matrix<'T,'S>(matrix2.Size, matrixData)
        elif matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _compOps.MaxXa(matrix1.Data, a)
            new Matrix<'T,'S>(matrix1.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _compOps.MaxXY(matrix1.Data, matrix2.Data)
            new Matrix<'T,'S>(matrix1.Size, matrixData)

    static member maxXY(matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _compOps.MaxXa(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member maxXY(a :'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _compOps.MaxXa(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member min(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _compOps.Min(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member max(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _compOps.Max(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

//***************************************************************************************************************************************
//********************BOOLEAN*********************************************************************************************************
//***************************************************************************************************************************************
  
    static member (.&&) (matrix1: Matrix<'T,'S>, matrix2: Matrix<'T,'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _boolOps.And(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _boolOps.And(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _boolOps.And(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (.&&) (matrix: Matrix<'T,'S>, a : 'T) =
        let matrixData = _boolOps.And(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member (.&&) (a : 'T, matrix: Matrix<'T,'S>) =
        let matrixData = _boolOps.And(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member (.||) (matrix1: Matrix<'T,'S>, matrix2: Matrix<'T,'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _boolOps.Or(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _boolOps.Or(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _boolOps.Or(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (.||) (matrix: Matrix<'T,'S>, a : 'T) =
        let matrixData = _boolOps.Or(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member (.||) (a : 'T, matrix: Matrix<'T,'S>) =
        let matrixData = _boolOps.Or(matrix.Data, a)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member (~~) (matrix: Matrix<'T,'S>) =
        let matrixData = _boolOps.Not(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member not (matrix: Matrix<'T,'S>) =
        let matrixData = _boolOps.Not(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

//***************************************************************************************************************************************
//********************NUMERIC OPERATORS*********************************************************************************************************
//***************************************************************************************************************************************

    static member (*) (a: 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Mul(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (*) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Mul(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (*) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Mul(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Mul(matrix1.Data, matrix2.[0]))
        else
            validate2D matrix1.Size
            validate2D matrix2.Size
            validateCanMultiply matrix1.Size matrix2.Size
            let matrixData = _numericOps.MulMatrix(matrix1.Data, matrix1.Size, matrix2.Data, matrix2.Size)
            let size = [|matrix1.Size.[0];matrix2.Size.[1]|]
            new Matrix<'T, 'S>(size, matrixData)

    static member (.*) (a: 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Mul(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.*) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Mul(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.*) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Mul(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Mul(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Mul(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (.+) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Add(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Add(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Add(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (.+) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Add(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.+) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Add(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (+) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Add(matrix2.Data, matrix1.[0]))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Add(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Add(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (+) (a : 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Add(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (+) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Add(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (-) (a: 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Sub(a, matrix.Data)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (-) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Sub(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (-) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Sub(matrix1.[0], matrix2.Data))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Sub(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Sub(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (.-) (a: 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Sub(a, matrix.Data)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.-) (matrix: Matrix<'T, 'S>, a : 'T) =
        let matrixData = _numericOps.Sub(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.-) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Sub(matrix1.[0], matrix2.Data))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Sub(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Sub(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

    static member (~-) (matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Minus(matrix.Data)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (./) (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix1.Length = 1 then
            new Matrix<'T, 'S>(matrix2.Size, _numericOps.Div(matrix1.[0], matrix2.Data))
        else if matrix2.Length = 1 then
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Div(matrix1.Data, matrix2.[0]))
        else
            validateSizesMatch matrix1.Size matrix2.Size
            new Matrix<'T, 'S>(matrix1.Size, _numericOps.Div(matrix1.Data, matrix2.Data))

    static member (/) (matrix : Matrix<'T, 'S>, a : 'T) =
        new Matrix<'T, 'S>(matrix.Size, _numericOps.Div(matrix.Data, a))

    static member (./) (matrix : Matrix<'T, 'S>, a : 'T) =
        new Matrix<'T, 'S>(matrix.Size, _numericOps.Div(matrix.Data, a))

    static member (/) ( a : 'T, matrix : Matrix<'T, 'S>) =
        new Matrix<'T, 'S>(matrix.Size, _numericOps.Div(a, matrix.Data))

    static member (./) ( a : 'T, matrix : Matrix<'T, 'S>) =
        new Matrix<'T, 'S>(matrix.Size, _numericOps.Div(a, matrix.Data))

    static member Pow (matrix: Matrix<'T, 'S>, a: 'T) =
        let matrixData = _numericOps.Pow(matrix.Data, a)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member (.^) (a: 'T, matrix: Matrix<'T, 'S>) =
        let matrixData = _numericOps.Pow(a, matrix.Data)
        new Matrix<'T, 'S>(matrix.Size, matrixData)

    static member Pow (matrix1: Matrix<'T, 'S>, matrix2: Matrix<'T, 'S>) =
        if matrix2.Length = 1 then
            let a = matrix2.[0]
            let matrixData = _numericOps.Pow(matrix1.Data, a)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)
        elif matrix1.Length = 1 then
            let a = matrix1.[0]
            let matrixData = _numericOps.Pow(a, matrix2.Data)
            new Matrix<'T, 'S>(matrix2.Size, matrixData)
        else
            validateSizesMatch matrix1.Size matrix2.Size
            let matrixData = _numericOps.Pow(matrix1.Data, matrix2.Data)
            new Matrix<'T, 'S>(matrix1.Size, matrixData)

//***************************************************************************************************************************************
//********************NUMERIC VECTOR FUNCTIONS*********************************************************************************************************
//***************************************************************************************************************************************

    static member Abs(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Abs(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Sqrt(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Sqrt(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Sin(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Sin(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Cos(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Cos(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Tan(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Tan(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Asin(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.ASin(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Acos(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.ACos(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Atan(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.ATan(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Sinh(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Sinh(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Cosh(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Cosh(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Tanh(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Tanh(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Exp(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Exp(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Log(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Log(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Log10(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Log10(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Erf(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Erf(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Erfc(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Erfc(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Erfinv(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Erfinv(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Erfcinv(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Erfcinv(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Normcdf(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Normcdf(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Norminv(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Norminv(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Round(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Round(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

    static member Ceiling(matrix : Matrix<'T,'S>) =
        let matrixData = _numericOps.Ceil(matrix.Data)
        new Matrix<'T,'S>(matrix.Size, matrixData)

//***************************************************************************************************************************************
//********************NUMERIC RANDOM*********************************************************************************************************
//***************************************************************************************************************************************

    static member unifRnd(a, b, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.UnifRnd(a, b, s)
        new Matrix<'T, 'S>(s, m)

    static member normalRnd(mean, sigma, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.NormalRnd(mean, sigma, s)
        new Matrix<'T, 'S>(s, m)

    static member lognormRnd(mean, sigma, a, scale, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.LognormalRnd(mean, sigma, a, scale, s)
        new Matrix<'T, 'S>(s, m)

    static member bernRnd(p, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.BernRnd(p, s)
        new Matrix<'T, 'S>(s, m)

    static member binomRnd(n, p, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.BinomRnd(n, p, s)
        new Matrix<'T, 'S>(s, m)

    static member poissRnd(lambda, size) =
        validateMatrixSize size
        let s = size |> Seq.toArray
        let m = _numericOps.PoissonRnd(lambda, s)
        new Matrix<'T, 'S>(s, m)

    static member mvNormRnd(mu : Matrix<'T,'S>, cov : Matrix<'T,'S>, n) =
        validateSquare2D cov.Size
        validateVector mu.Size
        let k = mu.Length
        if k <> cov.Size.[0] then raise (new ArgumentException("Covariance matrix must be kxk, k = mu.Length"))
        let m = _numericOps.MVnormalRnd(mu.Data, cov.Data, k, n)
        new Matrix<'T, 'S>([n;k], m)
 
//***************************************************************************************************************************************
//********************NUMERIC LINEAR ALGEBRA*********************************************************************************************************
//*************************************************************************************************************************************** 

    static member chol(matrix: Matrix<'T, 'S>) =
        validateSquare2D matrix.Size
        let n = matrix.Size.[0]
        if n = 0 then
            Matrix<'T, 'S>.Empty
        else
            let m = _numericOps.CholeskyDecomp(matrix.Data, n)
            new Matrix<'T, 'S>([|n;n|], m)

    static member cholSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) =
        validateSquare2D a.Size
        validateLinSolveSize a.Size b.Size 
        let n = a.Size.[0]
        let nrhs = b.Size.[1]
        let m = _numericOps.CholeskySolve(a.Data, n, b.Data, nrhs)
        new Matrix<'T, 'S>([|n;nrhs|], m)

    static member lu(matrix: Matrix<'T, 'S>) =
        validateSquare2D matrix.Size
        let m = matrix.Size.[0]
        let n = matrix.Size.[1]
        if matrix.Length = 0 then
            let p = Array.create 0 0
            (Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty, p)
        else
            let (l, sL, u, sU,  p) = _numericOps.LuDecomp(matrix.Data, m, n)
            (new Matrix<'T, 'S>(sL, l), new Matrix<'T, 'S>(sU, u), p) 

    static member luSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) =
        validateSquare2D a.Size
        validateLinSolveSize a.Size b.Size
        let n = a.Size.[0]
        let nrhs = b.Size.[1]
        let m = _numericOps.LuSolve(a.Data, n, b.Data, nrhs)
        new Matrix<'T, 'S>([|n;nrhs|], m)

    static member qr(matrix: Matrix<'T, 'S>) =
        validate2D matrix.Size
        let m = matrix.Size.[0]
        let n = matrix.Size.[1]
        if n > m then raise (new ArgumentException("Qr requires rows >= cols"))
        if matrix.Length = 0 then
            (Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty)
        else
            let (q, sQ, r, sR) = _numericOps.QrDecomp(matrix.Data, m, n)
            (new Matrix<'T, 'S>(sQ, q), new Matrix<'T, 'S>(sR, r)) 

    static member qrSolveFull(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>) =
        validateLinSolveSize a.Size b.Size
        let m = a.Size.[0]
        let n = a.Size.[1]
        if n > m then raise (new ArgumentException("Qr solver requires rows >= cols"))
        let nrhs = b.Size.[1]
        let res = _numericOps.QrSolveFull(a.Data, m, n, b.Data, nrhs)
        new Matrix<'T, 'S>([|n;nrhs|], res)

    static member svdSolve(a: Matrix<'T, 'S>, b: Matrix<'T, 'S>, tol : 'T) =
        validateLinSolveSize a.Size b.Size
        let m = a.Size.[0]
        let n = a.Size.[1]
        if n > m then raise (new ArgumentException("Svd solver requires rows >= cols"))
        let nrhs = b.Size.[1]
        let (res, rank) = _numericOps.SvdSolve(a.Data, m, n, b.Data, nrhs, tol)
        (new Matrix<'T, 'S>([|n;nrhs|], res), rank)

    static member svd(matrix: Matrix<'T, 'S>) =
        validate2D matrix.Size
        let m = matrix.Size.[0]
        let n = matrix.Size.[1]
        if n > m then raise (new ArgumentException("Svd requires rows >= cols"))
        if matrix.Length = 0 then
            (Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty, Matrix<'T, 'S>.Empty)
        else
            let (U, sU, S, sS, Vt, sVt) = _numericOps.SvdDecomp(matrix.Data, m, n)
            (new Matrix<'T, 'S>(sU, U), new Matrix<'T, 'S>(sS, S), new Matrix<'T, 'S>(sVt, Vt)) 

//***************************************************************************************************************************************
//********************NUMERIC BASIC STATS*********************************************************************************************************
//*************************************************************************************************************************************** 

    static member sum(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Sum(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member cumsum(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let m = _numericOps.CumSum(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(matrix.Size, m)

    static member prod(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Prod(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member cumprod(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let m = _numericOps.CumProd(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(matrix.Size, m)

    static member mean(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Mean(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member var(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Variance(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member skewness(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Skewness(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member kurtosis(matrix: Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        let (m, s) = _numericOps.Kurtosis(matrix.Data, matrix.Size, dim)
        new Matrix<'T, 'S>(s, m)

    static member quantile(matrix: Matrix<'T, 'S>, q : Matrix<'T, 'S>, dim) =
        validateDimAgainstSize matrix.Size dim
        validateVector q.Size
        let (m, s) = _numericOps.Quantiles(matrix.Data, matrix.Size, q.Data, dim)
        new Matrix<'T, 'S>(s, m)

    static member corr(matrix: Matrix<'T, 'S>) =
        validate2D matrix.Size
        let n = matrix.Size.[0]
        let d = matrix.Size.[1]
        let m = _numericOps.Correlation(matrix.Data, n, d)
        new Matrix<'T, 'S>([|d;d|], m)

    static member cov(matrix: Matrix<'T, 'S>) =
        validate2D matrix.Size
        let n = matrix.Size.[0]
        let d = matrix.Size.[1]
        let m = _numericOps.Covariance(matrix.Data, n, d)
        new Matrix<'T, 'S>([|d;d|], m)