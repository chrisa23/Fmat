namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixManipulationTests =

    [<Test>]
    let ``Should create diag matrix`` () =
        let m = Matrix([1;3], [1.0..3.0])
        let res = diag(m, 0)
        res.Size |> should equal [|3;3|]
        res.Data |> should equal (Managed([|1.;0.0;0.0;0.0;2.0;0.0;0.0;0.0;3.0|]))

    [<Test>]
    let ``Should concat matrices horizontally`` () =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        let res = concat([m1;m2], 1)
        res.Size |> should equal [|2;5|]
        res.Data |> should equal (Managed([1.0..4.] @ [1.0..6.0] |> List.toArray))

    [<Test>]
    let ``Should replicate matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = repmat(m, [2;2])
        res.Size |> should equal [|4;4|]
        res.Data |> should equal (Managed([|1.0;2.0;1.0;2.0;3.0;4.0;3.0;4.0;1.0;2.0;1.0;2.0;3.0;4.0;3.0;4.0|]))

    [<Test>]
    let ``Should get lower triangular`` () =
        let m = Matrix([3;3], [1.0..9.0])
        let res = triL(m, 1)
        res.Size |> should equal [|3;3|]
        res.Data |> should equal (Managed([|1.0;2.0;3.0;4.0;5.0;6.0;0.0;8.0;9.0|]))

    [<Test>]
    let ``Should get upper triangular`` () =
        let m = Matrix([3;3], [1.0..9.0])
        let res = triU(m, -1)
        res.Size |> should equal [|3;3|]
        res.Data |> should equal (Managed([|1.0;2.0;0.0;4.0;5.0;6.0;7.0;8.0;9.0|]))

    [<Test>]
    let ``Should reshape matrix`` () =
        let m = Matrix([2;3;2], [1.0..12.0])
        let res = reshape(m, [2;6])
        res.Size |> should equal [|2;6|]
        res.Data |> should equal (Managed([|1.0..12.0|]))

    [<Test>]
    let ``Should reshape matrix in place`` () =
        let m = Matrix([2;3;2], [1.0..12.0])
        m.Reshape([2;6])
        m.Size |> should equal [|2;6|]
        m.Data |> should equal (Managed([|1.0..12.0|]))

    [<Test>]
    let ``Should transpose matrix`` () =
        let m = Matrix([2;3], [1.0..6.0])
        let res = transpose(m)
        res.Size |> should equal [|3;2|]
        res.Data |> should equal (Managed([|1.0;3.0;5.0;2.0;4.0;6.0|]))

    [<Test>]
    let ``Should transpose matrix in place`` () =
        let m = Matrix([2;3], [1.0..6.0])
        m.Transpose()
        m.Size |> should equal [|3;2|]
        m.Data |> should equal (Managed([|1.0;3.0;5.0;2.0;4.0;6.0|]))

    [<Test>]
    let ``Should get diagonal`` () =
        let m = Matrix([3;3], [1.0..9.0])
        let res = m.Diag()
        res.Size |> should equal [|3;1|]
        res.Data |> should equal (Managed([|1.0;5.0;9.0|]))

    [<Test>]
    let ``Should apply fun in place`` () =
        let m = Matrix([2;3], [1.0..6.0])
        m.ApplyFun (fun x -> 2.0 * x)
        m.Size |> should equal [|2;3|]
        m.Data |> should equal (Managed([|2.0..2.0..12.0|]))

    [<Test>]
    let ``Should apply fun`` () =
        let m = Matrix([2;3], [1.0..6.0])
        let res = applyFun(m, (fun x -> 2.0 * x))
        res.Size |> should equal [|2;3|]
        res.Data |> should equal (Managed([|2.0..2.0..12.0|]))

    [<Test>]
    let ``Should apply fun 2 matrices`` () =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        let res = applyFun2Arg(m1, m2, (fun x y -> x + y))
        res.Size |> should equal [|2;3|]
        res.Data |> should equal (Managed([|2.0..2.0..12.0|]))

    [<Test>]
    let ``Should apply fun 3 matrices`` () =
        let m1 = Matrix([2;3], [1.0..6.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        let m3 = Matrix([2;3], [1.0..6.0])
        let res = applyFun3Arg(m1, m2, m3, (fun x y z -> x + y + z))
        res.Size |> should equal [|2;3|]
        res.Data |> should equal (Managed([|3.0..3.0..18.0|]))