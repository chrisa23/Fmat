module Fmat.Numerics.MatlabTests.Util

open System
open Fmat.Numerics
open FsUnit
open NUnit.Framework
open System.Diagnostics
open MLApp
open System.Text
open Fmat.Numerics.Conversion

let getMatrixFromMatlab (app : MLAppClass, varName : string) =
    let arr = app.GetVariable(varName, "base"):?>System.Array
    let rank = arr.Rank
    match rank with
        | 2 -> new Matrix(arr:?>float[,])
        | 3 -> new Matrix(arr:?>float[,,])
        | 4 -> new Matrix(arr:?>float[,,,])
        | _ -> raise (new NotImplementedException())

let getScalarFromMatlab (app : MLAppClass, varName : string) =
    let res = app.GetVariable(varName, "base")
    System.Convert.ToDouble(res)

let getScalar32FromMatlab (app : MLAppClass, varName : string) =
    let res = app.GetVariable(varName, "base")
    System.Convert.ToSingle(res)

let getMatrix32FromMatlab (app : MLAppClass, varName : string) =
    let arr = app.GetVariable(varName, "base"):?>System.Array
    let rank = arr.Rank
    match rank with
        | 2 -> new Matrix32(arr:?>float32[,])
        | 3 -> new Matrix32(arr:?>float32[,,])
        | 4 -> new Matrix32(arr:?>float32[,,,])
        | _ -> raise (new NotImplementedException())

let resetMatlabRng (app : MLAppClass) (seed : int) =
    ignore(app.Execute(String.Format("RandStream.setDefaultStream(RandStream('mt19937ar','Seed', {0}));", seed)))

let inline epsEqual x y eps =
    if x = y then true
    else
        if x = 0G then abs(y) <= eps
        elif y = 0G then abs(x) <= eps
        else abs(x-y)/(max (abs(x)) (abs(y))) <= eps
            
let inline nearlyEqual (a : Matrix<'T,'S>) (b : Matrix<'T,'S>) (eps : 'T) =
    let A = a.ToColMajorSeq()
    let B = b.ToColMajorSeq()
    B |> Seq.zip A |> Seq.map (fun (x,y) -> epsEqual x y eps) |> Seq.fold (&&) true


