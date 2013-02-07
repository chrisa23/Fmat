namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixComparisonTests =

    [<Test>]
    let ``Should apply .< to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .< m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;false;true|]))

    [<Test>]
    let ``Should apply .< to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .< a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;false;false|]))

    [<Test>]
    let ``Should apply .< to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .< m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;false;true;true|]))

    [<Test>]
    let ``Should apply .<= to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .<= m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;false;true|]))

    [<Test>]
    let ``Should apply .<= to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .<= a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;false;false|]))

    [<Test>]
    let ``Should apply .<= to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .<= m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;true;true|]))

    [<Test>]
    let ``Should apply .> to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .> m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;false;true;false|]))

    [<Test>]
    let ``Should apply .> to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .> a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;false;true;true|]))

    [<Test>]
    let ``Should apply .> to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .> m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;false;false|]))

    [<Test>]
    let ``Should apply .>= to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .>= m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;true;false|]))

    [<Test>]
    let ``Should apply .>= to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .>= a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;true;true|]))

    [<Test>]
    let ``Should apply .>= to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .>= m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;true;false;false|]))

    [<Test>]
    let ``Should apply .== to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .== m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .= to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .= m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .== to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .== a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .= to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .= a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .== to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .== m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .= to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .= m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|false;true;false;false|]))

    [<Test>]
    let ``Should apply .!= to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .!= m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should apply .<> to matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = m1 .<> m2
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should apply .!= to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .!= a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should apply .<> to matrix and scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = m .<> a
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should apply .!= to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .!= m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should apply .<> to scalar and matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.
        let res = a .<> m
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|true;false;true;true|]))

    [<Test>]
    let ``Should verify matrices are value equal`` () =
        let m1 = Matrix([2;2;3], [1.0..12.0])
        let m2 = Matrix([2;2;3], [1.0..12.0])
        m1 == m2 |> should be True
        m1 != m2 |> should be False

    [<Test>]
    let ``Should verify matrices with same sizes different data not value equal`` () =
        let m1 = Matrix([2;2;3], [1.0..12.0])
        let m2 = Matrix([2;2;3], [0.0..11.0])
        m1 == m2 |> should be False
        m1 != m2 |> should be True

    [<Test>]
    let ``Should apply &< to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 2.)
        let a = 3.
        m1 &< a |> should be False
        m2 &< a |> should be True

    [<Test>]
    let ``Should apply &< to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        a &< m1 |> should be False
        a &< m2 |> should be True

    [<Test>]
    let ``Should apply &<= to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        m1 &<= a |> should be False
        m2 &<= a |> should be True

    [<Test>]
    let ``Should apply &<= to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        a &<= m1 |> should be False
        a &<= m2 |> should be True

    [<Test>]
    let ``Should apply &> to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        m1 &> a |> should be False
        m2 &> a |> should be True

    [<Test>]
    let ``Should apply &> to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 2.)
        let a = 3.
        a &> m1 |> should be False
        a &> m2 |> should be True

    [<Test>]
    let ``Should apply &>= to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        m1 &>= a |> should be False
        m2 &>= a |> should be True

    [<Test>]
    let ``Should apply &>= to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        a &>= m1 |> should be False
        a &>= m2 |> should be True

    [<Test>]
    let ``Should apply &= to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        m1 &= a |> should be False
        m2 &= a |> should be True

    [<Test>]
    let ``Should apply &= to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 3.)
        let a = 3.
        a &= m1 |> should be False
        a &= m2 |> should be True

    [<Test>]
    let ``Should apply &!= to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        m1 &!= a |> should be False
        m2 &!= a |> should be True

    [<Test>]
    let ``Should apply &<> to matrix and scalar`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        m1 &<> a |> should be False
        m2 &<> a |> should be True

    [<Test>]
    let ``Should apply &!= to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        a &!= m1 |> should be False
        a &!= m2 |> should be True

    [<Test>]
    let ``Should apply &<> to scalar and matrix`` () =
        let m1, m2 = Matrix([2;2], [2.0;3.0;4.0;5.0]), Matrix([2;2], 4.)
        let a = 3.
        a &<> m1 |> should be False
        a &<> m2 |> should be True

    [<Test>]
    let ``Should minXY matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = minXY(m1,  m2)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.;3.;2.;5.|]))

    [<Test>]
    let ``Should minXY matrix scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.0
        let res = Matrix.minXY(m,  a)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.;3.;3.;3.|]))

    [<Test>]
    let ``Should minXY scalar matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.0
        let res = Matrix.minXY(a, m)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|2.;3.;3.;3.|]))

    [<Test>]
    let ``Should maxXY matrices`` () =
        let m1 = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let m2 = Matrix([2;2], [4.0;3.0;2.0;6.0])
        let res = maxXY(m1,  m2)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|4.;3.;4.;6.|]))

    [<Test>]
    let ``Should maxXY matrix scalar`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.0
        let res = Matrix.maxXY(m,  a)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.;3.;4.;5.|]))

    [<Test>]
    let ``Should maxXY scalar matrix`` () =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let a = 3.0
        let res = Matrix.maxXY(a, m)
        res.Size |> should equal [|2;2|]
        res.Data |> should equal (Managed([|3.;3.;4.;5.|]))


