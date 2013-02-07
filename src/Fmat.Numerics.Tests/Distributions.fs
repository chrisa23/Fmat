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
module DistributionTests =
    let n = 10000000

    [<Test>]
    let ``Should rand``() =
        let m = rand([2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        m.Size |> should equal [|2;n|]
        epsEqual a 0.5 1e-3 |> should be True
        epsEqual b (1.0/12.0) 1e-3  |> should be True

    [<Test>]
    let ``Should uniformRnd``() =
        let m = unifRnd(1.234, 2.345, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = (1.234 + 2.345)/2.
        let trueVar = 1./12.*(2.345 - 1.234)*(2.345 - 1.234)
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should normalRnd``() =
        let m = normalRnd(1.1234, 2.5432, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = 1.1234
        let trueVar = 2.5432*2.5432
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should lognormRnd``() =
        let m = lognormRnd(0.1234, 0.5432, 0.5666, 0.876, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = 0.876*(Math.Exp(0.1234 + 0.5432 * 0.5432 / 2.)) + 0.5666
        let trueVar = (Math.Exp(0.5432* 0.5432) - 1.)* Math.Exp(2.*0.1234 + 0.5432*0.5432)* 0.876* 0.876
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should BernoulliRnd``() =
        let m = bernRnd(0.232, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = 0.232
        let trueVar = 0.232 * (1. - 0.232)
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should binomialRnd``() =
        let m = binomRnd(20, 0.55, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = 20.*0.55
        let trueVar = 20.*0.55*(1. - 0.55)
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should poissonRnd``() =
        let m = poissRnd(2.243, [2;n]) : Matrix
        let a = (float)(mean(m, 1).[0])
        let b = (float)(var(m, 1).[0])
        let trueMean = 2.243
        let trueVar = 2.243
        m.Size |> should equal [|2;n|]
        epsEqual a trueMean 0.01 |> should be True
        epsEqual b trueVar 0.01 |> should be True

    [<Test>]
    let ``Should MVnormalRnd``() =
        let mn = !![2.345;2.345] : Matrix
        let cv = !![ [1.;1.]
                     [1.;2.] ] : Matrix
        let m = mvNormRnd(mn, cv, n)
        let a = (float)(mean(m, 0).[0])
        let b = cov(m)
        let trueMean = 2.345
        m.Size |> should equal [|n;2|]
        epsEqual a trueMean 0.01 |> should be True
        nearlyEqual b cv 1e-3 |> should be True

