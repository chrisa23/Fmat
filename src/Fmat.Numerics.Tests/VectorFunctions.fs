namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module VectorFunctionsTests =

    [<Test>]
    let ``Should sqrt matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = sqrt(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (sqrt(4.5))))

    [<Test>]
    let ``Should pow matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = m ** 3.4
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (4.5 ** 3.4)))

    [<Test>]
    let ``Should pow matrices elementwise`` () =
        let m1 = Matrix([2;3;4], 4.)
        let m2 = Matrix([2;3;4], 2.)
        let res = m1 ** m2
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 16.))

    [<Test>]
    let ``Should abs matrix elementwise`` () =
        let m = Matrix([2;3;4], -4.5)
        let res = abs(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 4.5))

    [<Test>]
    let ``Should exp matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = exp(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (exp(4.5))))

    [<Test>]
    let ``Should log matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = log(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (log(4.5))))

    [<Test>]
    let ``Should log10 matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = log10(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (log10(4.5))))

    [<Test>]
    let ``Should cos matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = cos(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (cos(4.5))))

    [<Test>]
    let ``Should sin matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = sin(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (sin(4.5))))

    [<Test>]
    let ``Should tan matrix elementwise`` () =
        let m = Matrix([2;3;4], 4.5)
        let res = tan(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (tan(4.5))))

    [<Test>]
    let ``Should acos matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.5)
        let res = acos(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (acos(0.5))))

    [<Test>]
    let ``Should asin matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.5)
        let res = asin(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (asin(0.5))))

    [<Test>]
    let ``Should atan matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.5)
        let res = atan(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (atan(0.5))))

    [<Test>]
    let ``Should cosh matrix elementwise`` () =
        let m = Matrix([2;3;4], 2.5)
        let res = cosh(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (cosh(2.5))))

    [<Test>]
    let ``Should sinh matrix elementwise`` () =
        let m = Matrix([2;3;4], 2.5)
        let res = sinh(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (sinh(2.5))))

    [<Test>]
    let ``Should tanh matrix elementwise`` () =
        let m = Matrix([2;3;4], 2.5)
        let res = tanh(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 (tanh(2.5))))

    [<Test>]
    let ``Should erf matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.5)
        let res = erf(m)
        res.Size |> should equal [|2;3;4|]
        Math.Abs(res.[0] - 0.520499877813047) < 1e-15 |> should be True

    [<Test>]
    let ``Should erfc matrix elementwise``() =
        let m = Matrix([2;2;2], 0.5)
        let res = erfc(m)
        let x = res.[0, 0, 0] : float
        Math.Round((res.[0, 0, 0] : float), 8) |> should equal 0.47950012

    [<Test>]
    let ``Should erfInv matrix elementwise``() =
        let m = Matrix([2;2;2], 0.5)
        let res = erfinv(m)
        Math.Round((res.[0, 0, 0] : float), 8) |> should equal 0.47693628

    [<Test>]
    let ``Should erfcInv matrix elementwise``() =
        let m = Matrix([2;2;2], 0.5)
        let res = erfcinv(m)
        Math.Round((res.[0, 0, 0] : float), 8) |> should equal 0.47693628

    [<Test>]
    let ``Should normCdf matrix elementwise``() =
        let m = Matrix([2;2;2], 0.5)
        let res = normcdf(m)
        Math.Round((res.[0, 0, 0] : float), 13) |> should equal 0.691462461274

    [<Test>]
    let ``Should normInv matrix elementwise``() =
        let m = Matrix([2;2;2], 0.7)
        let res = norminv(m)
        Math.Round((res.[0, 0, 0] : float), 13) |> should equal 0.524400512708

    [<Test>]
    let ``Should ceil matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.7)
        let res = ceil(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 1.))

    [<Test>]
    let ``Should round matrix elementwise`` () =
        let m = Matrix([2;3;4], 0.7)
        let res = round(m);
        res.Size |> should equal [|2;3;4|]
        res.Data |> should equal (Managed(Array.create 24 1.))

