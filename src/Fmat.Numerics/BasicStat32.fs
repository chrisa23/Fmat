namespace Fmat.Numerics
open Fmat.Numerics.Conversion

module BasicStat32 =
  
    let rand size = Matrix32.unifRnd(0.0f, 1.0f, size)

    let unifRnd(a, b, size) = Matrix32.unifRnd(a, b, size)

    let normalRnd(mu, sigma, size) = Matrix32.normalRnd(mu, sigma, size)

    let lognormRnd(mu, sigma, a, scale, size) = Matrix32.lognormRnd(mu, sigma, a, scale, size)

    let mvNormRnd(mu, cov, n) = Matrix32.mvNormRnd(mu, cov, n)

    let bernRnd(p, size) = Matrix32.bernRnd(p, size)

    let binomRnd(n, p, size) = Matrix32.binomRnd(n, p, size)

    let poissRnd(lambda, size) = Matrix32.poissRnd(lambda, size)

    let min(matrix, dim) = Matrix32.min(matrix, dim)

    let max(matrix, dim) = Matrix32.max(matrix, dim)

    let sum(matrix, dim) = Matrix32.sum(matrix, dim)

    let prod(matrix, dim) = Matrix32.prod(matrix, dim)

    let cumsum(matrix, dim) = Matrix32.cumsum(matrix, dim)

    let cumprod(matrix, dim) = Matrix32.cumprod(matrix, dim)

    let mean(matrix, dim) = Matrix32.mean(matrix, dim)

    let var(matrix, dim) = Matrix32.var(matrix, dim)

    let skewness(matrix, dim) = Matrix32.skewness(matrix, dim)

    let kurtosis(matrix, dim) = Matrix32.kurtosis(matrix, dim)

    let quantile(matrix, quantiles, dim) = Matrix32.quantile(matrix, quantiles, dim)

    let corr(matrix) = Matrix32.corr(matrix)

    let cov(matrix) = Matrix32.cov(matrix)

