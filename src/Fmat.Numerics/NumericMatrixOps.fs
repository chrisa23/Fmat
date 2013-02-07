namespace Fmat.Numerics

open System
open System.Collections.Generic
open MatrixUtil
open LinearAlgebraOps
open Fmat.Numerics.NumericLiteralG
open Fmat.Numerics.Conversion
open Fmat.Numerics.SymmetricOperators
open Fmat.Numerics.SpecialFunctions
open Fmat.Numerics.Distributions
open Fmat.Numerics.StatFunctions

type NumericMatrixOpsRec<'T> =
    {
        MulMatrix : matrixData<'T> -> int[] -> matrixData<'T> -> int[] -> matrixData<'T>
        AddScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Add : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        MulScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Mul : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        SubScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Sub : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        SubMatrix : 'T -> matrixData<'T> -> matrixData<'T>
        DivScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Div : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        DivMatrix : 'T -> matrixData<'T> -> matrixData<'T>
        PowScalar : matrixData<'T> -> 'T -> matrixData<'T>
        Pow : matrixData<'T> -> matrixData<'T> -> matrixData<'T>
        PowMatrix : 'T -> matrixData<'T> -> matrixData<'T>
        Minus : matrixData<'T> -> matrixData<'T>
        Abs : matrixData<'T> -> matrixData<'T>
        Sqrt : matrixData<'T> -> matrixData<'T>
        Sin : matrixData<'T> -> matrixData<'T>
        Cos : matrixData<'T> -> matrixData<'T>
        Tan : matrixData<'T> -> matrixData<'T>
        ASin : matrixData<'T> -> matrixData<'T>
        ACos : matrixData<'T> -> matrixData<'T>
        ATan : matrixData<'T> -> matrixData<'T>
        Sinh : matrixData<'T> -> matrixData<'T>
        Cosh : matrixData<'T> -> matrixData<'T>
        Tanh : matrixData<'T> -> matrixData<'T>
        Exp : matrixData<'T> -> matrixData<'T>
        Log : matrixData<'T> -> matrixData<'T>
        Log10 : matrixData<'T> -> matrixData<'T>
        Erf : matrixData<'T> -> matrixData<'T>
        Erfc : matrixData<'T> -> matrixData<'T>
        Erfinv : matrixData<'T> -> matrixData<'T>
        Erfcinv : matrixData<'T> -> matrixData<'T>
        Normcdf : matrixData<'T> -> matrixData<'T>
        Norminv : matrixData<'T> -> matrixData<'T>
        Round : matrixData<'T> -> matrixData<'T>
        Ceil : matrixData<'T> -> matrixData<'T>
        Identity : int -> int -> matrixData<'T>
        Zeros : int[] -> matrixData<'T>
        Ones : int[] -> matrixData<'T>
        CholeskyDecomp :  matrixData<'T> -> int -> matrixData<'T>
        CholeskySolve : matrixData<'T> -> int -> matrixData<'T> -> int -> matrixData<'T>
        LuDecomp : matrixData<'T> -> int -> int -> matrixData<'T> * int[] * matrixData<'T> * int[] * int[]
        LuSolve : matrixData<'T> -> int -> matrixData<'T> -> int -> matrixData<'T>
        QrDecomp : matrixData<'T> -> int -> int -> matrixData<'T> * int[] * matrixData<'T> * int[]
        QrSolveFull : matrixData<'T> -> int -> int -> matrixData<'T> -> int -> matrixData<'T>
        SvdSolve : matrixData<'T> -> int -> int -> matrixData<'T> -> int -> 'T -> matrixData<'T> * int
        SvdDecomp : matrixData<'T> -> int -> int -> matrixData<'T> * int[] * matrixData<'T> * int[] * matrixData<'T> * int[]
        UnifRnd : 'T -> 'T -> int[] -> matrixData<'T>
        NormalRnd : 'T -> 'T -> int[] -> matrixData<'T>
        LognormalRnd : 'T -> 'T -> 'T -> 'T -> int[] -> matrixData<'T>
        BernRnd : 'T -> int[] -> matrixData<'T>
        BinomRnd : int -> 'T -> int[] -> matrixData<'T>
        MVnormalRnd : matrixData<'T> -> matrixData<'T> -> int -> int -> matrixData<'T>
        PoissonRnd : 'T -> int[] -> matrixData<'T>
        Mean : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        Skewness : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        Kurtosis : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        Variance : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        Quantiles : matrixData<'T> -> int[] -> matrixData<'T> -> int -> matrixData<'T> * int[]
        Correlation : matrixData<'T> -> int -> int -> matrixData<'T>
        Covariance : matrixData<'T> -> int -> int -> matrixData<'T>
        Sum : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        CumSum : matrixData<'T> -> int[] -> int -> matrixData<'T>
        Prod : matrixData<'T> -> int[] -> int -> matrixData<'T> * int[]
        CumProd : matrixData<'T> -> int[] -> int -> matrixData<'T>
    }

