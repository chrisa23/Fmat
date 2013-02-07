namespace Fmat.Numerics
open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.LinearAlgebra
open FsUnit
open NUnit.Framework

[<TestFixture>]
module LinearAlgebraExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when Cholesky decompose non square``() =
        let m = Matrix([3;4], [1.0..12.])
        let upperChol = chol(m)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when Cholesky solve non square``() =
        let a = Matrix([3;4], [1.0..12.])
        let b = Matrix([3;2], [1.0..6.0])
        let x = cholSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when Cholesky solve B not 2D`` () =
        let a = Matrix([3;3], [1.0..9.])
        let b = Matrix([3;2;2], [1.0..12.0])
        let x = cholSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when Cholesky solve A and B not compatible``() =
        let a = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let b = Matrix([2;2], [1.0..4.0])
        let x = cholSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when lu decompose non 2D``() =
        let m = Matrix([3;4;2], [1.0..24.])
        let (l, u, p) = lu(m)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when lu solve non square``() =
        let a = Matrix([3;4], [1.0..12.])
        let b = Matrix([3;2], [1.0..6.0])
        let x = luSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when lu solve B not 2D``() =
        let a = Matrix([3;3], [1.0..9.])
        let b = Matrix([3;2;2], [1.0..12.0])
        let x = luSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when lu solve A and B not compatible``() =
        let a = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let b = Matrix([2;2], [1.0..4.0])
        let x = luSolve(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when qr decompose non 2D`` () =
        let m = Matrix([3;2;2], [1.0..12.])
        let (q, r) = qr(m)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when qr solve full rank A not 2D``() =
        let a = Matrix([4;3;2], [1.0..24.])
        let b = Matrix([4;2], [1.0..8.0])
        let x = qrSolveFull(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when qr solve full rank B not 2D``() =
        let a = Matrix([4;3], [1.0..12.])
        let b = Matrix([4;2;2], [1.0..16.0])
        let x = qrSolveFull(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when qr solve full rank A and B not compatible``() =
        let a = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let b = Matrix([2;2], [1.0..4.0])
        let x = qrSolveFull(a, b)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when svd solve A not 2D``() =
        let a = Matrix([4;3;2], [1.0..24.])
        let b = Matrix([4;2], [1.0..8.0])
        let x = svdSolve(a, b, 1e-20)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when svd solve B not 2D``() =
        let a = Matrix([4;3], [1.0..12.])
        let b = Matrix([4;2;2], [1.0..16.0])
        let x = svdSolve(a, b, 1e-20)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when svd solve A and B not compatible``() =
        let a = Matrix([3;3], [2.0;0.7;0.7;0.7;2.0;0.7;0.7;0.7;2.0])
        let b = Matrix([2;2], [1.0..4.0])
        let x = svdSolve(a, b, 1e-20)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when svd non 2D``() =
        let a = Matrix([3;3;2], [1.0..18.0])
        let s = svd(a)
        true |> should be False

