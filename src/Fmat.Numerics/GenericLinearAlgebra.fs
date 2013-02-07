namespace Fmat.Numerics

module GenericLinearAlgebra =
    open System

    let chol(matrix) = Matrix<'T,'S>.chol(matrix)

    let cholSolve(a, b) = Matrix<'T,'S>.cholSolve(a,b)

    let lu(matrix) = Matrix<'T,'S>.lu(matrix)

    let luSolve(a, b) = Matrix<'T,'S>.luSolve(a,b)

    let qr(matrix) = Matrix<'T,'S>.qr(matrix)

    let qrSolveFull(a, b) = Matrix<'T,'S>.qrSolveFull(a,b)

    let svdSolve(a, b, tol) = Matrix<'T,'S>.svdSolve(a,b,tol)

    let svd(matrix) = Matrix<'T,'S>.svd(matrix)


