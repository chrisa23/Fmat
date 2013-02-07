namespace Fmat.Numerics
///<summary>This module contains functions for random number generation and calculating basic stats in double precision
///</summary>
  module BasicStat = begin
    ///<summary>Generates matrix with continuous uniform random numbers in [0, 1] 
    ///</summary>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<returns>Matrix with random data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4]
    ///</code>
    ///</example>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified.</exception>
    val rand :
      size:seq<int> -> Matrix
    ///<summary>Generates matrix with continuous uniform random numbers in [a, b] 
    ///</summary>
    ///<param name="a">Lower endpoint</param>
    ///<param name="b">Upper endpoint</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<returns>Matrix with random data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = unifRnd(0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or b &lt;= a.</exception>
    val unifRnd :
      a:float * b:float * size:seq<int> -> Matrix
    ///<summary>Generates matrix with normal random numbers
    ///</summary>
    ///<param name="mu">Mean</param>
    ///<param name="sigma">Standard deviation</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = normRnd(0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or sigma &lt;= 0.</exception>
    val normalRnd :
      mu:float * sigma:float * size:seq<int> -> Matrix
    ///<summary>Generates matrix with lognormal random numbers
    ///</summary>
    ///<param name="mu">Mean</param>
    ///<param name="sigma">Standard deviation</param>
    ///<param name="a">Displacement</param>
    ///<param name="scale">Scale</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = lognormRnd(0.0, 1.0, 0.0, 1.0, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or sigma &lt;= 0.</exception>
    val lognormRnd :
      mu:float * sigma:float * a:float * scale:float * size:seq<int> -> Matrix
    ///<summary>Generates matrix with multivariate normal distribution
    ///</summary>
    ///<param name="mean">Vector kx1 or 1xk of means</param>
    ///<param name="cov">Covariance matrix kxk</param>
    ///<param name="n">Number of k-dimensional vectors to generate. Vectors are returned in rows of result matrix</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let mn = !![2.345;2.345]
    ///let cv = !![ [1.;1.]
    ///             [1.;2.] ]
    ///let m = mvNormRnd(mn, cv, 10) // returns matrix 10x2 
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or cov matrix not pos definite or incompatible size.</exception>
    val mvNormRnd :
      mean:Matrix * cov:Matrix * n:int -> Matrix
    ///<summary>Generates matrix with Bernoulli distributed random numbers
    ///</summary>
    ///<param name="p">Probability of success</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = bernRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or p not between 0 and 1.</exception>
    val bernRnd :
      p:float * size:seq<int> -> Matrix
    ///<summary>Generates matrix with binomial distributed random numbers
    ///</summary>
    ///<param name="n">Number of trials</param>
    ///<param name="p">Probability of success</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = binomRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or p not between 0 and 1 or n &lt;0.</exception>
    val binomRnd :
      n:int * p:float * size:seq<int> -> Matrix
    ///<summary>Generates matrix with Poisson distributed random numbers
    ///</summary>
    ///<param name="lambda">Lambda</param>
    ///<param name="size">Dimensions of the matrix to be generated</param>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = poissRnd(0.5, [2;3;4])
    ///</code>
    ///</example>
    ///<returns>Matrix with random data</returns>
    ///<remarks>The sequence is determined by RandStream settings
    ///</remarks>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dims are specified or lambda negative.</exception>
    val poissRnd :
      lambda:float * size:seq<int> -> Matrix
    ///<summary>Calculates minimum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which minimum will be calculated</param>
    ///<returns>Matrix with reduced data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = min(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val min :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates minimum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which minimum will be calculated</param>
    ///<returns>Matrix with reduced data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = max(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val max :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates sum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which sum will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = sum(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val sum :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates product of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which product will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = prod(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val prod :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates cumulative sum of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which cumulative sum will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = cumsum(x, 1) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val cumsum :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates cumulative product of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which cumulative product will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = cumprod(x, 1) // returns Matrix with size [2;3;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    ///<exception cref="T:System.OutOfMemoryException">Thrown when not enough memory available.</exception>
    ///<exception cref="T:System.ObjectDisposedException">Thrown when matrix has been disposed with Dispose().</exception>
    val cumprod :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates mean of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which mean will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = mean(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val mean :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates variance of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which variance will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = var(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val var :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates skewness of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which skewness will be calculated</param>
    ///<returns>Matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = skewness(x, 1) // returns Matrix with size [2;1;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val skewness :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates kurtosis of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="dim">Dimension along which kurtosis will be calculated</param>
    ///<returns>matrix with summarized data</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;3;4] 
    ///let y = kurtosis(x, 1) // returns Matrix with size [2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified.</exception>
    val kurtosis :
      matrix:Matrix * dim:int -> Matrix
    ///<summary>Calculates quantiles of matrix elements along given dimension
    ///</summary>
    ///<param name="matrix">Matrix argument</param>
    ///<param name="quantiles">Quantiles vector:  Matrix 1xn or nx1</param>
    ///<param name="dim">Dimension along which quantiles will be calculated</param>
    ///<returns>matrix with quantiles</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [2;30;4] 
    ///let q = Matrix([0.05;0.95])
    ///let y = quantile(x, q, 1) // returns Matrix with size [2;2;4]
    ///</code>
    ///</example>
    ///<exception cref="T:System.ArgumentException">Thrown when invalid dimension specified or quantile q not in 0&lt;=q&lt;=1.</exception>
    ///<exception cref="T:System.RankException">Thrown when quantiles not a vector 1xn or nx1.</exception>
    val quantile :
      matrix:Matrix * quantiles:Matrix * dim:int -> Matrix
    ///<summary>Calculates correlation between 2D matrix columns
    ///</summary>
    ///<param name="matrix">2D Matrix argument [nxp], with n observations and p variables</param>
    ///<returns>Correlation matrix pxp</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [10;3] //10 observations, 3 variables
    ///let y = corr(x) // returns Matrix with size [3;3]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    val corr :
      matrix:Matrix -> Matrix
    ///<summary>Calculates covariance between 2D matrix columns
    ///</summary>
    ///<param name="matrix">2D Matrix argument [nxp], with n observations and p variables</param>
    ///<returns>Covariance matrix</returns>
    ///<example>Code example:
    ///<code lang="F#">
    ///open Fmat.Numerics
    ///open Fmat.Numerics.BasicStat
    ///let x = rand [10;3] //10 observations, 3 variables
    ///let y = cov(x) // returns Matrix with size [3;3]
    ///</code>
    ///</example>
    ///<exception cref="T:System.RankException">Thrown when matrix not 2 dimensional.</exception>
    val cov :
      matrix:Matrix -> Matrix
  end