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
module BasicStatTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should min 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = min(a,[],1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        min(x, 0) == y |> should be True

    [<Test>]
    let ``Should min 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = min(a,[],2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        min(x, 1) == y |> should be True

    [<Test>]
    let ``Should min 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = min(a,[],3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        min(x, 2) == y |> should be True

    [<Test>]
    let ``Should max 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = max(a,[],1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        max(x, 0) == y |> should be True

    [<Test>]
    let ``Should max 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = max(a,[],2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        max(x, 1) == y |> should be True

    [<Test>]
    let ``Should max 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = max(a,[],3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        max(x, 2) == y |> should be True

    [<Test>]
    let ``Should mean 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = mean(a,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = mean(x, 0)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should mean 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = mean(a,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = mean(x, 1)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should mean 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = mean(a,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = mean(x, 2)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should var 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = var(a,0,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = var(x, 0)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should var 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = var(a,0,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = var(x, 1)
        abs((res - y) ./ y) &< 1e-13 |> should be True

    [<Test>]
    let ``Should var 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = var(a,0,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = var(x, 2)
        abs(res - y) &< 1e-15 |> should be True

    [<Test>]
    let ``Should skewness 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = skewness(a,1,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = skewness(x, 0)
        abs(res - y) &< 1e-12 |> should be True

    [<Test>]
    let ``Should skewness 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = skewness(a,1,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = skewness(x, 1)
        abs(res - y) &< 1e-13 |> should be True

    [<Test>]
    let ``Should skewness 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = skewness(a,1,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = skewness(x, 2)
        abs(res - y) &< 1e-13 |> should be True


    [<Test>]
    let ``Should kurtosis 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = kurtosis(a,1,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = kurtosis(x, 0)
        abs(res - y) &< 1e-11 |> should be True

    [<Test>]
    let ``Should kurtosis 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = kurtosis(a,1,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = kurtosis(x, 1)
        abs(res - y) &< 1e-12 |> should be True

    [<Test>]
    let ``Should kurtosis 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = kurtosis(a,1,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = kurtosis(x, 2)
        abs((res - y) ./ y) &< 1e-12 |> should be True

    [<Test>]
    let ``Should quantile 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100000,20);
                             p=0.01:0.01:0.99;
                             b = quantile(a,p,1);")
        let x = getMatrixFromMatlab(app, "a")
        let p = getMatrixFromMatlab(app, "p")
        let y = getMatrixFromMatlab(app, "b")
        let res = quantile(x, p, 0)
        abs(res - y) &< 1e-4 |> should be True

    [<Test>]
    let ``Should quantile 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 100000);
                             p=0.01:0.01:0.99;
                             b = quantile(a,p,2);")
        let x = getMatrixFromMatlab(app, "a")
        let p = getMatrixFromMatlab(app, "p")
        let y = getMatrixFromMatlab(app, "b")
        let res = quantile(x, p, 1)
        abs(res - y) &< 1e-4 |> should be True

    [<Test>]
    let ``Should quantile 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(5,5, 100000,3);
                             p=0.01:0.01:0.99;
                             b = quantile(a,p,3);")
        let x = getMatrixFromMatlab(app, "a")
        let p = getMatrixFromMatlab(app, "p")
        let y = getMatrixFromMatlab(app, "b")
        let res = quantile(x, p, 2)
        abs(res - y) &< 1e-4 |> should be True

    [<Test>]
    let ``Should get covariance matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10, 1000, 10);
                             b = cov(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = cov(x)
        abs(res - y) &< 1e-12 |> should be True

    [<Test>]
    let ``Should get corr matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10, 1000, 10);
                             b = corr(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = corr(x)
        abs(res - y) &< 1e-13 |> should be True

