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
module MatrixOperatorsTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should matrix * matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1000,20);
                             b = rand(20,30);
                             c = a * b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        x * y == z |> should be True

    [<Test>]
    let ``Should matrix * scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = 1.2345*a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        1.2345 * x == y |> should be True
        x * 1.2345 == y |> should be True
        (!!1.2345 : Matrix) * x == y |> should be True
        x * (!!1.2345 : Matrix) == y |> should be True
        1.2345 .* x == y |> should be True
        x .* 1.2345 == y |> should be True
        ((!!1.2345 : Matrix) : Matrix) .* x == y |> should be True
        x .* ((!!1.2345 : Matrix) : Matrix) == y |> should be True

    [<Test>]
    let ``Should matrix / scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = a/1.2345;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = x / 1.2345
        abs(res - y) &< 1e-15 |> should be True
        x / 1.2345 == x ./ 1.2345 |> should be True
        x ./ 1.2345 == x ./ ((!!1.2345 : Matrix) : Matrix) |> should be True

    [<Test>]
    let ``Should scalar / matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = 1.2345./a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = 1.2345 / x
        abs(res - y) &< 1e-15 |> should be True
        1.2345 / x == 1.2345 ./ x |> should be True
        1.2345 ./ x == ((!!1.2345 : Matrix) : Matrix) ./ x |> should be True

    [<Test>]
    let ``Should matrix + scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = 1.2345+a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = 1.2345 + x 
        res == y |> should be True
        1.2345 + x == 1.2345 .+ x  |> should be True
        1.2345 .+ x == x + 1.2345  |> should be True
        x + 1.2345 == x .+ 1.2345  |> should be True
        ((!!1.2345 : Matrix) : Matrix) .+ x == x .+ ((!!1.2345 : Matrix) : Matrix)  |> should be True
        (!!1.2345 : Matrix) + x == x + (!!1.2345 : Matrix)  |> should be True

    [<Test>]
    let ``Should scalar - matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = 1.2345-a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = 1.2345 - x
        res == y |> should be True
        1.2345 - x == 1.2345 .- x |> should be True
        (!!1.2345 : Matrix) - x == (!!1.2345 : Matrix) .- x |> should be True

    [<Test>]
    let ``Should matrix - scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = a - 1.2345;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = x - 1.2345
        res == y |> should be True
        x - 1.2345 == x .- 1.2345 |> should be True
        x - 1.2345 == x .- 1.2345 |> should be True

    [<Test>]
    let ``Should matrix + matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = rand(100,20,30);
                             c = a + b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = x + y
        res == z |> should be True
        x + y == x .+ y |> should be True
        x .+ y == y .+ x |> should be True
        y .+ x == y + x |> should be True

    [<Test>]
    let ``Should matrix - matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = rand(100,20,30);
                             c = a - b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = x - y
        res == z |> should be True
        x - y == x .- y |> should be True

    [<Test>]
    let ``Should -matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = -a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = -x
        res == y |> should be True

    [<Test>]
    let ``Should matrix .* matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = rand(100,20,30);
                             c = a .* b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = x .* y
        res == z |> should be True
        x .* y == y .* x |> should be True
            
    [<Test>]
    let ``Should matrix ./ matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = rand(100,20,30);
                             c = a ./ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = x ./ y
        res == z |> should be True

    [<Test>]
    let ``Should matrix ** scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = a .^ 3.4;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = x ** 3.4
        abs(res - y) &< 1.e-15 |> should be True
            
    [<Test>]
    let ``Should matrix ** matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = rand(100,20,30);
                             c = a .^ b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let res = x ** y
        abs(res - z) &< 1.e-15 |> should be True

    [<Test>]
    let ``Should scalar .^ matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,20,30);
                             b = 3.4 .^ a;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = 3.4 .^ x
        abs(res - y) &< 1.e-15 |> should be True

