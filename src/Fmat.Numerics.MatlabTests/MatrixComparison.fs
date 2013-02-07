namespace Fmat.Numerics.MatlabTests

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.Conversion
open FsUnit
open NUnit.Framework
open System
open MLApp
open Util

[<TestFixture>]
module MatrixComparisonTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should matrix .< matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = randi(10,100,15,20);
                             c = 1 * (a < b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .< y) == z |> should be True

    [<Test>]
    let ``Should matrix .< scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * (a < b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .< 5.0) == z |> should be True
        new Matrix(5.0 .> x) == z |> should be True
        new Matrix(x .< ((!!5.0 : Matrix) : Matrix)) == z |> should be True
        new Matrix(((!!5.0 : Matrix) : Matrix) .> x) == z |> should be True

    [<Test>]
    let ``Should matrix .<= matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = randi(10,100,15,20);
                             c = 1 * (a <= b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .<= y) == z |> should be True

    [<Test>]
    let ``Should matrix .<= scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * (a <= b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .<= 5.0) == z |> should be True
        new Matrix(5.0 .>= x) == z |> should be True
        new Matrix(x .<= (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .>= x) == z |> should be True

    [<Test>]
    let ``Should matrix .> matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = randi(10,100,15,20);
                             c = 1 * (a > b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .> y) == z |> should be True

    [<Test>]
    let ``Should matix .> scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * (a > b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .> 5.0) == z |> should be True
        new Matrix(5.0 .< x) == z |> should be True
        new Matrix(x .> (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .< x) == z |> should be True

    [<Test>]
    let ``Should matrix .>= matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = randi(10,100,15,20);
                             c = 1 * (a >= b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .>= y) == z |> should be True

    [<Test>]
    let ``Should matrix >= scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * (a >= b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .>= 5.0) == z |> should be True
        new Matrix(5.0 .<= x) == z |> should be True
        new Matrix(x .>= (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .<= x) == z |> should be True

    [<Test>]
    let ``Should matrix .= matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10, 100,15,20);
                             b = randi(10, 100,15,20);
                             c = 1 * (a == b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .= y) == z |> should be True
        new Matrix(x .== y) == z |> should be True

    [<Test>]
    let ``Should matrix .= scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * (a == b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .== 5.0) == z |> should be True
        new Matrix(5.0 .== x) == z |> should be True
        new Matrix(x .= 5.0) == z |> should be True
        new Matrix(5.0 .= x)  == z |> should be True
        new Matrix(x .== (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .== x) == z |> should be True
        new Matrix(x .= (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .= x) == z |> should be True

    [<Test>]
    let ``Should matrix .!= matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10, 100,15,20);
                             b = randi(10, 100,15,20);
                             c = 1 * not(a == b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .!= y) == z |> should be True
        new Matrix(x .<> y) == z |> should be True


    [<Test>]
    let ``Should matrix .!= scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = randi(10,100,15,20);
                             b = 5;
                             c = 1 * not(a == b);")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        new Matrix(x .!= 5.0) == z |> should be True
        new Matrix(5.0 .!= x)  == z |> should be True
        new Matrix(x .!= (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .!= x) == z |> should be True
        new Matrix(x .<> 5.0) == z |> should be True
        new Matrix(5.0 .<> x)  == z |> should be True
        new Matrix(x .<> (!!5.0 : Matrix)) == z |> should be True
        new Matrix((!!5.0 : Matrix) .<> x) == z |> should be True

    [<Test>]
    let ``Should matrix == matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = [1 2 3];
                             b = [1 2 3];
                             c = isequal(a,b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getScalarFromMatlab(app, "c")
        x == y |> should equal (if z = 1.0 then true else false)

    [<Test>]
    let ``Should matrix != matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = [1 2 3];
                             b = [1 2 4];
                             c = ~isequal(a,b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getScalarFromMatlab(app, "c")
        x != y |> should equal (if z = 1.0 then true else false)

    [<Test>]
    let ``Should minXY``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20,30);
                             b = rand(20,30);
                             c = min(a,b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        abs(minXY(x, y) - z) &<= 1e-14 |> should be True

    [<Test>]
    let ``Should maxXY``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20,30);
                             b = rand(20,30);
                             c = max(a,b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        abs(maxXY(x, y) - z) &<= 1e-14 |> should be True