namespace Fmat.Numerics

open System
open System.Collections.Generic
open MatrixUtil
open Fmat.Numerics.SymmetricOperators
open Fmat.Numerics.Conversion
open Fmat.Numerics.NumericLiteralG

module StatFunctions =

    type M4Stat<'T> =
        {
            N : 'T
            Mean : 'T
            M2 : 'T
            M3 : 'T
            M4 : 'T
        }

    type CovCorr<'T> =
        {
            N : 'T
            MeanX : 'T
            MeanY : 'T
            Cov : 'T
            M2X : 'T
            M2Y : 'T
        }

    let inline getM4StatZero () : M4Stat<'T> = 
        let zero : 'T = 0G
        {N = zero; Mean = zero; M2 = zero; M3 = zero; M4 = zero}

    let inline getCovCorrZero () = 
        let zero : 'T = 0G
        {N = zero; MeanX = zero; MeanY = zero; Cov = zero; M2X = zero; M2Y = zero}

    let inline updateM4Stat (x : M4Stat<'T>) (y : 'T) : M4Stat<'T> =
        let n  = x.N + 1G
        let n2 = n * n
        let n3 = n2 * n
        let delta = y - x.Mean
        let delta2 = delta * delta
        let delta3 = delta2 * delta
        let delta4 = delta3 * delta  
        let mean = x.Mean + delta / n
        let M2  = x.M2 + delta2 * x.N / n
        let M3  = x.M3 + delta3 * x.N * (x.N - 1G) / n2 + 3G * delta * (0G - 1G * x.M2) / n
        let M4  = x.M4 + delta4 * x.N * (x.N * x.N - x.N + 1G) / n3 + 6G * delta2 * x.M2 / n2 + 4G * delta * (0G - x.M3) / n
        {N = n; Mean = mean; M2 = M2; M3 = M3; M4 = M4}

    let inline updateCovCorr (x : CovCorr<'T>) (y : 'T * 'T) : CovCorr<'T> =
        let (y1,y2) = y
        let n = x.N + 1G
        let deltaX = y1 - x.MeanX
        let delta2X = deltaX * deltaX
        let deltaY = y2 - x.MeanY
        let delta2Y = deltaY * deltaY
        {N = n; MeanX = x.MeanX + deltaX / n; MeanY = x.MeanY + deltaY / n; M2X = x.M2X + delta2X * x.N / n;
         M2Y = x.M2Y + delta2Y * x.N / n; Cov = x.Cov + (x.MeanX - y1)*(x.MeanY - y2) * x.N / n}

    let extractMean (x : M4Stat<'T>) = x.Mean

    let inline extractVar  (x : M4Stat<'T>)  : 'T =
        x.M2 / (x.N - 1G)

    let inline extractSkewness (x : M4Stat<'T>) : 'T =
        (sqrt(x.N) : 'T) * x.M3 / (x.M2 ** (!!1.5 : 'T))

    let inline extractKurtosis (x : M4Stat<'T>) = x.N * x.M4 / (x.M2 * x.M2)

    let inline extractCorr (x : CovCorr<'T>) : 'T =
        let n = x.N - 1G
        (x.Cov / n) / ((sqrt(x.M2X / n) : 'T) * sqrt(x.M2Y / n))

    let inline extractCov (x : CovCorr<'T>) : 'T =
        let n = x.N - 1G
        x.Cov / n

    let foldNdim<'S,'T> (init : 'S) (op : 'S -> 'T -> 'S) f matrixData size dim : matrixData<'T>*int[] =
        let len = size |> Array.fold (*) 1
        let K = size |> Array.toSeq |> Seq.take dim |> Seq.fold (*) 1
        let N = size.[dim]
        let M = len / (K * N)
        let res = Managed (Array.zeroCreate<'T> (K*M))
        let dimProd = getDimProd [|K;M|]
        let resSize = size |> Array.mapi (fun i n -> if i = dim then 1 else n) 
        match matrixData with
          | Managed arr ->
                for i in 0..M-1 do
                    for j in 0..K-1 do
                        let v = [|0..N-1|] |> Array.map (fun k -> arr.[i*K*N+k*K+j]) |> Array.fold op init |> f
                        setMatrixDataItem dimProd [|j;i|] res v

        (res, resSize)

    let scanNdim init (op : 'T -> 'T -> 'T) matrixData size dim : matrixData<'T> =
        let len = size |> Array.fold (*) 1
        let K = size |> Array.toSeq |> Seq.take dim |> Seq.fold (*) 1
        let N = size.[dim]
        let M = len / (K * N)
        let res = Managed (Array.zeroCreate<'T> len)
        let dimProd = getDimProd [|K;N;M|]
        match matrixData with
          | Managed arr ->
                for i in 0..M-1 do
                    for j in 0..K-1 do
                        [|0..N-1|] |> Array.map (fun k -> arr.[i*K*N+k*K+j]) |> Array.scan op init
                                                                             |> Seq.skip 1
                                                                             |> Seq.iteri (fun k v -> setMatrixDataItem dimProd [|j;k;i|] res v)
        res

    let inline quantiles (matrixData : matrixData<'T>) (size : int[]) (q : matrixData<'T>) (dim : int) : matrixData<'T>*int[] =
        let Q = getMatrixDataLength q
        let qseq = matrixDataToSeq q |> Seq.toArray
        qseq |> Seq.iter (fun x -> if x > 1G || x < 0G then raise (new ArgumentException("Quantiles must be between 0.0 and 1.0")))
        let len = size |> Array.fold (*) 1
        let K = size |> Array.toSeq |> Seq.take dim |> Seq.fold (*) 1
        let N = size.[dim]
        let M = len / (K * N)
        let res = Managed (Array.zeroCreate<'T> (K*Q*M))
        let dimProd = getDimProd [|K;Q;M|]
        let resSize = size |> Array.mapi (fun i n -> if i = dim then Q else n) 
        match matrixData with
          | Managed arr ->
                for i in 0..M-1 do
                    for j in 0..K-1 do
                        let sample = [|0..N-1|] |> Array.map (fun k -> arr.[i*K*N+k*K+j]) |> Array.sort
                        qseq |> Array.iteri (fun qi x -> let pos =  !!(N-1) * x
                                                         let k = ceil(pos) - 1G
                                                         let delta = pos - k
                                                         let kInt : int = int k
                                                         let v = 
                                                            if kInt < Q - 1 then
                                                                sample.[kInt] + delta * (sample.[kInt+1] - sample.[kInt])
                                                            else
                                                                sample.[kInt]
                                                         setMatrixDataItem dimProd [|j;qi;i|] res v)
        (res, resSize)

    let inline covcorr (extract : CovCorr<'T> -> 'T) (matrixData : matrixData<'T>) (n : int) (k : int) : matrixData<'T> =
        let s = matrixDataToSeq matrixData
        let res = Managed(Array.zeroCreate<'T> (k*k))
        let dimProd = getDimProd [|k;k|]
        let covCorrZero = getCovCorrZero()
        for j in 0..k-1 do
            for i in 0..j-1 do
                let x = s |> Seq.skip (i*n) |> Seq.take n
                let y = s |> Seq.skip (j*n) |> Seq.take n
                let v = y |> Seq.zip x |> Seq.fold updateCovCorr covCorrZero |> extract
                setMatrixDataItem dimProd [|i;j|] res v
                setMatrixDataItem dimProd [|j;i|] res v
        for i in 0..k-1 do
            let x = s |> Seq.skip (i*n) |> Seq.take n
            let v = x |> Seq.map (fun x -> (x,x)) |> Seq.fold updateCovCorr covCorrZero |> extract
            setMatrixDataItem dimProd [|i;i|] res v
        res

