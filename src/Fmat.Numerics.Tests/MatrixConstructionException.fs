namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixConstructionExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when creating matrix with negative dimension`` () =
        let m = Matrix([2;-2], Managed([|2.0|]))
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when creating matrix with non matching data length`` () =
        let m = Matrix([2;2], Managed([|1.0..5.0|]))
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when creating matrix with < 2 dimensions`` () =
        let m = Matrix([2], Managed([|1.0|]))
        true |> should be False

