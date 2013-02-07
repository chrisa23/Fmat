namespace Fmat.Numerics
open Fmat.Numerics.Conversion

module MatrixFunctions =

    let I (n, m) = Matrix.Identity(n,m)

    let zeros size = Matrix.zeros(size)

    let ones size = Matrix.ones(size)

    let diag(vector, k) = Matrix.diag(vector, k)

    let triL(matrix, k) = Matrix.triL(matrix,k)

    let triU(matrix, k) = Matrix.triU(matrix,k)

    let concat(matrices, dimension) = Matrix.concat(matrices, dimension)

    let horzConcat(matrices) = Matrix.horzConcat(matrices)

    let vertConcat(matrices) = Matrix.vertConcat(matrices)

    let repmat(matrix, replicator) = Matrix.repmat(matrix,replicator)

    let reshape(matrix, size) = Matrix.reshape(matrix,size)

    let transpose(matrix) = Matrix.transpose(matrix)

    let inline minXY(x, y) =
        let a : Matrix = !!x
        let b : Matrix = !!y
        Matrix.minXY(a, b)

    let inline maxXY(x, y) =
        let a : Matrix = !!x
        let b : Matrix = !!y
        Matrix.maxXY(a, b)

    let applyFun(x, f) = Matrix.applyFun(x, f)

    let applyFun2Arg(x, y, f) = Matrix.applyFun2Arg(x, y, f)

    let applyFun3Arg(x, y, z, f) = Matrix.applyFun3Arg(x, y, z, f)

    let inline erf (x : 'T) : 'T = !!Matrix.Erf(!!x)

    let inline erfc (x : 'T) : 'T = !!Matrix.Erfc(!!x)

    let inline erfinv (x : 'T) : 'T = !!Matrix.Erfinv(!!x)

    let inline erfcinv (x : 'T) : 'T = !!Matrix.Erfcinv(!!x)

    let inline normcdf (x : 'T) : 'T = !!Matrix.Normcdf(!!x)

    let inline norminv (x : 'T) : 'T = !!Matrix.Norminv(!!x)

//    let erf x = Matrix.Erf(x)
//
//    let erfc x = Matrix.Erfc(x)
//
//    let erfinv x = Matrix.Erfinv(x)
//
//    let erfcinv x = Matrix.Erfcinv(x)
//
//    let normcdf x = Matrix.Normcdf(x)
//
//    let norminv x= Matrix.Norminv(x)


        