namespace Fmat.Numerics
open Fmat.Numerics.Conversion

module Matrix32Functions =

    let I (n, m) = Matrix32.Identity(n,m)

    let zeros size = Matrix32.zeros(size)

    let ones size = Matrix32.ones(size)

    let diag(vector, k) = Matrix32.diag(vector, k)

    let triL(matrix, k) = Matrix32.triL(matrix,k)

    let triU(matrix, k) = Matrix32.triU(matrix,k)

    let concat(matrices, dimension) = Matrix32.concat(matrices, dimension)

    let horzConcat(matrices) = Matrix32.horzConcat(matrices)

    let vertConcat(matrices) = Matrix32.vertConcat(matrices)

    let repmat(matrix, replicator) = Matrix32.repmat(matrix,replicator)

    let reshape(matrix, size) = Matrix32.reshape(matrix,size)

    let transpose(matrix) = Matrix32.transpose(matrix)

    let inline minXY(x, y) =
        let a : Matrix32 = !!x
        let b : Matrix32 = !!y
        Matrix32.minXY(a, b)

    let inline maxXY(x, y) =
        let a : Matrix32 = !!x
        let b : Matrix32 = !!y
        Matrix32.maxXY(a, b)

    let applyFun(x, f) = Matrix32.applyFun(x, f)

    let applyFun2Arg(x, y, f) = Matrix32.applyFun2Arg(x, y, f)

    let applyFun3Arg(x, y, z, f) = Matrix32.applyFun3Arg(x, y, z, f)

    let inline erf (x : 'T) : 'T = !!Matrix32.Erf(!!x)

    let inline erfc (x : 'T) : 'T = !!Matrix32.Erfc(!!x)

    let inline erfinv (x : 'T) : 'T = !!Matrix32.Erfinv(!!x)

    let inline erfcinv (x : 'T) : 'T = !!Matrix32.Erfcinv(!!x)

    let inline normcdf (x : 'T) : 'T = !!Matrix32.Normcdf(!!x)

    let inline norminv (x : 'T) : 'T = !!Matrix32.Norminv(!!x)

//    let erf x = Matrix32.Erf(x)
//
//    let erfc x = Matrix32.Erfc(x)
//
//    let erfinv x = Matrix32.Erfinv(x)
//
//    let erfcinv x = Matrix32.Erfcinv(x)
//
//    let normcdf x = Matrix32.Normcdf(x)
//
//    let norminv x= Matrix32.Norminv(x)

