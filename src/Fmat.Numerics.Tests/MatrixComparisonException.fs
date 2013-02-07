namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixComparisonExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .< matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .< m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .<= matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .<= m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .> matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .> m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .>= matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .>= m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .= matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .= m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .== matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .== m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .!= matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .!= m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when .<> matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = m1 .<> m2
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when minXY matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = minXY(m1, m2)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when maxXY matrices with non matching sizes``() =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([3;2], [1.0..6.0])
        let res = maxXY(m1, m2)
        true |> should be False






