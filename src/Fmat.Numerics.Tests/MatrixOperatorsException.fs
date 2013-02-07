namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixOperatorsExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when * matrices first non 2D``() =
        let m1 = Matrix([2;2;3], [1.0..12.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;1.0])
        let res = m1 * m2;
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<RankException>)>]
    let ``Should throw RankException when * matrices second non 2D``() =
        let m1 = Matrix([2;2;3], [1.0..12.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;1.0])
        let res = m2 * m1;
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when * non compatible matrices``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;1.0])
        let res = m1 * m2;
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when + matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 + m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .+ matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .+ m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when - matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 - m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .- matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .- m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .* matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .* m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when ./ matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 ./ m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when ** matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 ** m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .&& matrices with non matching sizes``() =
        let m1 = BoolMatrix([2;3], [true;false;true;false;true;false])
        let m2 = BoolMatrix([3;2], [true;false;true;false;true;false])
        let res = m1 .&& m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .|| matrices with non matching sizes``() =
        let m1 = BoolMatrix([2;3], [true;false;true;false;true;false])
        let m2 = BoolMatrix([3;2], [true;false;true;false;true;false])
        let res = m1 .|| m2
        true |> should be False