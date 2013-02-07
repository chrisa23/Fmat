namespace Fmat.Numerics

module LinearAlgebra32 =
    open System

    let chol(matrix) = Matrix32.chol(matrix)

    let cholSolve(a, b) = Matrix32.cholSolve(a,b)

    let lu(matrix) = Matrix32.lu(matrix)

    let luSolve(a, b) = Matrix32.luSolve(a,b)

    let qr(matrix) = Matrix32.qr(matrix)

    let qrSolveFull(a, b) = Matrix32.qrSolveFull(a,b)

    let svdSolve(a, b, tol) = Matrix32.svdSolve(a,b,tol)

    let svd(matrix) = Matrix32.svd(matrix)

