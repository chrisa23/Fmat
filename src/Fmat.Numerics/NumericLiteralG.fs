namespace Fmat.Numerics

module NumericLiteralG =
    open System
    open System.Collections.Generic
    open Fmat.Numerics.Conversion

    let inline FromZero() = LanguagePrimitives.GenericZero
    let inline FromOne() = LanguagePrimitives.GenericOne
    let inline FromInt32 (n : int) = !!n
    let inline FromInt64 (n : int64) = !!n

