namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixInfoTests =

    [<Test>]
    let ``Should get matrix data`` () =
        let m = Matrix([2;3;4], [1.0..24.0])
        m.Data |> should equal (Managed([|1.0..24.0|]))

    [<Test>]
    let ``Should get size`` ()  =
        let m = Matrix([2;3;4], [1.0..24.0])
        m.Size |> should equal [|2;3;4|]

    [<Test>]
    let ``Should get length`` () =
        let m = Matrix([2;3;4], [1.0..24.0])
        m.Length |> should equal 24

    [<Test>]
    let ``Should get ndims`` () =
        let m = Matrix([2;3;4], [1.0..24.0])
        m.NDims |> should equal 3

    [<Test>]
    let ``Should not change size``() =
        let m = Matrix([2;3;4], [1.0..24.0])
        let size = m.Size
        size.[0] <- 10
        m.Size |> should equal [|2;3;4|]

    [<Test>]
    let ``Should get empty matrix`` () =
        let m = Matrix.Empty
        m.NDims |> should equal 2
        m.Size |> should equal [|0;0|]
        m.Data |> should equal (Managed(Array.zeroCreate<float> 0))

    [<Test>]
    let ``Should squeeze size at construction`` () =
        let m = Matrix([1;2;1;3], Managed([|1.0..6.0|])) 
        m.Size |> should equal [|2;3|]
        let m = Matrix([2;1;1;3], Managed([|1.0..6.0|])) 
        m.Size |> should equal [|2;3|]
        let m = Matrix([1;1;1;3], Managed([|1.0..3.0|])) 
        m.Size |> should equal [|1;3|]
        let m = Matrix([2;3;1;4], Managed([|1.0..24.0|])) 
        m.Size |> should equal [|2;3;4|]

    [<Test>]
    let ``Should check is scalar matrix``() =
        let m = Matrix(2.)
        m.IsScalar |> should be True
        let m = Matrix([2;2], 2.)
        m.IsScalar |> should be False

    [<Test>]
    let ``Should check is vector matrix``() =
        let m = Matrix(2.)
        m.IsVector |> should be True
        let m = Matrix([1;2], 2.)
        m.IsVector |> should be True
        let m = Matrix([2;1], 2.)
        m.IsVector |> should be True
        let m = Matrix([2;2], 2.)
        m.IsVector |> should be False
