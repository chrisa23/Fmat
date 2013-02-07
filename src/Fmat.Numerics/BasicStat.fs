namespace Fmat.Numerics
open Fmat.Numerics.Conversion

module BasicStat =
  
    let rand size = Matrix.unifRnd(0.0, 1.0, size)

    let unifRnd(a, b, size) = Matrix.unifRnd(a, b, size)

    let normalRnd(mu, sigma, size) = Matrix.normalRnd(mu, sigma, size)

    let lognormRnd(mu, sigma, a, scale, size) = Matrix.lognormRnd(mu, sigma, a, scale, size)

    let mvNormRnd(mu, cov, n) = Matrix.mvNormRnd(mu, cov, n)

    let bernRnd(p, size) = Matrix.bernRnd(p, size)

    let binomRnd(n, p, size) = Matrix.binomRnd(n, p, size)

    let poissRnd(lambda, size) = Matrix.poissRnd(lambda, size)

    let min(matrix, dim) = Matrix.min(matrix, dim)

    let max(matrix, dim) = Matrix.max(matrix, dim)

    let sum(matrix, dim) = Matrix.sum(matrix, dim)

    let prod(matrix, dim) = Matrix.prod(matrix, dim)

    let cumsum(matrix, dim) = Matrix.cumsum(matrix, dim)

    let cumprod(matrix, dim) = Matrix.cumprod(matrix, dim)

    let mean(matrix, dim) = Matrix.mean(matrix, dim)

    let var(matrix, dim) = Matrix.var(matrix, dim)

    let skewness(matrix, dim) = Matrix.skewness(matrix, dim)

    let kurtosis(matrix, dim) = Matrix.kurtosis(matrix, dim)

    let quantile(matrix, quantiles, dim) = Matrix.quantile(matrix, quantiles, dim)

    let corr(matrix) = Matrix.corr(matrix)

    let cov(matrix) = Matrix.cov(matrix)



