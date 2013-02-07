namespace Fmat.Numerics
open System
open Fmat.Numerics
open Fmat.Numerics.BasicStat
open Fmat.Numerics.MatrixFunctions
open Fmat.Numerics.Conversion
open FsUnit
open NUnit.Framework

[<TestFixture>]
module BasicStatsTests =
    
    let inline (!!) x = !!x : Matrix

    [<Test>]
    let ``Should Get Col Sum``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = sum(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 3.0
        res.[1] |> should equal 5.0
        res.[2] |> should equal 7.0

    [<Test>]
    let ``Should Get Row Sum``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = sum(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 7.0
        res.[1] |> should equal 8.0

    [<Test>]
    let ``Should Get Row Sum 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = sum(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        res.[0, 0] |> should equal 4.0
        res.[1, 0] |> should equal 6.0
        res.[0, 1] |> should equal 12.0
        res.[1, 1] |> should equal 14.0

    [<Test>]
    let ``Should Get Col Cum Sum``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let test = !![[1.0;3.0;3.0]
                      [3.0;5.0;7.0]]
        let res = cumsum(m, 0)
        test == res |> should be True

    [<Test>]
    let ``Should Get Row Cum Sum``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let test = !![[1.0;4.0;7.0]
                      [2.0;4.0;8.0]]
        let res = cumsum(m, 1)
        test == res |> should be True

    [<Test>]
    let ``Should Get Row Cum Sum 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let test = Matrix([2;2;2], [1.0;2.0;4.0;6.0;5.0;6.0;12.0;14.0])
        let res = cumsum(m, 1)
        test == res |> should be True

    [<Test>]
    let ``Should Get Col Prod``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = prod(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 2.0
        res.[1] |> should equal 6.0
        res.[2] |> should equal 12.0

    [<Test>]
    let ``Should Get Row Prod``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = prod(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 9.0
        res.[1] |> should equal 16.0

    [<Test>]
    let ``Should Get Row Prod 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = prod(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        res.[0, 0] |> should equal 3.0
        res.[1, 0] |> should equal 8.0
        res.[0, 1] |> should equal 35.0
        res.[1, 1] |> should equal 48.0

    [<Test>]
    let ``Should Get Col Cum Prod``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = cumprod(m, 0)
        let test = !![[1.0;3.0;3.0]
                      [2.0;6.0;12.0]]
        test == res |> should be True

    [<Test>]
    let ``Should Get Row Cum Prod``() =
        let m = !![ [1.0;3.0;3.0]
                    [2.0;2.0;4.0]]
        let res = cumprod(m, 1)
        let test = !![[1.0;3.0;9.0]
                      [2.0;4.0;16.0]]
        test == res |> should be True

    [<Test>]
    let ``Should Get Row Cum Prod 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = cumprod(m, 1)
        let test = Matrix([2;2;2], [1.0;2.0;3.0;8.0;5.0;6.0;35.0;48.0])
        test == res |> should be True

    [<Test>]
    let ``Should Get Col Min``() =
        let m = !![[1.0;3.0;3.0]
                   [2.0;2.0;4.0]]
        let res = min(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 1.0
        res.[1] |> should equal 2.0
        res.[2] |> should equal 3.0

    [<Test>]
    let ``Should Get Row Min``() =
        let m = !![[1.0;3.0;3.0]
                   [2.0;2.0;4.0]]
        let res = min(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 1.0
        res.[1] |> should equal 2.0

    [<Test>]
    let ``Should Get Row Min 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = min(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        res.[0, 0] |> should equal 1.0
        res.[1, 0] |> should equal 2.0
        res.[0, 1] |> should equal 5.0
        res.[1, 1] |> should equal 6.0

    [<Test>]
    let ``Should Get Col Max``() =
        let m = !![[1.0;3.0;3.0]
                   [2.0;2.0;4.0]]
        let res = max(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 2.0
        res.[1] |> should equal 3.0
        res.[2] |> should equal 4.0

    [<Test>]
    let ``Should Get Row Max``() =
        let m = !![[1.0;3.0;3.0]
                   [2.0;2.0;4.0]]
        let res = max(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 3.0
        res.[1] |> should equal 4.0

    [<Test>]
    let ``Should Get Row Max 3D``() =
        let m = Matrix([2;2;2], [1.0..8.0])
        let res = max(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        res.[0, 0] |> should equal 3.0
        res.[1, 0] |> should equal 4.0
        res.[0, 1] |> should equal 7.0
        res.[1, 1] |> should equal 8.0

    [<Test>]
    let ``Should Get Col Mean``() =
        let m = !![[1.0;3.0;3.0]
                   [2.0;2.0;4.0]]
        let res = mean(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 1.5
        res.[1] |> should equal 2.5
        res.[2] |> should equal 3.5

    [<Test>]
    let ``Should Get Row Variance``() =
        let m = !![[1.0;2.0;3.0;10.0]
                   [2.0;1.0;10.0;3.0]]
        let res = var(m, 1)
        let x = 1.0 + 1.0 / 3.0
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        epsEqual res.[0] 16.6666666666667 1e-13 |> should be True
        epsEqual res.[1] 16.6666666666667 1e-13 |> should be True

    [<Test>]
    let ``Should Get Col Skewness``() =
        let m = Matrix([10;2], [1.0..20.])
        m.[2] <- 100.
        let res = skewness(m, 0)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 2
        res.[0] |> should equal  2.619752885493880

    [<Test>]
    let ``Should Get Row Kurtosis``() =
        let m = Matrix([2;10], [1.0..20.])
        m.[0, 2] <- 100.
        let res = kurtosis(m, 1)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 7.502360903579737

    [<Test>]
    let ``Should Get Col Quantiles``() =
        let m = (!![1.0..100.0]).T
        let q = !![0.1;0.5;0.9]
        let res = quantile(m, q, 0)
        res.Size.[0] |> should equal 3
        res.Size.[1] |> should equal 1
        res.[0] |> should equal 10.0
        res.[1] |> should equal 50.0

    [<Test>]
    let ``Should Get Row Quantiles``() =
        let m = !![1.0..100.0]
        let q = !![0.1;0.5;0.9]
        let res = quantile(m, q, 1)
        res.Size.[0] |> should equal 1
        res.Size.[1] |> should equal 3
        res.[0] |> should equal 10.0
        res.[1] |> should equal 50.0

    [<Test>]
    let ``Should Get 3D Quantiles``() =
        let m = repmat(!![1.0..100.0], [3;1;4])
        let q = !![0.1;0.5;0.9]
        let res = quantile(m, q, 1)
        res.Size.[0] |> should equal 3
        res.Size.[1] |> should equal 3
        res.Size.[2] |> should equal 4
        res.[0, 0, 0] |> should equal 10.0
        res.[0, 1, 0] |> should equal 50.0
        res.[0, 0, 2] |> should equal 10.0
        res.[0, 1, 2] |> should equal 50.0
        res.[1, 0, 2] |> should equal 10.0
        res.[1, 1, 2] |> should equal 50.0

    [<Test>]
    let ``Should Get Covariance``() =
        let m = Matrix([10;2], [1.0..20.])
        m.[4,0] <- 100.
        m.[1,1] <- 100.
        let res = cov(m)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        epsEqual res.[0,0] 901.1111111111111 1e-15 |> should be True
        epsEqual res.[1,0] -123.2222222222222 1e-15 |> should be True
        epsEqual res.[1,0] -123.2222222222222 1e-15 |> should be True
        epsEqual res.[1,1]  715.1222222222223 1e-15 |> should be True

    [<Test>]
    let ``Should Get Correlation``() =
        let m = Matrix([10;2], [1.0..20.])
        m.[4,0] <- 100.
        m.[1,1] <- 100.
        let res = corr(m)
        res.Size.[0] |> should equal 2
        res.Size.[1] |> should equal 2
        res.[0,0] = 1.|> should be True
        res.[1,0] =  -0.1535004763042152 |> should be True
        res.[1,0] =  -0.1535004763042152 |> should be True
        res.[1,1] =  1. |> should be True

