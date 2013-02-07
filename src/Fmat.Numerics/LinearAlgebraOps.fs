namespace Fmat.Numerics

open System
open System.Collections.Generic
open MatrixUtil
open Fmat.Numerics.Conversion
open Fmat.Numerics.NumericLiteralG
open Fmat.Numerics.SymmetricOperators
open Fmat.Numerics.GenericFloatTypes

module LinearAlgebraOps =

    let inline eps() = GenericFloat.Instance.EpsEqual()

    let inline nearlyEqual x y eps =
        if x = y then true
        else
            if x = 0G then abs(y) <= eps
            elif y = 0G then abs(x) <= eps
            else abs(x-y)/(max (abs(x)) (abs(y))) <= eps

    let inline drotg da db =
        let mutable roe = db
        let absda = abs(da)
        let absdb = abs(db)
        if absda > absdb then
            roe <- da

        let scale = absda + absdb
        let mutable c = 0G
        let mutable s = 0G
        let mutable r = 0G
        let mutable z = 0G
        if scale = 0G then
            c <- 1G
        else
            let sda = da / scale
            let sdb = db / scale
            r <- scale * sqrt((sda * sda) + (sdb * sdb))
            if (roe < 0G) then
                r <- -r
            c <- da / r
            s <- db / r
            z <- 1G
            if (absda > absdb) then
                z <- s

            if (absdb >= absda && c <> 0G) then
                z <- 1G / c
        (r,z,c,s)

    let inline mulMatrix (matrixData1 : matrixData<'T>) (size1 :int[]) (matrixData2 : matrixData<'T>) (size2 :int[]) : matrixData<'T> =
         match matrixData1, matrixData2 with
           | Managed a, Managed b -> 
                let N = size1.[0]
                let M = size1.[1]
                let L = size2.[1]
                let res = Array.create (N*L) 0G
                let dimProd1 = getDimProd size1
                let dimProd2 = getDimProd size2
                let dimProd3 = getDimProd [|N;L|]
                for i in 0..N-1 do
                    for j in 0..L-1 do
                        let mutable s = 0G
                        for k in 0..M-1 do
                            let v1 = a.[sub2ind dimProd1 [|i;k|]]
                            let v2 = b.[sub2ind dimProd2 [|k;j|]]
                            s <- s + (v1 * v2)
                        res.[sub2ind dimProd3 [|i;j|]] <- s
                Managed res
                                                         
    let inline choleskyDecomp (matrixData : matrixData<'T>)  (n : int) : matrixData<'T> = 
        let doCholeskyStep (data : 'T[]) rowDim firstCol colLimit (multipliers : 'T[]) = 
            let tmpColCount = colLimit - firstCol
            for j in firstCol..colLimit-1 do
                let tmpVal = multipliers.[j]
                for i in j..rowDim-1 do
                    data.[(i * rowDim) + j] <- data.[(i * rowDim) + j] - multipliers.[i] * tmpVal

        let tmpColumn = Array.zeroCreate<'T> n
        match matrixData with 
          | Managed m ->
            let a = m |> Array.copy
            for ij in 0..n-1 do
                let mutable tmpVal = a.[(ij * n) + ij]
                if tmpVal > 0G then
                    tmpVal <- sqrt(tmpVal)
                    a.[(ij * n) + ij] <- tmpVal
                    tmpColumn.[ij] <- tmpVal
                    for i in ij + 1..n-1 do
                        a.[(i * n) + ij] <- a.[(i * n) + ij] / tmpVal
                        tmpColumn.[i] <- a.[(i * n) + ij]
                    doCholeskyStep a n (ij + 1) n tmpColumn
                else
                    raise (new ArgumentException("Matrix not positive definite."))

                for i in ij + 1..n-1 do
                    a.[(ij * n) + i] <- 0G
            Managed a

    let inline choleskySolve (a : matrixData<'T>)  (n : int) (b : matrixData<'T>)  (k : int) : matrixData<'T> =
        let doCholeskySolve (a : 'T[]) orderA (b : 'T[]) index =
            let cindex = index * orderA

            // Solve L*Y = B;
            let mutable sum = LanguagePrimitives.GenericZero<'T>
            for i in 0..orderA-1 do
                sum <- b.[cindex + i]
                for k in i - 1..-1..0 do
                    sum <- sum - a.[(i * orderA) + k] * b.[cindex + k]
                b.[cindex + i] <- sum / a.[(i * orderA) + i]
            // Solve L'*X = Y;
            for i in orderA - 1..-1..0 do
                sum <- b.[cindex + i]
                for k in i + 1..orderA-1 do
                    sum <- sum - a.[k * orderA + i] * b.[cindex + k]
                b.[cindex + i] <- sum / a.[i * orderA + i]

        let chol = choleskyDecomp a n
        match chol,b with
          | Managed a, Managed B ->
            let b = B |> Array.copy
            for index in 0..k-1 do
                doCholeskySolve a n b index
            Managed b

    let choleskyInverse (matrixData : matrixData<'T>)  (n : int) : matrixData<'T> = raise (new NotImplementedException())

    let inline private luipiv (matrixData : matrixData<'T>)  (n : int) =
        let ipiv = Array.init n id
        let vecLUcolj = Array.zeroCreate<'T> n
        match matrixData with
          | Managed arr -> 
                let data = arr |> Array.copy
                for j in 0..n-1 do
                    let indexj = j * n
                    let indexjj = indexj + j
                    for i in 0..n-1 do
                        vecLUcolj.[i] <- data.[indexj + i]
                    for i in 0..n-1 do
                        let kmax = min i j
                        let mutable s = 0G
                        for k in 0..kmax-1 do
                            s <- s + data.[(k * n) + i] * vecLUcolj.[k]
                        vecLUcolj.[i] <- vecLUcolj.[i] - s
                        data.[indexj + i] <- vecLUcolj.[i]
                    let mutable p = j
                    for i in (j + 1)..n-1 do
                        if abs(vecLUcolj.[i]) > abs(vecLUcolj.[p]) then
                            p <- i
                    if p <> j then
                        for  k in 0..n-1 do
                            let indexk = k * n
                            let indexkp = indexk + p
                            let indexkj = indexk + j
                            let temp = data.[indexkp]
                            data.[indexkp] <- data.[indexkj]
                            data.[indexkj] <- temp
                        ipiv.[j] <- p
                    if j < n && data.[indexjj] <> 0G then
                        for i in j + 1..n-1 do
                            data.[indexj + i] <- data.[indexj + i] / data.[indexjj]
                Managed data, ipiv

    let inline luDecomp (matrixData : matrixData<'T>)  (n : int) (k : int) : matrixData<'T> * int[] * matrixData<'T> * int[] * int[] =
        //only implemented for n = k
        let Managed(lu),ipiv = luipiv matrixData n
        let ipiv2 = Array.init n id
        let L = Array.init (n*n) (fun i -> if i / n = i % n then 1G else 0G) 
        for i in 0..n-1 do
            let temp = ipiv2.[i]
            ipiv2.[i] <- ipiv2.[ipiv.[i]]
            ipiv2.[ipiv.[i]] <- temp
        for i in 0..n-1 do
            for j in 0..i-1 do
                L.[j*n + i] <- lu.[j*n+i]
                lu.[j*n+i] <- 0G
        (Managed L, [|n;n|], Managed lu, [|n;n|], ipiv2)

    let luInverse (matrixData : matrixData<'T>)  (n : int) : matrixData<'T> = raise (new NotImplementedException())

    let inline luSolve (a : matrixData<'T>)  (n : int) (b : matrixData<'T>)  (k : int) : matrixData<'T> =
        let (Managed a,ipiv) = luipiv a n
        let columnsOfB = k
        match b with
          | Managed B ->
              let b = B |> Array.copy
              for i in 0..n-1 do
                  if ipiv.[i] <> i then
                      let p = ipiv.[i]
                      for j in 0..columnsOfB-1 do
                          let indexk = j * n
                          let indexkp = indexk + p
                          let indexkj = indexk + i
                          let temp = b.[indexkp]
                          b.[indexkp] <- b.[indexkj]
                          b.[indexkj] <- temp

              // Solve L*Y = P*B
              for k in 0..n-1 do
                  let korder = k * n
                  for i in k + 1..n-1 do
                      for j in 0..columnsOfB-1 do
                            let index = j * n
                            b.[i + index] <- b.[i + index] - b.[k + index] * a.[i + korder]


              // Solve U*X = Y;
              for k in n - 1..-1..0 do
                let mutable korder = k + (k * n)
                for j in 0..columnsOfB-1 do
                    b.[k + (j * n)] <- b.[k + (j * n)] / a.[korder]

                korder <- k * n
                for i in 0..k-1 do
                    for j in 0..columnsOfB-1 do
                        let index = j * n
                        b.[i + index] <-  b.[i + index] - b.[k + index] * a.[i + korder]
              Managed b

    let inline qrDecomp (matrixData : matrixData<'T>)  (n : int) (k : int) : matrixData<'T> * int[] * matrixData<'T> * int[] =
        let inline computeQR (work : 'T[]) workIndex (a : 'T[]) rowStart rowCount columnStart columnCount =
            if not (rowStart > rowCount || columnStart > columnCount) then
                for j in columnStart..columnCount - 1 do
                    let mutable scale = 0G
                    for i in rowStart..rowCount-1 do 
                        scale <- scale + work.[(workIndex * rowCount) + i - rowStart] * a.[(j * rowCount) + i]
                
                    for i in rowStart..rowCount-1 do 
                        a.[(j * rowCount) + i] <- a.[(j * rowCount) + i] - work.[(workIndex * rowCount) + i - rowStart] * scale

        let inline generateColumn (work : 'T[]) (a : 'T[]) rowCount row column =
            let tmp = column * rowCount
            let index = tmp + row
            for i in row..rowCount-1 do
                let iIndex = tmp + i
                work.[iIndex - row] <- a.[iIndex]
                a.[iIndex] <- 0G

            let mutable norm = 0G
            for i in 0..rowCount - row - 1 do
                let iindex = tmp + i
                norm <- norm + work.[iindex] * work.[iindex]

            norm <- sqrt(norm)
            if row = rowCount - 1 || norm = 0G then
                a.[index] <- -work.[tmp]
                work.[tmp] <- sqrt(1G+1G)
            else
                let mutable scale = 1G / norm;
                if work.[tmp] < 0G then
                    scale <- scale * -1G
                a.[index] <- -1G / scale
                for i in 0..rowCount - row - 1 do
                    work.[tmp + i] <- work.[tmp + i] * scale
                work.[tmp] <- work.[tmp] + 1G

                let s = sqrt(1G / work.[tmp])
                for i in 0..(rowCount - row - 1) do work.[tmp + i] <- work.[tmp + i] * s

        let rowsA = n
        let columnsA = k
        let work = Array.zeroCreate<'T> (rowsA * columnsA)
        let r = Array.zeroCreate<'T> (k*k)
        match matrixData with
          | Managed m ->
              let a = m |> Array.copy
              let minmn = min rowsA columnsA
              for i in 0..minmn-1 do
                  generateColumn work a rowsA i i
                  computeQR work i a i rowsA (i + 1) columnsA

              //copy R 
              for j in 0..columnsA-1 do
                let rIndex = j * columnsA
                let aIndex = j * rowsA
                for i in 0..columnsA-1 do
                    r.[rIndex + i] <- a.[aIndex+i]

              //clear A and set diagonals to 1
              Array.Clear(a, 0, a.Length)
              for i in 0..columnsA-1 do
                a.[i * rowsA + i] <- 1G

              for i in minmn - 1..-1..0 do
                computeQR work i a i rowsA i columnsA
              (Managed a, [|n;k|], Managed r, [|k;k|])

    let inline qrSolveFull (a : matrixData<'T>)  (n : int) (k : int) (b : matrixData<'T>) (m : int) : matrixData<'T> =
        let work = Array.zeroCreate<'T> (n*k)
        let (q,_,r,_) = qrDecomp a n k
        let rowsA = n
        let columnsA = k
        let columnsB = m
        let rowsQ = n
        let columnsQ = k
        let rowsR = k
        let columnsR = k
        let x = Array.zeroCreate<'T> (k*m)
        match q,r,b with
          | Managed q, Managed r, Managed b ->
              let sol = b |> Array.copy

              // Compute Y = transpose(Q)*B
              let column = Array.zeroCreate<'T> rowsA
              for j in 0..columnsB-1 do
                let jm = j * rowsA
                for k in 0..rowsA-1 do 
                    column.[k] <- sol.[jm + k]
                for i in 0..columnsA-1 do 
                    let im = i * rowsA
                    let mutable sum = 0G
                    for k in 0..rowsA-1 do
                        sum <- sum + q.[im + k] * column.[k];
                    sol.[jm + i] <- sum

              // Solve R*X = Y;
              for k in columnsA - 1..-1..0 do
                let km = k * rowsR
                for j in 0..columnsB-1 do
                    sol.[(j * rowsA) + k] <- sol.[(j * rowsA) + k] / r.[km + k]
                for i in 0..k-1 do
                    for j in 0..columnsB-1 do
                        let jm = j * rowsA
                        sol.[jm + i] <- sol.[jm + i] - sol.[jm + k] * r.[km + i]

              for row in 0..columnsR-1 do
                for col in 0..columnsB-1 do
                    x.[(col * columnsA) + row] <- sol.[row + (col * rowsA)]

              Managed x

    let qrSolve (a : matrixData<'T>)  (n : int) (k : int) (b : matrixData<'T>) (m : int) (tol : 'T) : matrixData<'T>*int = raise (new NotImplementedException())

    let svdValues (a : matrixData<'T>)  (n : int) (k : int) : matrixData<'T>*int = raise (new NotImplementedException())

    let inline svdDecomp eps (matrixData : matrixData<'T>)  (n : int) (k : int) : matrixData<'T> * int[] * matrixData<'T> * int[] * matrixData<'T> * int[] =
        let rowsA = n
        let columnsA = k
        let u = Array.zeroCreate<'T> (rowsA * rowsA)
        let vt = Array.zeroCreate<'T> (columnsA * columnsA)
        let s = Array.zeroCreate<'T> (min rowsA columnsA)
        let work = Array.zeroCreate<'T> rowsA
        let Maxiter = 1000
        let e = Array.zeroCreate<'T> columnsA
        let v = Array.zeroCreate<'T> vt.Length
        let stemp = Array.zeroCreate<'T> (min (rowsA + 1) columnsA)
        let i = 0
        let j = 0
        let mutable l = 0
        let mutable lp1 = 0
        let mutable cs = 0G
        let mutable sn = 0G
        let mutable t = 0G
        let ncu = rowsA

        let a = match matrixData with
                  | Managed a -> a |> Array.copy

        // Reduce matrix to bidiagonal form, storing the diagonal elements
        // in "s" and the super-diagonal elements in "e".
        let nct = min (rowsA - 1) columnsA
        let nrt = max 0 (min (columnsA - 2) rowsA)
        let lu = max nct nrt

        for l in 0..lu-1 do
            lp1 <- l + 1
            if l < nct then
                // Compute the transformation for the l-th column and
                // place the l-th diagonal in vector s[l].
                let mutable sum = 0G
                for i1 in l..rowsA-1 do
                    sum <- sum + a.[(l * rowsA) + i1] * a.[(l * rowsA) + i1]

                stemp.[l] <- sqrt(sum)

                if stemp.[l] <> 0G then
                    if a.[(l * rowsA) + l] <> 0G then
                        stemp.[l] <- abs(stemp.[l]) * (a.[(l * rowsA) + l] / abs(a.[(l * rowsA) + l]))

                    // A part of column "l" of Matrix A from row "l" to end multiply by 1.0 / s[l]
                    for i in l..rowsA-1 do
                        a.[(l * rowsA) + i] <- a.[(l * rowsA) + i] * (1G / stemp.[l])

                    a.[(l * rowsA) + l] <- 1G + a.[(l * rowsA) + l]

                stemp.[l] <- -stemp.[l]

            for j in lp1..columnsA-1 do
                if l < nct then
                    if stemp.[l] <> 0G then
                        // Apply the transformation.
                        t <- 0G
                        for i in l..rowsA-1 do
                            t <- t + a.[(j * rowsA) + i] * a.[(l * rowsA) + i]

                        t <- -t / a.[(l * rowsA) + l]

                        for ii in l..rowsA-1 do
                            a.[(j * rowsA) + ii] <- a.[(j * rowsA) + ii] + t * a.[(l * rowsA) + ii]

                // Place the l-th row of matrix into "e" for the
                // subsequent calculation of the row transformation.
                e.[j] <- a.[(j * rowsA) + l]

            if l < nct then
                // Place the transformation in "u" for subsequent back multiplication.
                for i in l..rowsA-1 do
                    u.[(l * rowsA) + i] <- a.[(l * rowsA) + i]

            if l < nrt then
                // Compute the l-th row transformation and place the l-th super-diagonal in e(l).
                let mutable enorm = 0G
                for i in lp1..e.Length-1 do
                    enorm <- enorm + e.[i] * e.[i]

                e.[l] <- sqrt(enorm)
                if e.[l] <> 0G then
                    if e.[lp1] <> 0G then
                        e.[l] <- abs(e.[l]) * (e.[lp1] / abs(e.[lp1]))

                    // Scale vector "e" from "lp1" by 1.0 / e[l]
                    for i in lp1..e.Length-1 do
                        e.[i] <- e.[i] * (1G / e.[l])
                    e.[lp1] <- 1G + e.[lp1]

                e.[l] <- -e.[l]

                if lp1 < rowsA && e.[l] <> 0G then
                    // Apply the transformation.
                    for i in lp1..rowsA-1 do
                        work.[i] <- 0G

                    for j in lp1..columnsA-1 do
                        for ii in lp1..rowsA-1 do
                            work.[ii] <- work.[ii] + e.[j] * a.[(j * rowsA) + ii]

                    for j in lp1..columnsA-1 do
                        let ww = -e.[j] / e.[lp1]
                        for ii in lp1..rowsA-1 do
                            a.[(j * rowsA) + ii] <- a.[(j * rowsA) + ii] + ww * work.[ii]

                // Place the transformation in v for subsequent back multiplication.
                for i in lp1..columnsA-1 do
                    v.[(l * columnsA) + i] <- e.[i]

        // Set up the final bidiagonal matrix or order m.
        let mutable m = min columnsA (rowsA + 1)
        let nctp1 = nct + 1
        let nrtp1 = nrt + 1
        if nct < columnsA then
            stemp.[nctp1 - 1] <- a.[((nctp1 - 1) * rowsA) + (nctp1 - 1)]

        if rowsA < m then
            stemp.[m - 1] <- 0G

        if nrtp1 < m then
            e.[nrtp1 - 1] <- a.[((m - 1) * rowsA) + (nrtp1 - 1)]

        e.[m - 1] <- 0G

        for j in nctp1 - 1..ncu-1 do
            for i in 0..rowsA-1 do
                u.[(j * rowsA) + i] <- 0G

            u.[(j * rowsA) + j] <- 1G

        for l in nct - 1..-1..0 do
            if stemp.[l] <> 0G then
                for j in l + 1..ncu-1 do
                    t <- 0G
                    for i in l..rowsA-1 do
                        t <- t + u.[(j * rowsA) + i] * u.[(l * rowsA) + i]

                    t <-  -t / u.[(l * rowsA) + l]

                    for ii in l..rowsA-1 do 
                        u.[(j * rowsA) + ii] <- u.[(j * rowsA) + ii] + t * u.[(l * rowsA) + ii]

                // A part of column "l" of matrix A from row "l" to end multiply by -1.0
                for i in l..rowsA-1 do
                    u.[(l * rowsA) + i] <- u.[(l * rowsA) + i] * -1G

                u.[(l * rowsA) + l] <- 1G + u.[(l * rowsA) + l]
                for i in 0..l-1 do
                    u.[(l * rowsA) + i] <- 0G
            else
                for i in 0..rowsA-1 do
                    u.[(l * rowsA) + i] <- 0G

                u.[(l * rowsA) + l] <- 1G

        // If it is required, generate v.
        for l in columnsA - 1..-1..0 do
            lp1 <- l + 1;
            if l < nrt then
                if e.[l] <> 0G then
                    for j in lp1..columnsA-1 do
                        t <- 0G
                        for i in lp1..columnsA-1 do
                            t <- t + v.[(j * columnsA) + i] * v.[(l * columnsA) + i]

                        t <- -t / v.[(l * columnsA) + lp1]
                        for ii in l..columnsA-1 do
                            v.[(j * columnsA) + ii] <- v.[(j * columnsA) + ii] + t * v.[(l * columnsA) + ii]

            for i in 0..columnsA-1 do
                v.[(l * columnsA) + i] <- 0G

            v.[(l * columnsA) + l] <- 1G


        l <- -1
        // Transform "s" and "e" so that they are double
        let tref = ref t
        let mref = ref m
        let rec transform i maxi =
            if i > maxi then ()
            else
                let mutable r = 0G
                if stemp.[i] <> 0G then
                    tref := stemp.[i]
                    r <- stemp.[i] / !tref
                    stemp.[i] <- !tref
                    if i < !mref - 1 then
                        e.[i] <- e.[i] / r

                    // A part of column "i" of matrix U from row 0 to end multiply by r
                    for j in 0..rowsA-1 do
                        u.[(i * rowsA) + j] <- u.[(i * rowsA) + j] * r

                // Exit
                if i = !mref - 1 then transform (maxi+1) maxi
                else
                    if e.[i] = 0G then transform (i+1) maxi
                    else
                        tref := e.[i]
                        r <- !tref / e.[i]
                        e.[i] <- !tref
                        stemp.[i + 1] <- stemp.[i + 1] * r

                        // A part of column "i+1" of matrix VT from row 0 to end multiply by r
                        for j in 0..columnsA-1 do
                            v.[((i + 1) * columnsA) + j] <- v.[((i + 1) * columnsA) + j] * r
                        transform (i+1) maxi
        transform 0 (!mref-1)
        t <- !tref

//        // Main iteration loop for the singular values.
        let mn = m
        let mutable iter = 0
        while m > 0 do
            // Quit if all the singular values have been found.
            // If too many iterations have been performed throw exception.
            if iter >= Maxiter then
                raise (new ArgumentException("Svd did not converge"))

            // This section of the program inspects for negligible elements in the s and e arrays,  
            // on completion the variables kase and l are set as follows:
            // kase = 1: if mS[m] and e[l-1] are negligible and l < m
            // kase = 2: if mS[l] is negligible and l < m
            // kase = 3: if e[l-1] is negligible, l < m, and mS[l, ..., mS[m] are not negligible (qr step).
            // kase = 4: if e[m-1] is negligible (convergence).
            let ztest = ref 0G
            let test = ref 0G
            let lref = ref (m-2)
            let rec checkEqual l =
                lref := l
                if l < 0 then ()
                else
                //for l in m - 2..-1..0 do
                    test := abs(stemp.[l]) + abs(stemp.[l + 1])
                    ztest := !test + abs(e.[l])
                    if nearlyEqual !test !ztest eps then // (ztest.AlmostEqualInDecimalPlaces(test, 15))
                        e.[l] <- 0G
                        ()
                    else
                        checkEqual (l - 1)

            checkEqual (m - 2)
            l <- !lref

            let mutable kase = 0
            if l = m - 2 then
                kase <- 4
            else
                let ls = ref (m-1)
                let lref = ref l
                let mref = ref m
                let rec f (ls : int ref) =
                    if !ls <= !lref then
                        ()
                    else
                    //for (ls = m - 1; ls > l; ls--)
                        test := 0G
                        if !ls <> !mref - 1 then
                            test := !test + abs(e.[!ls])

                        if !ls <> !lref + 1 then
                            test := !test + abs(e.[!ls - 1])

                        ztest := !test + abs(stemp.[!ls])
                        if nearlyEqual !ztest !test eps then
                            stemp.[!ls] <- 0G
                            ()
                        else
                            ls := !ls - 1
                            f ls   
                f ls                    
                if !ls = l then
                    kase <- 3
                elif !ls = m - 1 then
                    kase <- 1
                else
                    kase <- 2
                    l <- !ls

            l <- l + 1
            // Perform the task indicated by kase.
            let mutable k = 0
            let mutable f = 0G
            let mutable t1 = 0G
            match kase with
                // Deflate negligible s[m].
                | 1 -> 
                    f <- e.[m - 2]
                    e.[m - 2] <- 0G
                    let mutable t1 = 0G
                    for kk in l..m - 2 do
                        k <- m - 2 - kk + l
                        t1 <- stemp.[k]

                        let (res0,res1,res2,res3) = drotg t1 f
                        t1 <- res0
                        f <- res1
                        cs <- res2
                        sn <- res3

                        stemp.[k] <- t1
                        if k <> l then
                            f <- -sn * e.[k - 1]
                            e.[k - 1] <- cs * e.[k - 1]

                        // Rotate
                        for i in 0..columnsA-1 do
                            let z = (cs * v.[(k * columnsA) + i]) + (sn * v.[((m - 1) * columnsA) + i])
                            v.[((m - 1) * columnsA) + i] <- (cs * v.[((m - 1) * columnsA) + i]) - (sn * v.[(k * columnsA) + i])
                            v.[(k * columnsA) + i] <- z
                 //Split at negligible s[l].
                | 2 -> 
                    f <- e.[l - 1]
                    e.[l - 1] <- 0G
                    for k in l..m-1 do
                        t1 <- stemp.[k]
                        let (res0,res1,res2,res3) = drotg t1 f
                        t1 <- res0
                        f <- res1
                        cs <- res2
                        sn <- res3 
                        stemp.[k] <- t1
                        f <- -sn * e.[k]
                        e.[k] <- cs * e.[k]
                        // Rotate
                        for i in 0..rowsA-1 do
                            let z = (cs * u.[(k * rowsA) + i]) + (sn * u.[((l - 1) * rowsA) + i])
                            u.[((l - 1) * rowsA) + i] <- (cs * u.[((l - 1) * rowsA) + i]) - (sn * u.[(k * rowsA) + i])
                            u.[(k * rowsA) + i] <- z
                    // Perform one qr step.
                | 3 -> 
                    // calculate the shift.
                    let mutable scale = 0G
                    scale <- max scale (abs(stemp.[m - 1]))
                    scale <- max scale (abs(stemp.[m - 2]))
                    scale <- max scale (abs(e.[m - 2]))
                    scale <- max scale (abs(stemp.[l]))
                    scale <- max scale (abs(e.[l]))
                    let sm = stemp.[m - 1] / scale
                    let smm1 = stemp.[m - 2] / scale
                    let emm1 = e.[m - 2] / scale
                    let sl = stemp.[l] / scale
                    let el = e.[l] / scale
                    let b = (((smm1 + sm) * (smm1 - sm)) + (emm1 * emm1)) / (1G + 1G)
                    let c = (sm * emm1) * (sm * emm1)
                    let mutable shift = 0G
                    if (b <> 0G || c <> 0G) then
                        shift <- sqrt((b * b) + c)
                        if (b < 0G) then
                            shift <- -shift;

                        shift <- c / (b + shift)

                    f <- ((sl + sm) * (sl - sm)) + shift
                    let mutable g = sl * el

                    // Chase zeros
                    for k in l..m - 2 do
                        let (res0,res1,res2,res3) = drotg f g
                        f <- res0
                        g <- res1
                        cs <- res2
                        sn <- res3
                        if (k <> l) then
                            e.[k - 1] <- f

                        f <- (cs * stemp.[k]) + (sn * e.[k])
                        e.[k] <- (cs * e.[k]) - (sn * stemp.[k])
                        g <- sn * stemp.[k + 1]
                        stemp.[k + 1] <- cs * stemp.[k + 1]
                        for i in 0..columnsA-1 do
                            let z = (cs * v.[(k * columnsA) + i]) + (sn * v.[((k + 1) * columnsA) + i])
                            v.[((k + 1) * columnsA) + i] <- (cs * v.[((k + 1) * columnsA) + i]) - (sn * v.[(k * columnsA) + i])
                            v.[(k * columnsA) + i] <- z

                        let (res0,res1,res2,res3) = drotg f g
                        f <- res0
                        g <- res1
                        cs <- res2
                        sn <- res3
                        stemp.[k] <- f
                        f <- (cs * e.[k]) + (sn * stemp.[k + 1])
                        stemp.[k + 1] <- -(sn * e.[k]) + (cs * stemp.[k + 1])
                        g <- sn * e.[k + 1]
                        e.[k + 1] <- cs * e.[k + 1]
                        if (k < rowsA) then
                            for i in 0..rowsA-1 do
                                let z = (cs * u.[(k * rowsA) + i]) + (sn * u.[((k + 1) * rowsA) + i])
                                u.[((k + 1) * rowsA) + i] <- (cs * u.[((k + 1) * rowsA) + i]) - (sn * u.[(k * rowsA) + i])
                                u.[(k * rowsA) + i] <- z
                    e.[m - 2] <- f
                    iter <- iter + 1
                    // Convergence
                | 4 -> 
                    // Make the singular value  positive
                    if (stemp.[l] < 0G) then
                        stemp.[l] <- -stemp.[l]
                        // A part of column "l" of matrix VT from row 0 to end multiply by -1
                        for i in 0..columnsA-1 do
                            v.[(l * columnsA) + i] <- v.[(l * columnsA) + i] * -1G

                    // Order the singular value.
                    let tref = ref t
                    let rec ff l endL =
                        if l = endL then ()
                    //while (l <> mn - 1) do
                        else
                            if stemp.[l] >= stemp.[l + 1] then ()
                            else
                                tref := stemp.[l]
                                stemp.[l] <- stemp.[l + 1]
                                stemp.[l + 1] <- !tref
                                if (l < columnsA) then
                                    // Swap columns l, l + 1
                                    for i in 0..columnsA-1 do
                                        let z = v.[(l * columnsA) + i]
                                        v.[(l * columnsA) + i] <- v.[((l + 1) * columnsA) + i]
                                        v.[((l + 1) * columnsA) + i] <- z

                                if (l < rowsA) then
                                    // Swap columns l, l + 1
                                    for i in 0..rowsA-1 do
                                        let z = u.[(l * rowsA) + i];
                                        u.[(l * rowsA) + i] <- u.[((l + 1) * rowsA) + i]
                                        u.[((l + 1) * rowsA) + i] <- z
                                ff (l + 1) endL
                    ff l (mn - 1)
                    iter <- 0
                    m <- m - 1
                | _ -> raise (new ArgumentException("Unknown case in svd"))

        // Finally transpose "v" to get "vt" matrix 
        for i in 0..columnsA-1 do
            for j in 0..columnsA-1 do
                vt.[(j * columnsA) + i] <- v.[(i * columnsA) + j]

        for i in 0..(min rowsA columnsA)-1 do
            s.[i] <- stemp.[i]

        let thinu = Array.zeroCreate<'T> (n*k)
        for i in 0..(n*k)-1 do
            thinu.[i] <- u.[i]
        (Managed thinu, [|n;k|], Managed s, [|s.Length;1|], Managed vt, [|k;k|])

    let inline svdSolve (a : matrixData<'T>)  (n : int) (k : int) (b : matrixData<'T>) (m : int) (tol : 'T) : matrixData<'T>*int =
        let rowsA = n
        let columnsA = k
        let columnsB = m
        let work = Array.zeroCreate<'T> rowsA
        let (u,_,s,_,vt,_) = svdDecomp tol a n k
        let x = Array.zeroCreate<'T> (k*m)
        let tmp = Array.zeroCreate<'T> columnsA

        match u,s,vt,b with
          | Managed u,Managed s,Managed vt,Managed b ->
                let rank = s |> Array.filter (fun x -> not (nearlyEqual x 0G tol)) |> Array.length
                for k in 0..columnsB-1 do
                    for j in 0..columnsA-1 do
                        let mutable value = 0G
                        if (j < rank) then
                            for  i in 0..rowsA-1 do
                                value <- value + u.[(j * rowsA) + i] * b.[(k * rowsA) + i]
                            value <- value / s.[j]
                        tmp.[j] <- value

                    for j in 0..columnsA-1 do
                        let mutable value = 0G
                        for i in 0..columnsA-1 do
                            value <- value + vt.[(j * columnsA) + i] * tmp.[i]

                        x.[(k * columnsA) + j] <- value
                Managed x, rank

    let eigenDecomp (matrixData : matrixData<'T>)  (n : int) : matrixData<'T>*matrixData<'T> = raise (new NotImplementedException())

    let eigenValues (matrixData : matrixData<'T>)  (n : int) : matrixData<'T> = raise (new NotImplementedException())

