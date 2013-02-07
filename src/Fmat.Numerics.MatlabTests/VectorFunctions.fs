namespace Fmat.Numerics.MatlabTests

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System
open MLApp
open Util

[<TestFixture>]
module VectorFunctionsTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should sqrt``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(0, 100,100,15,20);
                             b = sqrt(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = sqrt(x)
        abs((res - y) ./ y) &<= 1e-15 |> should be True

    [<Test>]
    let ``Should abs``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-100, 100,100,15,20);
                             b = abs(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = abs(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should exp``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-5, 5,100,15,20);
                             b = exp(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = exp(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should log``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(0, 1000,100,15,20);
                             b = log(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = log(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should log10``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(0, 1000,100,15,20);
                             b = log10(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = log10(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should cos``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-100, 100,100,15,20);
                             b = cos(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = cos(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should sin``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-100, 100,100,15,20);
                             b = sin(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = sin(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should tan``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-pi/2, pi/2,100,15,20);
                             b = tan(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = tan(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should acos``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-1, 1,100,15,20);
                             b = acos(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = acos(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should asin``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-1, 1,100,15,20);
                             b = asin(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = asin(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should atan``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-1000, 1000,100,15,20);
                             b = atan(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = atan(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should cosh``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-10, 10,100,15,20);
                             b = cosh(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = cosh(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should sinh``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-10, 10,100,15,20);
                             b = sinh(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = sinh(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should tanh``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-10, 10,100,15,20);
                             b = tanh(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = tanh(x)
        abs((res - y) ./ y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should erf`` () =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-111, 111,100,15,20);
                             b = erf(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = erf(x)
        nearlyEqual res y 1e-15 |> should be True

    [<Test>]
    let ``Should erfc``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-10, 10,100,15,20);
                             b = erfc(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = erfc(x)
        nearlyEqual res y 1e-10 |> should be True

    [<Test>]
    let ``Should erfinv``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-1, 1,100,15,20);
                             b = erfinv(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = erfinv(x)
        nearlyEqual res y 1e-13 |> should be True

    [<Test>]
    let ``Should erfcinv``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(0, 2,100,15,20);
                             b = erfcinv(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = erfcinv(x)
        nearlyEqual res y 1e-13 |> should be True

    [<Test>]
    let ``Should normcdf``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-10, 10,100,15,20);
                             b = normcdf(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = normcdf(x)
        nearlyEqual res y 1e-10 |> should be True

    [<Test>]
    let ``Should norminv``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(0, 1,100,15,20);
                             b = norminv(a,0,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = norminv(x)
        nearlyEqual res y 1e-14 |> should be True

    [<Test>]
    let ``Should ceil``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-100, 100,100,15,20);
                             b = ceil(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = ceil(x)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should round``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = unifrnd(-100, 100,100,15,20);
                             b = round(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = round(x)
        abs(res - y) &< 1e-15 |> should be True



