namespace Fmat.Numerics

open System

type INumericMatrixOps<'T> =
    abstract member CholeskyDecomp :  matrixData<'T> * int -> matrixData<'T>
    abstract member CholeskySolve : matrixData<'T> * int * matrixData<'T> * int -> matrixData<'T>
    abstract member LuDecomp : matrixData<'T> * int * int -> matrixData<'T> * int[] * matrixData<'T> * int[] * int[]
    abstract member LuSolve : matrixData<'T> * int * matrixData<'T> * int -> matrixData<'T>
    abstract member QrDecomp : matrixData<'T> * int * int -> matrixData<'T> * int[] * matrixData<'T> * int[]
    abstract member QrSolveFull : matrixData<'T> * int * int * matrixData<'T> * int -> matrixData<'T>
    abstract member SvdSolve : matrixData<'T> * int * int * matrixData<'T> * int * 'T -> matrixData<'T> * int
    abstract member SvdDecomp : matrixData<'T> * int * int -> matrixData<'T> * int[] * matrixData<'T> * int[] * matrixData<'T> * int[]
    abstract member MulMatrix : matrixData<'T> * int[] * matrixData<'T> * int[] -> matrixData<'T>
    abstract member Add : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Add : matrixData<'T> * 'T-> matrixData<'T>
    abstract member Mul : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Mul : matrixData<'T> * 'T-> matrixData<'T>
    abstract member Sub : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Sub : matrixData<'T> * 'T -> matrixData<'T>
    abstract member Sub : 'T * matrixData<'T> -> matrixData<'T>
    abstract member Div : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Div : matrixData<'T> * 'T -> matrixData<'T>
    abstract member Div : 'T * matrixData<'T> -> matrixData<'T>
    abstract member Pow : matrixData<'T> * matrixData<'T> -> matrixData<'T>
    abstract member Pow : matrixData<'T> * 'T-> matrixData<'T>
    abstract member Pow : 'T * matrixData<'T>-> matrixData<'T>
    abstract member Minus : matrixData<'T> -> matrixData<'T>
    abstract member Abs : matrixData<'T> -> matrixData<'T>
    abstract member Sqrt : matrixData<'T> -> matrixData<'T>
    abstract member Sin : matrixData<'T> -> matrixData<'T>
    abstract member Cos : matrixData<'T> -> matrixData<'T>
    abstract member Tan : matrixData<'T> -> matrixData<'T>
    abstract member ASin : matrixData<'T> -> matrixData<'T>
    abstract member ACos : matrixData<'T> -> matrixData<'T>
    abstract member ATan : matrixData<'T> -> matrixData<'T>
    abstract member Sinh : matrixData<'T> -> matrixData<'T>
    abstract member Cosh : matrixData<'T> -> matrixData<'T>
    abstract member Tanh : matrixData<'T> -> matrixData<'T>
    abstract member Exp : matrixData<'T> -> matrixData<'T>
    abstract member Log : matrixData<'T> -> matrixData<'T>
    abstract member Log10 : matrixData<'T> -> matrixData<'T>
    abstract member Erf : matrixData<'T> -> matrixData<'T>
    abstract member Erfc : matrixData<'T> -> matrixData<'T>
    abstract member Erfinv : matrixData<'T> -> matrixData<'T>
    abstract member Erfcinv : matrixData<'T> -> matrixData<'T>
    abstract member Normcdf : matrixData<'T> -> matrixData<'T>
    abstract member Norminv : matrixData<'T> -> matrixData<'T>
    abstract member Round : matrixData<'T> -> matrixData<'T>
    abstract member Ceil : matrixData<'T> -> matrixData<'T>
    abstract member Identity : int * int -> matrixData<'T>
    abstract member Zeros : int[] -> matrixData<'T>
    abstract member Ones : int[] -> matrixData<'T>
    abstract member UnifRnd : 'T * 'T * int[] -> matrixData<'T>
    abstract member NormalRnd : 'T * 'T * int[] -> matrixData<'T>
    abstract member LognormalRnd : 'T * 'T * 'T * 'T * int[] -> matrixData<'T>
    abstract member BernRnd : 'T * int[] -> matrixData<'T>
    abstract member BinomRnd : int * 'T * int[] -> matrixData<'T>
    abstract member MVnormalRnd : matrixData<'T> * matrixData<'T> * int * int -> matrixData<'T>
    abstract member PoissonRnd : 'T * int[] -> matrixData<'T>
    abstract member Mean : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member Skewness : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member Kurtosis : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member Variance : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member Quantiles : matrixData<'T> * int[] * matrixData<'T> * int -> matrixData<'T> * int[]
    abstract member Correlation : matrixData<'T> * int * int -> matrixData<'T>
    abstract member Covariance : matrixData<'T> * int * int -> matrixData<'T>
    abstract member Sum : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member CumSum : matrixData<'T> * int[] * int -> matrixData<'T>
    abstract member Prod : matrixData<'T> * int[] * int -> matrixData<'T> * int[]
    abstract member CumProd : matrixData<'T> * int[] * int -> matrixData<'T>