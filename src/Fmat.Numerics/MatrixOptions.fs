namespace FMath.Numerics

open System

///<summary>Type with static properties for getting and setting runtime matrix options
///</summary>
type MatrixOptions() =
    static let mutable _displayDigits = 2
    static let mutable _maxDisplaySize = 10

    ///<summary>Gets or sets number of characters to be used when displaying matrix elements in F# interactive or when calling Matrix.ToString()
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open FCore.Numerics
    ///let x = Matrix(1.22222)
    ///Matrix.DisplayDigits &lt;- 3 // 3 digits accuracy
    ///let s = x.ToString() //returns "1.222"
    ///</code>
    ///</example>
    ///<remarks>Default value is 2
    ///This setting has no influence on precision of matrix calculations
    ///</remarks>
    static member DisplayDigits
        with get() = _displayDigits
        and set(value) = _displayDigits <- value

    ///<summary>Gets or sets maximum number of rows/columns/pages to display when calling Matrix.ToString()
    ///</summary>
    ///<example>Code example:
    ///<code lang="F#">
    ///open FCore.Numerics
    ///open FCore.Numerics.MatrixFunctions
    ///let x = ones [10;10]
    ///Matrix.MaxDisplaySize &lt;- 2 // show first 2 rows and 2 columns only
    ///let s = x.ToString() 
    ///</code>
    ///</example>
    ///<remarks>Default value is 10
    ///</remarks>
    static member MaxDisplaySize
        with get() = _maxDisplaySize
        and set(value) = _maxDisplaySize <- value