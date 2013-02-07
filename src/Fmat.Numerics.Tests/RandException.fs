namespace Fmat.Numerics
open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework

[<TestFixture>]
module RandExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when rand with invalid size`` () =
        let m = rand [-1;2] : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when unifrnd with invalid size`` () =
        let m = unifRnd(0.0, 1.0, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when normalrnd with invalid size`` () =
        let m = normalRnd(0.0, 1.0, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when lognormalrnd with invalid size`` () =
        let m = lognormRnd(0.0, 1.0, 0.0, 1.0, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when bernRnd with invalid size`` () =
        let m = bernRnd(0.5, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when binomRnd with invalid size`` () =
        let m = binomRnd(3, 0.5, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when poissRnd with invalid size`` () =
        let m = poissRnd(0.5, [2]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when unifrnd with b < a`` () =
        let m = unifRnd(1.0, 0.0, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when normalrnd with sigma < 0`` () =
        let m = normalRnd(0.0, -1.0, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when lognormalrnd with sigma < 0`` () =
        let m = lognormRnd(0.0, -1.0, 0.0, 1.0, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when bernRnd with p < 0`` () =
        let m = bernRnd(-0.5, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when binomRnd with p < 0`` () =
        let m = binomRnd(3, -0.5, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when binomRnd with n < 0`` () =
        let m = binomRnd(-3, 0.5, [2;3]) : Matrix
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when poissRnd with lambda < 0`` () =
        let m = poissRnd(-0.5, [2;3]) : Matrix
        true |> should be False

