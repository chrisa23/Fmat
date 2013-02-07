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
module MatrixConstructionTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should create zeros matrix``() =
        let a = zeros [20;30;40;50] : Matrix
        let r = app.Execute("a = zeros(20,30,40,50);")
        let x = getMatrixFromMatlab(app, "a")
        a == x |> should be True

    [<Test>]
    let ``Should create ones matrix``() =
        let a = ones [200;300;10] : Matrix
        let r = app.Execute("a = ones(200,300, 10);")
        let x = getMatrixFromMatlab(app, "a")
        a == x |> should be True

    [<Test>]
    let ``Should create I(rows,cols) matrix cols > rows``() =
        let a = I(200,300) : Matrix
        let r = app.Execute("a = eye(200,300);")
        let x = getMatrixFromMatlab(app, "a")
        a == x |> should be True

    [<Test>]
    let ``Should create I(rows,cols) matrix cols < rows``() =
        let a = I(300,200) : Matrix
        let r = app.Execute("a = eye(300,200);")
        let x = getMatrixFromMatlab(app, "a")
        a == x |> should be True

