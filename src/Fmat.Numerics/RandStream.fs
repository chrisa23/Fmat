namespace Fmat.Numerics

open System

///<summary>Interface for types which implement random number generation. Instances can be plugged into RandStream
///</summary>
type IRandomGenerator =
    ///<summary>Generates int array filled with random numbers between a and b
    ///</summary>
    ///<param name="param0">Length of generated array</param>
    ///<param name="param1">Inclusive left end</param>
    ///<param name="param2">Exclusive right end</param>
    ///<returns>Int array filled with random numbers between a and b</returns>
    abstract member NextIntArray : int * int * int -> int[]
    ///<summary>Generates int array filled with random numbers between 0 and (exclusive) maxValue
    ///</summary>
    ///<param name="param0">Length of generated array</param>
    ///<param name="param1">Exclusive maxValue</param>
    ///<returns>Int array filled with random numbers between a and b</returns>
    abstract member NextIntArray : int * int -> int[]
    ///<summary>Generates int array filled with nonnegative random numbers
    ///</summary>
    ///<param name="param0">Length of generated array</param>
    ///<returns>Int array filled with random numbers between a and b</returns>
    abstract member NextIntArray : int -> int[]
    ///<summary>Generates double precision float array filled with numbers between 0.0 and 1.0
    ///</summary>
    ///<param name="param0">Length of generated array</param>
    ///<returns>Float array filled with random numbers between a and b</returns>
    abstract member NextDoubleArray : int -> float[]
    ///<summary>Generates single precision float array filled with numbers between 0.0f and 1.0f
    ///</summary>
    ///<param name="param0">Length of generated array</param>
    ///<returns>Float32 array filled with random numbers between a and b</returns>
    abstract member NextSingleArray : int -> float32[]
    ///<summary>Resets random number generator to new seed
    ///</summary>
    ///<param name="param0">New seed to reset random number generator</param>
    abstract member Reset : int -> unit

///<summary>Default random number generator used by RandStream. This generator uses internally System.Random.
///</summary>
///<param name="seed">Initial random number generator seed</param>
type DefaultGenerator(seed) =
    let mutable rnd = new Random(seed)
    interface IRandomGenerator with
        ///<summary>Returns int array filled with random numbers between a and b
        ///</summary>
        ///<param name="length">Length of generated array</param>
        ///<param name="a">Inclusive left end</param>
        ///<param name="b">Exclusive right end</param>
        member this.NextIntArray(length, a, b) = Array.init length (fun i -> rnd.Next(a,b))
        ///<summary>Returns int array filled with random numbers between 0 and (exclusive) maxValue
        ///</summary>
        ///<param name="length">Length of generated array</param>
        ///<param name="maxValue">Exclusive maxValue</param>
        member this.NextIntArray(length, maxValue) = Array.init length (fun i -> rnd.Next(maxValue))
        ///<summary>Returns int array filled with nonnegative random numbers
        ///</summary>
        ///<param name="length">Length of generated array</param>
        member this.NextIntArray(length) = Array.init length (fun i -> rnd.Next())
        ///<summary>Returns double precision float array filled with numbers between 0.0 and 1.0
        ///</summary>
        ///<param name="length">Length of generated array</param>
        member this.NextDoubleArray(length) = Array.init length (fun i -> rnd.NextDouble())
        ///<summary>Returns single precision float32 array filled with numbers between 0.0f and 1.0f
        ///</summary>
        ///<param name="length">Length of generated array</param>
        member this.NextSingleArray(length) = Array.init length (fun i -> rnd.NextDouble() |> float32)
        ///<summary>Resets internal random number generator to new seed
        ///</summary>
        ///<param name="newSeed">New seed to reset random number generator</param>
        member this.Reset(newSeed) = rnd <- new Random(newSeed)

///<summary>Represents a source of pseudorandom numbers
///</summary>
type RandStream() =
    ///<summary>Gets or sets current IRandomGenerator
    ///</summary>
    static member val Generator = (new DefaultGenerator(1)):>IRandomGenerator with get, set 

