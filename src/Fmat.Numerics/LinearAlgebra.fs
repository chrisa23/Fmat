namespace Fmat.Numerics

module LinearAlgebra =
    open System

    let chol(matrix) = Matrix.chol(matrix)

    let cholSolve(a, b) = Matrix.cholSolve(a,b)

    let lu(matrix) = Matrix.lu(matrix)

    let luSolve(a, b) = Matrix.luSolve(a,b)

    let qr(matrix) = Matrix.qr(matrix)

    let qrSolveFull(a, b) = Matrix.qrSolveFull(a,b)

    let svdSolve(a, b, tol) = Matrix.svdSolve(a,b,tol)

    let svd(matrix) = Matrix.svd(matrix)