module NumericMatrixOps =

    let inline identity rows cols =
        let res = Array.create (rows*cols) 0G
        [|0..(min rows cols)-1|] |> Array.iteri (fun i x -> res.[i*rows+i] <- 1G)
        Managed(res) 

    let inline createZeros size =
        let len = size|> Array.fold (*) 1
        let res = Array.create len 0G
        Managed(res)

    let inline createOnes size =
        let len = size|> Array.fold (*) 1
        let res = Array.create len 1G
        Managed(res)

    let noneNumericMatrixOpsRec<'T> =
        {
            AddScalar = (invalidOp2Arg : matrixData<'T> -> 'T -> matrixData<'T>)
            MulMatrix = invalidOp4Arg
            Add = invalidOp2Arg
            MulScalar = invalidOp2Arg
            Mul = invalidOp2Arg
            SubScalar = invalidOp2Arg
            Sub = invalidOp2Arg
            SubMatrix = invalidOp2Arg
            DivScalar = invalidOp2Arg
            Div = invalidOp2Arg
            DivMatrix = invalidOp2Arg
            PowScalar = invalidOp2Arg
            Pow = invalidOp2Arg
            PowMatrix = invalidOp2Arg
            Minus = invalidOp1Arg
            Abs = invalidOp1Arg
            Sqrt = invalidOp1Arg
            Sin = invalidOp1Arg
            Cos = invalidOp1Arg
            Tan = invalidOp1Arg
            ASin = invalidOp1Arg
            ACos = invalidOp1Arg
            ATan = invalidOp1Arg
            Sinh = invalidOp1Arg
            Cosh = invalidOp1Arg
            Tanh = invalidOp1Arg
            Exp = invalidOp1Arg
            Log = invalidOp1Arg
            Log10 = invalidOp1Arg
            Erf = invalidOp1Arg
            Erfc = invalidOp1Arg
            Erfinv = invalidOp1Arg
            Erfcinv = invalidOp1Arg
            Normcdf = invalidOp1Arg
            Norminv = invalidOp1Arg
            Round = invalidOp1Arg
            Ceil = invalidOp1Arg
            Identity = invalidOp2Arg
            Zeros = invalidOp1Arg
            Ones = invalidOp1Arg
            CholeskyDecomp = invalidOp2Arg
            CholeskySolve = invalidOp4Arg
            LuDecomp = invalidOp3Arg
            LuSolve = invalidOp4Arg
            QrDecomp = invalidOp3Arg
            QrSolveFull = invalidOp5Arg
            SvdSolve = invalidOp5Arg
            SvdDecomp = invalidOp3Arg
            UnifRnd = invalidOp3Arg
            NormalRnd = invalidOp3Arg
            LognormalRnd = invalidOp5Arg
            BernRnd = invalidOp2Arg
            BinomRnd = invalidOp3Arg
            MVnormalRnd = invalidOp4Arg
            PoissonRnd = invalidOp2Arg
            Mean = invalidOp3Arg
            Skewness = invalidOp3Arg
            Kurtosis = invalidOp3Arg
            Variance = invalidOp3Arg
            Quantiles = invalidOp4Arg
            Correlation = invalidOp3Arg
            Covariance = invalidOp3Arg
            Sum = invalidOp3Arg
            Prod = invalidOp3Arg
            CumSum = invalidOp3Arg
            CumProd = invalidOp3Arg
        }

    let inline createArithmeticMatrixOpsRec<'T> identity createZeros createOnes mulMatrix (+) (-) (*) (/) (~-) abs randGen =
        { noneNumericMatrixOpsRec<'T> with 
            AddScalar = (funMatrixScalar (+) : matrixData<'T> -> 'T -> matrixData<'T>) 
            MulMatrix = mulMatrix
            Add = funMatrixMatrix (+)
            MulScalar = funMatrixScalar (*)
            Mul = funMatrixMatrix (*)
            DivScalar = funMatrixScalar (/)
            Div = funMatrixMatrix (/)
            DivMatrix = funScalarMatrix (/)
            SubScalar = funMatrixScalar (-)
            Sub = funMatrixMatrix (-)
            SubMatrix = funScalarMatrix (-)
            Minus = funMatrix (~-)
            Abs = funMatrix abs
            Identity = identity
            Zeros = createZeros
            Ones = createOnes
            UnifRnd = unifRnd randGen
        }

    let inline arithmeticMatrixOpsRec randGen = createArithmeticMatrixOpsRec identity createZeros createOnes mulMatrix (+) (-) (*) (/) (~-) abs randGen

    let inline createFloatingMatrixOpsRec<'T> identity createZeros createOnes mulMatrix (+) (-) (*) (/) (~-) abs ( ** )
                                     sqrt sin cos tan asin acos atan sinh cosh tanh exp log log10 erf erfc erfinv erfcinv normcdf norminv round ceil 
                                     choleskyDecomp choleskySolve luDecomp luSolve qrDecomp qrSolveFull svdSolve svdDecomp 
                                     unifRnd normalRnd lognormRnd bernRnd binomRnd mvNormRnd poissonRnd
                                     (m4StatZero : M4Stat<'T>) extractMean extractSkewness updateM4Stat quantiles extractKurtosis 
                                     extractVar covcorr (extractCov : CovCorr<'T> -> 'T) extractCorr genericZero genericOne
                                     (randGen : int -> 'T -> 'T -> 'T[]) (eps :'T) =
        { noneNumericMatrixOpsRec<'T> with
            AddScalar = (funMatrixScalar (+) : matrixData<'T> -> 'T -> matrixData<'T>) 
            MulMatrix = mulMatrix
            Add = funMatrixMatrix (+)
            MulScalar = funMatrixScalar (*)
            Mul = funMatrixMatrix (*)
            DivScalar = funMatrixScalar (/)
            Div = funMatrixMatrix (/)
            DivMatrix = funScalarMatrix (/)
            SubScalar = funMatrixScalar (-)
            Sub = funMatrixMatrix (-)
            SubMatrix = funScalarMatrix (-)
            Minus = funMatrix (~-)
            Abs = funMatrix abs
            Identity = identity
            Zeros = createZeros
            Ones = createOnes
            PowScalar = funMatrixScalar ( ** )
            Pow = funMatrixMatrix ( ** )
            PowMatrix = funScalarMatrix ( ** )
            Sqrt = funMatrix sqrt
            Sin = funMatrix sin
            Cos = funMatrix cos
            Tan = funMatrix tan
            ASin = funMatrix asin
            ACos = funMatrix acos
            ATan = funMatrix atan
            Sinh = funMatrix sinh
            Cosh = funMatrix cosh
            Tanh = funMatrix tanh
            Exp = funMatrix exp
            Log = funMatrix log
            Log10 = funMatrix log10
            Erf = erf
            Erfc = erfc
            Erfinv = erfinv
            Erfcinv = erfcinv
            Normcdf = normcdf
            Norminv = norminv
            Round = funMatrix round
            Ceil = funMatrix ceil
            CholeskyDecomp = choleskyDecomp
            CholeskySolve = choleskySolve
            LuDecomp = luDecomp
            LuSolve = luSolve
            QrDecomp = qrDecomp
            QrSolveFull = qrSolveFull
            SvdSolve = svdSolve
            SvdDecomp = svdDecomp eps
            UnifRnd = unifRnd randGen
            NormalRnd = normalRnd randGen
            LognormalRnd = lognormRnd randGen
            BernRnd = bernRnd randGen
            BinomRnd = binomRnd randGen
            MVnormalRnd = mvNormRnd randGen
            PoissonRnd = poissonRnd randGen
            Mean = foldNdim m4StatZero updateM4Stat extractMean
            Skewness = foldNdim m4StatZero updateM4Stat  extractSkewness
            Kurtosis = foldNdim m4StatZero updateM4Stat extractKurtosis
            Variance = foldNdim m4StatZero updateM4Stat extractVar
            Quantiles = quantiles
            Correlation = covcorr extractCorr
            Covariance = covcorr extractCov
            Sum = foldNdim genericZero (+) id
            Prod = foldNdim genericOne (*) id
            CumSum = scanNdim genericZero (+)
            CumProd =  scanNdim genericOne (*)
        }

    let inline floatingMatrixOpsRec randGen = 
            createFloatingMatrixOpsRec
                identity createZeros createOnes mulMatrix (+) (-) (*) (/) (~-) abs ( ** )
                sqrt sin cos tan asin acos atan sinh cosh tanh exp log log10 Erf Erfc Erfinv Erfcinv
                Normcdf Norminv round ceil 
                choleskyDecomp choleskySolve luDecomp luSolve qrDecomp qrSolveFull svdSolve svdDecomp unifRnd
                normalRnd lognormRnd bernRnd binomRnd mvNormRnd poissonRnd
                (getM4StatZero()) extractMean extractSkewness updateM4Stat quantiles extractKurtosis 
                extractVar covcorr extractCov extractCorr 0G 1G randGen (eps())
       
    let createNumericMatrixOps (numOpsRec : NumericMatrixOpsRec<'T>) =
        {new INumericMatrixOps<'T> with
            member this.MulMatrix(matixData1, size1, matrixData2, size2) = numOpsRec.MulMatrix matixData1 size1 matrixData2 size2
            member this.Add(matrixData, scalar) = numOpsRec.AddScalar matrixData scalar
            member this.Add(matrixData1, matrixData2) = numOpsRec.Add matrixData1 matrixData2
            member this.Mul(matrixData, scalar) = numOpsRec.MulScalar matrixData scalar
            member this.Mul(matrixData1, matrixData2) = numOpsRec.Mul matrixData1 matrixData2
            member this.Sub(matrixData1, matrixData2) = numOpsRec.Sub matrixData1 matrixData2
            member this.Sub(matrixData, scalar) = numOpsRec.SubScalar matrixData scalar
            member this.Sub(scalar, matrixData) = numOpsRec.SubMatrix scalar matrixData
            member this.Div(matrixData1, matrixData2) = numOpsRec.Div matrixData1 matrixData2
            member this.Div(matrixData, scalar) = numOpsRec.DivScalar matrixData scalar
            member this.Div(scalar, matrixData) = numOpsRec.DivMatrix scalar matrixData
            member this.Pow(matrixData, scalar) = numOpsRec.PowScalar matrixData scalar
            member this.Pow(matrixData1, matrixData2) = numOpsRec.Pow matrixData1 matrixData2
            member this.Pow(scalar, matrixData) = numOpsRec.PowMatrix scalar matrixData
            member this.Minus(matrixData) = numOpsRec.Minus matrixData
            member this.Abs(matrixData) = numOpsRec.Abs matrixData
            member this.Sqrt(matrixData) = numOpsRec.Sqrt matrixData
            member this.Sin(matrixData) = numOpsRec.Sin matrixData
            member this.Cos(matrixData) = numOpsRec.Cos matrixData
            member this.Tan(matrixData) = numOpsRec.Tan matrixData
            member this.ASin(matrixData) = numOpsRec.ASin matrixData
            member this.ACos(matrixData) = numOpsRec.ACos matrixData
            member this.ATan(matrixData) = numOpsRec.ATan matrixData
            member this.Sinh(matrixData) = numOpsRec.Sinh matrixData
            member this.Cosh(matrixData) = numOpsRec.Cosh matrixData
            member this.Tanh(matrixData) = numOpsRec.Tanh matrixData
            member this.Exp(matrixData) = numOpsRec.Exp matrixData
            member this.Log(matrixData) = numOpsRec.Log matrixData
            member this.Log10(matrixData) = numOpsRec.Log10 matrixData
            member this.Erf(matrixData) = numOpsRec.Erf matrixData
            member this.Erfc(matrixData) = numOpsRec.Erfc matrixData
            member this.Erfinv(matrixData) = numOpsRec.Erfinv matrixData
            member this.Erfcinv(matrixData) = numOpsRec.Erfcinv matrixData
            member this.Normcdf(matrixData) = numOpsRec.Normcdf matrixData
            member this.Norminv(matrixData) = numOpsRec.Norminv matrixData
            member this.Round(matrixData) = numOpsRec.Round matrixData
            member this.Ceil(matrixData) = numOpsRec.Ceil matrixData
            member this.Identity(rows, cols) = numOpsRec.Identity rows cols
            member this.Zeros(size) = numOpsRec.Zeros size
            member this.Ones(size) = numOpsRec.Ones size
            member this.CholeskyDecomp(matrixData, n) = numOpsRec.CholeskyDecomp matrixData n
            member this.CholeskySolve(a, n, b, k) = numOpsRec.CholeskySolve a n b k
            member this.LuDecomp(matrixData, n, k) = numOpsRec.LuDecomp matrixData n k 
            member this.LuSolve(a, n, b, k) = numOpsRec.LuSolve a n b k
            member this.QrDecomp(matrixData, n, k) = numOpsRec.QrDecomp matrixData n k
            member this.QrSolveFull(a, n, k, b, m) = numOpsRec.QrSolveFull a n k b m
            member this.SvdSolve(a, n, k, b, m, tol) = numOpsRec.SvdSolve a n k b m tol
            member this.SvdDecomp(matrixData, n, k) = numOpsRec.SvdDecomp matrixData n k
            member this.UnifRnd(a, b, size) = numOpsRec.UnifRnd a b size
            member this.NormalRnd(mu, sigma, size) = numOpsRec.NormalRnd mu sigma size
            member this.LognormalRnd(mu, sigma, a, scale, size) = numOpsRec.LognormalRnd mu sigma a scale size
            member this.BernRnd(p, size) = numOpsRec.BernRnd p size
            member this.BinomRnd(n, p, size) = numOpsRec.BinomRnd n p size
            member this.MVnormalRnd(mu, cov, k, n) = numOpsRec.MVnormalRnd mu cov k n
            member this.PoissonRnd(lambda, size) = numOpsRec.PoissonRnd lambda size
            member this.Mean(matrixData, size, dim) = numOpsRec.Mean matrixData size dim
            member this.Skewness(matrixData, size, dim) = numOpsRec.Skewness matrixData size dim
            member this.Kurtosis(matrixData, size, dim) = numOpsRec.Kurtosis matrixData size dim
            member this.Variance(matrixData, size, dim) = numOpsRec.Variance matrixData size dim
            member this.Quantiles(matrixData, size, q, dim) = numOpsRec.Quantiles matrixData size q  dim
            member this.Correlation(matrixData, n, k) = numOpsRec.Correlation matrixData n k
            member this.Covariance(matrixData, n, k) = numOpsRec.Covariance matrixData n k
            member this.Sum(matrixData, size, dim) = numOpsRec.Sum matrixData size dim
            member this.Prod(matrixData, size, dim) = numOpsRec.Prod matrixData size dim
            member this.CumSum(matrixData, size, dim) = numOpsRec.CumSum matrixData size dim
            member this.CumProd(matrixData, size, dim) = numOpsRec.CumProd matrixData size dim
        }                      

