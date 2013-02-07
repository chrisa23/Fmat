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
module MatrixManipulationTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should create diag matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1,2001);
                             b = diag(a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        diag(x, 0) == y |> should be True

    [<Test>]
    let ``Should create diag matrix positive offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1,2001);
                             b = diag(a, 5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        diag(x, 5) == y |> should be True

    [<Test>]
    let ``Should create diag matrix negative offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1,2001);
                             b = diag(a, -5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        diag(x, -5) == y |> should be True

    [<Test>]
    let ``Should create diag matrix positive offset off limit``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1,2001);
                             b = diag(a, 2002);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        diag(x, 2002) == y |> should be True

    [<Test>]
    let ``Should create diag matrix negative offset off limit``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1,2001);
                             b = diag(a, -2002);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        diag(x, -2002) == y |> should be True

    [<Test>]
    let ``Should concat 2D matrices along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = rand(120,200);
                             c = rand(150,200);   
                             d = cat(1,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 0) == s |> should be True
        vertConcat([x;y;z])  == s |> should be True

    [<Test>]
    let ``Should concat 2D matrices along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = rand(100,220);
                             c = rand(100,250);   
                             d = cat(2,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 1) == s |> should be True
        horzConcat([x;y;z]) == s |> should be True

    [<Test>]
    let ``Should concat 2D matrices along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = rand(100,200);
                             c = rand(100,200);   
                             d = cat(3,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 2) == s |> should be True

    [<Test>]
    let ``Should concat 3D matrices along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,5);
                             b = rand(20,20,5);
                             c = rand(1200,20,5);   
                             d = cat(1,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 0) == s |> should be True
        vertConcat([x;y;z])  == s |> should be True

    [<Test>]
    let ``Should concat 3D matrices along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,5);
                             b = rand(10,22,5);
                             c = rand(10,1000,5);   
                             d = cat(2,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 1) == s |> should be True
        horzConcat([x;y;z])  == s |> should be True

    [<Test>]
    let ``Should concat 3D matrices along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(10,20,5);
                             c = rand(10,20,100);   
                             d = cat(3,a,b,c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        concat([x;y;z], 2) == s |> should be True

    [<Test>]
    let ``Should replicate 2D matrix vertical``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = repmat(a,5,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        repmat(x, [5;1]) == y |> should be True

    [<Test>]
    let ``Should replicate 2D matrix horizontally``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = repmat(a,1,5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        repmat(x, [1;5]) == y |> should be True

    [<Test>]
    let ``Should replicate 2D matrix into 3D``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = repmat(a,[1 1 5]);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        repmat(x, [1;1;5]) == y |> should be True

    [<Test>]
    let ``Should replicate 3D matrix into 4D``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,30);
                             b = repmat(a,[3 4 5 6]);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        repmat(x, [3;4;5;6]) == y |> should be True

    [<Test>]
    let ``Should get lower triangular``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = rand(200,100);
                             c = tril(a);
                             d = tril(b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triL(x, 0)  == z |> should be True
        triL(y, 0) == u |> should be True

    [<Test>]
    let ``Should get lower triangular positive offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = tril(a,5);
                             d = tril(b,5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triL(x, 5) == z |> should be True
        triL(y, 5) == u |> should be True

    [<Test>]
    let ``Should get lower triangular negative offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = tril(a,-5);
                             d = tril(b,-5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triL(x, -5) == z |> should be True
        triL(y, -5) == u |> should be True

    [<Test>]
    let ``Should get lower triangular positive offset off limit``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = tril(a,21);
                             d = tril(b,11);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triL(x, 21) == z |> should be True
        triL(y, 11) == u |> should be True

    [<Test>]
    let ``Should get upper triangular``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = rand(200,100);
                             c = triu(a);
                             d = triu(b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triU(x, 0) == z |> should be True
        triU(y, 0) == u |> should be True

    [<Test>]
    let ``Should get upper triangular positive offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = triu(a,5);
                             d = triu(b,5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triU(x, 5) == z |> should be True
        triU(y, 5) == u |> should be True

    [<Test>]
    let ``Should get upper triangular negative offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = triu(a,-5);
                             d = triu(b,-5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triU(x, -5) == z |> should be True
        triU(y, -5) == u |> should be True

    [<Test>]
    let ``Should get upper triangular negative offset off limit``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = rand(20,10);
                             c = triu(a,-11);
                             d = triu(b,-21);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        triU(x, -11) == z |> should be True
        triU(y, -21) == u |> should be True

    [<Test>]
    let ``Should reshape matrix function``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = reshape(a,20,5,100,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        reshape(x, [20;5;100;2]) == y |> should be True

    [<Test>]
    let ``Should reshape matrix method``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = reshape(a,2,5,10,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.Reshape([2;5;10;2])
        x == y |> should be True

    [<Test>]
    let ``Should transpose matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = a';")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        transpose(x) == y |> should be True
        x.T == y |> should be True   
        x.Transpose()
        x == y |> should be True 

    [<Test>]
    let ``Should get diagonal``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(1000,2000);
                             b = diag(a);
                             c = rand(2000,1000);
                             d = diag(c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        x.Diag() == y |> should be True
        z.Diag() == s |> should be True


    [<Test>]
    let ``Should get diagonal positive offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = diag(a,5);
                             c = rand(20,10);
                             d = diag(c,5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        x.Diag(5) == y |> should be True
        z.Diag(5) == s |> should be True

    [<Test>]
    let ``Should get diagonal negative offset``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20);
                             b = diag(a,-5);
                             c = rand(20,10);
                             d = diag(c,-5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let s = getMatrixFromMatlab(app, "d")
        x.Diag(-5) == y |> should be True
        z.Diag(-5) == s |> should be True

    [<Test>]
    let ``Should sum 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = sum(a,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = sum(x, 0)
        abs(res - y) &< 1e-14 |> should be True

    [<Test>]
    let ``Should sum 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = sum(a,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        sum(x, 1) == y |> should be True

    [<Test>]
    let ``Should sum 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = sum(a,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        sum(x, 2) == y |> should be True

    [<Test>]
    let ``Should prod 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,5000);
                             b = prod(a,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        prod(x, 0) == y |> should be True

    [<Test>]
    let ``Should prod 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = prod(a,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        prod(x, 1) == y |> should be True

    [<Test>]
    let ``Should prod 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = prod(a,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        prod(x, 2) == y |> should be True

    [<Test>]
    let ``Should cumsum 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = cumsum(a,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        cumsum(x, 0) == y |> should be True

    [<Test>]
    let ``Should cumsum 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = cumsum(a,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        cumsum(x, 1) == y |> should be True

    [<Test>]
    let ``Should cumsum 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = cumsum(a,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        cumsum(x, 2) == y |> should be True

    [<Test>]
    let ``Should cumprod 2D matrix along dim0``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = cumprod(a,1);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        cumprod(x, 0) == y |> should be True

    [<Test>]
    let ``Should cumprod 2D matrix along dim1``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100,200);
                             b = cumprod(a,2);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        cumprod(x, 1) == y |> should be True

    [<Test>]
    let ``Should cumprod 4D matrix along dim2``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = cumprod(a,3);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let res = cumprod(x, 2)
        cumprod(x, 2) == y |> should be True

    [<Test>]
    let ``Should applyFun in place``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = a;
                             a = arrayfun(@(x) x*x+1, a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        y.ApplyFun(fun x -> x * x + 1.)
        y == x |> should be True

    [<Test>]
    let ``Should applyFun``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = arrayfun(@(x) x*x+1, a);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        applyFun(x, fun y -> y * y + 1.) == y |> should be True

    [<Test>]
    let ``Should applyFun2Arg``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = rand(10,20,25,30);
                             c = arrayfun(@(x,y) x*y+x+y, a, b);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        applyFun2Arg(x, y, fun x y -> x * y + x + y) == z |> should be True

    [<Test>]
    let ``Should applyFun3Arg``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10,20,25,30);
                             b = rand(10,20,25,30);
                             c = rand(10,20,25,30);
                             d = arrayfun(@(x,y,z) x*y*z+x+y+z, a, b, c);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        let u = getMatrixFromMatlab(app, "d")
        applyFun3Arg(x, y, z, fun x y z -> x * y * z + x + y + z) == u |> should be True

