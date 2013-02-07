namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixManipulationExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when create diag from 2D but non vector``() =
        let m = Matrix([2;2], 2.0)
        let d = diag(m, 0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when create diag from non 2D matrix``() =
        let m = Matrix([2;2;2], 2.0)
        let d = diag(m, 0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when concat matrices along negative dimension``() =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        let m3 = concat([m1;m2], -1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when concat matrices with non compatible sizes``() =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([3;3], [1.0..9.0])
        let m3 = concat([m1;m2], 1)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when repmat matrix with negative replicator``() =
        let m = Matrix([2;2], [1.0..4.0])
        let rep = repmat(m, [-2;2])
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when triL non 2D matrix``() =
        let m = Matrix([3;3;3], [1.0..27.0])
        let res = triL(m, 0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when triU non 2D matrix``() =
        let m = Matrix([3;3;3], [1.0..27.0])
        let res = triU(m, 0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when reshape matrix into invalid size``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        let reshaped = reshape(m, [-2;-6])
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when reshape matrix into size with non matching length``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        let reshaped = reshape(m, [2;7])
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when reshape matrix in place into invalid size``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        m.Reshape([-2;-6])
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when reshape matrix in place into size with non matching length``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        m.Reshape([2;7])
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when transpose non 2D matrix``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        let res = transpose(m)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when .T non 2D Matrix``() =
        let m = Matrix([2;3;2], [1.0..12.0])
        let transposed = m.T
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when .Diag Non2D matrix``() =
        let m = Matrix([3;3;2], [1.0..18.0])
        let d = m.Diag(1)
        true |> should be False







