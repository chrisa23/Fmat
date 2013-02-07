namespace Fmat.Numerics

module GenericFloatTypes =
    open System
    open System.Collections.Generic

    type GenericFloat<'T> () =
        static member val EpsEqual : Option<'T> = None with get, set

        static member val NaN : Option<'T> = None with get, set

        static member val IsNaN : Option<'T -> bool> = None with get, set

        static member val PositiveInfinity : Option<'T> = None with get, set

        static member val NegativeInfinity : Option<'T> = None with get, set

    type GenericFloat private() =

        static do GenericFloat<float32>.EpsEqual <-  Some 1e-7f
        static do GenericFloat<float32>.NaN <-  Some Single.NaN
        static do GenericFloat<float32>.IsNaN <-  Some Single.IsNaN
        static do GenericFloat<float32>.PositiveInfinity <-  Some Single.PositiveInfinity
        static do GenericFloat<float32>.NegativeInfinity <-  Some Single.NegativeInfinity
        static do GenericFloat<float>.EpsEqual <-  Some 1e-15
        static do GenericFloat<float>.NaN <-  Some Double.NaN
        static do GenericFloat<float>.IsNaN <-  Some Double.IsNaN
        static do GenericFloat<float>.PositiveInfinity <-  Some Double.PositiveInfinity
        static do GenericFloat<float>.NegativeInfinity <-  Some Double.NegativeInfinity

        static let instance = new GenericFloat()

        static member Instance = instance

        member inline this.EpsEqual<'T when 'T : (static member IsNaN : 'T -> bool)
                                       and 'T : (static member IsInfinity : 'T -> bool)
                                       and 'T : (static member IsPositiveInfinity : 'T -> bool)
                                       and 'T : (static member IsNegativeInfinity : 'T -> bool)>() =
           match GenericFloat<'T>.EpsEqual with
             | Some e -> e
             | None -> raise (new InvalidOperationException())

        member inline this.NaN<'T when 'T : (static member IsNaN : 'T -> bool)
                                       and 'T : (static member IsInfinity : 'T -> bool)
                                       and 'T : (static member IsPositiveInfinity : 'T -> bool)
                                       and 'T : (static member IsNegativeInfinity : 'T -> bool)>() =
           match GenericFloat<'T>.NaN with
             | Some x -> x
             | None -> raise (new InvalidOperationException())

        member inline this.PositiveInfinity<'T when 'T : (static member IsNaN : 'T -> bool)
                                       and 'T : (static member IsInfinity : 'T -> bool)
                                       and 'T : (static member IsPositiveInfinity : 'T -> bool)
                                       and 'T : (static member IsNegativeInfinity : 'T -> bool)>() =
           match GenericFloat<'T>.PositiveInfinity with
             | Some x -> x
             | None -> raise (new InvalidOperationException())

        member inline this.NegativeInfinity<'T when 'T : (static member IsNaN : 'T -> bool)
                                       and 'T : (static member IsInfinity : 'T -> bool)
                                       and 'T : (static member IsPositiveInfinity : 'T -> bool)
                                       and 'T : (static member IsNegativeInfinity : 'T -> bool)>() =
           match GenericFloat<'T>.NegativeInfinity with
             | Some x -> x
             | None -> raise (new InvalidOperationException())

        member inline this.IsNaN<'T when 'T : (static member IsNaN : 'T -> bool)
                                       and 'T : (static member IsInfinity : 'T -> bool)
                                       and 'T : (static member IsPositiveInfinity : 'T -> bool)
                                       and 'T : (static member IsNegativeInfinity : 'T -> bool)>(x) =
           match GenericFloat<'T>.IsNaN with
             | Some f -> f x
             | None -> raise (new InvalidOperationException())


       



