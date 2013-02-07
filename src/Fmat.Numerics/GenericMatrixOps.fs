namespace Fmat.Numerics

open System
open MatrixUtil
open System.Text
open Validation

type GenericMatrixOpsRec<'T> =
    {
        CreateMatrixDataFromSeq : seq<'T> -> matrixData<'T> * int
        CreateMatrixDataFromScalar : 'T -> matrixData<'T>
        CreateMatrixDataFromSizeScalar : seq<int> -> 'T -> matrixData<'T>
        CreateMatrixDataFromArray2D : 'T[,] -> matrixData<'T> * int[]
        CreateMatrixDataFromArray3D : 'T[,,] -> matrixData<'T> * int[]
        CreateMatrixDataFromArray4D : 'T[,,,] -> matrixData<'T> * int[]
        CreateMatrixDataFromBool : matrixData<bool> -> matrixData<'T>
        CreateMatrixDataFromSeqSeq : seq<seq<'T>> -> matrixData<'T> * int[]
        CreateMatrixDataFrom1DFun : int -> (int -> 'T) -> matrixData<'T>
        CreateMatrixDataFrom2DFun : int[] -> (int -> int -> 'T) -> matrixData<'T>
        CreateMatrixDataFrom3DFun : int[] -> (int -> int -> int -> 'T) -> matrixData<'T>
        CreateMatrixDataFrom4DFun : int[] -> (int -> int -> int -> int -> 'T) -> matrixData<'T>
        CloneMatrixData : matrixData<'T> -> matrixData<'T>
        ConvertToArray : matrixData<'T> -> 'T[]
        ConvertToArray2D : matrixData<'T> -> int -> 'T[,]
        ConvertToArray3D : matrixData<'T> -> int -> int -> 'T[,,]
        ConvertToArray4D : matrixData<'T> -> int -> int -> int -> 'T[,,,]
        ConvertToColMajorSeq : matrixData<'T> -> seq<'T>
        TransposeMatrixData : matrixData<'T> -> int[] -> matrixData<'T>
        TransposeMatrixDataInPlace : matrixData<'T> -> int[] -> unit
        GetLinearSlice : matrixData<'T> -> int -> int -> matrixData<'T>
        SetLinearSliceScalar : matrixData<'T> -> int -> int -> 'T -> unit
        SetLinearSlice : matrixData<'T> -> int -> int -> matrixData<'T> -> unit
        GetSlice : matrixData<'T> -> int[] -> (int * int)[] -> matrixData<'T>
        SetSliceScalar : matrixData<'T> -> int[] -> (int * int)[] -> 'T -> unit
        SetSlice : matrixData<'T> -> int[] -> (int * int)[] -> matrixData<'T> -> unit
        GetItemLinear : matrixData<'T> -> int -> 'T
        SetItemLinear : matrixData<'T> -> int -> 'T -> unit
        GetItem : matrixData<'T> -> int[] -> int[] -> 'T
        SetItem : matrixData<'T> -> int[] -> int[] -> 'T -> unit
        GetItemsLinear : matrixData<'T> -> seq<int> -> matrixData<'T>
        SetItemsLinear : matrixData<'T> -> seq<int> -> matrixData<'T> -> unit
        GetItems : matrixData<'T> -> int[] -> seq<int>[] -> matrixData<'T>*int[]
        SetItems : matrixData<'T> -> int[] -> seq<int>[] -> matrixData<'T> -> unit
        GetBoolItems : matrixData<'T> ->  matrixData<bool> -> matrixData<'T>
        SetBoolItems : matrixData<'T> ->  matrixData<bool> -> matrixData<'T> -> unit
        SetItemsLinearScalar : matrixData<'T> -> seq<int> -> 'T -> unit
        SetItemsScalar : matrixData<'T> -> int[] -> seq<int>[] -> 'T -> unit
        SetBoolItemsScalar : matrixData<'T> -> matrixData<bool> -> 'T -> unit
        GetPredicateItems : matrixData<'T> -> ('T -> bool) -> matrixData<'T>
        SetPredicateItems : matrixData<'T> -> ('T -> bool) -> matrixData<'T> -> unit
        SetPredicateItemsScalar : matrixData<'T> -> ('T -> bool) -> 'T -> unit
        GetDiag : matrixData<'T> -> int[] -> int -> matrixData<'T>
        FormatMatrixData : matrixData<'T> -> int[] -> string -> int[] -> string
        Concat : seq<matrixData<'T> * int[]> -> int -> matrixData<'T> * int[]
        SetDiagonal : matrixData<'T> -> int -> matrixData<'T> * int[]
        TriLower: int -> int -> matrixData<'T> -> int -> matrixData<'T>
        TriUpper: int -> int -> matrixData<'T> -> int -> matrixData<'T>
        Repmat : matrixData<'T> -> int[] -> int[] -> matrixData<'T> * int[]
        ApplyFunInPlace : matrixData<'T> -> ('T -> 'T) -> unit
        ApplyFun1Arg : matrixData<'T> -> ('T -> 'T) -> matrixData<'T>
        ApplyFun2Arg : matrixData<'T> -> matrixData<'T> -> ('T -> 'T -> 'T) -> matrixData<'T>
        ApplyFun3Arg : matrixData<'T> -> matrixData<'T> -> matrixData<'T> -> ('T -> 'T -> 'T -> 'T) -> matrixData<'T>
    }


module GenericMatrixOps =
    let createMatrixDataFromSeq (data : seq<'T>) =
        let array = data |> Array.ofSeq
        Managed(array), array.Length

    let createMatrixDataFromSeqSeq (data : seq<seq<'T>>) =
        let rows = data |> Seq.length
        let cols = data |> Seq.map Seq.length |> Seq.max
        let res = Managed (Array.zeroCreate<'T> (rows*cols))
        let dimProd = getDimProd [|rows;cols|]
        data |> Seq.iteri (fun row rowSeq -> rowSeq |> Seq.iteri (fun col x -> setMatrixDataItem dimProd [|row;col|] res x))
        res, [|rows;cols|]

    let createMatrixDataFromScalar (scalar : 'T) =
        Managed([|scalar|]) 

    let createMatrixDataFromSizeScalar (size : seq<int>) (scalar : 'T) =  
        let len = size |> Seq.fold (*) 1
        let array = Array.create len scalar
        Managed(array) 

    let createMatrixDataFromArray2D (data : 'T[,]) =
        let size = [|data.GetLength(0); data.GetLength(1)|]
        let dataSeq = seq{for j in 0..size.[1] - 1 do
                                for i in 0..size.[0] - 1 do
                                    yield data.[i, j]}
        fst(createMatrixDataFromSeq(dataSeq)), size

    let createMatrixDataFromArray3D (data : 'T[,,]) =
        let size = [|data.GetLength(0); data.GetLength(1); data.GetLength(2)|]
        let dataSeq = seq{for k in 0..size.[2]-1 do
                                for j in 0..size.[1]-1 do
                                    for i in 0..size.[0]-1 do
                                        yield data.[i, j, k]}
        fst(createMatrixDataFromSeq(dataSeq)), size

    let createMatrixDataFromArray4D (data : 'T[,,,]) =
        let size = [|data.GetLength(0); data.GetLength(1); data.GetLength(2); data.GetLength(3)|]
        let dataSeq = seq{for l in 0..size.[3]-1 do
                                for k in 0..size.[2]-1 do
                                    for j in 0..size.[1]-1 do
                                        for i in 0..size.[0]-1 do
                                            yield data.[i, j, k, l]}
        fst(createMatrixDataFromSeq(dataSeq)), size

    let createMatrixDataFrom1DFun len f =
        Managed (Array.init len f)

    let createMatrixDataFrom2DFun (size : int[]) f =
        let n = size.[0]
        let k = size.[1]
        let res = Managed(Array.zeroCreate<'T> (n*k))
        let dimProd = getDimProd [|n;k|]
        for i in 0..n-1 do
            for j in 0..k-1 do
                setMatrixDataItem dimProd [|i;j|] res (f i j)
        res

    let createMatrixDataFrom3DFun (size : int[]) f =
        let dim0 = size.[0]
        let dim1 = size.[1]
        let dim2 = size.[2]
        let res = Managed(Array.zeroCreate<'T> (dim0*dim1*dim2))
        let dimProd = getDimProd [|dim0;dim1;dim2|]
        for i in 0..dim0-1 do
            for j in 0..dim1-1 do
                for k in 0..dim2-1 do
                    setMatrixDataItem dimProd [|i;j;k|] res (f i j k)
        res

    let createMatrixDataFrom4DFun (size : int[]) f =
        let dim0 = size.[0]
        let dim1 = size.[1]
        let dim2 = size.[2]
        let dim3 = size.[3]
        let res = Managed(Array.zeroCreate<'T> (dim0*dim1*dim2*dim3))
        let dimProd = getDimProd [|dim0;dim1;dim2;dim3|]
        for i in 0..dim0-1 do
            for j in 0..dim1-1 do
                for k in 0..dim2-1 do
                    for l in 0..dim3-1 do
                        setMatrixDataItem dimProd [|i;j;k;l|] res (f i j k l)
        res

    let cloneMatrixData (matrixData : matrixData<'T>) =
        Managed(matrixData |> matrixDataToSeq |> Seq.map id |> Seq.toArray)

    let convertToArray matrixData =
        match matrixData with
          | Managed x -> x |> Array.copy

    let convertToArray2D matrixData rows =
        match matrixData with
          | Managed x -> 
              let cols = x.Length / rows
              Array2D.init rows cols (fun i j -> x.[j * rows + i])

    let convertToArray3D matrixData rows cols =
        match matrixData with
          | Managed x -> 
              let pages = x.Length / (rows * cols)
              Array3D.init rows cols pages (fun i j k -> x.[k * (rows * cols) + j * rows + i])

    let convertToArray4D matrixData rows cols pages =
        match matrixData with
          | Managed x -> 
              let dim3 = x.Length / (rows * cols * pages)
              Array4D.init rows cols pages dim3 (fun i j k l -> x.[l * (rows * cols * pages) + k * (rows * cols) + j * rows + i])

    let convertToColMajorSeq matrixData =
        match matrixData with
          | Managed x -> x |> Array.toSeq

    let transposeMatrixData matrixData (size : int[]) =
        match matrixData with
            | Managed arr -> let res = Array.zeroCreate<'T> (arr.Length)
                             let rows = size.[0]
                             let cols = size.[1]
                             for j in 0..cols-1 do
                                for i in 0..rows-1 do
                                    res.[i*cols+j] <- arr.[j*rows+i]
                             Managed(res)   
     
    let transposeMatrixDataInPlace matrixData (size : int[]) =
        match matrixData with
            | Managed arr -> let res = Array.zeroCreate<'T> (arr.Length)
                             let rows = size.[0]
                             let cols = size.[1]
                             for j in 0..cols-1 do
                                for i in 0..rows-1 do
                                    res.[i*cols+j] <- arr.[j*rows+i]
                             res.CopyTo(arr, 0)

    let getLinearSlice matrixData start finish =
        match matrixData with
        | Managed arr -> let len = finish - start + 1
                         let res = Array.zeroCreate<'T> len
                         for k in start..finish do
                            res.[k - start] <- arr.[k]
                         Managed(res)

    let setLinearSliceScalar matrixData start finish value =
        match matrixData with
        | Managed arr -> for k in start..finish do
                                 arr.[k] <- value

    let setLinearSlice matrixData start finish value =
        match matrixData, value with
        | Managed arr, Managed v -> for k in start..finish do
                                                arr.[k] <- v.[k - start]            
                                                
                                                   
    let getSlice matrixData (size : int[]) (slice : (int * int)[]) =
        match matrixData with
            | Managed srcArray -> 
                    let seqArr = slice |> Seq.mapi (fun i x -> if (i = 0) then {fst(slice.[0])..fst(slice.[0])} else {fst(slice.[i])..snd(slice.[i])})
                                |> cartesian
                    let resLen = slice |> Array.map (fun x -> snd(x) - fst(x) + 1) |> Array.fold (*) 1
                    let resArray = Array.zeroCreate<'T> resLen
                    let n = snd(slice.[0]) - fst(slice.[0]) + 1
                    let dimProd = getDimProd(size)
                    let mutable indexOut = 0
                    for col in seqArr do
                        let index = sub2ind dimProd col
                        for i in 0..n-1 do
                            resArray.[indexOut + i] <- srcArray.[index + i]
                        indexOut <- indexOut + n
                    Managed(resArray)

    let setSliceScalar matrixData (size : int[]) (slice : (int * int)[]) value =
        match matrixData with
            | Managed srcArray -> 
                    let seqArr = slice |> Seq.mapi (fun i x -> if (i = 0) then {fst(slice.[0])..fst(slice.[0])} else {fst(slice.[i])..snd(slice.[i])})
                                |> cartesian
                    let n = snd(slice.[0]) - fst(slice.[0]) + 1
                    let dimProd = getDimProd(size)
                    for col in seqArr do
                        let index = sub2ind dimProd col
                        for i in 0..n-1 do
                            srcArray.[index + i] <- value

    let setSlice matrixData (size : int[]) (slice : (int * int)[]) value =
        match matrixData,value with
            | Managed srcArray, Managed v -> 
                    let seqArr = slice |> Seq.mapi (fun i x -> if (i = 0) then {fst(slice.[0])..fst(slice.[0])} else {fst(slice.[i])..snd(slice.[i])})
                                |> cartesian
                    let n = snd(slice.[0]) - fst(slice.[0]) + 1
                    let dimProd = getDimProd(size)
                    let mutable indexValue = 0
                    for col in seqArr do
                        let index = sub2ind dimProd col
                        for i in 0..n-1 do
                            srcArray.[index + i] <- v.[indexValue]
                            indexValue <- indexValue + 1

    let getItemLinear matrixData index =
        match matrixData with
            | Managed arr -> arr.[index]

    let setItemLinear matrixData index value =
        match matrixData with
            | Managed arr -> arr.[index] <- value                 

    let getItem matrixData (size : int[]) (indices : int[]) =
        let dimProd = getDimProd(size)
        let index = sub2ind dimProd indices
        getItemLinear matrixData index    
 
    let setItem matrixData (size : int[]) (indices : int[]) value =
        let dimProd = getDimProd(size)
        let index = sub2ind dimProd indices
        setItemLinear matrixData index value
        
    let getItemsLinear matrixData (indices : seq<int>) =                                   
        match matrixData with
            | Managed arr -> let len = indices |> Seq.length
                             let res = Array.zeroCreate<'T> len
                             indices |> Seq.iteri (fun i index -> res.[i] <- arr.[index])
                             Managed(res)  
    let setItemsLinear matrixData (indices : seq<int>) value =                                    
        match matrixData, value with
            | Managed arr, Managed v -> indices |> Seq.iteri (fun i index -> arr.[index] <- v.[i])    
            
    let getItems matrixData (size : int[]) (range : seq<int>[]) =   
        let dimProd = getDimProd(size)
        let resSize = range |> Array.map (fun x -> Seq.length(x))
        let indices = range |> cartesian |> Seq.map (fun x -> sub2ind dimProd x)
        (getItemsLinear matrixData indices, resSize)                                                                          

    let setItems matrixData (size : int[]) (range : seq<int>[]) value =  
        let dimProd = getDimProd(size)
        let indices = range |> cartesian |> Seq.map (fun x -> sub2ind dimProd x)
        setItemsLinear matrixData indices value

    let getBoolItems matrixData boolMatrixData =
        match (matrixData, boolMatrixData) with
            | (Managed(x), Managed(y)) -> 
                let indices = seq
                                {
                                let index = ref 0
                                let boolSeq = matrixDataToSeq boolMatrixData
                                for b in boolSeq do
                                    if b then yield !index
                                    index := !index + 1
                                }
                getItemsLinear matrixData indices

    let setBoolItems matrixData boolMatrixData value =
        match (matrixData, boolMatrixData) with
            | (Managed(x), Managed(y)) -> let nValue = getMatrixDataLength value                
                                          let indices = seq
                                                            {
                                                            let index = ref 0
                                                            let boolSeq = matrixDataToSeq boolMatrixData
                                                            for b in boolSeq do
                                                                if b then yield !index
                                                                index := !index + 1
                                                            }
                                          validateSeqLength indices nValue
                                          setItemsLinear matrixData indices value


    let setItemsLinearScalar matrixData indices value =
        match matrixData with
            | Managed arr -> indices |> Seq.iter (fun index ->  arr.[index] <- value)

    let setItemsScalar matrixData (size : int[]) (range : seq<int>[]) value =
        let dimProd = getDimProd(size)
        let rangeSize = range |> Array.map (fun x -> Seq.length(x))
        let indices = range |> cartesian |> Seq.map (fun x -> sub2ind dimProd x)
        setItemsLinearScalar matrixData indices value    
                           
    let setBoolItemsScalar matrixData boolMatrixData value =      
        match (matrixData, boolMatrixData) with
            | (Managed(x), Managed(y)) -> 
                    let indices = seq
                                    {
                                    let index = ref 0
                                    let boolSeq = matrixDataToSeq boolMatrixData
                                    for b in boolSeq do
                                        if b then yield !index
                                        index := !index + 1
                                    }
                    setItemsLinearScalar matrixData indices value  
    let getPredicateItems matrixData predicate =
        matrixData |> matrixDataToSeq |> Seq.filter predicate |> Seq.toArray |> Managed
                   
    let setPredicateItemsScalar matrixData predicate value =
        match matrixData with
            | Managed arr -> arr |> Array.iteri (fun i x -> if predicate x then arr.[i] <- value)    
            
    let setPredicateItems matrixData predicate value =
        match matrixData, value with
          | Managed m, Managed v -> let len = m |> Array.fold (fun state x -> if predicate x then state + 1 else state) 0
                                    if len <> v.Length then raise (new ArgumentException("Length mismatch"))
                                    let mutable index = 0
                                    for i in 0..m.Length - 1 do
                                        if predicate m.[i] then
                                            m.[i] <- v.[index]
                                            index <- index + 1
                    
    let getDiag matrixData (size : int[]) offset =
        match matrixData with
            | Managed arr -> let resLen = if (offset < 0) then 
                                                    if (size.[0] + offset < size.[1]) then size.[0] + offset else size.[1]
                                              else
                                                    if (size.[1] - offset < size.[0]) then size.[1] - offset else size.[0]
                             let res = Array.zeroCreate<'T> resLen
                             let k = if (offset < 0) then -offset else offset
                             let n = size.[0]
                             let mutable indexIn = if (offset > 0) then k*n else k
                             for i in 0..resLen-1 do
                                res.[i] <- arr.[indexIn]
                                indexIn <- indexIn + n + 1
                             Managed(res)    
                       
    let formatMatrixData (matrixData : matrixData<'T>) (size : int[]) format (displaySize : int[]) =
        let ndims = size.Length
        let rows = size.[0]
        let cols = size.[1]
        let maxDisplaySize = seq{
                                 for i in 0..displaySize.Length - 1 do
                                     yield max displaySize.[i] 0
                                 while true do
                                     yield max displaySize.[displaySize.Length - 1] 0
                                } |> Seq.take ndims |> Seq.toArray |> Array.mapi (fun i x -> min x size.[i])
        let pageMatSize = seq{
                               yield 1
                               yield 1
                               yield! maxDisplaySize |> Seq.skip 2
                             } |> getSqueezedSize
        let collapsedSize = seq{
                               yield rows*cols
                               yield 1
                               yield! size |> Seq.skip 2
                               } |> getSqueezedSize
        let dimProd = getDimProd collapsedSize
        let pageCount = pageMatSize |> Array.fold (*) 1
        let moreRows = rows > maxDisplaySize.[0]
        let moreCols = cols > maxDisplaySize.[1]
        let more = "..."
        match matrixData with
            | Managed arr ->
                let sb = new StringBuilder()
                sb.Append("\r\n") |> ignore
                sb.Append(String.Format("Size: {0} x {1}", size.[0], size.[1])) |> ignore    
                for i in 2..ndims-1 do
                    sb.Append(String.Format(" x {0}", size.[i])) |> ignore
                sb.Append("\r\n") |> ignore
                for p in 0..pageCount - 1 do
                    let sub = ind2sub p pageMatSize
                    let pageStart = sub2ind dimProd sub
                    let mutable maxChars = 3
                    for i in 0..maxDisplaySize.[0]-1 do
                        for j in 0..maxDisplaySize.[1]-1 do
                            let v = getItemLinear matrixData (pageStart + j * rows + i) 
                            let len = String.Format("{0:" + format + "}", v).Length
                            if (len > maxChars) then maxChars <- len
                    maxChars <- maxChars + 1
                    if pageCount > 1 then
                        let pageStr = String.Join(",", sub)
                        sb.Append(String.Format("Page : ({0})\r\n", p)) |> ignore
                    for i in 0..maxDisplaySize.[0]-1 do
                        for j in 0..maxDisplaySize.[1]-1 do
                            let v = getItemLinear matrixData (pageStart + j * rows + i) 
                            let formattedVal = String.Format("{0:" + format + "}", v).PadLeft(maxChars)
                            sb.Append(formattedVal) |> ignore
                        if moreCols then sb.Append(more.PadLeft(maxChars)) |> ignore
                        sb.Append("\r\n") |> ignore
                    if moreRows then 
                        for j in 0..maxDisplaySize.[1]-1 do
                            sb.Append(more.PadLeft(maxChars)) |> ignore
                    if moreCols then sb.Append(more.PadLeft(maxChars)) |> ignore
                    sb.Append("\r\n") |> ignore
                sb.ToString()
                    
    let concat (matrices : seq<matrixData<'T> * int[]>) dim =
        let resLen = matrices |> Seq.map snd |> Seq.map (Array.fold (*) 1) |> Seq.fold (+) 0
        let maxDim = matrices |> Seq.map (fun x -> snd(x).Length) |> Seq.max
        let resDim = if (dim >= maxDim) then dim + 1 else maxDim
        let resSize = Array.create resDim 1
        let sumDim = matrices |> Seq.map (fun x -> if (snd(x).Length > dim) then snd(x).[dim] else 1) |> Seq.sum
        let fstSize = snd(matrices |> Seq.nth 0)
        for i in 0..fstSize.Length-1 do
            resSize.[i] <- fstSize.[i]
        let n = matrices |> Seq.length
        resSize.[dim] <- sumDim
        let preLengths = matrices |> Seq.map (fun item -> let s = snd(item)
                                                          s |> Seq.take (if s.Length >= dim+1 then dim+1 else s.Length)
                                                            |> Seq.fold (*) 1
                                              ) |> Seq.toArray
        let trailLen = resSize |> Seq.skip (dim+1) |> Seq.fold (*) 1


        let dataSeq = matrices |> Seq.map (fun m -> match fst(m) with 
                                                      | Managed arr-> (arr, snd(m))
                                        ) |> Seq.toArray
        let res = Array.zeroCreate<'T> resLen
        let mutable offsetOut = 0
        let offsetIn = Array.zeroCreate<int> n
        for i in 0..trailLen-1 do
            for j in 0..n-1 do
                for k in 0..preLengths.[j]-1 do
                    res.[offsetOut + k] <- fst(dataSeq.[j]).[offsetIn.[j] + k]
                offsetOut <- offsetOut + preLengths.[j]
                offsetIn.[j] <- offsetIn.[j] + preLengths.[j]
        (Managed(res), resSize)  
              
    let setDiagonal matrixData offset = 
        match matrixData with
            | Managed a -> 
                let n = a.Length
                let k = if offset < 0 then -offset else offset
                let len = (n + k) * (n + k)
                let resArr = Array.zeroCreate len
                let mutable index = if offset > 0 then offset * (n + offset) else -offset
                for i in 0..n-1 do
                    resArr.[index] <- a.[i]
                    index <- index + n + k + 1
                (Managed(resArr), [|n+k;n+k|])

    let triLower rows cols matrixData offset = 
        match matrixData with
            | Managed a -> 
                let len = rows * cols
                let resArr = Array.zeroCreate len
                if offset <= 0 then 
                    let mutable index = -offset
                    let n = if rows + offset < cols then rows + offset else cols
                    for i in 0..n-1 do
                        let len = rows + offset - i
                        for j in 0..len-1 do
                            resArr.[index + j] <- a.[index + j]
                        index <- index + rows + 1
                else
                    let n = rows * if offset < cols then offset + 1 else cols
                    for i in 0..n-1 do
                        resArr.[i] <- a.[i]
                    let mutable index = (offset + 1) * rows + 1
                    for i in offset+1..cols-1 do
                        let len = rows - i + offset
                        for j in 0..len-1 do
                            resArr.[index + j] <- a.[index + j]
                        index <- index + rows + 1
                Managed(resArr)

    let triUpper rows cols matrixData offset = 
        match matrixData with
            | Managed a -> 
                let len = rows * cols
                let resArr = Array.zeroCreate len
                if offset <= 0 then
                    let mutable index = 0
                    for i in 0..cols-1 do
                        let len = if -offset + i < rows then -offset + i + 1 else rows
                        for j in 0..len-1 do
                            resArr.[index + j] <- a.[index + j]
                        index <- index + rows
                else
                    let mutable index = offset * rows
                    for i in offset..cols-1 do
                        let len = if i - offset < rows then i - offset + 1 else rows
                        for j in 0..len-1 do
                            resArr.[index + j] <- a.[index + j]
                        index <- index + rows
                Managed(resArr)

    let repmat matrixData (size : int[]) (replicators : int[]) =
            let rec replicate (dims : int) (size : int[]) (replicators : int[]) (x : 'T[]) (y : 'T[]) (offsetX : int) (offsetY : int) =
                if dims = 1 then 
                    for i in 0..replicators.[0]-1 do
                        for j in 0..size.[0]-1 do
                            y.[offsetY + i * size.[0] + j] <- x.[offsetX + j]
                else
                    let n = size |> Seq.take (dims - 1) |> Seq.fold (fun state item -> state * item) 1
                    let N = size |> Seq.zip replicators |> Seq.take (dims - 1) |> Seq.fold (fun state item -> state * fst(item) * snd(item)) 1
                    for i in 0..size.[dims - 1]-1 do
                        replicate (dims - 1) size replicators x y (offsetX + i * n) (offsetY + i * N)
                    let M = N * size.[dims - 1]
                    for j in 1..replicators.[dims - 1]-1 do
                        for i in 0..M-1 do
                            y.[offsetY + j * M + i] <- y.[offsetY + i]
            match matrixData with
              | Managed a -> 
                    let len = size |> Seq.zip replicators |> Seq.fold (fun state item -> state * fst(item) * snd(item)) 1
                    let dims = size.Length
                    let resArr = Array.zeroCreate len
                    replicate dims size replicators a resArr 0 0
                    let resSize = size |> Seq.zip replicators |> Seq.map (fun (s, r) -> s*r) |> Seq.toArray 
                    (Managed(resArr), resSize)

    let applyFunInPlace matrixData f =
        match matrixData with
          | Managed v -> v |> Array.iteri (fun i x -> v.[i] <- f x)

    let applyFun1Arg matrixData f =
        funMatrix f matrixData

    let applyFun2Arg matrixData1 matrixData2 f =
        funMatrixMatrix f matrixData1 matrixData2

    let applyFun3Arg matrixData1 matrixData2 matrixData3 f =
        funMatrixMatrixMatrix f matrixData1 matrixData2 matrixData3

    //let getNumericFormat () = String.Format("G{0}", MatrixOptions.DisplayDigits)

    //let getGFormat = fun () -> "G"
    
    //let getDisplaySize () = MatrixOptions.MaxDisplaySize

    let inline boolToArithmetic x : 'T = if x then LanguagePrimitives.GenericOne<'T>  else LanguagePrimitives.GenericZero<'T>

    let genericMatrixOpsRec<'T> (boolConverter : bool -> 'T) =
        {
            CreateMatrixDataFromSeq = createMatrixDataFromSeq
            CreateMatrixDataFromScalar = createMatrixDataFromScalar
            CreateMatrixDataFromSizeScalar = createMatrixDataFromSizeScalar
            CreateMatrixDataFromArray2D = createMatrixDataFromArray2D
            CreateMatrixDataFromArray3D = createMatrixDataFromArray3D
            CreateMatrixDataFromArray4D = createMatrixDataFromArray4D
            CreateMatrixDataFromBool = funMatrix boolConverter
            CreateMatrixDataFromSeqSeq = createMatrixDataFromSeqSeq
            CreateMatrixDataFrom1DFun = createMatrixDataFrom1DFun
            CreateMatrixDataFrom2DFun = createMatrixDataFrom2DFun
            CreateMatrixDataFrom3DFun = createMatrixDataFrom3DFun
            CreateMatrixDataFrom4DFun = createMatrixDataFrom4DFun
            ConvertToColMajorSeq = convertToColMajorSeq
            CloneMatrixData = cloneMatrixData
            ConvertToArray = convertToArray
            ConvertToArray2D = convertToArray2D
            ConvertToArray3D = convertToArray3D
            ConvertToArray4D = convertToArray4D
            TransposeMatrixData = transposeMatrixData
            TransposeMatrixDataInPlace = transposeMatrixDataInPlace
            GetLinearSlice = getLinearSlice
            SetLinearSliceScalar = setLinearSliceScalar
            SetLinearSlice = setLinearSlice
            GetSlice = getSlice
            SetSliceScalar = setSliceScalar
            SetSlice = setSlice
            GetItemLinear = getItemLinear
            SetItemLinear = setItemLinear
            GetItem = getItem
            SetItem = setItem
            GetItemsLinear = getItemsLinear
            SetItemsLinear = setItemsLinear
            GetItems = getItems
            SetItems = setItems
            GetBoolItems = getBoolItems
            SetBoolItems = setBoolItems
            SetItemsLinearScalar = setItemsLinearScalar
            SetItemsScalar = setItemsScalar
            SetBoolItemsScalar = setBoolItemsScalar
            GetPredicateItems = getPredicateItems
            SetPredicateItems = setPredicateItems
            SetPredicateItemsScalar = setPredicateItemsScalar
            GetDiag = getDiag
            FormatMatrixData = formatMatrixData
            Concat = concat
            SetDiagonal = setDiagonal
            TriLower = triLower
            TriUpper = triUpper
            Repmat = repmat
            ApplyFunInPlace = applyFunInPlace
            ApplyFun1Arg = applyFun1Arg
            ApplyFun2Arg = applyFun2Arg
            ApplyFun3Arg = applyFun3Arg
        }

    let noneBoolConversionGenericMatrixOpsRec<'T> =
        genericMatrixOpsRec<'T> invalidOp1Arg

    let inline arithmeticGenericMatrixOpsRec<'T when  ^T : (static member Zero :  ^T) and ^T : (static member One :  ^T)> =
        genericMatrixOpsRec<'T> boolToArithmetic

    let createGenericMatrixOps (genOpsRec : GenericMatrixOpsRec<'T>) =
        {new IGenericMatrixOps<'T> with
            member this.CreateMatrixData(data : seq<'T>) = genOpsRec.CreateMatrixDataFromSeq data
            member this.CreateMatrixData(size, scalar) = genOpsRec.CreateMatrixDataFromSizeScalar size scalar
            member this.CreateMatrixData(scalar : 'T) = genOpsRec.CreateMatrixDataFromScalar scalar
            member this.CreateMatrixData(data : 'T[,]) = genOpsRec.CreateMatrixDataFromArray2D data
            member this.CreateMatrixData(data : 'T[,,]) = genOpsRec.CreateMatrixDataFromArray3D data
            member this.CreateMatrixData(data : 'T[,,,]) = genOpsRec.CreateMatrixDataFromArray4D data
            member this.CreateMatrixData(boolMatrixData) = genOpsRec.CreateMatrixDataFromBool boolMatrixData
            member this.CreateMatrixData(data : seq<seq<'T>>) = genOpsRec.CreateMatrixDataFromSeqSeq data
            member this.CreateMatrixData(n, f) = genOpsRec.CreateMatrixDataFrom1DFun n f
            member this.CreateMatrixData(size, f) = genOpsRec.CreateMatrixDataFrom2DFun size f
            member this.CreateMatrixData(size, f) = genOpsRec.CreateMatrixDataFrom3DFun size f
            member this.CreateMatrixData(size, f) = genOpsRec.CreateMatrixDataFrom4DFun size f
            member this.Transpose(matrixData, size) = genOpsRec.TransposeMatrixData matrixData size
            member this.TransposeInPlace(matrixData, size) = genOpsRec.TransposeMatrixDataInPlace matrixData size
            member this.CloneMatrixData(matrixData) = genOpsRec.CloneMatrixData matrixData
            member this.ConvertToArray(matrixData) = genOpsRec.ConvertToArray matrixData
            member this.ConvertToArray2D(matrixData, rows) = genOpsRec.ConvertToArray2D matrixData rows
            member this.ConvertToArray3D(matrixData, rows, cols) = genOpsRec.ConvertToArray3D matrixData rows cols
            member this.ConvertToArray4D(matrixData, rows, cols, pages) = genOpsRec.ConvertToArray4D matrixData rows cols pages
            member this.ConvertToColMajorSeq(matrixData) = genOpsRec.ConvertToColMajorSeq matrixData
            member this.GetLinearSlice(matrixData, start, finish) = genOpsRec.GetLinearSlice matrixData start finish
            member this.SetLinearSlice(matrixData, start, finish, v) = genOpsRec.SetLinearSliceScalar matrixData start finish v
            member this.SetLinearSlice(matrixData, start, finish, v) = genOpsRec.SetLinearSlice matrixData start finish v
            member this.GetSlice(matrixData, size, range) = genOpsRec.GetSlice matrixData size range
            member this.SetSlice(matrixData, size, range, v) = genOpsRec.SetSliceScalar matrixData size range v
            member this.SetSlice(matrixData, size, range, v) = genOpsRec.SetSlice matrixData size range v
            member this.Item
                with get(matrixData, index) = genOpsRec.GetItemLinear matrixData index
                and set(matrixData, index) value = genOpsRec.SetItemLinear matrixData index value
            member this.Item
                with get(matrixData, size, indices) = genOpsRec.GetItem matrixData size indices
                and set(matrixData, size, indices) value = genOpsRec.SetItem matrixData size indices value
            member this.Item
                with get(matrixData, indices) = genOpsRec.GetItemsLinear matrixData indices
                and set(matrixData, indices) value = genOpsRec.SetItemsLinear matrixData indices value
            member this.Item
                with get(size, matrixData, range) = genOpsRec.GetItems matrixData size range
                and set(size, matrixData, range) value = genOpsRec.SetItems matrixData size range value
            member this.Item
                with get(matrixData, boolMatrixData) = genOpsRec.GetBoolItems matrixData boolMatrixData
                and set(matrixData, boolMatrixData) value = genOpsRec.SetBoolItems matrixData boolMatrixData value
            member this.Item
                with get(matrixData, predicate : 'T -> bool) = genOpsRec.GetPredicateItems matrixData predicate
                and set(matrixData, predicate : 'T -> bool) value = genOpsRec.SetPredicateItems matrixData predicate value
            member this.Set(matrixData, indices, v) = genOpsRec.SetItemsLinearScalar matrixData indices v
            member this.Set(matrixData, size, range, v) = genOpsRec.SetItemsScalar matrixData size range v
            member this.Set(matrixData, boolMatrixData, v) = genOpsRec.SetBoolItemsScalar matrixData boolMatrixData v
            member this.Set(matrixData, predicate, v) = genOpsRec.SetPredicateItemsScalar matrixData predicate v
            member this.Diag(matrixData, size, dim) = genOpsRec.GetDiag matrixData size dim
            member this.ToString(matrixData, size, format, maxSize) = genOpsRec.FormatMatrixData matrixData size format maxSize
            member this.Concat(matrices, dim) = genOpsRec.Concat matrices dim
            member this.SetDiagonal(matrixData, offset) = genOpsRec.SetDiagonal matrixData offset
            member this.TriLower(rows, cols, matrixData, offset) = genOpsRec.TriLower rows cols matrixData offset
            member this.TriUpper(rows, cols, matrixData, offset) = genOpsRec.TriUpper rows cols matrixData offset
            member this.Repmat(size, matrixData, replicator) = genOpsRec.Repmat matrixData size replicator
            member this.ApplyFunInPlace(matrixData, f) = genOpsRec.ApplyFunInPlace matrixData f
            member this.ApplyFun(matrixData, f) = genOpsRec.ApplyFun1Arg matrixData f
            member this.ApplyFun(matrixData1, matrixData2, f) = genOpsRec.ApplyFun2Arg matrixData1 matrixData2 f
            member this.ApplyFun(matrixData1, matrixData2, matrixData3, f) = genOpsRec.ApplyFun3Arg matrixData1 matrixData2 matrixData3 f
        }



         