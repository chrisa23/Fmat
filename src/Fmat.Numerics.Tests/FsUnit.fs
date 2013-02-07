module FsUnit
open NUnit.Framework
open NUnit.Framework.Constraints
open System
open Fmat.Numerics
open Fmat.Numerics.GenericMatrixOps
open Fmat.Numerics.ComparableMatrixOps
open Fmat.Numerics.BoolMatrixOps
open Fmat.Numerics.NumericMatrixOps

let should (f : 'a -> #Constraint) x (y : obj) =
    let c = f x
    let y =
        match y with
        | :? (unit -> unit) -> box (new TestDelegate(y :?> unit -> unit))
        | _                 -> y
    Assert.That(y, c)

let equal x = new EqualConstraint(x)

let not x = new NotConstraint(x)

let contain x = new ContainsConstraint(x)

let haveLength n = Has.Length.EqualTo(n)

let haveCount n = Has.Count.EqualTo(n)

let be = id

let Null = new NullConstraint()

let Empty = new EmptyConstraint()

let EmptyString = new EmptyStringConstraint()

let NullOrEmptyString = new NullOrEmptyStringConstraint()

let True = new TrueConstraint()

let False = new FalseConstraint()

let sameAs x = new SameAsConstraint(x)

let throw = Throws.TypeOf

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

//let makeMock1Arg (counter : int ref) (expected : 'a) (ret : 'b) (arg : 'a) = 
//    counter := counter.Value + 1 
//    arg |> should equal expected
//    ret
//
//let makeMock2Arg (counter : int ref) (expected1 : 'a) (expected2: 'b) (ret : 'c) (arg1 : 'a) (arg2 : 'b) = 
//    counter := counter.Value + 1 
//    arg1 |> should equal expected1
//    arg2 |> should equal expected2
//    ret
//
//let makeMock3Arg (counter : int ref) (expected1 : 'a) (expected2: 'b) (expected3: 'c) (ret : 'd) (arg1 : 'a) (arg2 : 'b)  (arg3 : 'c)= 
//    counter := counter.Value + 1 
//    arg1 |> should equal expected1
//    arg2 |> should equal expected2
//    arg3 |> should equal expected3
//    ret
//
//let makeMock4Arg (counter : int ref) (expected1 : 'a) (expected2: 'b) (expected3: 'c) (expected4: 'd) (ret : 'e) (arg1 : 'a) (arg2 : 'b)  (arg3 : 'c) (arg4 : 'd)= 
//    counter := counter.Value + 1 
//    arg1 |> should equal expected1
//    arg2 |> should equal expected2
//    arg3 |> should equal expected3
//    arg4 |> should equal expected4
//    ret
//
//let notImpl1Arg = fun x -> raise (new NotImplementedException())
//
//let notImpl2Arg = fun x y -> raise (new NotImplementedException())
//
//let notImpl3Arg = fun x y z -> raise (new NotImplementedException())
//
//let notImpl4Arg = fun x y z u -> raise (new NotImplementedException())
//
//let notImpl5Arg = fun x y z u t -> raise (new NotImplementedException())
//
//let notImpl6Arg = fun x y z u t v -> raise (new NotImplementedException())
//
//let notImplGenericMatrixOps<'T> =
//    {
//        CreateMatrixDataFromSeq = (notImpl1Arg : seq<'T> -> matrixData<'T>)
//        CreateMatrixDataFromScalar = notImpl1Arg
//        CreateMatrixDataFromSizeScalar = notImpl2Arg
//        CreateMatrixDataFromArray2D = notImpl1Arg
//        CreateMatrixDataFromArray3D = notImpl1Arg
//        CreateMatrixDataFromArray4D = notImpl1Arg
//        CreateMatrixDataFromBool = notImpl1Arg
//        CloneMatrixData = notImpl1Arg
//        TransposeMatrixData = notImpl2Arg
//        TransposeMatrixDataInPlace = notImpl2Arg
//        GetLinearSlice = notImpl3Arg
//        SetLinearSliceScalar = notImpl4Arg
//        SetLinearSlice = notImpl4Arg
//        GetSlice = notImpl3Arg
//        SetSliceScalar = notImpl4Arg
//        SetSlice = notImpl4Arg
//        GetItemLinear = notImpl2Arg
//        SetItemLinear = notImpl3Arg
//        GetItem = notImpl3Arg
//        SetItem = notImpl4Arg
//        GetItemsLinear = notImpl2Arg
//        SetItemsLinear = notImpl3Arg
//        GetItems = notImpl3Arg
//        SetItems = notImpl4Arg
//        GetBoolItems = notImpl2Arg
//        SetBoolItems = notImpl3Arg
//        SetItemsLinearScalar = notImpl3Arg
//        SetItemsScalar = notImpl4Arg
//        SetBoolItemsScalar = notImpl3Arg
//        GetDiag = notImpl3Arg
//        FormatMatrixData = notImpl3Arg
//        Concat = notImpl2Arg
//        SetDiagonal = notImpl2Arg
//        TriLower = notImpl4Arg
//        TriUpper = notImpl4Arg
//        Repmat = notImpl3Arg
//    }
//
//let notImplComparableMatrixOps<'T> =
//    {
//        AreEqual = (notImpl2Arg : matrixData<'T> -> matrixData<'T> -> bool)
//        AllEqual = notImpl2Arg
//        AllNotEqual = notImpl2Arg
//        AllLessThan = notImpl2Arg
//        AllLessThanEqual = notImpl2Arg
//        AllGreaterThan = notImpl2Arg
//        AllGreaterThanEqual = notImpl2Arg
//        LessThanScalar = notImpl2Arg
//        LessThanEqualScalar = notImpl2Arg
//        GreaterThanScalar = notImpl2Arg
//        GreaterThanEqualScalar = notImpl2Arg
//        EqualElementwiseScalar = notImpl2Arg
//        NotEqualElementwiseScalar = notImpl2Arg
//        LessThan= notImpl2Arg
//        LessThanEqual = notImpl2Arg
//        GreaterThan = notImpl2Arg
//        GreaterThanEqual = notImpl2Arg
//        EqualElementwise = notImpl2Arg
//        NotEqualElementwise = notImpl2Arg
//        MinXY = notImpl2Arg
//        MaxXY = notImpl2Arg
//    }
//
//let notImplBoolMatrixOps<'T> =
//    {
//        And = (notImpl2Arg : matrixData<'T> -> matrixData<'T> -> matrixData<'T>) 
//        Or = notImpl2Arg
//        Not = notImpl1Arg
//    }
//
//let notImplNumericMatrixOps<'T> =
//    {
//        MulMatrix = (notImpl4Arg : matrixData<'T> -> int[] -> matrixData<'T> -> int[] -> matrixData<'T>)
//        AddScalar = notImpl2Arg
//        Add = notImpl2Arg
//        MulScalar = notImpl2Arg
//        Mul = notImpl2Arg
//        SubScalar = notImpl2Arg
//        Sub = notImpl2Arg
//        SubMatrix = notImpl2Arg
//        DivScalar = notImpl2Arg
//        Div = notImpl2Arg
//        DivMatrix = notImpl2Arg
//        PowScalar = notImpl2Arg
//        Pow = notImpl2Arg
//        PowMatrix = notImpl2Arg
//        Minus = notImpl1Arg
//        Abs = notImpl1Arg
//        Sqrt = notImpl1Arg
//        Sin = notImpl1Arg
//        Cos = notImpl1Arg
//        Tan = notImpl1Arg
//        ASin = notImpl1Arg
//        ACos = notImpl1Arg
//        ATan = notImpl1Arg
//        Sinh = notImpl1Arg
//        Cosh = notImpl1Arg
//        Tanh = notImpl1Arg
//        Exp = notImpl1Arg
//        Log = notImpl1Arg
//        Log10 = notImpl1Arg
//        Round = notImpl1Arg
//        Ceil = notImpl1Arg
//        Identity = notImpl2Arg
//        Zeros = notImpl1Arg
//        Ones = notImpl1Arg
//        CholeskyDecomp = notImpl2Arg
//        CholeskySolve = notImpl4Arg
//        CholeskyInverse = notImpl2Arg
//        LuDecomp = notImpl3Arg
//        LuInverse = notImpl2Arg
//        LuSolve = notImpl4Arg
//        QrDecomp = notImpl3Arg
//        QrSolveFull = notImpl5Arg
//        QrSolve = notImpl6Arg
//        SvdSolve = notImpl5Arg
//        SvdValues = notImpl3Arg
//        SvdDecomp = notImpl3Arg
//        EigenDecomp = notImpl2Arg
//        EigenValues = notImpl2Arg
//        UnifRnd = notImpl3Arg
//        NormalRnd = notImpl3Arg
//        Mean = notImpl3Arg
//        Skewness = notImpl3Arg
//        Kurtosis = notImpl3Arg
//        Variance = notImpl3Arg
//        Quantiles = notImpl4Arg
//        Correlation = notImpl3Arg
//        Covariance = notImpl3Arg
//        Sum = notImpl3Arg
//        CumSum = notImpl3Arg
//        Prod = notImpl3Arg
//        CumProd = notImpl3Arg
//    }

