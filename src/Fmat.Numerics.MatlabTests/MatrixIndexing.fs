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
module MatrixIndexingTests =
    let app = new MLAppClass()
    do app.Visible <- 0

    [<Test>]
    let ``Should slice 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(500, 500);
                             b = a(5:150, 1:100);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[4..149, 0..99] == y |> should be True

    [<Test>]
    let ``Should slice 2D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 100);
                             b = a(1:25, 1:76);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[..24, ..75] == y |> should be True

    [<Test>]
    let ``Should slice 2D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 100);
                             b = a(10:end, 50:end);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[9.., 49..] == y |> should be True

    [<Test>]
    let ``Should slice 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 40);
                             b = a(2:10, 1:15, 20:40);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[1..9, 0..14, 19..39] == y |> should be True

    [<Test>]
    let ``Should slice 3D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 40);
                             b = a(1:10, 1:15, 1:30);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[..9, ..14, ..29] == y |> should be True

    [<Test>]
    let ``Should slice 3D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 40);
                             b = a(10:end, 15:end, 20:end);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[9.., 14.., 19..] == y |> should be True

    [<Test>]
    let ``Should slice 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 10, 10);
                             b = a(2:10, 1:15, 3:8, 2:10);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[1..9, 0..14, 2..7, 1..9] == y |> should be True

    [<Test>]
    let ``Should slice 4D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 10, 10);
                             b = a(1:10, 1:15, 1:9, 1:5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[..9, ..14, ..8, ..4] == y |> should be True

    [<Test>]
    let ``Should slice 4D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 30, 10, 10);
                             b = a(10:end, 15:end, 5:end, 2:end);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[9.., 14.., 4.., 1..] == y |> should be True

    [<Test>]
    let ``Should set slice 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(500, 50);
                             b = rand(150, 40); 
                             c = a;
                             a(5:154, 1:40) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[4..153, 0..39] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 50);
                             b = rand(46, 26); 
                             c = a;
                             a(1:46, 1:26) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[..45, ..25] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 50);
                             b = rand(49, 41); 
                             c = a;
                             a(2:end, 10:end) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[1.., 9..] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             b = rand(25, 10, 15); 
                             c = a;
                             a(3:27, 5:14, 12:26) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[2..26, 4..13, 11..25] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             b = rand(27, 14, 26); 
                             c = a;
                             a(1:27, 1:14, 1:26) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[..26, ..13, ..25] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             b = rand(48, 36, 19); 
                             c = a;
                             a(3:end, 5:end, 12:end) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[2.., 4.., 11..] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             b = rand(18, 5, 6, 10); 
                             c = a;
                             a(3:20, 5:9, 3:8, 2:11) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[2..19, 4..8, 2..7, 1..10] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             b = rand(20, 9, 8, 11); 
                             c = a;
                             a(1:20, 1:9, 1:8, 1:11) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[..19, ..8, ..7, ..10] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             b = rand(48, 7, 3, 8); 
                             c = a;
                             a(3:end, 4:end, 10:end, 6:end) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[2.., 3.., 9.., 5..] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(500, 50);
                             c = a;
                             a(5:154, 1:40) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[4..153, 0..39] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix left open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 50);
                             c = a;
                             a(1:46, 1:26) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[..45, ..25] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix right open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 50);
                             c = a;
                             a(2:end, 10:end) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[1.., 9..] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             c = a;
                             a(3:27, 5:14, 12:26) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[2..26, 4..13, 11..25] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix left open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             c = a;
                             a(1:27, 1:14, 1:26) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[..26, ..13, ..25] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 3D matrix right open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 40, 30);
                             c = a;
                             a(3:end, 5:end, 12:end) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[2.., 4.., 11..] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             c = a;
                             a(3:20, 5:9, 3:8, 2:11) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[2..19, 4..8, 2..7, 1..10] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix left open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             c = a;
                             a(1:20, 1:9, 1:8, 1:11) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[..19, ..8, ..7, ..10] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 4D matrix right open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 10, 12, 13);
                             c = a;
                             a(3:end, 4:end, 10:end, 6:end) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[2.., 3.., 9.., 5..] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should slice 2D matrix linear``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = a(200:10000);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[199..9999] == y |> should be True

    [<Test>]
    let ``Should slice 2D matrix linear left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10);
                             b = a(1:100);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[..99] == y |> should be True

    [<Test>]
    let ``Should slice 2D matrix linear right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10);
                             b = a(50:end);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[49..] == y |> should be True

    [<Test>]
    let ``Should set slice 2D matrix linear``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = rand(100, 60);
                             c = a;
                             a(101:6100) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[100..6099] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix linear left open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10);
                             b = rand(2, 50);
                             c = a;
                             a(1:100) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[..99] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix linear right open``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = rand(2, 5000);
                             c = a;
                             a(10001:end) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[10000..] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set slice2D matrix linear scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             c = a;
                             a(101:6100) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[100..6099] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix linear left open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10);
                             c = a;
                             a(1:100) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[..99] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set slice 2D matrix linear right open scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             c = a;
                             a(10001:end) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[10000..] <- 1.1234
        z == x |> should be True

    [<Test>]
    let ``Should index 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = [a(5, 7) 0];")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[4, 6] = y.[0] |> should be True

    [<Test>]
    let ``Should index 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100, 10);
                             b = [a(50, 70, 6) 0];")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[49, 69, 5] = y.[0] |> should be True

    [<Test>]
    let ``Should index 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 20, 10);
                             b = [a(10, 6, 12, 8) 0];")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[9, 5, 11, 7] = y.[0] |> should be True

    [<Test>]
    let ``Should index linear 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 20, 10);
                             b = [a(12000) 0];")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[11999] = y.[0] |> should be True

    [<Test>]
    let ``Should set index 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = a;
                             a(5, 7) = 1.1234")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        y.[4, 6] <- 1.1234
        y == x |> should be True

    [<Test>]
    let ``Should set index 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100, 10);
                             b = a;
                             a(50, 70, 6) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        y.[49, 69, 5] <- 1.1234
        y == x |> should be True

    [<Test>]
    let ``Should set index 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 20, 10);
                             b = a;
                             a(10, 6, 12, 8) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        y.[9, 5, 11, 7] <- 1.1234
        y == x |> should be True

    [<Test>]
    let ``Should set index linear 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 20, 10);
                             b = a;
                             a(12000) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        y.[11999] <- 1.1234
        y == x |> should be True

    [<Test>]
    let ``Should sub 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 100);
                             b = a(2:50, 10:2:40);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[{1..49}, {9..2..39}] == y |> should be True

    [<Test>]
    let ``Should sub 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15);
                             b = a(2:10, 1:2:5, 5:15);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[{1..9}, {0..2..4}, {4..14}] == y |> should be True

    [<Test>]
    let ``Should sub 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15, 10);
                             b = a(10:-1:2, 1:2:5, 5:15, 3:9);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[{9..-1..1}, {0..2..4}, {4..14}, {2..8}] == y |> should be True

    [<Test>]
    let ``Should set sub 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 150);
                             b = rand(149, 66);
                             c = a;
                             a(2:150, 10:2:140) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[{1..149}, {9..2..139}] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set sub 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15);
                             b = rand (9, 3, 11);
                             c = a;
                             a(2:10, 1:2:5, 5:15) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[{1..9}, {0..2..4}, {4..14}] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set sub 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15, 10);
                             b = rand(9, 3, 11, 7);
                             c = a;
                             a(10:-1:2, 1:2:5, 5:15, 3:9) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[{9..-1..1}, {0..2..4}, {4..14}, {2..8}] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set sub 2D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(200, 150);
                             c = a;
                             a(2:150, 10:2:140) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.Set({1..149}, {9..2..139} , 1.1234)
        z == x |> should be True

    [<Test>]
    let ``Should set sub 3D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15);
                             c = a;
                             a(2:10, 1:2:5, 5:15) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.Set({1..9}, {0..2..4}, {4..14} , 1.1234)
        z == x |> should be True

    [<Test>]
    let ``Should set sub 4D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(20, 10, 15, 10);
                             c = a;
                             a(10:-1:2, 1:2:5, 5:15, 3:9) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.Set({9..-1..1}, {0..2..4}, {4..14}, {2..8}, 1.1234)
        z == x |> should be True

    [<Test>]
    let ``Should sub linear 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(30, 50, 10, 50);
                             b = a(100:4:7410);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[{99..4..7409}] == y |> should be True

    [<Test>]
    let ``Should set sub linear 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(30, 50, 10, 50);
                             b = rand(2,3500);
                             c = a;
                             a(100:7099) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.[{99..7098}] <- y
        z == x |> should be True

    [<Test>]
    let ``Should set sub linear 4D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(30, 50, 10, 50);
                             c = a;
                             a(100:7099) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.Set({99..7098} , 1.1234)
        z == x |> should be True

    [<Test>]
    let ``Should logical index 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100, 200);
                             b = a(a > 0.5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[x .> 0.5] == y |> should be True

    [<Test>]
    let ``Should logical index 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10, 20, 30);
                             b = a(a > 0.5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[x .> 0.5] == y |> should be True

    [<Test>]
    let ``Should logical index 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10, 20, 30, 10);
                             b = a(a > 0.5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[x .> 0.5] == y |> should be True

    [<Test>]
    let ``Should set logical index 2D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = reshape(1:10000, 50, 200);
                             b = rand(50, 100);
                             c = a;
                             a(a > 5000) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.Set(z .> 5000. , y)
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 3D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = reshape(1:1000, 10, 10, 10);
                             b = rand(50, 10);
                             c = a;
                             a(a > 500) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.Set(z .> 500. , y)
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 4D matrix``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = reshape(1:10000, 10, 10, 10, 10);
                             b = rand(50, 100);
                             c = a;
                             a(a > 5000) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.Set(z .> 5000. , y)
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 2D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 200);
                             c = a;
                             a(a > 0.5) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[z .> 0.5] <-  1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 3D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10, 10, 10);
                             c = a;
                             a(a > 0.5) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[z .> 0.5] <-  1.1234
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 4D matrix scalar``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(10, 10, 10, 10);
                             c = a;
                             a(a > 0.5) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[z .> 0.5] <-  1.1234
        z == x |> should be True

    [<Test>]
    let ``Should logical index 2D matrix predicate``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(100, 200);
                                  b = a(a > 0.5);")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        x.[fun x -> x > 0.5] == y |> should be True

    [<Test>]
    let ``Should set logical index 2D matrix predicate``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = reshape(1:10000, 50, 200);
                                  b = rand(50, 100);
                                  c = a;
                                  a(a > 5000) = b;")
        let x = getMatrixFromMatlab(app, "a")
        let y = getMatrixFromMatlab(app, "b")
        let z = getMatrixFromMatlab(app, "c")
        z.Set((fun z -> z > 5000.0), y)
        z == x |> should be True

    [<Test>]
    let ``Should set logical index 2D matrix scalar predicate``() =
        resetMatlabRng app 1234
        let r = app.Execute("a = rand(50, 200);
                             c = a;
                             a(a > 0.5) = 1.1234;")
        let x = getMatrixFromMatlab(app, "a")
        let z = getMatrixFromMatlab(app, "c")
        z.[fun z -> z > 0.5] <-  1.1234
        z == x |> should be True


