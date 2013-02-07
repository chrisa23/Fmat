namespace Fmat.Numerics

module GenericMatrixFunctions =

    let I (n, m) = Matrix<'T, 'S>.Identity(n,m)

    let zeros size = Matrix<'T, 'S>.zeros(size)

    let ones size = Matrix<'T, 'S>.ones(size)

    let diag(vector, k) = Matrix<'T,'S>.diag(vector, k)

    let triL(matrix, k) = Matrix<'T,'S>.triL(matrix,k)

    let triU(matrix, k) = Matrix<'T,'S>.triU(matrix,k)

    let concat(matrices, dimension) = Matrix<'T,'S>.concat(matrices, dimension)

    let horzConcat(matrices) = Matrix<'T,'S>.horzConcat(matrices)

    let vertConcat(matrices) = Matrix<'T,'S>.vertConcat(matrices)

    let repmat(matrix, replicator) = Matrix<'T,'S>.repmat(matrix,replicator)

    let reshape(matrix, size) = Matrix<'T,'S>.reshape(matrix,size)

    let transpose(matrix) = Matrix<'T,'S>.transpose(matrix)

    let inline minXY(x, y) =
        let a : Matrix<'T,'S> = !!x
        let b : Matrix<'T,'S> = !!y
        Matrix<'T,'S>.minXY(a, b)

    let inline maxXY(x, y) =
        let a : Matrix<'T,'S> = !!x
        let b : Matrix<'T,'S> = !!y
        Matrix<'T,'S>.maxXY(a, b)

    let applyFun(x, f) = Matrix<'T,'S>.applyFun(x, f)

    let applyFun2Arg(x, y, f) = Matrix<'T,'S>.applyFun2Arg(x, y, f)

    let applyFun3Arg(x, y, z, f) = Matrix<'T,'S>.applyFun3Arg(x, y, z, f)

    let erf x = Matrix<'T,'S>.Erf(x)

    let erfc x = Matrix<'T,'S>.Erfc(x)

    let erfinv x = Matrix<'T,'S>.Erfinv(x)

    let erfcinv x = Matrix<'T,'S>.Erfcinv(x)

    let normcdf x = Matrix<'T,'S>.Normcdf(x)

    let norminv x = Matrix<'T,'S>.Norminv(x)
