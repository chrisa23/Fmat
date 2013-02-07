namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.Conversion
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixConstructionTests =
    
    [<Test>]
    let ``Should create matrix from size and column major seq`` () =
        let m = Matrix([10;10;10], {1.0..1000.0})
        m.Size |> should equal [|10;10;10|]
        m.Data |> should equal (Managed([|1.0..1000.|]))

    [<Test>]
    let ``Should create matrix from scalar`` () =
        let m = Matrix(10.)
        m.Size |> should equal [|1;1|]
        m.Data |> should equal (Managed([|10.|]))

    [<Test>]
    let ``Should create matrix from !!scalar`` () =
        let m = !!10.0 : Matrix
        m.Size |> should equal [|1;1|]
        m.Data |> should equal (Managed([|10.|]))

    [<Test>]
    let ``Should create matrix from size and scalar`` () =
        let m = Matrix([2;3;4], 10.)
        m.Size |> should equal [|2;3;4|]
        m.Data |> should equal (Managed(Array.create 24 10.))

    [<Test>]
    let ``Should create matrix from 1D array`` () =
        let m = Matrix([|1.0;2.0;3.0|])
        m.Size |> should equal [|1;3|]
        m.Data |> should equal (Managed([|1.0;2.0;3.0|]))

    [<Test>]
    let ``Should create matrix from seq of list`` () =
        let data = [[1.0;2.0;3.0]
                    [4.0;5.0]
                    [6.0;7.0;8.0]]
        let m = Matrix(data)
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;4.0;6.0;2.0;5.0;7.0;3.0;0.0;8.0|]))

    [<Test>]
    let ``Should create matrix from seq of array`` () =
        let data = [| [|1.0;2.0;3.0|]
                      [|4.0;5.0|]
                      [|6.0;7.0;8.0|]|]
        let m = Matrix(data)
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;4.0;6.0;2.0;5.0;7.0;3.0;0.0;8.0|]))

    [<Test>]
    let ``Should create matrix from list of seq`` () =
        let data = [ {1.0..3.0}
                     {4.0..5.0}
                     {6.0..8.0} ]
        let m = Matrix(data)
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;4.0;6.0;2.0;5.0;7.0;3.0;0.0;8.0|]))

    [<Test>]
    let ``Should create matrix from !!`` () =
        let m = !! [[1.0;2.0;3.0]
                    [4.0;5.0]
                    [6.0;7.0;8.0]] : Matrix
        m.Size |> should equal [|3;3|]
        m.Data |> should equal (Managed([|1.0;4.0;6.0;2.0;5.0;7.0;3.0;0.0;8.0|]))

    [<Test>]
    let ``Should create matrix from 2D array`` () =
        let arr = Array2D.init 2 3 (fun i j -> float(j * 2 + i))
        let m = Matrix(arr)
        m.Size |> should equal [|2;3|]
        m.Data |> should equal (Managed([|0.0..5.|]))

    [<Test>]
    let ``Should create matrix from 3D array`` () =
        let arr = Array3D.init 2 3 4 (fun i j k -> float(k * 6 + j * 2 + i))
        let m = Matrix(arr)
        m.Size |> should equal [|2;3;4|]
        m.Data |> should equal (Managed([|0.0..23.|]))

    [<Test>]
    let ``Should create matrix from 4D array`` () =
        let arr = Array4D.init 2 3 4 5 (fun i j k l -> float(l * 24 + k * 6 + j * 2 + i))
        let m = Matrix(arr)
        m.Size |> should equal [|2;3;4;5|]
        m.Data |> should equal (Managed([|0.0..119.|]))

    [<Test>]
    let ``Should create matrix from 1D initializer`` () =
        let size = [|2;3;4|]
        let m = Matrix(size, fun i -> float(i) + 1.0)
        m.Size |> should equal size
        m.Data |> should equal (Managed([|1.0..24.|]))

    [<Test>]
    let ``Should create matrix from 2D initializer`` () =
        let size = [|2;3|]
        let m = Matrix(size, fun i j -> float(i+j))
        m.Size |> should equal size
        m.Data |> should equal (Managed([|0.0;1.0;1.0;2.0;2.0;3.0|]))

    [<Test>]
    let ``Should create matrix from 3D initializer`` () =
        let size = [|2;2;2|]
        let m = Matrix(size, fun i j k -> float(i+j+k))
        m.Size |> should equal size
        m.Data |> should equal (Managed([|0.0;1.0;1.0;2.0;1.0;2.0;2.0;3.0|]))

    [<Test>]
    let ``Should create matrix from 4D initializer`` () =
        let size = [|2;2;2;2|]
        let m = Matrix(size, fun i j k l -> float(i+j+k+l))
        m.Size |> should equal size
        m.Data |> should equal (Managed([|0.0;1.0;1.0;2.0;1.0;2.0;2.0;3.0;1.0;2.0;2.0;3.0;2.0;3.0;3.0;4.0|]))

    [<Test>]
    let ``Should create matrix from bool matrix`` () =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;2], [0.0;1.0;5.0;6.0])
        let m3 = Matrix(m1 .< m2)
        m3.Size |> should equal [|2;2|]
        m3.Data |> should equal (Managed([|0.0;0.0;1.0;1.0|]))

    [<Test>]
    let ``should create zeros matrix`` () =
        let m = zeros [2;3;4] : Matrix
        m.Size |> should equal [|2;3;4|]
        m.Data |> should equal (Managed(Array.create 24 0.))

    [<Test>]
    let ``Should create ones matrix`` () =
        let m = ones [2;3;4] : Matrix
        m.Size |> should equal [|2;3;4|]
        m.Data |> should equal (Managed(Array.create 24 1.))

    [<Test>]
    let ``should create identity matrix`` () =
        let m = I(2, 3) : Matrix
        m.Size |> should equal [|2;3|]
        m.Data |> should equal (Managed([|1.;0.;0.;1.;0.;0.|]))

    [<Test>]
    let ``should cast scalar matrix`` () =
        let m = !!2.0 : Matrix
        !!m : float |> should equal 2.0

    [<Test>]
    let ``should cast row vector matrix`` () =
        let m = !![|1.0..3.0|] : Matrix
        !!m : float[] |> should equal [|1.0..3.0|]

    [<Test>]
    let ``should cast col vector matrix`` () =
        let m = !![|1.0..3.0|] : Matrix
        m.Transpose()
        !!m : float[] |> should equal [|1.0..3.0|]

    [<Test>]
    let ``should cast 2D matrix`` () =
        let data = Array2D.init 2 3 (fun i j -> float(i) + float(j))
        let m = !!data : Matrix
        !!m : float[,] |> should equal data

    [<Test>]
    let ``should cast 3D matrix`` () =
        let data = Array3D.init 2 3 4 (fun i j k -> float(i) + float(j) + float(k))
        let m = !!data : Matrix
        !!m : float[,,] |> should equal data

    [<Test>]
    let ``should cast 4D matrix`` () =
        let data = Array4D.init 2 3 4 5 (fun i j k l -> float(i) + float(j) + float(k) + float(l))
        let m = !!data : Matrix
        !!m : float[,,,] |> should equal data

    [<Test>]
    let ``should cast `` () =
        let data = [ [0.0;1.0]
                     [1.0;2.0]]
        let m = !!data : Matrix
        !!m : float[,] |> should equal (Array2D.init 2 2 (fun i j -> float(i + j)))
