Fmat Numerical Library
======================

Copyright (c) 2007-2013 StatFactory Ltd

Fmat is an open source and 100% F# numerical library. It contains types and functions for construction and manipulation of N-dimensional generic dense matrices. 5 concrete implementations are predefined for: single/double precision floating numbers, int32, bool and string.

Fmat contains 4 matrix factorizations and linear solvers based on them. It can also generate random numbers from uniform, normal, lognormal, multivariate normal, Bernoulli, binomial and Poisson distributions. The default random generator is based on .NET Random class, but user defined generators can easily be plugged in. 

Multidimensional basic stat routines are also provided, e.g. mean(X,dim=2). All standard math functions, e.g. sqrt, will automatically work as elementwise functions when applied to matrices. 6 special functions are provided: erf, erfc, erfinv, erfcinv, normcdf and norminv.

Library documentation is provided in the form of a .chm file. 

Fmat is covered under the terms of the MIT/X11. Linear Algebra and special functions have been adapted from Math.NET under the same MIT license.
