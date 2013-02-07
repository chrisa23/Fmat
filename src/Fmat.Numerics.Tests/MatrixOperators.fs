namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixOperatorsTests =

    [<Test>]
    let ``Should matrix * matrix`` () =
        let m1 = Matrix([2;2], [1.0;2.0;3.0;4.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;1.0])
        let res = m1 * m2;
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|13.;20.;5.;8.|]))

    [<Test>]
    let ``Should matrix .* matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [2.0..9.0])
        let res = m1 .* m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|2.;6.;12.;20.;30.;42.;56.;72.;|]))

    [<Test>]
    let ``Should scalar * matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 * m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0|]))

    [<Test>]
    let ``Should matrix * scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m * 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0|]))

    [<Test>]
    let ``Should scalar .* matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 .* m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0|]))

    [<Test>]
    let ``Should matrix .* scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m .* 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0|]))

    [<Test>]
    let ``Should matrix / scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m / 2.
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|0.5;1.0;1.5;2.0|]))

    [<Test>]
    let ``Should scalar / matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 / m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;1.0;2.0/3.0;0.5|]))

    [<Test>]
    let ``Should matrix ./ scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m ./ 2.
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|0.5;1.0;1.5;2.0|]))

    [<Test>]
    let ``Should scalar ./ matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 ./ m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.0;1.0;2.0/3.0;0.5|]))

    [<Test>]
    let ``Should matrix ./ matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [0.0..7.0])
        let res = m1 ./ m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|Double.PositiveInfinity;2.;1.5;4./3.;5./4.;6./5.;7./6.0;8.0/7.0;|]))

    [<Test>]
    let ``Should scalar + matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 + m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.0;4.0;5.0;6.0|]))

    [<Test>]
    let ``Should matrix + scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m + 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.0;4.0;5.0;6.0|]))

    [<Test>]
    let ``Should scalar .+ matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 .+ m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.0;4.0;5.0;6.0|]))

    [<Test>]
    let ``Should matrix .+ scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m .+ 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.0;4.0;5.0;6.0|]))

    [<Test>]
    let ``Should matrix .+ matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [1.0..8.0])
        let res = m1 .+ m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0;10.;12.;14.;16.|]))

    [<Test>]
    let ``Should matrix + matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [1.0..8.0])
        let res = m1 + m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|2.0;4.0;6.0;8.0;10.;12.;14.;16.|]))

    [<Test>]
    let ``Should scalar - matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 - m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|1.0;0.0;-1.0;-2.0|]))

    [<Test>]
    let ``Should matrix - scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m - 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|-1.0;0.0;1.0;2.0|]))

    [<Test>]
    let ``Should scalar .- matrix`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = 2.0 .- m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|1.0;0.0;-1.0;-2.0|]))

    [<Test>]
    let ``Should matrix .- scalar`` () =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m .- 2.0
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|-1.0;0.0;1.0;2.0|]))

    [<Test>]
    let ``Should matrix .- matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [2.0..9.0])
        let res = m1 .- m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|-1.;-1.;-1.;-1.;-1.;-1.;-1.;-1.;|]))

    [<Test>]
    let ``Should matrix - matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [2.0..9.0])
        let res = m1 - m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|-1.;-1.;-1.;-1.;-1.;-1.;-1.;-1.;|]))

    [<Test>]
    let ``Should -matrix`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = -m
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|-1.;-2.;-3.;-4.;-5.;-6.;-7.;-8.;|]))

    [<Test>]
    let ``Should matrix ** scalar`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = m ** 2.
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|1.;4.;9.;16.;25.;36.;49.;64.;|]))

    [<Test>]
    let ``Should scalar .^ matrix`` () =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = 2.0 .^ m
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|2.;4.;8.;16.;32.;64.;128.;256.;|]))

    [<Test>]
    let ``Should matrix ** matrix`` () =
        let m1 = Matrix([2;2;2], [1.0..8.0])
        let m2 = Matrix([2;2;2], [2.0..9.0])
        let res = m1 ** m2
        res.Size |> should equal [|2;2;2|]
        res.Data |> should equal (Managed([|1.;2.0**3.0;3.0**4.0;4.0**5.0;5.0**6.0;6.0**7.0;7.0**8.0;8.0**9.0;|]))

    [<Test>]
    let ``Should matrix .&& matrix`` () =
        let m1 = BoolMatrix([2;2], [true;false;true;false])
        let m2 = BoolMatrix([2;2], [true;true;false;false])
        let res = m1 .&& m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;false;false|]))

    [<Test>]
    let ``Should matrix .&& scalar`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let a = true
        let res = m .&& a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;false|]))

    [<Test>]
    let ``Should scalar .&& matrix`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let a = true
        let res = a .&& m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;false|]))

    [<Test>]
    let ``Should matrix .|| matrix`` () =
        let m1 = BoolMatrix([2;2], [true;false;true;false])
        let m2 = BoolMatrix([2;2], [true;true;false;false])
        let res = m1 .|| m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;true;false|]))

    [<Test>]
    let ``Should matrix .|| scalar`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let a = true
        let res = m .|| a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;true;true|]))

    [<Test>]
    let ``Should scalar .|| matrix`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let a = true
        let res = a .|| m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;true;true|]))

    [<Test>]
    let ``Should ~~matrix`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let res = ~~ m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;true|]))

    [<Test>]
    let ``Should not matrix`` () =
        let m = BoolMatrix([2;2], [true;false;true;false])
        let res = BoolMatrix.not m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;true|]))