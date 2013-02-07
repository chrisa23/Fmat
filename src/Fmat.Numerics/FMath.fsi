

module AssemblyInfo

namespace FMath.Numerics
  type matrixData<'T> = | Managed of 'T []

namespace FMath.Numerics
  module GenericFormatting = begin
    type GenericFormat<'T> =
      class
        new : unit -> GenericFormat<'T>
        static member Value : ('T -> string)
        static member Value : ('T -> string) with set
      end
    type GenericFormat =
      class
        private new : unit -> GenericFormat
        member GetFormat : unit -> ('T -> string)
        member SetFormat : format:('T -> string) -> unit
        static member Instance : GenericFormat
      end
  end

namespace FMath.Numerics
  module GenericFloatTypes = begin
    type GenericFloat<'T> =
      class
        new : unit -> GenericFloat<'T>
        static member EpsEqual : Option<'T>
        static member IsNaN : Option<('T -> bool)>
        static member NaN : Option<'T>
        static member NegativeInfinity : Option<'T>
        static member PositiveInfinity : Option<'T>
        static member EpsEqual : Option<'T> with set
        static member IsNaN : Option<('T -> bool)> with set
        static member NaN : Option<'T> with set
        static member NegativeInfinity : Option<'T> with set
        static member PositiveInfinity : Option<'T> with set
      end
    type GenericFloat =
      class
        private new : unit -> GenericFloat
        member
          EpsEqual : unit ->  ^T
                       when  ^T : (static member IsNaN :  ^T -> bool) and
                             ^T : (static member IsInfinity :  ^T -> bool) and
                             ^T : (static member IsPositiveInfinity :  ^T ->
                                                                        bool) and
                             ^T : (static member IsNegativeInfinity :  ^T ->
                                                                        bool)
        member
          IsNaN : x: ^T -> bool
                    when  ^T : (static member IsNaN :  ^T -> bool) and
                          ^T : (static member IsInfinity :  ^T -> bool) and
                          ^T : (static member IsPositiveInfinity :  ^T -> bool) and
                          ^T : (static member IsNegativeInfinity :  ^T -> bool)
        member
          NaN : unit ->  ^T
                  when  ^T : (static member IsNaN :  ^T -> bool) and
                        ^T : (static member IsInfinity :  ^T -> bool) and
                        ^T : (static member IsPositiveInfinity :  ^T -> bool) and
                        ^T : (static member IsNegativeInfinity :  ^T -> bool)
        member
          NegativeInfinity : unit ->  ^T
                               when  ^T : (static member IsNaN :  ^T -> bool) and
                                     ^T : (static member IsInfinity :  ^T ->
                                                                        bool) and
                                     ^T : (static member IsPositiveInfinity :  ^T
                                                                                ->
                                                                                bool) and
                                     ^T : (static member IsNegativeInfinity :  ^T
                                                                                ->
                                                                                bool)
        member
          PositiveInfinity : unit ->  ^T
                               when  ^T : (static member IsNaN :  ^T -> bool) and
                                     ^T : (static member IsInfinity :  ^T ->
                                                                        bool) and
                                     ^T : (static member IsPositiveInfinity :  ^T
                                                                                ->
                                                                                bool) and
                                     ^T : (static member IsNegativeInfinity :  ^T
                                                                                ->
                                                                                bool)
        static member Instance : GenericFloat
      end
  end

namespace FMath.Numerics
  module Conversion = begin
    val getMethod :
      t:System.Type ->
        methName:string ->
          argType:System.Type ->
            retType:System.Type -> System.Reflection.MethodInfo option
    type ExplicitConverter<'T,'S> =
      class
        new : unit -> ExplicitConverter<'T,'S>
        static member Value : Option<('T -> 'S)>
      end
    type GenericConverter<'T,'S> =
      class
        new : unit -> GenericConverter<'T,'S>
        static member Value : Option<('T -> 'S)>
        static member Value : Option<('T -> 'S)> with set
      end
    type GenericConverter =
      class
        private new : unit -> GenericConverter
        member Convert : x:'T -> 'S
        static member Instance : GenericConverter
      end
    val inline ( !! ) :
      x: ^T ->  ^S when ( ^T or  ^S) : (static member op_Explicit :  ^T ->  ^S)
  end

namespace FMath.Numerics
  module NumericLiteralG = begin
    val inline FromZero :
      unit ->  ^a when  ^a : (static member get_Zero : ->  ^a)
    val inline FromOne : unit ->  ^a when  ^a : (static member get_One : ->  ^a)
    val inline FromInt32 :
      n:int ->  ^a when (int or  ^a) : (static member op_Explicit : int ->  ^a)
    val inline FromInt64 :
      n:int64 ->  ^a
        when (int64 or  ^a) : (static member op_Explicit : int64 ->  ^a)
  end

namespace FMath.Numerics
  module SymmetricOperators = begin
    val inline ( + ) :
      x: ^T -> y: ^T ->  ^T when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T)
    val inline ( * ) :
      x: ^T -> y: ^T ->  ^T when  ^T : (static member ( * ) :  ^T *  ^T ->  ^T)
    val inline ( / ) :
      x: ^T -> y: ^T ->  ^T when  ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val inline ( - ) :
      x: ^T -> y: ^T ->  ^T when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T)
    val inline ( ~- ) :
      x: ^T ->  ^T when  ^T : (static member ( ~- ) :  ^T ->  ^T)
    val inline ( ** ) :
      x: ^T -> y: ^T ->  ^T when  ^T : (static member Pow :  ^T *  ^T ->  ^T)
  end

namespace FMath.Numerics
  type IRandomGenerator =
    interface
      abstract member NextDoubleArray : int -> float []
      abstract member NextIntArray : int * int * int -> int []
      abstract member NextIntArray : int * int -> int []
      abstract member NextIntArray : int -> int []
      abstract member NextSingleArray : int -> float32 []
      abstract member Reset : int -> unit
    end
  type DefaultGenerator =
    class
      interface IRandomGenerator
      new : seed:int -> DefaultGenerator
    end
  type RandStream =
    class
      new : unit -> RandStream
      static member Generator : IRandomGenerator
      static member Generator : IRandomGenerator with set
    end

namespace FMath.Numerics
  module MatrixUtil = begin
    val getLinSliceStartEnd :
      start:Option<int> -> finish:Option<int> -> length:int -> int * int
    val getSliceStartEnd :
      slice:(Option<int> * Option<int>) [] ->
        size:int [] -> (int * int) [] * int []
    val getMatrixDataLength : matrixData:matrixData<'a> -> int
    val matrixDataToSeq : m:matrixData<'a> -> seq<'a>
    val funMatrixScalar :
      f:('a -> 'b -> 'c) -> matrixData:matrixData<'a> -> a:'b -> matrixData<'c>
    val funScalarMatrix :
      f:('a -> 'b -> 'c) -> a:'a -> matrixData:matrixData<'b> -> matrixData<'c>
    val funMatrixMatrix :
      f:('a -> 'b -> 'c) ->
        matrixData1:matrixData<'a> ->
          matrixData2:matrixData<'b> -> matrixData<'c>
    val funMatrixMatrixMatrix :
      f:('a -> 'b -> 'c -> 'd) ->
        matrixData1:matrixData<'a> ->
          matrixData2:matrixData<'b> ->
            matrixData3:matrixData<'c> -> matrixData<'d>
    val funMatrix : f:('T -> 'S) -> matrixData:matrixData<'T> -> matrixData<'S>
    val invalidOp0Arg : unit -> 'a
    val invalidOp1Arg : x:'a -> 'b
    val invalidOp2Arg : x:'a -> y:'b -> 'c
    val invalidOp3Arg : x:'a -> y:'b -> z:'c -> 'd
    val invalidOp4Arg : x:'a -> y:'b -> z:'c -> u:'d -> 'e
    val invalidOp5Arg : x:'a -> y:'b -> z:'c -> u:'d -> t:'e -> 'f
    val cartesian : x:seq<seq<'T>> -> seq<'T []>
    val getDimProd : size:int [] -> int []
    val sub2ind : dimProd:int [] -> subscripts:int [] -> int
    val ind2sub : index:int -> size:int [] -> int []
    val getMatrixDataItem :
      dimProd:int [] -> subscripts:int [] -> matrixData:matrixData<'a> -> 'a
    val cloneMatrixData : matrixData:matrixData<'a> -> matrixData<'a>
    val setMatrixDataItem :
      dimProd:int [] ->
        subscripts:int [] -> matrixData:matrixData<'a> -> value:'a -> unit
    val getSqueezedSize : size:seq<int> -> int []
  end

namespace FMath.Numerics
  module internal Validation = begin
    val validateMatrixSize : s:seq<int> -> unit
    val validateLengthAndSizeMatch : length:int -> size:seq<int> -> unit
    val validate2D : s:int [] -> unit
    val validateSameLengths : s1:int [] -> s2:int [] -> unit
    val validateIndices : ind:int [] -> size:int [] -> unit
    val validateSeqIndices : ind:seq<int> -> len:int -> unit
    val validateSeqLength : s:seq<int> -> len:int -> unit
    val validateIndexRangeSeq : ind:seq<int> [] -> size:int [] -> unit
    val validateIndexRangeMatchesSize : ind:seq<int> [] -> size:int [] -> unit
    val validateMatrixDataLengthsMatch :
      x:matrixData<'a> -> y:matrixData<'b> -> unit
    val validateSizesMatch : s1:int [] -> s2:int [] -> unit
    val areSizesSame : s1:int [] -> s2:int [] -> bool
    val validateCanMultiply : size1:int [] -> size2:int [] -> unit
    val validateVector : size:int [] -> unit
    val validateCanConcat : sizes:seq<int []> -> dim:int -> unit
    val validateSquare2D : size:int [] -> unit
    val validateLinSolveSize : sizeA:int [] -> sizeB:int [] -> unit
    val validateDimAgainstSize : size:int [] -> dim:int -> unit
    val validateIndex : i:int -> len:int -> unit
  end

namespace FMath.Numerics
  type IGenericMatrixOps<'T> =
    interface
      abstract member ApplyFun : matrixData<'T> * ('T -> 'T) -> matrixData<'T>
      abstract member
        ApplyFun : matrixData<'T> * matrixData<'T> * ('T -> 'T -> 'T) ->
                     matrixData<'T>
      abstract member
        ApplyFun : matrixData<'T> * matrixData<'T> * matrixData<'T> *
                   ('T -> 'T -> 'T -> 'T) -> matrixData<'T>
      abstract member ApplyFunInPlace : matrixData<'T> * ('T -> 'T) -> unit
      abstract member CloneMatrixData : matrixData<'T> -> matrixData<'T>
      abstract member
        Concat : seq<matrixData<'T> * int []> * int -> matrixData<'T> * int []
      abstract member ConvertToArray : matrixData<'T> -> 'T []
      abstract member ConvertToArray2D : matrixData<'T> * int -> 'T [,]
      abstract member ConvertToArray3D : matrixData<'T> * int * int -> 'T [,,]
      abstract member
        ConvertToArray4D : matrixData<'T> * int * int * int -> 'T [,,,]
      abstract member ConvertToColMajorSeq : matrixData<'T> -> seq<'T>
      abstract member CreateMatrixData : seq<'T> -> matrixData<'T> * int
      abstract member CreateMatrixData : seq<int> * 'T -> matrixData<'T>
      abstract member CreateMatrixData : 'T [,] -> matrixData<'T> * int []
      abstract member CreateMatrixData : 'T -> matrixData<'T>
      abstract member CreateMatrixData : 'T [,,] -> matrixData<'T> * int []
      abstract member CreateMatrixData : 'T [,,,] -> matrixData<'T> * int []
      abstract member CreateMatrixData : matrixData<bool> -> matrixData<'T>
      abstract member CreateMatrixData : seq<seq<'T>> -> matrixData<'T> * int []
      abstract member CreateMatrixData : int * (int -> 'T) -> matrixData<'T>
      abstract member
        CreateMatrixData : int [] * (int -> int -> 'T) -> matrixData<'T>
      abstract member
        CreateMatrixData : int [] * (int -> int -> int -> 'T) -> matrixData<'T>
      abstract member
        CreateMatrixData : int [] * (int -> int -> int -> int -> 'T) ->
                             matrixData<'T>
      abstract member Diag : matrixData<'T> * int [] * int -> matrixData<'T>
      abstract member
        GetLinearSlice : matrixData<'T> * int * int -> matrixData<'T>
      abstract member
        GetSlice : matrixData<'T> * int [] * (int * int) [] -> matrixData<'T>
      abstract member
        Repmat : int [] * matrixData<'T> * int [] -> matrixData<'T> * int []
      abstract member Set : matrixData<'T> * seq<int> * 'T -> unit
      abstract member Set : matrixData<'T> * int [] * seq<int> [] * 'T -> unit
      abstract member Set : matrixData<'T> * matrixData<bool> * 'T -> unit
      abstract member Set : matrixData<'T> * ('T -> bool) * 'T -> unit
      abstract member
        SetDiagonal : matrixData<'T> * int -> matrixData<'T> * int []
      abstract member SetLinearSlice : matrixData<'T> * int * int * 'T -> unit
      abstract member
        SetLinearSlice : matrixData<'T> * int * int * matrixData<'T> -> unit
      abstract member
        SetSlice : matrixData<'T> * int [] * (int * int) [] * matrixData<'T> ->
                     unit
      abstract member
        SetSlice : matrixData<'T> * int [] * (int * int) [] * 'T -> unit
      abstract member
        ToString : matrixData<'T> * int [] * string * int [] -> string
      abstract member Transpose : matrixData<'T> * int [] -> matrixData<'T>
      abstract member TransposeInPlace : matrixData<'T> * int [] -> unit
      abstract member
        TriLower : int * int * matrixData<'T> * int -> matrixData<'T>
      abstract member
        TriUpper : int * int * matrixData<'T> * int -> matrixData<'T>
      abstract member Item : matrixData<'T> * int -> 'T with get
      abstract member Item : matrixData<'T> * int [] * int [] -> 'T with get
      abstract member
        Item : matrixData<'T> * seq<int> -> matrixData<'T> with get
      abstract member
        Item : int [] * matrixData<'T> * seq<int> [] -> matrixData<'T> * int []
                 with get
      abstract member
        Item : matrixData<'T> * matrixData<bool> -> matrixData<'T> with get
      abstract member
        Item : matrixData<'T> * ('T -> bool) -> matrixData<'T> with get
      abstract member Item : matrixData<'T> * int -> 'T with set
      abstract member Item : matrixData<'T> * int [] * int [] -> 'T with set
      abstract member
        Item : matrixData<'T> * seq<int> -> matrixData<'T> with set
      abstract member
        Item : int [] * matrixData<'T> * seq<int> [] -> matrixData<'T> with set
      abstract member
        Item : matrixData<'T> * matrixData<bool> -> matrixData<'T> with set
      abstract member
        Item : matrixData<'T> * ('T -> bool) -> matrixData<'T> with set
    end

namespace FMath.Numerics
  type IComparableMatrixOps<'T> =
    interface
      abstract member AllEqual : matrixData<'T> * 'T -> bool
      abstract member AllGreaterThan : matrixData<'T> * 'T -> bool
      abstract member AllGreaterThanEqual : matrixData<'T> * 'T -> bool
      abstract member AllLessThan : matrixData<'T> * 'T -> bool
      abstract member AllLessThanEqual : matrixData<'T> * 'T -> bool
      abstract member AllNotEqual : matrixData<'T> * 'T -> bool
      abstract member AreEqual : matrixData<'T> * matrixData<'T> -> bool
      abstract member EqualElementwise : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        EqualElementwise : matrixData<'T> * matrixData<'T> -> matrixData<bool>
      abstract member GreaterThan : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        GreaterThan : matrixData<'T> * matrixData<'T> -> matrixData<bool>
      abstract member GreaterThanEqual : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        GreaterThanEqual : matrixData<'T> * matrixData<'T> -> matrixData<bool>
      abstract member LessThan : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        LessThan : matrixData<'T> * matrixData<'T> -> matrixData<bool>
      abstract member LessThanEqual : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        LessThanEqual : matrixData<'T> * matrixData<'T> -> matrixData<bool>
      abstract member
        Max : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member MaxXY : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member MaxXa : matrixData<'T> * 'T -> matrixData<'T>
      abstract member
        Min : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member MinXY : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member MinXa : matrixData<'T> * 'T -> matrixData<'T>
      abstract member
        NotEqualElementwise : matrixData<'T> * 'T -> matrixData<bool>
      abstract member
        NotEqualElementwise : matrixData<'T> * matrixData<'T> ->
                                matrixData<bool>
    end

namespace FMath.Numerics
  type IBoolMatrixOps<'T> =
    interface
      abstract member And : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member And : matrixData<'T> * 'T -> matrixData<'T>
      abstract member Not : matrixData<'T> -> matrixData<'T>
      abstract member Or : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Or : matrixData<'T> * 'T -> matrixData<'T>
    end

namespace FMath.Numerics
  type INumericMatrixOps<'T> =
    interface
      abstract member ACos : matrixData<'T> -> matrixData<'T>
      abstract member ASin : matrixData<'T> -> matrixData<'T>
      abstract member ATan : matrixData<'T> -> matrixData<'T>
      abstract member Abs : matrixData<'T> -> matrixData<'T>
      abstract member Add : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Add : matrixData<'T> * 'T -> matrixData<'T>
      abstract member BernRnd : 'T * int [] -> matrixData<'T>
      abstract member BinomRnd : int * 'T * int [] -> matrixData<'T>
      abstract member Ceil : matrixData<'T> -> matrixData<'T>
      abstract member CholeskyDecomp : matrixData<'T> * int -> matrixData<'T>
      abstract member
        CholeskySolve : matrixData<'T> * int * matrixData<'T> * int ->
                          matrixData<'T>
      abstract member Correlation : matrixData<'T> * int * int -> matrixData<'T>
      abstract member Cos : matrixData<'T> -> matrixData<'T>
      abstract member Cosh : matrixData<'T> -> matrixData<'T>
      abstract member Covariance : matrixData<'T> * int * int -> matrixData<'T>
      abstract member CumProd : matrixData<'T> * int [] * int -> matrixData<'T>
      abstract member CumSum : matrixData<'T> * int [] * int -> matrixData<'T>
      abstract member Div : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Div : matrixData<'T> * 'T -> matrixData<'T>
      abstract member Div : 'T * matrixData<'T> -> matrixData<'T>
      abstract member Erf : matrixData<'T> -> matrixData<'T>
      abstract member Erfc : matrixData<'T> -> matrixData<'T>
      abstract member Erfcinv : matrixData<'T> -> matrixData<'T>
      abstract member Erfinv : matrixData<'T> -> matrixData<'T>
      abstract member Exp : matrixData<'T> -> matrixData<'T>
      abstract member Identity : int * int -> matrixData<'T>
      abstract member
        Kurtosis : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member Log : matrixData<'T> -> matrixData<'T>
      abstract member Log10 : matrixData<'T> -> matrixData<'T>
      abstract member
        LognormalRnd : 'T * 'T * 'T * 'T * int [] -> matrixData<'T>
      abstract member
        LuDecomp : matrixData<'T> * int * int ->
                     matrixData<'T> * int [] * matrixData<'T> * int [] * int []
      abstract member
        LuSolve : matrixData<'T> * int * matrixData<'T> * int -> matrixData<'T>
      abstract member
        MVnormalRnd : matrixData<'T> * matrixData<'T> * int * int ->
                        matrixData<'T>
      abstract member
        Mean : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member Minus : matrixData<'T> -> matrixData<'T>
      abstract member Mul : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Mul : matrixData<'T> * 'T -> matrixData<'T>
      abstract member
        MulMatrix : matrixData<'T> * int [] * matrixData<'T> * int [] ->
                      matrixData<'T>
      abstract member NormalRnd : 'T * 'T * int [] -> matrixData<'T>
      abstract member Normcdf : matrixData<'T> -> matrixData<'T>
      abstract member Norminv : matrixData<'T> -> matrixData<'T>
      abstract member Ones : int [] -> matrixData<'T>
      abstract member PoissonRnd : 'T * int [] -> matrixData<'T>
      abstract member Pow : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Pow : matrixData<'T> * 'T -> matrixData<'T>
      abstract member Pow : 'T * matrixData<'T> -> matrixData<'T>
      abstract member
        Prod : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member
        QrDecomp : matrixData<'T> * int * int ->
                     matrixData<'T> * int [] * matrixData<'T> * int []
      abstract member
        QrSolveFull : matrixData<'T> * int * int * matrixData<'T> * int ->
                        matrixData<'T>
      abstract member
        Quantiles : matrixData<'T> * int [] * matrixData<'T> * int ->
                      matrixData<'T> * int []
      abstract member Round : matrixData<'T> -> matrixData<'T>
      abstract member Sin : matrixData<'T> -> matrixData<'T>
      abstract member Sinh : matrixData<'T> -> matrixData<'T>
      abstract member
        Skewness : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member Sqrt : matrixData<'T> -> matrixData<'T>
      abstract member Sub : matrixData<'T> * matrixData<'T> -> matrixData<'T>
      abstract member Sub : matrixData<'T> * 'T -> matrixData<'T>
      abstract member Sub : 'T * matrixData<'T> -> matrixData<'T>
      abstract member
        Sum : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member
        SvdDecomp : matrixData<'T> * int * int ->
                      matrixData<'T> * int [] * matrixData<'T> * int [] *
                      matrixData<'T> * int []
      abstract member
        SvdSolve : matrixData<'T> * int * int * matrixData<'T> * int * 'T ->
                     matrixData<'T> * int
      abstract member Tan : matrixData<'T> -> matrixData<'T>
      abstract member Tanh : matrixData<'T> -> matrixData<'T>
      abstract member UnifRnd : 'T * 'T * int [] -> matrixData<'T>
      abstract member
        Variance : matrixData<'T> * int [] * int -> matrixData<'T> * int []
      abstract member Zeros : int [] -> matrixData<'T>
    end

namespace FMath.Numerics
  type IMatrixOps<'T> =
    interface
      abstract member BoolMatrixOps : IBoolMatrixOps<'T>
      abstract member ComparableMatrixOps : IComparableMatrixOps<'T>
      abstract member GenericMatrixOps : IGenericMatrixOps<'T>
      abstract member NumericMatrixOps : INumericMatrixOps<'T>
    end

namespace FMath.Numerics
  type __BoolMatrix =
    class
      new : size:seq<int> * data:matrixData<bool> -> __BoolMatrix
      member Data : matrixData<bool>
      member Size : int []
    end
  type Matrix<'T,'S when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>> =
    class
      interface System.IFormattable
      new : data:'T [,] -> Matrix<'T,'S>
      new : data:'T [,,] -> Matrix<'T,'S>
      new : data:'T [,,,] -> Matrix<'T,'S>
      new : data:seq<'T> -> Matrix<'T,'S>
      new : data:seq<'T list> -> Matrix<'T,'S>
      new : data:seq<seq<'T>> -> Matrix<'T,'S>
      new : data:seq<'T []> -> Matrix<'T,'S>
      new : data:'T -> Matrix<'T,'S>
      new : boolMatrix:__BoolMatrix -> Matrix<'T,'S>
      new : size:seq<int> * data:matrixData<'T> -> Matrix<'T,'S>
      new : size:seq<int> * data:seq<'T> -> Matrix<'T,'S>
      new : size:seq<int> * init:'T -> Matrix<'T,'S>
      new : size:seq<int> * initializer:(int -> 'T) -> Matrix<'T,'S>
      new : size:seq<int> * initializer:(int -> int -> 'T) -> Matrix<'T,'S>
      new : size:seq<int> * initializer:(int -> int -> int -> 'T) ->
              Matrix<'T,'S>
      new : size:seq<int> * initializer:(int -> int -> int -> int -> 'T) ->
              Matrix<'T,'S>
      member ApplyFun : f:('T -> 'T) -> unit
      member GetSlice : start:Option<int> * finish:int option -> Matrix<'T,'S>
      member
        GetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> -> Matrix<'T,'S>
      member
        GetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> ->
                     Matrix<'T,'S>
      member
        GetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> *
                   start3:Option<int> * end3:Option<int> -> Matrix<'T,'S>
      member Reshape : size:seq<int> -> unit
      member Set : indices:seq<int> * value:'T -> unit
      member Set : indexSeqs:seq<seq<int>> * value:'T -> unit
      member Set : indexSeqs:seq<int []> * value:'T -> unit
      member Set : indexSeqs:seq<int list> * value:'T -> unit
      member Set : boolMatrix:__BoolMatrix * value:Matrix<'T,'S> -> unit
      member Set : predicate:('T -> bool) * value:Matrix<'T,'S> -> unit
      member Set : s0:seq<int> * s1:seq<int> * value:'T -> unit
      member Set : s0:seq<int> * s1:seq<int> * s2:seq<int> * value:'T -> unit
      member
        Set : s0:seq<int> * s1:seq<int> * s2:seq<int> * s3:seq<int> * value:'T ->
                unit
      member
        SetSlice : start:Option<int> * finish:int option * value:Matrix<'T,'S> ->
                     unit
      member SetSlice : start:Option<int> * finish:int option * value:'T -> unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * value:Matrix<'T,'S> -> unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * value:'T -> unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> *
                   value:Matrix<'T,'S> -> unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> *
                   value:'T -> unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> *
                   start3:Option<int> * end3:Option<int> * value:Matrix<'T,'S> ->
                     unit
      member
        SetSlice : start0:Option<int> * end0:Option<int> * start1:Option<int> *
                   end1:Option<int> * start2:Option<int> * end2:Option<int> *
                   start3:Option<int> * end3:Option<int> * value:'T -> unit
      member ToColMajorSeq : unit -> seq<'T>
      override ToString : unit -> string
      member Transpose : unit -> unit
      member Data : matrixData<'T>
      member Diag : ?k:int -> Matrix<'T,'S> with get
      member IsScalar : bool
      member IsVector : bool
      member Item : i:int -> 'T with get
      member Item : indices:int [] -> 'T with get
      member Item : indices:seq<int> -> Matrix<'T,'S> with get
      member Item : params indexSeqs:seq<int> [] -> Matrix<'T,'S> with get
      member Item : boolMatrix:__BoolMatrix -> Matrix<'T,'S> with get
      member Item : predicate:('T -> bool) -> Matrix<'T,'S> with get
      member Item : i:int * j:int -> 'T with get
      member Item : s0:seq<int> * s1:seq<int> -> Matrix<'T,'S> with get
      member Item : i:int * j:int * k:int -> 'T with get
      member
        Item : s0:seq<int> * s1:seq<int> * s2:seq<int> -> Matrix<'T,'S> with get
      member Item : i:int * j:int * k:int * l:int -> 'T with get
      member
        Item : s0:seq<int> * s1:seq<int> * s2:seq<int> * s3:seq<int> ->
                 Matrix<'T,'S> with get
      member Length : int
      member NDims : int
      member Size : int []
      member T : Matrix<'T,'S>
      member Item : i:int -> 'T with set
      member Item : indices:int [] -> 'T with set
      member Item : indices:seq<int> -> Matrix<'T,'S> with set
      member Item : params indexSeqs:seq<int> [] -> Matrix<'T,'S> with set
      member Item : boolMatrix:__BoolMatrix -> 'T with set
      member Item : predicate:('T -> bool) -> 'T with set
      member Item : i:int * j:int -> 'T with set
      member Item : s0:seq<int> * s1:seq<int> -> Matrix<'T,'S> with set
      member Item : i:int * j:int * k:int -> 'T with set
      member
        Item : s0:seq<int> * s1:seq<int> * s2:seq<int> -> Matrix<'T,'S> with set
      member Item : i:int * j:int * k:int * l:int -> 'T with set
      member
        Item : s0:seq<int> * s1:seq<int> * s2:seq<int> * s3:seq<int> ->
                 Matrix<'T,'S> with set
      static member Abs : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Acos : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Asin : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Atan : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Ceiling : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Cos : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Cosh : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Erf : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Erfc : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Erfcinv : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Erfinv : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Exp : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Identity : rows:int * cols:int -> Matrix<'T,'S>
      static member Log : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Log10 : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Normcdf : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Norminv : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Pow : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        Pow : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Round : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Sin : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Sinh : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Sqrt : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Tan : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member Tanh : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        applyFun : matrix:Matrix<'T,'S> * f:('T -> 'T) -> Matrix<'T,'S>
      static member
        applyFun2Arg : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> *
                       f:('T -> 'T -> 'T) -> Matrix<'T,'S>
      static member
        applyFun3Arg : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> *
                       matrix3:Matrix<'T,'S> * f:('T -> 'T -> 'T -> 'T) ->
                         Matrix<'T,'S>
      static member bernRnd : p:'T * size:seq<int> -> Matrix<'T,'S>
      static member binomRnd : n:int * p:'T * size:seq<int> -> Matrix<'T,'S>
      static member chol : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        cholSolve : a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        concat : matrices:seq<Matrix<'T,'S>> * dim:int -> Matrix<'T,'S>
      static member corr : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member cov : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member cumprod : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member cumsum : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member diag : matrix:Matrix<'T,'S> * offset:int -> Matrix<'T,'S>
      static member DisplayFormat : string
      static member Empty : Matrix<'T,'S>
      static member MaxDisplaySize : int []
      static member horzConcat : matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
      static member kurtosis : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member
        lognormRnd : mean:'T * sigma:'T * a:'T * scale:'T * size:seq<int> ->
                       Matrix<'T,'S>
      static member
        lu : matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S> * int []
      static member luSolve : a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
      static member max : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member
        maxXY : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member maxXY : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member maxXY : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member mean : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member min : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member
        minXY : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member minXY : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member minXY : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        mvNormRnd : mu:Matrix<'T,'S> * cov:Matrix<'T,'S> * n:int ->
                      Matrix<'T,'S>
      static member
        normalRnd : mean:'T * sigma:'T * size:seq<int> -> Matrix<'T,'S>
      static member not : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ones : size:seq<int> -> Matrix<'T,'S>
      static member
        ( + ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( + ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( + ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member ( &!= ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &!= ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &= ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &= ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &> ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &> ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &>= ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &>= ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &< ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &< ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &<= ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &<= ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( &<> ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( &<> ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member
        ( != ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> bool
      static member ( != ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( != ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member ( / ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member ( / ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        ( .&& ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .&& ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member ( .&& ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .!= ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .!= ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .!= ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .|| ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .|| ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member ( .|| ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        ( ./ ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( ./ ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member ( ./ ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .= ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .= ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .= ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .== ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .== ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .== ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .> ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .> ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .> ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .>= ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .>= ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .>= ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .^ ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .< ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .< ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .< ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .<= ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .<= ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .<= ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .<> ) : matrix:Matrix<'T,'S> * a:'T -> __BoolMatrix
      static member ( .<> ) : a:'T * matrix:Matrix<'T,'S> -> __BoolMatrix
      static member
        ( .<> ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> __BoolMatrix
      static member ( .- ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .- ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        ( .- ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .* ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .* ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        ( .* ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        ( .+ ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .+ ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( .+ ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        ( == ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> bool
      static member ( == ) : matrix:Matrix<'T,'S> * a:'T -> bool
      static member ( == ) : a:'T * matrix:Matrix<'T,'S> -> bool
      static member op_Explicit : matrix:Matrix<'T,'S> -> 'T
      static member op_Explicit : matrix:Matrix<'T,'S> -> 'T []
      static member op_Explicit : matrix:Matrix<'T,'S> -> 'T [,]
      static member op_Explicit : matrix:Matrix<'T,'S> -> 'T [,,]
      static member op_Explicit : matrix:Matrix<'T,'S> -> 'T [,,,]
      static member op_Explicit : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member op_Explicit : data:seq<seq<'T>> -> Matrix<'T,'S>
      static member op_Explicit : data:seq<'T list> -> Matrix<'T,'S>
      static member op_Explicit : data:seq<'T array> -> Matrix<'T,'S>
      static member op_Explicit : data:seq<'T> list -> Matrix<'T,'S>
      static member op_Explicit : data:'T list list -> Matrix<'T,'S>
      static member op_Explicit : data:'T array list -> Matrix<'T,'S>
      static member op_Explicit : data:seq<'T> array -> Matrix<'T,'S>
      static member op_Explicit : data:'T list array -> Matrix<'T,'S>
      static member op_Explicit : data:'T array array -> Matrix<'T,'S>
      static member op_Explicit : data:seq<'T> -> Matrix<'T,'S>
      static member op_Explicit : data:'T list -> Matrix<'T,'S>
      static member op_Explicit : data:'T array -> Matrix<'T,'S>
      static member op_Explicit : data:'T [,] -> Matrix<'T,'S>
      static member op_Explicit : data:'T [,,] -> Matrix<'T,'S>
      static member op_Explicit : data:'T [,,,] -> Matrix<'T,'S>
      static member op_Explicit : data:'T -> Matrix<'T,'S>
      static member ( * ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( * ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        ( * ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( - ) : a:'T * matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( - ) : matrix:Matrix<'T,'S> * a:'T -> Matrix<'T,'S>
      static member
        ( - ) : matrix1:Matrix<'T,'S> * matrix2:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( ~~ ) : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member ( ~- ) : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member poissRnd : lambda:'T * size:seq<int> -> Matrix<'T,'S>
      static member prod : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member qr : matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S>
      static member
        qrSolveFull : a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
      static member
        quantile : matrix:Matrix<'T,'S> * q:Matrix<'T,'S> * dim:int ->
                     Matrix<'T,'S>
      static member
        repmat : matrix:Matrix<'T,'S> * replicator:seq<int> -> Matrix<'T,'S>
      static member
        reshape : matrix:Matrix<'T,'S> * size:seq<int> -> Matrix<'T,'S>
      static member DisplayDigits : int with set
      static member DisplayFormat : string with set
      static member MaxDisplaySize : int [] with set
      static member skewness : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member sum : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member
        svd : matrix:Matrix<'T,'S> ->
                Matrix<'T,'S> * Matrix<'T,'S> * Matrix<'T,'S>
      static member
        svdSolve : a:Matrix<'T,'S> * b:Matrix<'T,'S> * tol:'T ->
                     Matrix<'T,'S> * int
      static member transpose : matrix:Matrix<'T,'S> -> Matrix<'T,'S>
      static member triL : matrix:Matrix<'T,'S> * offset:int -> Matrix<'T,'S>
      static member triU : matrix:Matrix<'T,'S> * offset:int -> Matrix<'T,'S>
      static member unifRnd : a:'T * b:'T * size:seq<int> -> Matrix<'T,'S>
      static member var : matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
      static member vertConcat : matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
      static member zeros : size:seq<int> -> Matrix<'T,'S>
    end

namespace FMath.Numerics
  type GenericMatrixOpsRec<'T> =
    {CreateMatrixDataFromSeq: seq<'T> -> matrixData<'T> * int;
     CreateMatrixDataFromScalar: 'T -> matrixData<'T>;
     CreateMatrixDataFromSizeScalar: seq<int> -> 'T -> matrixData<'T>;
     CreateMatrixDataFromArray2D: 'T [,] -> matrixData<'T> * int [];
     CreateMatrixDataFromArray3D: 'T [,,] -> matrixData<'T> * int [];
     CreateMatrixDataFromArray4D: 'T [,,,] -> matrixData<'T> * int [];
     CreateMatrixDataFromBool: matrixData<bool> -> matrixData<'T>;
     CreateMatrixDataFromSeqSeq: seq<seq<'T>> -> matrixData<'T> * int [];
     CreateMatrixDataFrom1DFun: int -> (int -> 'T) -> matrixData<'T>;
     CreateMatrixDataFrom2DFun: int [] -> (int -> int -> 'T) -> matrixData<'T>;
     CreateMatrixDataFrom3DFun:
       int [] -> (int -> int -> int -> 'T) -> matrixData<'T>;
     CreateMatrixDataFrom4DFun:
       int [] -> (int -> int -> int -> int -> 'T) -> matrixData<'T>;
     CloneMatrixData: matrixData<'T> -> matrixData<'T>;
     ConvertToArray: matrixData<'T> -> 'T [];
     ConvertToArray2D: matrixData<'T> -> int -> 'T [,];
     ConvertToArray3D: matrixData<'T> -> int -> int -> 'T [,,];
     ConvertToArray4D: matrixData<'T> -> int -> int -> int -> 'T [,,,];
     ConvertToColMajorSeq: matrixData<'T> -> seq<'T>;
     TransposeMatrixData: matrixData<'T> -> int [] -> matrixData<'T>;
     TransposeMatrixDataInPlace: matrixData<'T> -> int [] -> unit;
     GetLinearSlice: matrixData<'T> -> int -> int -> matrixData<'T>;
     SetLinearSliceScalar: matrixData<'T> -> int -> int -> 'T -> unit;
     SetLinearSlice: matrixData<'T> -> int -> int -> matrixData<'T> -> unit;
     GetSlice: matrixData<'T> -> int [] -> (int * int) [] -> matrixData<'T>;
     SetSliceScalar: matrixData<'T> -> int [] -> (int * int) [] -> 'T -> unit;
     SetSlice:
       matrixData<'T> -> int [] -> (int * int) [] -> matrixData<'T> -> unit;
     GetItemLinear: matrixData<'T> -> int -> 'T;
     SetItemLinear: matrixData<'T> -> int -> 'T -> unit;
     GetItem: matrixData<'T> -> int [] -> int [] -> 'T;
     SetItem: matrixData<'T> -> int [] -> int [] -> 'T -> unit;
     GetItemsLinear: matrixData<'T> -> seq<int> -> matrixData<'T>;
     SetItemsLinear: matrixData<'T> -> seq<int> -> matrixData<'T> -> unit;
     GetItems:
       matrixData<'T> -> int [] -> seq<int> [] -> matrixData<'T> * int [];
     SetItems: matrixData<'T> -> int [] -> seq<int> [] -> matrixData<'T> -> unit;
     GetBoolItems: matrixData<'T> -> matrixData<bool> -> matrixData<'T>;
     SetBoolItems: matrixData<'T> -> matrixData<bool> -> matrixData<'T> -> unit;
     SetItemsLinearScalar: matrixData<'T> -> seq<int> -> 'T -> unit;
     SetItemsScalar: matrixData<'T> -> int [] -> seq<int> [] -> 'T -> unit;
     SetBoolItemsScalar: matrixData<'T> -> matrixData<bool> -> 'T -> unit;
     GetPredicateItems: matrixData<'T> -> ('T -> bool) -> matrixData<'T>;
     SetPredicateItems: matrixData<'T> -> ('T -> bool) -> matrixData<'T> -> unit;
     SetPredicateItemsScalar: matrixData<'T> -> ('T -> bool) -> 'T -> unit;
     GetDiag: matrixData<'T> -> int [] -> int -> matrixData<'T>;
     FormatMatrixData: matrixData<'T> -> int [] -> string -> int [] -> string;
     Concat: seq<matrixData<'T> * int []> -> int -> matrixData<'T> * int [];
     SetDiagonal: matrixData<'T> -> int -> matrixData<'T> * int [];
     TriLower: int -> int -> matrixData<'T> -> int -> matrixData<'T>;
     TriUpper: int -> int -> matrixData<'T> -> int -> matrixData<'T>;
     Repmat: matrixData<'T> -> int [] -> int [] -> matrixData<'T> * int [];
     ApplyFunInPlace: matrixData<'T> -> ('T -> 'T) -> unit;
     ApplyFun1Arg: matrixData<'T> -> ('T -> 'T) -> matrixData<'T>;
     ApplyFun2Arg:
       matrixData<'T> -> matrixData<'T> -> ('T -> 'T -> 'T) -> matrixData<'T>;
     ApplyFun3Arg:
       matrixData<'T> -> matrixData<'T> -> matrixData<'T> ->
         ('T -> 'T -> 'T -> 'T) -> matrixData<'T>;}
  module GenericMatrixOps = begin
    val createMatrixDataFromSeq : data:seq<'T> -> matrixData<'T> * int
    val createMatrixDataFromSeqSeq :
      data:seq<seq<'T>> -> matrixData<'T> * int []
    val createMatrixDataFromScalar : scalar:'T -> matrixData<'T>
    val createMatrixDataFromSizeScalar :
      size:seq<int> -> scalar:'T -> matrixData<'T>
    val createMatrixDataFromArray2D : data:'T [,] -> matrixData<'T> * int []
    val createMatrixDataFromArray3D : data:'T [,,] -> matrixData<'T> * int []
    val createMatrixDataFromArray4D : data:'T [,,,] -> matrixData<'T> * int []
    val createMatrixDataFrom1DFun : len:int -> f:(int -> 'a) -> matrixData<'a>
    val createMatrixDataFrom2DFun :
      size:int [] -> f:(int32 -> int32 -> 'T) -> matrixData<'T>
    val createMatrixDataFrom3DFun :
      size:int [] -> f:(int32 -> int32 -> int32 -> 'T) -> matrixData<'T>
    val createMatrixDataFrom4DFun :
      size:int [] ->
        f:(int32 -> int32 -> int32 -> int32 -> 'T) -> matrixData<'T>
    val cloneMatrixData : matrixData:matrixData<'T> -> matrixData<'T>
    val convertToArray : matrixData:matrixData<'a> -> 'a []
    val convertToArray2D : matrixData:matrixData<'a> -> rows:int -> 'a [,]
    val convertToArray3D :
      matrixData:matrixData<'a> -> rows:int -> cols:int -> 'a [,,]
    val convertToArray4D :
      matrixData:matrixData<'a> -> rows:int -> cols:int -> pages:int -> 'a [,,,]
    val convertToColMajorSeq : matrixData:matrixData<'a> -> seq<'a>
    val transposeMatrixData :
      matrixData:matrixData<'T> -> size:int [] -> matrixData<'T>
    val transposeMatrixDataInPlace :
      matrixData:matrixData<'T> -> size:int [] -> unit
    val getLinearSlice :
      matrixData:matrixData<'T> -> start:int -> finish:int -> matrixData<'T>
    val setLinearSliceScalar :
      matrixData:matrixData<'a> -> start:int -> finish:int -> value:'a -> unit
    val setLinearSlice :
      matrixData:matrixData<'a> ->
        start:int -> finish:int -> value:matrixData<'a> -> unit
    val getSlice :
      matrixData:matrixData<'T> ->
        size:int [] -> slice:(int * int) [] -> matrixData<'T>
    val setSliceScalar :
      matrixData:matrixData<'a> ->
        size:int [] -> slice:(int * int) [] -> value:'a -> unit
    val setSlice :
      matrixData:matrixData<'a> ->
        size:int [] -> slice:(int * int) [] -> value:matrixData<'a> -> unit
    val getItemLinear : matrixData:matrixData<'a> -> index:int -> 'a
    val setItemLinear :
      matrixData:matrixData<'a> -> index:int -> value:'a -> unit
    val getItem :
      matrixData:matrixData<'a> -> size:int [] -> indices:int [] -> 'a
    val setItem :
      matrixData:matrixData<'a> ->
        size:int [] -> indices:int [] -> value:'a -> unit
    val getItemsLinear :
      matrixData:matrixData<'T> -> indices:seq<int> -> matrixData<'T>
    val setItemsLinear :
      matrixData:matrixData<'a> ->
        indices:seq<int> -> value:matrixData<'a> -> unit
    val getItems :
      matrixData:matrixData<'a> ->
        size:int [] -> range:seq<int> [] -> matrixData<'a> * int []
    val setItems :
      matrixData:matrixData<'a> ->
        size:int [] -> range:seq<int> [] -> value:matrixData<'a> -> unit
    val getBoolItems :
      matrixData:matrixData<'a> ->
        boolMatrixData:matrixData<bool> -> matrixData<'a>
    val setBoolItems :
      matrixData:matrixData<'a> ->
        boolMatrixData:matrixData<bool> -> value:matrixData<'a> -> unit
    val setItemsLinearScalar :
      matrixData:matrixData<'a> -> indices:seq<int> -> value:'a -> unit
    val setItemsScalar :
      matrixData:matrixData<'a> ->
        size:int [] -> range:seq<int> [] -> value:'a -> unit
    val setBoolItemsScalar :
      matrixData:matrixData<'a> ->
        boolMatrixData:matrixData<bool> -> value:'a -> unit
    val getPredicateItems :
      matrixData:matrixData<'a> -> predicate:('a -> bool) -> matrixData<'a>
    val setPredicateItemsScalar :
      matrixData:matrixData<'a> -> predicate:('a -> bool) -> value:'a -> unit
    val setPredicateItems :
      matrixData:matrixData<'a> ->
        predicate:('a -> bool) -> value:matrixData<'a> -> unit
    val getDiag :
      matrixData:matrixData<'T> -> size:int [] -> offset:int -> matrixData<'T>
    val formatMatrixData :
      matrixData:matrixData<'T> ->
        size:int [] -> format:string -> displaySize:int [] -> string
    val concat :
      matrices:seq<matrixData<'T> * int []> ->
        dim:int -> matrixData<'T> * int []
    val setDiagonal :
      matrixData:matrixData<'a> -> offset:int -> matrixData<'a> * int []
    val triLower :
      rows:int ->
        cols:int -> matrixData:matrixData<'a> -> offset:int -> matrixData<'a>
    val triUpper :
      rows:int ->
        cols:int -> matrixData:matrixData<'a> -> offset:int -> matrixData<'a>
    val repmat :
      matrixData:matrixData<'a> ->
        size:int [] -> replicators:int [] -> matrixData<'a> * int []
    val applyFunInPlace : matrixData:matrixData<'a> -> f:('a -> 'a) -> unit
    val applyFun1Arg :
      matrixData:matrixData<'a> -> f:('a -> 'b) -> matrixData<'b>
    val applyFun2Arg :
      matrixData1:matrixData<'a> ->
        matrixData2:matrixData<'b> -> f:('a -> 'b -> 'c) -> matrixData<'c>
    val applyFun3Arg :
      matrixData1:matrixData<'a> ->
        matrixData2:matrixData<'b> ->
          matrixData3:matrixData<'c> ->
            f:('a -> 'b -> 'c -> 'd) -> matrixData<'d>
    val inline boolToArithmetic :
      x:bool ->  ^T
        when  ^T : (static member get_One : ->  ^T) and
              ^T : (static member get_Zero : ->  ^T)
    val genericMatrixOpsRec :
      boolConverter:(bool -> 'T) -> GenericMatrixOpsRec<'T>
    val noneBoolConversionGenericMatrixOpsRec<'T> : GenericMatrixOpsRec<'T>
    val inline arithmeticGenericMatrixOpsRec< ^T
                                               when  ^T : (static member
                                                           get_Zero : ->  ^T) and
                                                     ^T : (static member get_One : ->
                                                                                      ^T)>
          :
      GenericMatrixOpsRec< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T)
    val createGenericMatrixOps :
      genOpsRec:GenericMatrixOpsRec<'T> -> IGenericMatrixOps<'T>
  end

namespace FMath.Numerics
  type ComparableMatrixOpsRec<'T> =
    {AreEqual: matrixData<'T> -> matrixData<'T> -> bool;
     AllEqual: matrixData<'T> -> 'T -> bool;
     AllNotEqual: matrixData<'T> -> 'T -> bool;
     AllLessThan: matrixData<'T> -> 'T -> bool;
     AllLessThanEqual: matrixData<'T> -> 'T -> bool;
     AllGreaterThan: matrixData<'T> -> 'T -> bool;
     AllGreaterThanEqual: matrixData<'T> -> 'T -> bool;
     LessThanScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     LessThanEqualScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     GreaterThanScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     GreaterThanEqualScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     EqualElementwiseScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     NotEqualElementwiseScalar: matrixData<'T> -> 'T -> matrixData<bool>;
     LessThan: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     LessThanEqual: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     GreaterThan: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     GreaterThanEqual: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     EqualElementwise: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     NotEqualElementwise: matrixData<'T> -> matrixData<'T> -> matrixData<bool>;
     MinXY: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     MaxXY: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     MinXa: matrixData<'T> -> 'T -> matrixData<'T>;
     MaxXa: matrixData<'T> -> 'T -> matrixData<'T>;
     Min: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     Max: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];}
  module ComparableMatrixOps = begin
    val allMeetCondition :
      cond:('T -> 'T -> bool) -> matrixData:matrixData<'T> -> a:'T -> bool
    val compareElementwiseScalar :
      cond:('T -> 'T -> bool) ->
        matrixData:matrixData<'T> -> a:'T -> matrixData<bool>
    val compareElementwise :
      cond:('T -> 'T -> bool) ->
        matrixData1:matrixData<'T> ->
          matrixData2:matrixData<'T> -> matrixData<bool>
    val areEqual :
      comp:('T -> 'T -> bool) ->
        matrixData1:matrixData<'T> -> matrixData2:matrixData<'T> -> bool
    val foldNdim :
      comp:('T -> 'T -> 'T) ->
        matrixData:matrixData<'T> ->
          size:int [] -> dim:int -> matrixData<'T> * int []
    val noneComparableMatrixOpsRec<'T> : ComparableMatrixOpsRec<'T>
    val inline compMatrixOpsRec<'T when 'T : comparison> :
      ComparableMatrixOpsRec<'T> when 'T : comparison
    val createComparableMatrixOps :
      compOpsRec:ComparableMatrixOpsRec<'T> -> IComparableMatrixOps<'T>
  end

namespace FMath.Numerics
  type BoolMatrixOpsRec<'T> =
    {And: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     Or: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     AndScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     OrScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Not: matrixData<'T> -> matrixData<'T>;}
  module BoolMatrixOps = begin
    val boolMatrixOpsRec : BoolMatrixOpsRec<bool>
    val noneBoolMatrixOpsRec<'T> : BoolMatrixOpsRec<'T>
    val createBoolMatrixOps :
      boolOpsRec:BoolMatrixOpsRec<'T> -> IBoolMatrixOps<'T>
  end

namespace FMath.Numerics
  module LinearAlgebraOps = begin
    val inline eps :
      unit ->  ^a
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool)
    val inline nearlyEqual :
      x: ^a -> y: ^a -> eps: ^a -> bool
        when  ^a : (static member Abs :  ^a ->  ^a) and
              ^a : (static member ( - ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( / ) :  ^a *  ^a ->  ^a) and
              ^a : (static member get_Zero : ->  ^a) and  ^a : comparison
    val inline drotg :
      da: ^a -> db: ^a ->  ^a *  ^a *  ^a *  ^a
        when  ^a : (static member get_One : ->  ^a) and
              ^a : (static member ( * ) :  ^a *  ^a ->  ^a) and
              ^a : (static member get_Zero : ->  ^a) and
              ^a : (static member ( / ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( + ) :  ^a *  ^a ->  ^a) and
              ^a : (static member Abs :  ^a ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^a) and
              ^a : (static member ( ~- ) :  ^a ->  ^a) and  ^a : comparison
    val inline mulMatrix :
      matrixData1:matrixData< ^T> ->
        size1:int [] ->
          matrixData2:matrixData< ^T> -> size2:int [] -> matrixData< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T)
    val inline choleskyDecomp :
      matrixData:matrixData< ^T> -> n:int -> matrixData< ^T>
        when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val inline choleskySolve :
      a:matrixData< ^T> ->
        n:int -> b:matrixData< ^T> -> k:int -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member Sqrt :  ^T ->  ^T)
    val choleskyInverse : matrixData:matrixData<'T> -> n:int -> matrixData<'T>
    val inline private luipiv :
      matrixData:matrixData< ^T> -> n:int -> matrixData< ^T> * int []
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Abs :  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val inline luDecomp :
      matrixData:matrixData< ^T> ->
        n:int ->
          k:int -> matrixData< ^T> * int [] * matrixData< ^T> * int [] * int []
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Abs :  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T)
    val luInverse : matrixData:matrixData<'T> -> n:int -> matrixData<'T>
    val inline luSolve :
      a:matrixData< ^T> ->
        n:int -> b:matrixData< ^T> -> k:int -> matrixData< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Abs :  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val inline qrDecomp :
      matrixData:matrixData< ^T> ->
        n:int -> k:int -> matrixData< ^T> * int [] * matrixData< ^T> * int []
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^a : (static member get_One : ->  ^a) and
              ^a : (static member ( + ) :  ^a *  ^a ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^T)
    val inline qrSolveFull :
      a:matrixData< ^T> ->
        n:int -> k:int -> b:matrixData< ^T> -> m:int -> matrixData< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^a : (static member get_One : ->  ^a) and
              ^a : (static member ( + ) :  ^a *  ^a ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^T)
    val qrSolve :
      a:matrixData<'T> ->
        n:int ->
          k:int -> b:matrixData<'T> -> m:int -> tol:'T -> matrixData<'T> * int
    val svdValues : a:matrixData<'T> -> n:int -> k:int -> matrixData<'T> * int
    val inline svdDecomp :
      eps: ^T ->
        matrixData:matrixData< ^T> ->
          n:int ->
            k:int ->
              matrixData< ^T> * int [] * matrixData< ^T> * int [] *
              matrixData< ^T> * int []
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member Abs :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and  ^T : comparison
    val inline svdSolve :
      a:matrixData< ^T> ->
        n:int ->
          k:int ->
            b:matrixData< ^T> -> m:int -> tol: ^T -> matrixData< ^T> * int
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member Abs :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and  ^T : comparison
    val eigenDecomp :
      matrixData:matrixData<'T> -> n:int -> matrixData<'T> * matrixData<'T>
    val eigenValues : matrixData:matrixData<'T> -> n:int -> matrixData<'T>
  end

namespace FMath.Numerics
  module SpecialFunctions = begin
    val inline nan :
      unit ->  ^a
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool)
    val inline posinf :
      unit ->  ^a
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool)
    val inline neginf :
      unit ->  ^a
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool)
    val inline isnan :
      x: ^a -> bool
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool)
    val inline ( !> ) :
      x: ^a [] ->  ^b []
        when ( ^a or  ^b) : (static member op_Explicit :  ^a ->  ^b)
    val ( n z < 0.5 ) : float []
    val ( d z < 0.5 ) : float []
    val ( n z < 0.75 ) : float []
    val ( d z < 0.75 ) : float []
    val ( n z < 1.25 ) : float []
    val ( d z < 1.25 ) : float []
    val ( n z < 2.25 ) : float []
    val ( d z < 2.25 ) : float []
    val ( n z < 3.5 ) : float []
    val ( d z < 3.5 ) : float []
    val ( n z < 5.25 ) : float []
    val ( d z < 5.25 ) : float []
    val ( n z < 8 ) : float []
    val ( d z < 8 ) : float []
    val ( n z < 11.5 ) : float []
    val ( d z < 11.5 ) : float []
    val ( n z < 17 ) : float []
    val ( d z < 17 ) : float []
    val ( n z < 24 ) : float []
    val ( d z < 24 ) : float []
    val ( n z < 38 ) : float []
    val ( d z < 38 ) : float []
    val ( n z < 60 ) : float []
    val ( d z < 60 ) : float []
    val ( n z < 85 ) : float []
    val ( d z < 85 ) : float []
    val ( n z < 110 ) : float []
    val ( d z < 110 ) : float []
    val ( p p<=0.5 ) : float []
    val ( q p<=0.5 ) : float []
    val ( p q>=0.25 ) : float []
    val ( q q>=0.25 ) : float []
    val ( p x<3 ) : float []
    val ( q x<3 ) : float []
    val ( p x<6 ) : float []
    val ( q x<6 ) : float []
    val ( p x<18 ) : float []
    val ( q x<18 ) : float []
    val ( p x<44 ) : float []
    val ( q x<44 ) : float []
    val ( p x>=44 ) : float []
    val ( q x>=44 ) : float []
    val inline evalPoly :
      poly: ^T [] * z: ^T ->  ^T
        when  ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T)
    val inline erfImpPos :
      n z < 0.5: ^T [] ->
        d z < 0.5: ^T [] ->
          n z < 0.75: ^T [] ->
            d z < 0.75: ^T [] ->
              n z < 1.25: ^T [] ->
                d z < 1.25: ^T [] ->
                  n z < 2.25: ^T [] ->
                    d z < 2.25: ^T [] ->
                      n z < 3.5: ^T [] ->
                        d z < 3.5: ^T [] ->
                          n z < 5.25: ^T [] ->
                            d z < 5.25: ^T [] ->
                              n z < 8: ^T [] ->
                                d z < 8: ^T [] ->
                                  n z < 11.5: ^T [] ->
                                    d z < 11.5: ^T [] ->
                                      n z < 17: ^T [] ->
                                        d z < 17: ^T [] ->
                                          n z < 24: ^T [] ->
                                            d z < 24: ^T [] ->
                                              n z < 38: ^T [] ->
                                                d z < 38: ^T [] ->
                                                  n z < 60: ^T [] ->
                                                    d z < 60: ^T [] ->
                                                      n z < 85: ^T [] ->
                                                        d z < 85: ^T [] ->
                                                          n z < 110: ^T [] ->
                                                            d z < 110: ^T [] ->
                                                              invert:bool ->
                                                                z: ^T ->  ^T
        when  ^T : comparison and  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member get_One : ->  ^T)
    val inline erfImpNegPos :
      n z < 0.5: ^T [] ->
        d z < 0.5: ^T [] ->
          n z < 0.75: ^T [] ->
            d z < 0.75: ^T [] ->
              n z < 1.25: ^T [] ->
                d z < 1.25: ^T [] ->
                  n z < 2.25: ^T [] ->
                    d z < 2.25: ^T [] ->
                      n z < 3.5: ^T [] ->
                        d z < 3.5: ^T [] ->
                          n z < 5.25: ^T [] ->
                            d z < 5.25: ^T [] ->
                              n z < 8: ^T [] ->
                                d z < 8: ^T [] ->
                                  n z < 11.5: ^T [] ->
                                    d z < 11.5: ^T [] ->
                                      n z < 17: ^T [] ->
                                        d z < 17: ^T [] ->
                                          n z < 24: ^T [] ->
                                            d z < 24: ^T [] ->
                                              n z < 38: ^T [] ->
                                                d z < 38: ^T [] ->
                                                  n z < 60: ^T [] ->
                                                    d z < 60: ^T [] ->
                                                      n z < 85: ^T [] ->
                                                        d z < 85: ^T [] ->
                                                          n z < 110: ^T [] ->
                                                            d z < 110: ^T [] ->
                                                              invert:bool ->
                                                                z: ^T ->  ^T
        when  ^T : comparison and  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T)
    val inline erfinvImp :
      p p<=0.5: ^T [] ->
        q p<=0.5: ^T [] ->
          p q>=0.25: ^T [] ->
            q q>=0.25: ^T [] ->
              p x<3: ^T [] ->
                q x<3: ^T [] ->
                  p x<6: ^T [] ->
                    q x<6: ^T [] ->
                      p x<18: ^T [] ->
                        q x<18: ^T [] ->
                          p x<44: ^T [] ->
                            q x<44: ^T [] ->
                              p x>=44: ^T [] ->
                                q x>=44: ^T [] -> p: ^T -> q: ^T -> s: ^T ->  ^T
        when  ^T : comparison and  ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T)
    val inline Erf :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member get_One : ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : comparison
    val inline Erfc :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member get_One : ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : comparison
    val inline Erfinv :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when (int or  ^T) : (static member op_Explicit : int ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and  ^T : comparison
    val inline Erfcinv :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when (int or  ^T) : (static member op_Explicit : int ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison
    val inline Normcdf :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member get_One : ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
              ^T : comparison and
             (int or  ^a) : (static member op_Explicit : int ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^T)
    val inline Norminv :
      matrixData:matrixData< ^T> -> matrixData< ^T>
        when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member IsNaN :  ^T -> bool) and
              ^T : (static member IsInfinity :  ^T -> bool) and
              ^T : (static member IsPositiveInfinity :  ^T -> bool) and
              ^T : (static member IsNegativeInfinity :  ^T -> bool) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
             (float32 or  ^T) : (static member op_Explicit : float32 ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : comparison and
             (int or  ^a) : (static member op_Explicit : int ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^T)
  end

namespace FMath.Numerics
  module Distributions = begin
    val getRandArray : length:int -> a:float -> b:float -> float []
    val getRand32Array : length:int -> a:float32 -> b:float32 -> float32 []
    val getRandIntArray : length:int -> a:int -> b:int -> int []
    val unifRnd :
      randGen:(int -> 'T -> 'T -> 'T []) ->
        a:'T -> b:'T -> size:int [] -> matrixData<'T>
    val inline normalRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        mu: ^T -> sigma: ^T -> size:int [] -> matrixData< ^T>
        when  ^T : comparison and  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T)
    val inline lognormRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        mu: ^T ->
          sigma: ^T -> a: ^T -> beta: ^T -> size:int [] -> matrixData< ^T>
        when  ^T : comparison and  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Exp :  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T)
    val inline bernRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        p: ^T -> size:int [] -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison
    val inline binomRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        n:int -> p: ^T -> size:int [] -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T)
    val inline mvNormRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        mu:matrixData< ^T> ->
          cov:matrixData< ^T> -> k:int -> n:int -> matrixData< ^T>
        when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and  ^T : comparison and
              ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member Log :  ^T ->  ^T)
    val inline poissonRnd :
      randGen:(int ->  ^T ->  ^T ->  ^T []) ->
        lambda: ^T -> size:int [] -> matrixData< ^T>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and  ^T : comparison and
              ^T : (static member Exp :  ^T ->  ^T) and
              ^T : (static member ( ~- ) :  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T)
  end

namespace FMath.Numerics
  module StatFunctions = begin
    type M4Stat<'T> =
      {N: 'T;
       Mean: 'T;
       M2: 'T;
       M3: 'T;
       M4: 'T;}
    type CovCorr<'T> =
      {N: 'T;
       MeanX: 'T;
       MeanY: 'T;
       Cov: 'T;
       M2X: 'T;
       M2Y: 'T;}
    val inline getM4StatZero :
      unit -> M4Stat< ^T> when  ^T : (static member get_Zero : ->  ^T)
    val inline getCovCorrZero :
      unit -> CovCorr< ^T> when  ^T : (static member get_Zero : ->  ^T)
    val inline updateM4Stat :
      x:M4Stat< ^T> -> y: ^T -> M4Stat< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member get_Zero : ->  ^T)
    val inline updateCovCorr :
      x:CovCorr< ^T> ->  ^T *  ^T -> CovCorr< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val extractMean : x:M4Stat<'T> -> 'T
    val inline extractVar :
      x:M4Stat< ^T> ->  ^T
        when  ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T)
    val inline extractSkewness :
      x:M4Stat< ^T> ->  ^T
        when  ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T) and
              ^T : (static member Pow :  ^T *  ^T ->  ^T) and
             (float or  ^T) : (static member op_Explicit : float ->  ^T)
    val inline extractKurtosis :
      x:M4Stat< ^T> ->  ^T
        when  ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T)
    val inline extractCorr :
      x:CovCorr< ^T> ->  ^T
        when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Sqrt :  ^T ->  ^T)
    val inline extractCov :
      x:CovCorr< ^T> ->  ^T
        when  ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T)
    val foldNdim :
      init:'S ->
        op:('S -> 'T -> 'S) ->
          f:('S -> 'T) ->
            matrixData:matrixData<'T> ->
              size:int [] -> dim:int -> matrixData<'T> * int []
    val scanNdim :
      init:'T ->
        op:('T -> 'T -> 'T) ->
          matrixData:matrixData<'T> -> size:int [] -> dim:int -> matrixData<'T>
    val inline quantiles :
      matrixData:matrixData< ^T> ->
        size:int [] -> q:matrixData< ^T> -> dim:int -> matrixData< ^T> * int []
        when  ^T : comparison and  ^T : (static member get_One : ->  ^T) and
              ^T : (static member get_Zero : ->  ^T) and
             (int or  ^T) : (static member op_Explicit : int ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member Ceiling :  ^T ->  ^T) and
              ^T : (static member op_Explicit :  ^T -> int) and
              ^T : (static member ( + ) :  ^T *  ^T ->  ^T)
    val inline covcorr :
      extract:(CovCorr< ^T> ->  ^T) ->
        matrixData:matrixData< ^T> -> n:int -> k:int -> matrixData< ^T>
        when  ^T : (static member ( + ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_One : ->  ^T) and
              ^T : (static member ( - ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( * ) :  ^T *  ^T ->  ^T) and
              ^T : (static member ( / ) :  ^T *  ^T ->  ^T) and
              ^T : (static member get_Zero : ->  ^T)
  end

namespace FMath.Numerics
  type NumericMatrixOpsRec<'T> =
    {MulMatrix:
       matrixData<'T> -> int [] -> matrixData<'T> -> int [] -> matrixData<'T>;
     AddScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Add: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     MulScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Mul: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     SubScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Sub: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     SubMatrix: 'T -> matrixData<'T> -> matrixData<'T>;
     DivScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Div: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     DivMatrix: 'T -> matrixData<'T> -> matrixData<'T>;
     PowScalar: matrixData<'T> -> 'T -> matrixData<'T>;
     Pow: matrixData<'T> -> matrixData<'T> -> matrixData<'T>;
     PowMatrix: 'T -> matrixData<'T> -> matrixData<'T>;
     Minus: matrixData<'T> -> matrixData<'T>;
     Abs: matrixData<'T> -> matrixData<'T>;
     Sqrt: matrixData<'T> -> matrixData<'T>;
     Sin: matrixData<'T> -> matrixData<'T>;
     Cos: matrixData<'T> -> matrixData<'T>;
     Tan: matrixData<'T> -> matrixData<'T>;
     ASin: matrixData<'T> -> matrixData<'T>;
     ACos: matrixData<'T> -> matrixData<'T>;
     ATan: matrixData<'T> -> matrixData<'T>;
     Sinh: matrixData<'T> -> matrixData<'T>;
     Cosh: matrixData<'T> -> matrixData<'T>;
     Tanh: matrixData<'T> -> matrixData<'T>;
     Exp: matrixData<'T> -> matrixData<'T>;
     Log: matrixData<'T> -> matrixData<'T>;
     Log10: matrixData<'T> -> matrixData<'T>;
     Erf: matrixData<'T> -> matrixData<'T>;
     Erfc: matrixData<'T> -> matrixData<'T>;
     Erfinv: matrixData<'T> -> matrixData<'T>;
     Erfcinv: matrixData<'T> -> matrixData<'T>;
     Normcdf: matrixData<'T> -> matrixData<'T>;
     Norminv: matrixData<'T> -> matrixData<'T>;
     Round: matrixData<'T> -> matrixData<'T>;
     Ceil: matrixData<'T> -> matrixData<'T>;
     Identity: int -> int -> matrixData<'T>;
     Zeros: int [] -> matrixData<'T>;
     Ones: int [] -> matrixData<'T>;
     CholeskyDecomp: matrixData<'T> -> int -> matrixData<'T>;
     CholeskySolve:
       matrixData<'T> -> int -> matrixData<'T> -> int -> matrixData<'T>;
     LuDecomp:
       matrixData<'T> -> int -> int ->
         matrixData<'T> * int [] * matrixData<'T> * int [] * int [];
     LuSolve: matrixData<'T> -> int -> matrixData<'T> -> int -> matrixData<'T>;
     QrDecomp:
       matrixData<'T> -> int -> int ->
         matrixData<'T> * int [] * matrixData<'T> * int [];
     QrSolveFull:
       matrixData<'T> -> int -> int -> matrixData<'T> -> int -> matrixData<'T>;
     SvdSolve:
       matrixData<'T> -> int -> int -> matrixData<'T> -> int -> 'T ->
         matrixData<'T> * int;
     SvdDecomp:
       matrixData<'T> -> int -> int ->
         matrixData<'T> * int [] * matrixData<'T> * int [] * matrixData<'T> *
         int [];
     UnifRnd: 'T -> 'T -> int [] -> matrixData<'T>;
     NormalRnd: 'T -> 'T -> int [] -> matrixData<'T>;
     LognormalRnd: 'T -> 'T -> 'T -> 'T -> int [] -> matrixData<'T>;
     BernRnd: 'T -> int [] -> matrixData<'T>;
     BinomRnd: int -> 'T -> int [] -> matrixData<'T>;
     MVnormalRnd:
       matrixData<'T> -> matrixData<'T> -> int -> int -> matrixData<'T>;
     PoissonRnd: 'T -> int [] -> matrixData<'T>;
     Mean: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     Skewness: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     Kurtosis: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     Variance: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     Quantiles:
       matrixData<'T> -> int [] -> matrixData<'T> -> int ->
         matrixData<'T> * int [];
     Correlation: matrixData<'T> -> int -> int -> matrixData<'T>;
     Covariance: matrixData<'T> -> int -> int -> matrixData<'T>;
     Sum: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     CumSum: matrixData<'T> -> int [] -> int -> matrixData<'T>;
     Prod: matrixData<'T> -> int [] -> int -> matrixData<'T> * int [];
     CumProd: matrixData<'T> -> int [] -> int -> matrixData<'T>;}
  module NumericMatrixOps = begin
    val inline identity :
      rows:int -> cols:int -> matrixData< ^a>
        when  ^a : (static member get_One : ->  ^a) and
              ^a : (static member get_Zero : ->  ^a)
    val inline createZeros :
      size:int [] -> matrixData< ^a>
        when  ^a : (static member get_Zero : ->  ^a)
    val inline createOnes :
      size:int [] -> matrixData< ^a> when  ^a : (static member get_One : ->  ^a)
    val noneNumericMatrixOpsRec<'T> : NumericMatrixOpsRec<'T>
    val inline createArithmeticMatrixOpsRec :
      identity:(int -> int -> matrixData<'T>) ->
        createZeros:(int [] -> matrixData<'T>) ->
          createOnes:(int [] -> matrixData<'T>) ->
            mulMatrix:(matrixData<'T> -> int [] -> matrixData<'T> -> int [] ->
                         matrixData<'T>) ->
              op_Addition:('T -> 'T -> 'T) ->
                op_Subtraction:('T -> 'T -> 'T) ->
                  op_Multiply:('T -> 'T -> 'T) ->
                    op_Division:('T -> 'T -> 'T) ->
                      op_UnaryNegation:('T -> 'T) ->
                        abs:('T -> 'T) ->
                          randGen:(int -> 'T -> 'T -> 'T []) ->
                            NumericMatrixOpsRec<'T>
    val inline arithmeticMatrixOpsRec :
      randGen:(int ->  ^a ->  ^a ->  ^a []) -> NumericMatrixOpsRec< ^a>
        when  ^a : (static member Abs :  ^a ->  ^a) and
              ^a : (static member ( / ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( - ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( * ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( + ) :  ^a *  ^a ->  ^a) and
              ^a : (static member get_One : ->  ^a) and
              ^a : (static member get_Zero : ->  ^a) and
              ^a : (static member ( ~- ) :  ^a ->  ^a)
    val inline createFloatingMatrixOpsRec :
      identity:(int -> int -> matrixData<'T>) ->
        createZeros:(int [] -> matrixData<'T>) ->
          createOnes:(int [] -> matrixData<'T>) ->
            mulMatrix:(matrixData<'T> -> int [] -> matrixData<'T> -> int [] ->
                         matrixData<'T>) ->
              op_Addition:('T -> 'T -> 'T) ->
                op_Subtraction:('T -> 'T -> 'T) ->
                  op_Multiply:('T -> 'T -> 'T) ->
                    op_Division:('T -> 'T -> 'T) ->
                      op_UnaryNegation:('T -> 'T) ->
                        abs:('T -> 'T) ->
                          op_Exponentiation:('T -> 'T -> 'T) ->
                            sqrt:('T -> 'T) ->
                              sin:('T -> 'T) ->
                                cos:('T -> 'T) ->
                                  tan:('T -> 'T) ->
                                    asin:('T -> 'T) ->
                                      acos:('T -> 'T) ->
                                        atan:('T -> 'T) ->
                                          sinh:('T -> 'T) ->
                                            cosh:('T -> 'T) ->
                                              tanh:('T -> 'T) ->
                                                exp:('T -> 'T) ->
                                                  log:('T -> 'T) ->
                                                    log10:('T -> 'T) ->
                                                      erf:(matrixData<'T> ->
                                                             matrixData<'T>) ->
                                                        erfc:(matrixData<'T> ->
                                                                matrixData<'T>) ->
                                                          erfinv:(matrixData<'T> ->
                                                                    matrixData<'T>) ->
                                                            erfcinv:(matrixData<'T> ->
                                                                       matrixData<'T>) ->
                                                              normcdf:(matrixData<'T> ->
                                                                         matrixData<'T>) ->
                                                                norminv:(matrixData<'T> ->
                                                                           matrixData<'T>) ->
                                                                  round:('T ->
                                                                           'T) ->
                                                                    ceil:('T ->
                                                                            'T) ->
                                                                      choleskyDecomp:(matrixData<'T> ->
                                                                                        int ->
                                                                                        matrixData<'T>) ->
                                                                        choleskySolve:(matrixData<'T> ->
                                                                                         int ->
                                                                                         matrixData<'T> ->
                                                                                         int ->
                                                                                         matrixData<'T>) ->
                                                                          luDecomp:(matrixData<'T> ->
                                                                                      int ->
                                                                                      int ->
                                                                                      matrixData<'T> *
                                                                                      int [] *
                                                                                      matrixData<'T> *
                                                                                      int [] *
                                                                                      int []) ->
                                                                            luSolve:(matrixData<'T> ->
                                                                                       int ->
                                                                                       matrixData<'T> ->
                                                                                       int ->
                                                                                       matrixData<'T>) ->
                                                                              qrDecomp:(matrixData<'T> ->
                                                                                          int ->
                                                                                          int ->
                                                                                          matrixData<'T> *
                                                                                          int [] *
                                                                                          matrixData<'T> *
                                                                                          int []) ->
                                                                                qrSolveFull:(matrixData<'T> ->
                                                                                               int ->
                                                                                               int ->
                                                                                               matrixData<'T> ->
                                                                                               int ->
                                                                                               matrixData<'T>) ->
                                                                                  svdSolve:(matrixData<'T> ->
                                                                                              int ->
                                                                                              int ->
                                                                                              matrixData<'T> ->
                                                                                              int ->
                                                                                              'T ->
                                                                                              matrixData<'T> *
                                                                                              int) ->
                                                                                    svdDecomp:('T ->
                                                                                                 matrixData<'T> ->
                                                                                                 int ->
                                                                                                 int ->
                                                                                                 matrixData<'T> *
                                                                                                 int [] *
                                                                                                 matrixData<'T> *
                                                                                                 int [] *
                                                                                                 matrixData<'T> *
                                                                                                 int []) ->
                                                                                      unifRnd:((int ->
                                                                                                  'T ->
                                                                                                  'T ->
                                                                                                  'T []) ->
                                                                                                 'T ->
                                                                                                 'T ->
                                                                                                 int [] ->
                                                                                                 matrixData<'T>) ->
                                                                                        normalRnd:((int ->
                                                                                                      'T ->
                                                                                                      'T ->
                                                                                                      'T []) ->
                                                                                                     'T ->
                                                                                                     'T ->
                                                                                                     int [] ->
                                                                                                     matrixData<'T>) ->
                                                                                          lognormRnd:((int ->
                                                                                                         'T ->
                                                                                                         'T ->
                                                                                                         'T []) ->
                                                                                                        'T ->
                                                                                                        'T ->
                                                                                                        'T ->
                                                                                                        'T ->
                                                                                                        int [] ->
                                                                                                        matrixData<'T>) ->
                                                                                            bernRnd:((int ->
                                                                                                        'T ->
                                                                                                        'T ->
                                                                                                        'T []) ->
                                                                                                       'T ->
                                                                                                       int [] ->
                                                                                                       matrixData<'T>) ->
                                                                                              binomRnd:((int ->
                                                                                                           'T ->
                                                                                                           'T ->
                                                                                                           'T []) ->
                                                                                                          int ->
                                                                                                          'T ->
                                                                                                          int [] ->
                                                                                                          matrixData<'T>) ->
                                                                                                mvNormRnd:((int ->
                                                                                                              'T ->
                                                                                                              'T ->
                                                                                                              'T []) ->
                                                                                                             matrixData<'T> ->
                                                                                                             matrixData<'T> ->
                                                                                                             int ->
                                                                                                             int ->
                                                                                                             matrixData<'T>) ->
                                                                                                  poissonRnd:((int ->
                                                                                                                 'T ->
                                                                                                                 'T ->
                                                                                                                 'T []) ->
                                                                                                                'T ->
                                                                                                                int [] ->
                                                                                                                matrixData<'T>) ->
                                                                                                    m4StatZero:StatFunctions.M4Stat<'T> ->
                                                                                                      extractMean:(StatFunctions.M4Stat<'T> ->
                                                                                                                     'T) ->
                                                                                                        extractSkewness:(StatFunctions.M4Stat<'T> ->
                                                                                                                           'T) ->
                                                                                                          updateM4Stat:(StatFunctions.M4Stat<'T> ->
                                                                                                                          'T ->
                                                                                                                          StatFunctions.M4Stat<'T>) ->
                                                                                                            quantiles:(matrixData<'T> ->
                                                                                                                         int [] ->
                                                                                                                         matrixData<'T> ->
                                                                                                                         int ->
                                                                                                                         matrixData<'T> *
                                                                                                                         int []) ->
                                                                                                              extractKurtosis:(StatFunctions.M4Stat<'T> ->
                                                                                                                                 'T) ->
                                                                                                                extractVar:(StatFunctions.M4Stat<'T> ->
                                                                                                                              'T) ->
                                                                                                                  covcorr:((StatFunctions.CovCorr<'T> ->
                                                                                                                              'T) ->
                                                                                                                             matrixData<'T> ->
                                                                                                                             int ->
                                                                                                                             int ->
                                                                                                                             matrixData<'T>) ->
                                                                                                                    extractCov:(StatFunctions.CovCorr<'T> ->
                                                                                                                                  'T) ->
                                                                                                                      extractCorr:(StatFunctions.CovCorr<'T> ->
                                                                                                                                     'T) ->
                                                                                                                        genericZero:'T ->
                                                                                                                          genericOne:'T ->
                                                                                                                            randGen:(int ->
                                                                                                                                       'T ->
                                                                                                                                       'T ->
                                                                                                                                       'T []) ->
                                                                                                                              eps:'T ->
                                                                                                                                NumericMatrixOpsRec<'T>
    val inline floatingMatrixOpsRec :
      randGen:(int ->  ^a ->  ^a ->  ^a []) -> NumericMatrixOpsRec< ^a>
        when  ^a : (static member IsNaN :  ^a -> bool) and
              ^a : (static member IsInfinity :  ^a -> bool) and
              ^a : (static member IsPositiveInfinity :  ^a -> bool) and
              ^a : (static member IsNegativeInfinity :  ^a -> bool) and
              ^a : (static member op_Explicit :  ^a -> int) and
              ^a : (static member Ceiling :  ^a ->  ^a) and
              ^a : (static member Log10 :  ^a ->  ^a) and
              ^a : (static member Exp :  ^a ->  ^a) and
              ^a : (static member Cosh :  ^a ->  ^a) and
              ^a : (static member Atan :  ^a ->  ^a) and
              ^a : (static member Asin :  ^a ->  ^a) and
              ^a : (static member Cos :  ^a ->  ^a) and
              ^a : (static member Pow :  ^a *  ^a ->  ^a) and
              ^a : (static member ( ~- ) :  ^a ->  ^a) and
              ^a : (static member get_Zero : ->  ^a) and
              ^a : (static member get_One : ->  ^a) and
              ^a : (static member ( + ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( * ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( - ) :  ^a *  ^a ->  ^a) and
              ^a : (static member ( / ) :  ^a *  ^a ->  ^a) and
              ^a : (static member Abs :  ^a ->  ^a) and
              ^a : (static member Sqrt :  ^a ->  ^a) and
              ^a : (static member Sin :  ^a ->  ^a) and
              ^a : (static member Tan :  ^a ->  ^a) and
              ^a : (static member Acos :  ^a ->  ^a) and
              ^a : (static member Sinh :  ^a ->  ^a) and
              ^a : (static member Tanh :  ^a ->  ^a) and
              ^a : (static member Log :  ^a ->  ^a) and
             (int or  ^a) : (static member op_Explicit : int ->  ^a) and
             (float32 or  ^a) : (static member op_Explicit : float32 ->  ^a) and
             (float or  ^a) : (static member op_Explicit : float ->  ^a) and
              ^a : (static member Round :  ^a ->  ^a) and  ^a : comparison and
              ^b : (static member get_One : ->  ^b) and
              ^b : (static member ( + ) :  ^b *  ^b ->  ^b) and
              ^b : (static member Sqrt :  ^b ->  ^a) and
              ^c : (static member get_One : ->  ^c) and
              ^c : (static member ( + ) :  ^c *  ^c ->  ^c) and
              ^c : (static member Sqrt :  ^c ->  ^a) and
             (int or  ^d) : (static member op_Explicit : int ->  ^d) and
              ^d : (static member Sqrt :  ^d ->  ^a) and
             (int or  ^e) : (static member op_Explicit : int ->  ^e) and
              ^e : (static member Sqrt :  ^e ->  ^a)
    val createNumericMatrixOps :
      numOpsRec:NumericMatrixOpsRec<'T> -> INumericMatrixOps<'T>
  end

namespace FMath.Numerics
  type GenericLib<'T> =
    class
      interface IMatrixOps<'T>
      new : unit -> GenericLib<'T>
    end
  type GenericMatrix<'T> = Matrix<'T,GenericLib<'T>>
  type ComparableLib<'T when 'T : comparison> =
    class
      interface IMatrixOps<'T>
      new : unit -> ComparableLib<'T>
    end
  type ComparableMatrix<'T when 'T : comparison> = Matrix<'T,ComparableLib<'T>>
  type StringLib =
    class
      interface IMatrixOps<string>
      new : unit -> StringLib
    end
  type StringMatrix = Matrix<string,StringLib>
  type BoolLib =
    class
      interface IMatrixOps<bool>
      new : unit -> BoolLib
    end
  type BoolMatrix = Matrix<bool,BoolLib>
  type IntLib =
    class
      interface IMatrixOps<int>
      new : unit -> IntLib
    end
  type IntMatrix = Matrix<int,IntLib>
  type Float32Lib =
    class
      interface IMatrixOps<float32>
      new : unit -> Float32Lib
    end
  type Matrix32 = Matrix<float32,Float32Lib>
  type FloatLib =
    class
      interface IMatrixOps<float>
      new : unit -> FloatLib
    end
  type Matrix = Matrix<float,FloatLib>

namespace FMath.Numerics
  module GenericMatrixFunctions = begin
    val I :
      n:int * m:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val zeros :
      size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val ones :
      size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val diag :
      vector:Matrix<'T,'S> * k:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val triL :
      matrix:Matrix<'T,'S> * k:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val triU :
      matrix:Matrix<'T,'S> * k:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val concat :
      matrices:seq<Matrix<'T,'S>> * dimension:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val horzConcat :
      matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val vertConcat :
      matrices:seq<Matrix<'T,'S>> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val repmat :
      matrix:Matrix<'T,'S> * replicator:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val reshape :
      matrix:Matrix<'T,'S> * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val transpose :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val inline minXY :
      x: ^a * y: ^b -> Matrix<'T,'S>
        when  ^a : (static member ( !! ) :  ^a -> Matrix<'T,'S>) and
             'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T> and
              ^b : (static member ( !! ) :  ^b -> Matrix<'T,'S>)
    val inline maxXY :
      x: ^a * y: ^b -> Matrix<'T,'S>
        when  ^a : (static member ( !! ) :  ^a -> Matrix<'T,'S>) and
             'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T> and
              ^b : (static member ( !! ) :  ^b -> Matrix<'T,'S>)
    val applyFun :
      x:Matrix<'T,'S> * f:('T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val applyFun2Arg :
      x:Matrix<'T,'S> * y:Matrix<'T,'S> * f:('T -> 'T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val applyFun3Arg :
      x:Matrix<'T,'S> * y:Matrix<'T,'S> * z:Matrix<'T,'S> *
      f:('T -> 'T -> 'T -> 'T) -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val erf :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val erfc :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val erfinv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val erfcinv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val normcdf :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val norminv :
      x:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
  end

namespace FMath.Numerics
  module MatrixFunctions = begin
    val I : n:int * m:int -> Matrix<float,FloatLib>
    val zeros : size:seq<int> -> Matrix<float,FloatLib>
    val ones : size:seq<int> -> Matrix<float,FloatLib>
    val diag : vector:Matrix<float,FloatLib> * k:int -> Matrix<float,FloatLib>
    val triL : matrix:Matrix<float,FloatLib> * k:int -> Matrix<float,FloatLib>
    val triU : matrix:Matrix<float,FloatLib> * k:int -> Matrix<float,FloatLib>
    val concat :
      matrices:seq<Matrix<float,FloatLib>> * dimension:int ->
        Matrix<float,FloatLib>
    val horzConcat :
      matrices:seq<Matrix<float,FloatLib>> -> Matrix<float,FloatLib>
    val vertConcat :
      matrices:seq<Matrix<float,FloatLib>> -> Matrix<float,FloatLib>
    val repmat :
      matrix:Matrix<float,FloatLib> * replicator:seq<int> ->
        Matrix<float,FloatLib>
    val reshape :
      matrix:Matrix<float,FloatLib> * size:seq<int> -> Matrix<float,FloatLib>
    val transpose : matrix:Matrix<float,FloatLib> -> Matrix<float,FloatLib>
    val inline minXY :
      x: ^a * y: ^b -> Matrix<float,FloatLib>
        when ( ^a or Matrix) : (static member op_Explicit :  ^a -> Matrix) and
             ( ^b or Matrix) : (static member op_Explicit :  ^b -> Matrix)
    val inline maxXY :
      x: ^a * y: ^b -> Matrix<float,FloatLib>
        when ( ^a or Matrix) : (static member op_Explicit :  ^a -> Matrix) and
             ( ^b or Matrix) : (static member op_Explicit :  ^b -> Matrix)
    val applyFun :
      x:Matrix<float,FloatLib> * f:(float -> float) -> Matrix<float,FloatLib>
    val applyFun2Arg :
      x:Matrix<float,FloatLib> * y:Matrix<float,FloatLib> *
      f:(float -> float -> float) -> Matrix<float,FloatLib>
    val applyFun3Arg :
      x:Matrix<float,FloatLib> * y:Matrix<float,FloatLib> *
      z:Matrix<float,FloatLib> * f:(float -> float -> float -> float) ->
        Matrix<float,FloatLib>
    val inline erf :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
    val inline erfc :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
    val inline erfinv :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
    val inline erfcinv :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
    val inline normcdf :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
    val inline norminv :
      x: ^T ->  ^T
        when (Matrix<float,FloatLib> or  ^T) : (static member op_Explicit : Matrix<float,
                                                                                   FloatLib>
                                                                              ->
                                                                               ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float,FloatLib>
  end

namespace FMath.Numerics
  module Matrix32Functions = begin
    val I : n:int * m:int -> Matrix<float32,Float32Lib>
    val zeros : size:seq<int> -> Matrix<float32,Float32Lib>
    val ones : size:seq<int> -> Matrix<float32,Float32Lib>
    val diag :
      vector:Matrix<float32,Float32Lib> * k:int -> Matrix<float32,Float32Lib>
    val triL :
      matrix:Matrix<float32,Float32Lib> * k:int -> Matrix<float32,Float32Lib>
    val triU :
      matrix:Matrix<float32,Float32Lib> * k:int -> Matrix<float32,Float32Lib>
    val concat :
      matrices:seq<Matrix<float32,Float32Lib>> * dimension:int ->
        Matrix<float32,Float32Lib>
    val horzConcat :
      matrices:seq<Matrix<float32,Float32Lib>> -> Matrix<float32,Float32Lib>
    val vertConcat :
      matrices:seq<Matrix<float32,Float32Lib>> -> Matrix<float32,Float32Lib>
    val repmat :
      matrix:Matrix<float32,Float32Lib> * replicator:seq<int> ->
        Matrix<float32,Float32Lib>
    val reshape :
      matrix:Matrix<float32,Float32Lib> * size:seq<int> ->
        Matrix<float32,Float32Lib>
    val transpose :
      matrix:Matrix<float32,Float32Lib> -> Matrix<float32,Float32Lib>
    val inline minXY :
      x: ^a * y: ^b -> Matrix<float32,Float32Lib>
        when ( ^a or Matrix32) : (static member op_Explicit :  ^a -> Matrix32) and
             ( ^b or Matrix32) : (static member op_Explicit :  ^b -> Matrix32)
    val inline maxXY :
      x: ^a * y: ^b -> Matrix<float32,Float32Lib>
        when ( ^a or Matrix) : (static member op_Explicit :  ^a -> Matrix) and
              ^a :> Matrix<float32,Float32Lib> and
             ( ^b or Matrix) : (static member op_Explicit :  ^b -> Matrix) and
              ^b :> Matrix<float32,Float32Lib>
    val applyFun :
      x:Matrix<float32,Float32Lib> * f:(float32 -> float32) ->
        Matrix<float32,Float32Lib>
    val applyFun2Arg :
      x:Matrix<float32,Float32Lib> * y:Matrix<float32,Float32Lib> *
      f:(float32 -> float32 -> float32) -> Matrix<float32,Float32Lib>
    val applyFun3Arg :
      x:Matrix<float32,Float32Lib> * y:Matrix<float32,Float32Lib> *
      z:Matrix<float32,Float32Lib> *
      f:(float32 -> float32 -> float32 -> float32) -> Matrix<float32,Float32Lib>
    val inline erf :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
    val inline erfc :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
    val inline erfinv :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
    val inline erfcinv :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
    val inline normcdf :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
    val inline norminv :
      x: ^T ->  ^T
        when (Matrix<float32,Float32Lib> or  ^T) : (static member op_Explicit : Matrix<float32,
                                                                                       Float32Lib>
                                                                                  ->
                                                                                   ^T) and
             ( ^T or  ^a) : (static member op_Explicit :  ^T ->  ^a) and
              ^a :> Matrix<float32,Float32Lib>
  end

namespace FMath.Numerics
  module GenericLinearAlgebra = begin
    val chol :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val cholSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val lu :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S> * int []
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val luSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val qr :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val qrSolveFull :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val svdSolve :
      a:Matrix<'T,'S> * b:Matrix<'T,'S> * tol:'T -> Matrix<'T,'S> * int
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val svd :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S> * Matrix<'T,'S> * Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
  end

namespace FMath.Numerics
  module LinearAlgebra = begin
    val chol : matrix:Matrix<float,FloatLib> -> Matrix<float,FloatLib>
    val cholSolve :
      a:Matrix<float,FloatLib> * b:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib>
    val lu :
      matrix:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib> * Matrix<float,FloatLib> * int []
    val luSolve :
      a:Matrix<float,FloatLib> * b:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib>
    val qr :
      matrix:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib> * Matrix<float,FloatLib>
    val qrSolveFull :
      a:Matrix<float,FloatLib> * b:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib>
    val svdSolve :
      a:Matrix<float,FloatLib> * b:Matrix<float,FloatLib> * tol:float ->
        Matrix<float,FloatLib> * int
    val svd :
      matrix:Matrix<float,FloatLib> ->
        Matrix<float,FloatLib> * Matrix<float,FloatLib> * Matrix<float,FloatLib>
  end

namespace FMath.Numerics
  module LinearAlgebra32 = begin
    val chol : matrix:Matrix<float32,Float32Lib> -> Matrix<float32,Float32Lib>
    val cholSolve :
      a:Matrix<float32,Float32Lib> * b:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib>
    val lu :
      matrix:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib> * Matrix<float32,Float32Lib> * int []
    val luSolve :
      a:Matrix<float32,Float32Lib> * b:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib>
    val qr :
      matrix:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib> * Matrix<float32,Float32Lib>
    val qrSolveFull :
      a:Matrix<float32,Float32Lib> * b:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib>
    val svdSolve :
      a:Matrix<float32,Float32Lib> * b:Matrix<float32,Float32Lib> * tol:float32 ->
        Matrix<float32,Float32Lib> * int
    val svd :
      matrix:Matrix<float32,Float32Lib> ->
        Matrix<float32,Float32Lib> * Matrix<float32,Float32Lib> *
        Matrix<float32,Float32Lib>
  end

namespace FMath.Numerics
  module GenericBasicStat = begin
    val inline rand :
      size:seq<int> -> Matrix< ^T,'S>
        when  ^T : (static member get_Zero : ->  ^T) and
              ^T : (static member get_One : ->  ^T) and 'S : (new : unit ->  'S) and
             'S :> IMatrixOps< ^T>
    val unifRnd :
      a:'T * b:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val normalRnd :
      mu:'T * sigma:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val lognormRnd :
      mu:'T * sigma:'T * a:'T * scale:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val mvNormRnd :
      mu:Matrix<'T,'S> * cov:Matrix<'T,'S> * n:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val bernRnd :
      p:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val binomRnd :
      n:int * p:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val poissRnd :
      lambda:'T * size:seq<int> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val min :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val max :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val sum :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val prod :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val cumsum :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val cumprod :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val mean :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val var :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val skewness :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val kurtosis :
      matrix:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val quantile :
      matrix:Matrix<'T,'S> * quantiles:Matrix<'T,'S> * dim:int -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val corr :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
    val cov :
      matrix:Matrix<'T,'S> -> Matrix<'T,'S>
        when 'S : (new : unit ->  'S) and 'S :> IMatrixOps<'T>
  end

namespace FMath.Numerics
  module BasicStat = begin
    val rand : size:seq<int> -> Matrix<float,FloatLib>
    val unifRnd : a:float * b:float * size:seq<int> -> Matrix<float,FloatLib>
    val normalRnd :
      mu:float * sigma:float * size:seq<int> -> Matrix<float,FloatLib>
    val lognormRnd :
      mu:float * sigma:float * a:float * scale:float * size:seq<int> ->
        Matrix<float,FloatLib>
    val mvNormRnd :
      mu:Matrix<float,FloatLib> * cov:Matrix<float,FloatLib> * n:int ->
        Matrix<float,FloatLib>
    val bernRnd : p:float * size:seq<int> -> Matrix<float,FloatLib>
    val binomRnd : n:int * p:float * size:seq<int> -> Matrix<float,FloatLib>
    val poissRnd : lambda:float * size:seq<int> -> Matrix<float,FloatLib>
    val min : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val max : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val sum : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val prod : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val cumsum :
      matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val cumprod :
      matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val mean : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val var : matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val skewness :
      matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val kurtosis :
      matrix:Matrix<float,FloatLib> * dim:int -> Matrix<float,FloatLib>
    val quantile :
      matrix:Matrix<float,FloatLib> * quantiles:Matrix<float,FloatLib> * dim:int ->
        Matrix<float,FloatLib>
    val corr : matrix:Matrix<float,FloatLib> -> Matrix<float,FloatLib>
    val cov : matrix:Matrix<float,FloatLib> -> Matrix<float,FloatLib>
  end

namespace FMath.Numerics
  module BasicStat32 = begin
    val rand : size:seq<int> -> Matrix<float32,Float32Lib>
    val unifRnd :
      a:float32 * b:float32 * size:seq<int> -> Matrix<float32,Float32Lib>
    val normalRnd :
      mu:float32 * sigma:float32 * size:seq<int> -> Matrix<float32,Float32Lib>
    val lognormRnd :
      mu:float32 * sigma:float32 * a:float32 * scale:float32 * size:seq<int> ->
        Matrix<float32,Float32Lib>
    val mvNormRnd :
      mu:Matrix<float32,Float32Lib> * cov:Matrix<float32,Float32Lib> * n:int ->
        Matrix<float32,Float32Lib>
    val bernRnd : p:float32 * size:seq<int> -> Matrix<float32,Float32Lib>
    val binomRnd :
      n:int * p:float32 * size:seq<int> -> Matrix<float32,Float32Lib>
    val poissRnd : lambda:float32 * size:seq<int> -> Matrix<float32,Float32Lib>
    val min :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val max :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val sum :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val prod :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val cumsum :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val cumprod :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val mean :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val var :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val skewness :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val kurtosis :
      matrix:Matrix<float32,Float32Lib> * dim:int -> Matrix<float32,Float32Lib>
    val quantile :
      matrix:Matrix<float32,Float32Lib> * quantiles:Matrix<float32,Float32Lib> *
      dim:int -> Matrix<float32,Float32Lib>
    val corr : matrix:Matrix<float32,Float32Lib> -> Matrix<float32,Float32Lib>
    val cov : matrix:Matrix<float32,Float32Lib> -> Matrix<float32,Float32Lib>
  end

