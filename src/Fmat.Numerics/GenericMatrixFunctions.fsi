namespace Fmat.Numerics

///<summary>This module contains generic functions for matrix manipulation
///</summary>
  module GenericMatrixFunctions = begin

    ///<summary>Creates matrix with diagonal elements set to generic 1 and generic 0 otherwise.
    ///</summary>
    ///<param name="n">Number of rows</param>
    ///<param name="m">Number of columns</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Diagonal matrix.</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = I(2,3) : Matrix
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when n or m &lt; 0.</exception>
    val I :
      n:int * m:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Creates a matrix of given size and sets all elements to generic zero
    ///</summary>
    ///<param name="size">Size of matrix</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Matrix with all elements equal zero</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = zeros [2;3;4] : Matrix
    ///</code>
    ///</example>
    val zeros :
      size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Creates a matrix of given size and sets all elements to generic one
    ///</summary>
    ///<param name="size">Size of matrix</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Matrix with all elements equal zero</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = ones [2;3;4] : Matrix
    ///</code>
    ///</example>
    val ones :
      size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Creates diagonal matrix based on given vector (matrix 1xN or Nx1)
    ///</summary>
    ///<param name="vector">Values to store in diagonal</param>
    ///<param name="offset">Offset. Positive to store values above main digonal</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Matrix with k-th diagonal set to given vector</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let v = !![[1.;2.;3.]]
    ///let x = diag(x, 1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when offset too big.</exception>
    ///<exception cref="T:System.RankException">Thrown when not vector.</exception>
    val diag :
      vector:Matrix<'T,'S> * offset:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Extracts lower triangular matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="k">Offset. Specifies which diagonal should be included</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Lower triangular matrix up to k-th diagonal</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [3;4] : Matrix
    ///let y = triL(x, 1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when not 2D matrix.</exception>
    val triL :
      matrix:Matrix<'T,'S> * k:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Extracts upper triangular matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="k">Offset. Specifies which diagonal should be included</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Upper triangular matrix down to k-th diagonal</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [3;4] : Matrix
    ///let y = triU(x, -1)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when not 2D matrix.</exception>
    val triU :
      matrix:Matrix<'T,'S> * k:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Concatenates matrices along given dimension
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<param name="dimension">Dimension</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x1 = rand [3;2;5] : Matrix
    ///let x2 = rand [3;3;5]
    ///let x3 = rand [3;4;5]
    ///let y = concat([x1;x2;x3], 1) // returns matrix 3x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val concat :
      matrices:seq<Matrix<'T,'S>> * dimension:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Concatenates matrices along dimension 1
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x1 = rand [3;2;5] : Matrix
    ///let x2 = rand [3;3;5]
    ///let x3 = rand [3;4;5]
    ///let y = horzConcat([x1;x2;x3]) // returns matrix 3x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val horzConcat :
      matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Concatenates matrices along dimension 0
    ///</summary>
    ///<param name="matrices">Sequence of matrices</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Concatenated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x1 = rand [2;2;5 : Matrix
    ///let x2 = rand [3;2;5]
    ///let x3 = rand [4;2;5]
    ///let y = vertConcat([x1;x2;x3]) // returns matrix 9x2x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when not all dimensions are equal except specified dimension.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val vertConcat :
      matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Replicates matrix in each dimension
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="replicator">Array of replicators, one for each dimension</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Replicated matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [2;3;5] : Matrix
    ///let y = repmat(x, [2;3;1] // returns matrix 4x9x5
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when replicator has less than 2 elements or negative element.</exception>
    val repmat :
      matrix:Matrix<'T,'S> * replicator:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Reshapes matrix. Number of elements must not change.
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<param name="size">New size</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Reshaped matrix. Input matrix is not changed</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [3;4] : Matrix
    ///let y = reshape(x, [3;2;2]) // returns matrix 3x2x2
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when new size is invalid or length is different.</exception>
    val reshape :
      matrix:Matrix<'T,'S> * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Transposes matrix
    ///</summary>
    ///<param name="matrix">Input matrix</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Transposed matrix. Input matrix does not change</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [3;4] : Matrix
    ///let y = transpose(x) // returns matrix 4x3
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    val transpose :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise minimum of 2 arguments. Each argument can be scalar of type 'T or Matrix with elements of type 'T
    ///</summary>
    ///<param name="x">First argument</param>
    ///<param name="y">Second argument</param>
    ///<typeparam name="a">Type of first argument</typeparam>
    ///<typeparam name="b">Type of second argument</typeparam>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise minimum of x and y</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [2;3;4]
    ///let y = rand [2;3;4]
    ///let z = minXY(x, y)
    ///</code>
    ///</example>
    val inline minXY :
      x: ^a * y: ^b -> Matrix<'T,'S>
        when  ^a : (static member ( !! ) :  ^a -> Matrix<'T,'S>) and
             'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T> and
              ^b : (static member ( !! ) :  ^b -> Matrix<'T,'S>)
    ///<summary>Calculates elementwise maximum of 2 arguments. Each argument can be scalar of type 'T or Matrix with elements of type 'T
    ///</summary>
    ///<param name="x">First argument</param>
    ///<param name="y">Second argument</param>
    ///<typeparam name="a">Type of first argument</typeparam>
    ///<typeparam name="b">Type of second argument</typeparam>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise maximum of x and y</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [2;3;4] : Matrix
    ///let y = rand [2;3;4] : Matrix
    ///let z = maxXY(x, y)
    ///</code>
    ///</example>
    val inline maxXY :
      x: ^a * y: ^b -> Matrix<'T,'S>
        when  ^a : (static member ( !! ) :  ^a -> Matrix<'T,'S>) and
             'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T> and
              ^b : (static member ( !! ) :  ^b -> Matrix<'T,'S>)
    ///<summary>Applies given function elementwise to a matrix. New matrix is returned. Input matrix is not modified.
    ///</summary>
    ///<param name="x">Input matrix</param>
    ///<param name="f">Elementwise function</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>New matrix as elementwise transformation of input matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x = rand [3;4] : Matrix
    ///let y = applyFun(x, fun x -&gt; x + 1.0)
    ///</code>
    ///</example>
    val applyFun :
      x:Matrix<'T,'S> * f:('T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Applies given function elementwise to 2 matrices. New matrix is returned. Input matrices are not modified and must have the same size.
    ///</summary>
    ///<param name="x">First matrix</param>
    ///<param name="y">Second matrix</param>
    ///<param name="f">Elementwise function of 2 args</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>New matrix as elementwise transformation of 2 input matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x1 = rand [3;4] : Matrix
    ///let x2 = rand [3;4]
    ///let y = applyFun2Arg(x1, x2, fun x1 x2 -&gt; x1 + x2)
    ///</code>
    ///</example>
    val applyFun2Arg :
      x:Matrix<'T,'S> * y:Matrix<'T,'S> * f:('T -> 'T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Applies given function elementwise to 3 matrices. New matrix is returned. Input matrices are not modified and must have the same size.
    ///</summary>
    ///<param name="x">First matrix</param>
    ///<param name="y">Second matrix</param>
    ///<param name="z">Third matrix</param>
    ///<param name="f">Elementwise function of 3 args</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>New matrix as elementwise transformation of 3 input matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericMatrixFunctions
    ///let x1 = rand [3;4] : Matrix
    ///let x2 = rand [3;4]
    ///let x3 = rand [3;4]
    ///let y = applyFun3Arg(x1, x2, x3, fun x1 x2 x3 -&gt; x1 + x2 + x3)
    ///</code>
    ///</example>
    val applyFun3Arg :
      x:Matrix<'T,'S> * y:Matrix<'T,'S> * z:Matrix<'T,'S> *
      f:('T -> 'T -> 'T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise error function
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = erf(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val erf :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise complementary error function
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise complementary error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = erfc(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val erfc :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise inverse error function
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise inverse error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = erfinv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val erfinv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise inverse complementary error function
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise inverse complementary error function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = erfcinv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val erfcinv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise standard normal cumulative distribution
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise standard normal cumulative distribution of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = normcdf(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val normcdf :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Calculates elementwise inverse standard normal cumulative distribution
    ///</summary>
    ///<param name="x">Matrix argument</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Elementwise inverse standard normal cumulative distribution function of matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///let x = rand [2;3;4] 
    ///let y = norminv(x) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    val norminv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    end
