namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixIndexingTests =

    [<Test>]
    let ``Should get item by linear index`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[0] |> should equal 2.0
        m.[1] |> should equal 3.0
        m.[2] |> should equal 4.0
        m.[3] |> should equal 5.0

    [<Test>]
    let ``Should set item by linear index`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[2] <- 10.0
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;3.0;10.0;5.0|]))

    [<Test>]
    let ``Should get item by 2D index`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[0, 0] |> should equal 2.0
        m.[1, 0] |> should equal 3.0
        m.[0, 1] |> should equal 4.0
        m.[1, 1] |> should equal 5.0

    [<Test>]
    let ``Should set item by 2D index`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[0, 1] <- 10.0
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;3.0;10.0;5.0|]))

    [<Test>]
    let ``Should get item by 3D index`` () =
        let m = Matrix([2;2;2], [2.0..9.0])
        m.[0, 0, 0] |> should equal 2.0
        m.[1, 0, 0] |> should equal 3.0
        m.[0, 1, 0] |> should equal 4.0
        m.[1, 1, 0] |> should equal 5.0
        m.[0, 0, 1] |> should equal 6.0
        m.[1, 0, 1] |> should equal 7.0
        m.[0, 1, 1] |> should equal 8.0
        m.[1, 1, 1] |> should equal 9.0

    [<Test>]
    let ``Should set item by 3D index`` () =
        let m = Matrix([2;2;2], [2.0..9.0])
        m.[0, 1, 1] <- 10.0
        m.Size |> should equal [|2;2;2|]
        m.Data |> should equal (Managed([|2.0;3.0;4.0;5.0;6.0;7.0;10.0;9.0|]))

    [<Test>]
    let ``Should get slice by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[1..]
        sub.Size |> should equal [|1;3|]
        sub.Data |> should equal (Managed([|3.0;4.0;5.0|]))

    [<Test>]
    let ``Should set slice to matrix by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[2..] <- ones [1;2]
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;3.0;1.0;1.0|]))

    [<Test>]
    let ``Should set slice to scalar by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[2..] <- 1.
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;3.0;1.0;1.0|]))

    [<Test>]
    let ``Should get slice by 2D indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[1.., 0..]
        sub.Size |> should equal [|1;2|]
        sub.Data |> should equal (Managed([|3.0;5.0|]))

    [<Test>]
    let ``Should set slice to matrix by 2D indexing`` () =
        let m = Matrix([3;3], [1.0..9.])
        m.[0.., 1..] <- ones [3;2] 
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;2.0;3.0;1.0;1.0;1.0;1.0;1.0;1.0|]))

    [<Test>]
    let ``Should set slice to scalar by 2D indexing`` () =
        let m = Matrix([3;3], [1.0..9.])
        m.[0.., 1..] <- 1.
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;2.0;3.0;1.0;1.0;1.0;1.0;1.0;1.0|]))

    [<Test>]
    let ``Should get slice by 3D indexing`` () =
        let m = Matrix([2;3;4], [1.0..24.])
        let sub = m.[0.., 1..1, 2..2]
        sub.Size |> should equal [|2;1|]
        sub.Data |> should equal (Managed([|15.0;16.0|]))

    let ``Should get slice by 4D indexing`` () =
        let m = Matrix([2;3;4;2], [1.0..48.])
        let sub = m.[0.., 1..1, 3..3, 0..1]
        sub.Size |> should equal [|2;2|]
        sub.Data |> should equal (Managed([|21.0;22.0;45.0;46.0|]))

    [<Test>]
    let ``Should get submatrix by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[{1..3}]
        sub.Size |> should equal [|1;3|]
        sub.Data |> should equal (Managed([|3.0;4.0;5.0|]))

    [<Test>]
    let ``Should set submatrix to matrix by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[{1..3}] <- ones [1;3]
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;1.0;1.0;1.0|]))

    [<Test>]
    let ``Should set submatrix to scalar by linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.Set({1..3},  1.0)
        m.Size |> should equal [|2;2|]
        m.Data |> should equal (Managed([|2.0;1.0;1.0;1.0|]))

    [<Test>]
    let ``Should get submatrix by unordered linear indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[[3;1;2]]
        sub.Size |> should equal [|1;3|]
        sub.Data |> should equal (Managed([|5.0;3.0;4.0|]))

    [<Test>]
    let ``Should get submatrix by 2D indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[{1..1}, {0..1}]
        sub.Size |> should equal [|1;2|]
        sub.Data |> should equal (Managed([|3.0;5.0|]))

    [<Test>]
    let ``Should set submatrix to matrix by 2D indexing`` () =
        let m = Matrix([2;3], [2.0;3.0;4.0;5.0;6.0;7.0])
        m.[{1..1},{0..1..2}] <- ones [1;3]
        m.Size |> should equal [|2;3|]
        m.Data |> should equal (Managed([|2.0;1.0;4.0;1.0;6.0;1.0|]))

    [<Test>]
    let ``Should set submatrix to scalar by 2D indexing`` () =
        let m = Matrix([2;3], [2.0;3.0;4.0;5.0;6.0;7.0])
        m.Set({1..1}, {0..1..2}, 1.)
        m.Size |> should equal [|2;3|]
        m.Data |> should equal (Managed([|2.0;1.0;4.0;1.0;6.0;1.0|]))

    [<Test>]
    let ``Should get submatrix by unordered 2D indexing`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let sub = m.[[1;1], [1;0]]
        sub.Size |> should equal [|2;2|]
        sub.Data |> should equal (Managed([|5.0;5.0;3.0;3.0|]))

    [<Test>]
    let ``Should get submatrix by logical indexing`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let m3 = Matrix([2;2], [1.0;2.0;3.0;4.0])
        let res = m3.[m1 .< m2]
        res.Size |> should equal [|2;1|]
        res.Data |> should equal (Managed([|1.0;4.0|]))

    [<Test>]
    let ``Should set submatrix to matrix by logical indexing`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let m3 = Matrix([2;2], [1.0;2.0;3.0;4.0])
        m3.Set(m1 .< m2, ones [1;2])
        m3.Size |> should equal [|2;2|]
        m3.Data |> should equal (Managed([|1.0;2.0;3.0;1.0|]))

    [<Test>]
    let ``Should set submatrix to scalar by logical indexing`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let m3 = Matrix([2;2], [1.0;2.0;3.0;4.0])
        m3.[m1 .< m2] <- 1.
        m3.Size |> should equal [|2;2|]
        m3.Data |> should equal (Managed([|1.0;2.0;3.0;1.0|]))

    [<Test>]
    let ``Should get submatrix by predicate indexing`` () =
        let m = Matrix([3;4], [1.0..12.0])
        let res = m.[fun x -> int(x) % 2 = 0 ]
        res.Size |> should equal [|6;1|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0;10.0;12.0|]))

    [<Test>]
    let ``Should set submatrix to matrix by predicate indexing`` () =
        let m1 = Matrix([3;4], [1.0..12.0])
        let m2 = Matrix([1;6], [0.0..5.0])
        m1.Set((fun x -> int(x) % 2 = 0), m2)
        m1.Size |> should equal [|3;4|]
        m1.Data |> should equal (Managed([|1.0;0.0;3.0;1.0;5.0;2.0;7.0;3.0;9.0;4.0;11.0;5.0|]))

    [<Test>]
    let ``Should set submatrix to scalar by predicate indexing`` () =
        let m = Matrix([3;4], [1.0..12.0])
        m.[fun x -> int(x) % 2 = 0] <- 0.0
        m.Size |> should equal [|3;4|]
        m.Data |> should equal (Managed([|1.0;0.0;3.0;0.0;5.0;0.0;7.0;0.0;9.0;0.0;11.0;0.0|]))