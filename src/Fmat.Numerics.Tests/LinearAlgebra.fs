namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.Conversion
open Fmat.Numerics.LinearAlgebra
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module LinearAlgebraTests =
    
    let inline (!!) x = !!x : Matrix

    [<Test>]
    let ``Should Cholesky Decompose``() =
        let m = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let upperChol = chol(m)
        let chk = upperChol.T * upperChol
        nearlyEqual chk m 1e-15 |> should be True

    [<Test>]
    let ``Should Cholesky Solve``() =
        let a = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let b = Matrix([3;2], [1.0..6.0])
        let x = cholSolve(a, b)
        let chk = a * x
        nearlyEqual chk b 1e-15 |> should be True

    [<Test>]
    let ``Should LU Decompose``() =
        let a = !![[4.;2.;-1.;3.] 
                   [3.;-4.;2.;5.]
                   [-2.;6.;-5.;-2.]
                   [5.;1.;6.;-3.]]
        let (l, u, p) = lu(a)
        p |> should equal [|3;2;0;1|]
        let a = !![[0.;1.]
                   [1.;0.]]
        let (l, u, p) = lu(a)
        p |> should equal [|1;0|]
        let a = Matrix([3;3], [8.0;3.0;4.0;1.0;5.0;9.0;6.0;7.0;2.])
        let (l, u, p) = lu(a)
        p |> should equal [|0;2;1|]
        let chk = l * u
        let m = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let (l, u, p) = lu(m)
        p |> should equal [|0;1;2|]
        let chk = l * u
        nearlyEqual chk m 1e-15 |> should be True

    [<Test>]
    let ``Should LU Solve``() =
        let a = !![[4.;2.;-1.;3.] 
                   [3.;-4.;2.;5.]
                   [-2.;6.;-5.;-2.]
                   [5.;1.;6.;-3.]]
        let b = Matrix([4;2], [1.0..8.0])
        let x = luSolve(a, b)
        let chk = a * x
        nearlyEqual chk b 1e-14 |> should be True

    [<Test>]
    let ``Should QR Decompose``() =
        let m = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let (q, r) = qr(m)
        let chk = q * r
        nearlyEqual chk m 1e-15 |> should be True

    [<Test>]
    let ``Should QR Solve Full``() =
        let a = Matrix([4;3], [2.0..13.0])
        let b = Matrix([4;2], [1.0..8.0])
        let x = qrSolveFull(a, b)
        let chk = a * x
        nearlyEqual chk b 1e-15 |> should be True

    [<Test>]
    let ``Should Svd Solve``() =
        let a = Matrix([4;3], [2.0..13.0])
        let b = Matrix([4;2], [1.0..8.0])
        let (x, rank) = svdSolve(a, b, 1e-20)
        let chk = a * x
        nearlyEqual chk b 1e-15 |> should be True

    [<Test>]
    let ``Should Svd Decompose``() =
        let a = Matrix([3;2], [9.0; 6.0; 2.0; 4.0; 8.0; 7.0])
        let (u, s, vt) = svd(a)
        let chk = u * (diag(s, 0)) * vt
        nearlyEqual chk a 1e-15 |> should be True

