namespace Fmat.Numerics
///<summary>This module contains generic functions for matrix linear algebra
///</summary>
  module GenericLinearAlgebra = begin
    ///<summary>Performs cholesky factorization
    ///</summary>
    ///<param name="matrix">Input matrix. Must be positive definite.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Upper triangular matrix calculated in factorization</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let x = Matrix([ [1.0;0.5]
    ///                 [0.5;1.0] ]
    ///let y = chol(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix not symmetrical.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when matrix not positive definite.</exception>
    val chol :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Solves linear equation using chol factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must be positive definite.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let a = Matrix([ [1.0;0.5]
    ///                 [0.5;1.0] ]
    ///let b = rand [2;1]
    ///let x = cholSolve(a, b) // ax=b
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not square or A and B have non compatible dimensions.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix not symmetrical.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when matrix A not positive definite.</exception>
    val cholSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Performs LU factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>(L, U, P) Lower/Upper matrices, P is a vector with row permutations</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let x = rand [20;10] : Matrix
    ///let (l, u, p) = lu(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    val lu :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S> * int []
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Solves linear equation using LU factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let a = rand [2;2] : Matrix
    ///let b = rand [2;1]
    ///let x = luSolve(a, b) // ax=b
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D square or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found.</exception>
    val luSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Performs QR factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>(Q, R) matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let x = rand [20;10] : Matrix
    ///let (q, r) = qr(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when factorization failed.</exception>
    val qr :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Finds least squares solution of linear equation using QR factorization assuming full rank
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must have full rank</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Least squares solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let a = rand [2;2] : Matrix
    ///let b = rand [2;1]
    ///let x = qrSolveFull(a, b)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Matrix does not have full rank</exception>
    val qrSolveFull :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Finds least squares solution of linear equation using singular value factorization with given tolerance
    ///</summary>
    ///<param name="a">A in equation Ax=b.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<param name="tol">Tolerance to determine rank of A.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>Least squares solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let a = rand [2;2] : Matrix
    ///let b = rand [2;1]
    ///let x = svdSolve(a, b, 1e-10)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Try different tolerance.</exception>
    val svdSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> * tol:'T -> Matrix<'T,'S> * int
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    ///<summary>Performs singular value factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<typeparam name="T">Type of matrix elements</typeparam>
    ///<typeparam name="S">Type of matrix operations interface</typeparam>
    ///<returns>(U, S, Vt) matrices, S is vector of singular values, Vt is transposed V </returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.GenericBasicStat
    ///open Fmat.Numerics.GenericLinearAlgebra
    ///let x = rand [20;10] : Matrix
    ///let (u, s, vt) = svd(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when algorithm did not converge.</exception>
    val svd :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S> * Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    end