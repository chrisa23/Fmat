namespace Fmat.Numerics
open Fmat.Numerics.Conversion

module GenericBasicStat =
  
    let inline rand size = Matrix<'T,'S>.unifRnd(LanguagePrimitives.GenericZero<'T>, LanguagePrimitives.GenericOne<'T>, size)

    let unifRnd(a, b, size) = Matrix<'T,'S>.unifRnd(a, b, size)

    let normalRnd(mu, sigma, size) = Matrix<'T,'S>.normalRnd(mu, sigma, size)

    let lognormRnd(mu, sigma, a, scale, size) = Matrix<'T,'S>.lognormRnd(mu, sigma, a, scale, size)

    let mvNormRnd(mu, cov, n) = Matrix<'T,'S>.mvNormRnd(mu, cov, n)

    let bernRnd(p, size) = Matrix<'T,'S>.bernRnd(p, size)

    let binomRnd(n, p, size) = Matrix<'T,'S>.binomRnd(n, p, size)

    let poissRnd(lambda, size) = Matrix<'T,'S>.poissRnd(lambda, size)

    let min(matrix, dim) = Matrix<'T, 'S>.min(matrix, dim)

    let max(matrix, dim) = Matrix<'T, 'S>.max(matrix, dim)

    let sum(matrix, dim) = Matrix<'T, 'S>.sum(matrix, dim)

    let prod(matrix, dim) = Matrix<'T, 'S>.prod(matrix, dim)

    let cumsum(matrix, dim) = Matrix<'T, 'S>.cumsum(matrix, dim)

    let cumprod(matrix, dim) = Matrix<'T, 'S>.cumprod(matrix, dim)

    let mean(matrix, dim) = Matrix<'T, 'S>.mean(matrix, dim)

    let var(matrix, dim) = Matrix<'T, 'S>.var(matrix, dim)

    let skewness(matrix, dim) = Matrix<'T, 'S>.skewness(matrix, dim)

    let kurtosis(matrix, dim) = Matrix<'T, 'S>.kurtosis(matrix, dim)

    let quantile(matrix, quantiles, dim) = Matrix<'T, 'S>.quantile(matrix, quantiles, dim)

    let corr(matrix) = Matrix<'T, 'S>.corr(matrix)

    let cov(matrix) = Matrix<'T, 'S>.cov(matrix)

