namespace Fmat.Numerics.MatlabTests

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.LinearAlgebra
open FsUnit
open NUnit.Framework
open System
open MLApp
open Util

[<TestFixture>]
module MatrixAlgebraTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should cholesky decompose``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = gallery('lehmer', 100);
                             b = chol(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = chol(x)
        abs(res - y) &< 1e-14 |> should be True

    [<Test>]
    let ``Should cholesky solve``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = gallery('lehmer', 100);
                             b = rand(100, 1);
                             c = a \ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = cholSolve(x, y)
        abs(res - z) &< 1e-11 |> should be True

    [<Test>]
    let ``Should lu decompose square matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,100);
                             [l, u, p] = lu(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "l")
        let z = getMatrixFromMatlab(app, "u")
        let s = getMatrixFromMatlab(app, "p")
        let (l, u, p) = lu(x)
        abs(l - y) &< 1e-12 |> should be True
        abs(u - z) &< 1e-12 |> should be True
        (I(100, 100):Matrix).[p, {0..99}] == s |> should be True

    [<Test>]
    let ``Should lu solve``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100, 100);
                             b = rand(100, 1);
                             c = a \ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = luSolve(x, y)
        abs(res - z) &< 1e-11 |> should be True

    [<Test>]
    let ``Should qr decompose more rows``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1000,100);
                             [q, r] = qr(a, 0);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "q")
        let z = getMatrixFromMatlab(app, "r")
        let (q, r) = qr(x)
        abs(abs(q) - abs(y)) &< 1e-14 |> should be True
        abs(abs(r) - abs(z)) &< 1e-13 |> should be True
        abs(x - q * r) &< 1e-13 |> should be True

    [<Test>]
    let ``Should qr solve full``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(500, 100);
                             b = rand(500, 1);
                             c = a \ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = qrSolveFull(x, y)
        abs(res - z) &< 1e-14 |> should be True

    [<Test>]
    let ``Should svd solve``() =
        let r = app.Execute("a = reshape(2:13, 4, 3);
                             a(:, 1) = 0;
                             b = reshape(1:8, 4, 2);
                             c = a \ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let (res, rank) = svdSolve(x, y, 1e-15)
        rank |> should equal 2
        abs(res - z) &< 1e-14 |> should be True

    [<Test>]
    let ``Should svd decompose more rows``() =
        let r = app.Execute(" a=[9 4;6 8;2 7];
                             [u, s, v] = svd(a, 0);")
        let x = getMatrixFromMatlab(app, "a")
        let U = getMatrixFromMatlab(app, "u")
        let S = getMatrixFromMatlab(app, "s")
        let V = getMatrixFromMatlab(app, "v")
        let (u, s, vt) = svd(x)
        abs(u - U) &< 1e-14 |> should be True
        abs(diag(s, 0) - S) &< 1e-14 |> should be True
        abs(vt.T - V) &< 1e-14 |> should be True



