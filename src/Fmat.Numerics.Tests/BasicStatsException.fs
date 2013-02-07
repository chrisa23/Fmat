namespace Fmat.Numerics
open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework

[<TestFixture>]
module BasicStatsExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when prod with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = prod(m, 3)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when cumprod with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = cumprod(m, 3)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when sum with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = sum(m, 3)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when cumsum with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = cumsum(m, 3)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when mean with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = mean(m, 3)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when var with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = var(m, -1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when skewness with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = skewness(m, -1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when kurtosis with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = kurtosis(m, -1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when quantile with invalid dimension`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let q = Matrix([1;2], [0.1;0.5])
        let res = quantile(m, q, -1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when quantile with non vector q`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let q = Matrix([2;2], [1.0..4.0])
        let res = quantile(m, q, 0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when cov with non 2D matrix`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = cov(m)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when corr with non 2D matrix`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = corr(m)
        true |> should be False
