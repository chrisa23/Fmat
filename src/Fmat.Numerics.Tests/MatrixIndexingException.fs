namespace Fmat.Numerics

open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open FsUnit
open NUnit.Framework
open System

[<TestFixture>]
module MatrixIndexingExceptionTests =

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i..j] and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[-1..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i..j] and j >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[1..4]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i..j] and i > j``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[2..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set slice [i..j] to scalar and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[-1..1] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set slice [i..j] to scalar and j >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[1..4] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set slice [i..j] to matrix and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[-1..1] <- Matrix([1;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set slice [i..j] to matrix and j >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[1..4] <- Matrix([1;4], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set slice [i..j] to matrix with incompatible length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[1..3] <- Matrix([1;2], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1] and i0 < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[-1..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1] and j1 >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[1..1, 1..4]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1] and i0 > j0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let slice = m.[1..0, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set slice [i0..j0,i1..j1] to matrix with non matching size``() =
        let m = Matrix([3;4], [1.0..12.0])
        m.[1..2, 1..3] <- Matrix([2;2], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2] and i0 < 0``() =
        let m = Matrix([2;2;3], [1.0..12.0])
        let slice = m.[-1..1, 0..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2] and j1 >= length``() =
        let m = Matrix([2;2;3], [1.0..12.0])
        let slice = m.[1..1, 1..4, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2] and i0 > j0``() =
        let m = Matrix([2;2;3], [1.0..12.0])
        let slice = m.[1..0, 0..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set slice [i0..j0,i1..j1,i2..j2] to matrix with non matching size``() =
        let m = Matrix([2;2;3], [1.0..12.0])
        m.[0..1, 0..1, 0..1] <- Matrix([2;2;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2,i3..j3] and i0 < 0``() =
        let m = Matrix([2;2;3;3], [1.0..36.0])
        let slice = m.[-1..1, 0..1, 0..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2,i3..j3] and j1 >= length``() =
        let m = Matrix([2;2;3;3], [1.0..36.0])
        let slice = m.[1..1, 1..4, 0..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get slice [i0..j0,i1..j1,i2..j2,i3..j3] and i0 > j0``() =
        let m = Matrix([2;2;3;3], [1.0..36.0])
        let slice = m.[1..0, 0..1, 0..1, 0..1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set slice [i0..j0,i1..j1,i2..j2,i3..j3] to matrix with non matching size``() =
        let m = Matrix([2;2;3;3], [1.0..36.0])
        m.[0..1, 0..1, 0..1, 0..1] <- Matrix([2;2;2;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i] and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let res = m.[-1]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i] and i >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let res = m.[4]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i] and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[-1] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i] and i >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[4] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j] and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let res = m.[-1, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j] and i >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        let res = m.[4, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j] and i < 0``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[-1, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j] and i >= length``() =
        let m = Matrix([2;2], [2.0;3.0;4.0;5.0])
        m.[4, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j, k] and i < 0``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = m.[-1, 0, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j, k] and i >= length``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = m.[4, 0, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j, k] and i < 0``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        m.[-1, 0, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j, k] and i >= length``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        m.[4, 0, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j, k, l] and i < 0``() =
        let m = Matrix([2;2;2;2], [1.0..16.0])
        let res = m.[-1, 0, 0, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get item [i, j, k, l] and i >= length``() =
        let m = Matrix([2;2;2;2], [1.0..16.0])
        let res = m.[4, 0, 0, 0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j, k, l] and i < 0``() =
        let m = Matrix([2;2;2;2], [1.0..16.0])
        m.[-1, 0, 0, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set item [i, j, k, l] and i >= length``() =
        let m = Matrix([2;2;2;2], [1.0..16.0])
        m.[4, 0, 0, 0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get items [seq<int>] and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m.[[-1;0;1]]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get items [seq<int>] and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m.[[0;1;4]]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>] to scalar and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set([-1;0;1], 0.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>] to scalar and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set([0;1;4], 0.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>] to matrix and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.[[-1;0;1]] <- Matrix([1;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>] to matrix and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.[[0;1;4]] <- Matrix([1;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items [seq<int>] to matrix with non matching length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.[[0;1;2]] <- Matrix([1;2], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get items [seq<int>,seq<int>] and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m.[[-1;0;1],[0;1]]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when get items [seq<int>,seq<int>] and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        let res = m.[[0;1;4],[0;1]]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>,seq<int>] to scalar and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set([ [-1;1]; [0;1] ], 0.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>,seq<int>] to scalar and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set([ [0;4]; [0;1] ], 0.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>,seq<int>] to matrix and index < 0``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.[[-1;0;1], [0;1]] <- Matrix([1;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<IndexOutOfRangeException>)>]
    let ``Should throw IndexOutOfRangeException when set items [seq<int>,seq<int>] to matrix and index >= length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.[[0;1;4], [0;1]] <- Matrix([1;3], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items [seq<int>,seq<int>] to matrix with non matching length``() =
        let m = Matrix([3;3], [1.0..9.0])
        m.[[0;1], [0;1]] <- Matrix([2;1], 2.0)
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when get items with bool indexing when non matching matrix sizes``() =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        let res = m1.[m2 .< 0.0]
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items to scalar with bool indexing when non matching matrix sizes``() =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        m1.[m2 .< 0.0] <- 0.0
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items to matrix with bool indexing when non matching matrix sizes``() =
        let m1 = Matrix([2;2], [1.0..4.0])
        let m2 = Matrix([2;3], [1.0..6.0])
        m1.Set(m2 .> 2.0, Matrix([2;2], 2.0))
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items to matrix with bool indexing when matrix incompatible length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set(m .>= 2.0, Matrix([1;2], 2.0))
        true |> should be False

    [<Test>]
    [<ExpectedException(typeof<ArgumentException>)>]
    let ``Should throw ArgumentException when set items to matrix with predicate indexing when matrix incompatible length``() =
        let m = Matrix([2;2], [1.0..4.0])
        m.Set((fun x -> x >= 2.0), Matrix([1;2], 2.0))
        true |> should be False