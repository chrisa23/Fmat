namespace Fmat.Numerics
///<summary>This module contains functions for double precision matrix linear algebra
///</summary>
  module LinearAlgebra = begin
    ///<summary>Performs cholesky factorization
    ///</summary>
    ///<param name="matrix">Input matrix. Must be positive definite.</param>
    ///<returns>Upper triangular matrix calculated in factorization</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.LinearAlgebra
    ///let x = Matrix([ [1.0;0.5]
    ///                 [0.5;1.0] ]
    ///let y = chol(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix not symmetrical.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when matrix not positive definite.</exception>
    val chol :
      matrix:Matrix -> Matrix
    ///<summary>Solves linear equation using chol factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must be positive definite.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
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
      a:Matrix * b:Matrix -> Matrix
    ///<summary>Performs LU factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(L, U, P) Lower/Upper matrices, P is a vector with row permutations</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
    ///let x = rand [20;10]
    ///let (l, u, p) = lu(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    val lu :
      matrix:Matrix -> Matrix * Matrix * int []
    ///<summary>Solves linear equation using LU factorization
    ///</summary>
    ///<param name="a">A in equation Ax=b.</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = luSolve(a, b) // ax=b
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D square or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found.</exception>
    val luSolve :
      a:Matrix * b:Matrix -> Matrix
    ///<summary>Performs QR factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(Q, R) matrices</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
    ///let x = rand [20;10]
    ///let (q, r) = qr(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when factorization failed.</exception>
    val qr :
      matrix:Matrix -> Matrix * Matrix
    ///<summary>Finds least squares solution of linear equation using QR factorization assuming full rank
    ///</summary>
    ///<param name="a">A in equation Ax=b. Must have full rank</param>
    ///<param name="b">b in equation Ax=b.</param>
    ///<returns>Least squares solution of Ax=b</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = qrSolveFull(a, b)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Matrix does not have full rank</exception>
    val qrSolveFull :
      a:Matrix * b:Matrix -> Matrix
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
    ///open Fmat.Numerics.LinearAlgebra
    ///let a = rand [2;2]
    ///let b = rand [2;1]
    ///let x = svdSolve(a, b, 1e-10)
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when matrix A not 2D or B not equal number of rows as A.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when solution cannot be found. Try different tolerance.</exception>
    val svdSolve :
      a:Matrix * b:Matrix * tol:float -> Matrix * int
    ///<summary>Performs singular value factorization
    ///</summary>
    ///<param name="matrix">Input matrix.</param>
    ///<returns>(U, S, Vt) matrices, S is vector of singular values, Vt is transposed V </returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///open Fmat.Numerics.LinearAlgebra
    ///let x = rand [20;10]
    ///let (u, s, vt) = svd(x)
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2D.</exception>
    ///<exception cref="T:System.InvalidOperationException">Thrown when algorithm did not converge.</exception>
    val svd :
      matrix:Matrix -> Matrix * Matrix * Matrix
   end