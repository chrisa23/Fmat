namespace Fmat.Numerics

module Conversion =
    open System
    open System.Collections.Generic

    let getMethod (t : Type) methName argType retType =
        let m = t.GetMethods() |> Array.filter (fun x -> x.Name = methName && x.ReturnType = retType && x.GetParameters().Length = 1 &&
                                                         x.GetParameters().[0].ParameterType = argType)
        if m.Length = 1 then Some m.[0]
        else None

    type ExplicitConverter<'T,'S>() =
        static let value : Option<'T -> 'S> = 
            let t = typeof<'T>
            let s = typeof<'S>
            seq{yield getMethod s  "op_Explicit" t s
                yield getMethod t  "op_Explicit" t s
                yield getMethod s  "op_Implicit" t s
                yield getMethod t  "op_Implicit" t s
               }
            |> Seq.tryFind (fun x -> x.IsSome)
            |> Option.map (fun mi ->
                    System.Delegate.CreateDelegate(typeof<System.Converter<'T,'S>>, mi.Value):?> System.Converter<'T,'S> |> FuncConvert.ToFSharpFunc)

        static member Value
            with get() = value

    type GenericConverter<'T,'S>() =
        static let mutable value : Option<'T -> 'S> = None

        static member Value
            with get() = match value with
                          | Some conv -> value
                          | None -> ExplicitConverter<'T,'S>.Value
            and set(v) = value <- v

    type GenericConverter private() =
        static do GenericConverter<byte, float>.Value <-  Some float
        static do GenericConverter<int, float>.Value <-  Some float
        static do GenericConverter<int16, float>.Value <-  Some float
        static do GenericConverter<int64, float>.Value <-  Some float
        static do GenericConverter<decimal, float>.Value <-  Some float
        static do GenericConverter<float32, float>.Value <-  Some float
        static do GenericConverter<float, float>.Value <-  Some float

        static do GenericConverter<byte, float32>.Value <-  Some float32
        static do GenericConverter<int, float32>.Value <-  Some float32
        static do GenericConverter<int16, float32>.Value <-  Some float32
        static do GenericConverter<int64, float32>.Value <-  Some float32
        static do GenericConverter<decimal, float32>.Value <-  Some float32
        static do GenericConverter<float32, float32>.Value <-  Some float32
        static do GenericConverter<float, float32>.Value <-  Some float32

        static do GenericConverter<byte, decimal>.Value <-  Some decimal
        static do GenericConverter<int, decimal>.Value <-  Some decimal
        static do GenericConverter<int16, decimal>.Value <-  Some decimal
        static do GenericConverter<int64, decimal>.Value <-  Some decimal
        static do GenericConverter<decimal, decimal>.Value <-  Some decimal
        static do GenericConverter<float32, decimal>.Value <-  Some decimal
        static do GenericConverter<float, decimal>.Value <-  Some decimal

        static do GenericConverter<byte, int64>.Value <-  Some int64
        static do GenericConverter<int, int64>.Value <-  Some int64
        static do GenericConverter<int16, int64>.Value <-  Some int64
        static do GenericConverter<int64, int64>.Value <-  Some int64
        static do GenericConverter<decimal, int64>.Value <-  Some int64
        static do GenericConverter<float32, int64>.Value <-  Some int64
        static do GenericConverter<float, int64>.Value <-  Some int64

        static do GenericConverter<byte, int>.Value <-  Some int
        static do GenericConverter<int, int>.Value <-  Some int
        static do GenericConverter<int16, int>.Value <-  Some int
        static do GenericConverter<int64, int>.Value <-  Some int
        static do GenericConverter<decimal, int>.Value <-  Some int
        static do GenericConverter<float32, int>.Value <-  Some int
        static do GenericConverter<float, int>.Value <-  Some int

        static do GenericConverter<byte, int16>.Value <-  Some int16
        static do GenericConverter<int, int16>.Value <-  Some int16
        static do GenericConverter<int16, int16>.Value <-  Some int16
        static do GenericConverter<int64, int16>.Value <-  Some int16
        static do GenericConverter<decimal, int16>.Value <-  Some int16
        static do GenericConverter<float32, int16>.Value <-  Some int16
        static do GenericConverter<float, int16>.Value <-  Some int16

        static do GenericConverter<byte, byte>.Value <-  Some byte
        static do GenericConverter<int, byte>.Value <-  Some byte
        static do GenericConverter<int16, byte>.Value <-  Some byte
        static do GenericConverter<int64, byte>.Value <-  Some byte
        static do GenericConverter<decimal, byte>.Value <-  Some byte
        static do GenericConverter<float32, byte>.Value <-  Some byte
        static do GenericConverter<float, byte>.Value <-  Some byte

        static let instance = new GenericConverter()

        static member Instance = instance

        member this.Convert<'T,'S>(x:'T) =
            match GenericConverter<'T,'S>.Value with
              | Some conv -> conv x
              | None -> raise (new InvalidOperationException("No explicit conversion found"))
 
    let inline (!!) (x : 'T when (^T or ^S) : (static member op_Explicit : ^T -> ^S)) : 'S = GenericConverter.Instance.Convert<'T,'S>(x)
