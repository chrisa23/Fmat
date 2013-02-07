namespace Fmat.Numerics
open System
open Fmat.Numerics.GenericMatrixOps
open Fmat.Numerics.ComparableMatrixOps
open Fmat.Numerics.BoolMatrixOps
open Fmat.Numerics.NumericMatrixOps
open Fmat.Numerics.Distributions

type GenericLib<'T>() =
    interface IMatrixOps<'T> with
        member this.GenericMatrixOps = createGenericMatrixOps noneBoolConversionGenericMatrixOpsRec
        member this.ComparableMatrixOps = createComparableMatrixOps noneComparableMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps noneNumericMatrixOpsRec

type GenericMatrix<'T> = Matrix<'T,GenericLib<'T>>

type ComparableLib<'T when 'T : comparison>() =
    interface IMatrixOps<'T> with
        member this.GenericMatrixOps = createGenericMatrixOps noneBoolConversionGenericMatrixOpsRec
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps noneNumericMatrixOpsRec

type ComparableMatrix<'T when 'T : comparison> = Matrix<'T,ComparableLib<'T>>

type StringLib() =
    interface IMatrixOps<string> with
        member this.GenericMatrixOps = createGenericMatrixOps (genericMatrixOpsRec string)
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps noneNumericMatrixOpsRec

type StringMatrix = Matrix<string,StringLib>

type BoolLib() =
    interface IMatrixOps<bool> with
        member this.GenericMatrixOps = createGenericMatrixOps (genericMatrixOpsRec id)
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps boolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps noneNumericMatrixOpsRec

type BoolMatrix = Matrix<bool,BoolLib>

type IntLib() =
    interface IMatrixOps<int> with
        member this.GenericMatrixOps = createGenericMatrixOps arithmeticGenericMatrixOpsRec
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps (arithmeticMatrixOpsRec getRandIntArray)

type IntMatrix = Matrix<int,IntLib>

type Float32Lib() =
    interface IMatrixOps<float32> with
        member this.GenericMatrixOps = createGenericMatrixOps arithmeticGenericMatrixOpsRec
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps (floatingMatrixOpsRec getRand32Array)

type Matrix32 = Matrix<float32,Float32Lib>

type FloatLib () =
    interface IMatrixOps<float> with
        member this.GenericMatrixOps = createGenericMatrixOps arithmeticGenericMatrixOpsRec
        member this.ComparableMatrixOps = createComparableMatrixOps compMatrixOpsRec
        member this.BoolMatrixOps = createBoolMatrixOps noneBoolMatrixOpsRec
        member this.NumericMatrixOps = createNumericMatrixOps (floatingMatrixOpsRec getRandArray)
                                                              
type Matrix = Matrix<float,FloatLib>

