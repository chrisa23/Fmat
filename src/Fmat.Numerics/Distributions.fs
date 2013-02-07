namespace Fmat.Numerics

open System
open System.Collections.Generic
open MatrixUtil
open Fmat.Numerics.SymmetricOperators
open Fmat.Numerics.Conversion
open Fmat.Numerics.NumericLiteralG
open Fmat.Numerics.LinearAlgebraOps

module Distributions =

    let getRandArray length a b = if a = 0.0 && b = 1.0 then RandStream.Generator.NextDoubleArray(length)
                                  else 
                                      if b < a then raise (new ArgumentException("b must be > a in uniformRnd(a,b)"))
                                      RandStream.Generator.NextDoubleArray(length) |> Array.map (fun x -> a + (b-a)*x)

    let getRand32Array length a b = if a = 0.0f && b = 1.0f then RandStream.Generator.NextSingleArray(length)
                                    else 
                                        if b < a then raise (new ArgumentException("b must be > a in uniformRnd(a,b)"))
                                        RandStream.Generator.NextSingleArray(length) |> Array.map (fun x -> a + (b-a)*x)

    let getRandIntArray length a b = 
        if b < a then raise (new ArgumentException("b must be > a in uniformRnd(a,b)"))
        RandStream.Generator.NextIntArray(length, a, b)

    let unifRnd (randGen : int -> 'T -> 'T -> 'T[]) (a :'T) (b :'T) size =
        let len = size |> Array.fold (*) 1
        let res = randGen len a b
        Managed(res)

    let inline normalRnd (randGen : int -> 'T -> 'T -> 'T[]) (mu :'T) (sigma :'T) size : matrixData<'T> =
        if sigma <= 0G then raise (new ArgumentException("Sigma must be positive in normalRnd"))
        let len = size |> Array.fold (*) 1
        let _1G = 1G
        let _2G = 2G
        let rndSeq = seq{while true do
                             let u = randGen 2 -_1G _1G
                             yield (u.[0], u.[1])
                        }
        let res = rndSeq |> Seq.map (fun (x,y) -> (x,y,x*x+y*y)) |> Seq.filter (fun (x,y,u) -> u < _1G)
                         |> Seq.map (fun (x,y,u) -> mu + sigma * (x * sqrt(-_2G*log(u)/u)))
                         |> Seq.take len |> Seq.toArray
        Managed(res)

    let inline lognormRnd (randGen : int -> 'T -> 'T -> 'T[]) (mu :'T) (sigma :'T) a beta size : matrixData<'T> =
        if sigma <= 0G then raise (new ArgumentException("Sigma must be positive in lognormalRnd"))
        let len = size |> Array.fold (*) 1
        let _1G = 1G
        let _2G = 2G
        let rndSeq = seq{while true do
                             let u = randGen 2 -_1G _1G
                             yield (u.[0], u.[1])
                        }
        let res = rndSeq |> Seq.map (fun (x,y) -> (x,y,x*x+y*y)) |> Seq.filter (fun (x,y,u) -> u < _1G)
                         |> Seq.map (fun (x,y,u) -> a + beta * exp(mu + sigma * (x * sqrt(-_2G*log(u)/u))))
                         |> Seq.take len |> Seq.toArray
        Managed(res)

    let inline bernRnd (randGen : int -> 'T -> 'T -> 'T[]) p size =
        if p < 0G || p > 1G then raise (new ArgumentException("p must be between 0 and 1 in bernRnd"))
        let len = size |> Array.fold (*) 1
        let _0G = 0G
        let _1G = 1G
        let res = randGen len _0G _1G |> Array.map (fun x -> if x < p then _1G else _0G)
        Managed(res)

    let inline binomRnd (randGen : int -> 'T -> 'T -> 'T[]) n p size =
        if p < 0G || p > 1G then raise (new ArgumentException("p must be between 0 and 1 in binomRnd"))
        if n < 0 then raise (new ArgumentException("n must be positive in binomRnd"))
        let len = size |> Array.fold (*) 1
        let _0G = 0G
        let _1G = 1G
        let res = Array.zeroCreate<'T> len
        for i in 0..n-1 do
            let bern = randGen len _0G _1G |> Array.map (fun x -> if x < p then _1G else _0G)
            for j in 0..len-1 do
                res.[j] <- res.[j] + bern.[j]
        Managed(res)

    let inline mvNormRnd (randGen : int -> 'T -> 'T -> 'T[]) (mu : matrixData<'T>) (cov : matrixData<'T>) k n : matrixData<'T> =
        let A = choleskyDecomp cov k
        let _1G = 1G
        let _2G = 2G
        let rndSeq = seq{while true do
                             let u = randGen 2 -_1G _1G
                             yield (u.[0], u.[1])
                        }
        let z = Managed( rndSeq |> Seq.map (fun (x,y) -> (x,y,x*x+y*y)) |> Seq.filter (fun (x,y,u) -> u < _1G)
                                |> Seq.map (fun (x,y,u) -> (x * sqrt(-_2G*log(u)/u)))
                                |> Seq.take (k*n) |> Seq.toArray)
        let zA =  (mulMatrix z [|n;k|] A [|k;k|] )
        match zA, mu with
          | Managed zA, Managed mu ->
              for j in 0..k-1 do
                for i in 0..n-1 do
                  zA.[j*n+i] <- zA.[j*n+i] + mu.[j]
              Managed zA
               
    let inline poissonRnd (randGen : int -> 'T -> 'T -> 'T[]) lambda size : matrixData<'T> =
        if lambda < 0G then raise (new ArgumentException("lambda must be positive poissRnd"))
        let _0G = 0G
        let _1G = 1G
        let getU() = (randGen 1 _0G _1G).[0]
        let L = exp(-lambda)
        let getPoisson() =
            let mutable k = 0
            let mutable p = _1G
            while p > L do
                k <- k + 1
                p <- p * getU()
            !!(k - 1)
        let len = size |> Array.fold (*) 1
        let res = Array.init len (fun i -> getPoisson())
        Managed res
