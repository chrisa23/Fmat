namespace Fmat.Numerics

module SymmetricOperators =

    let inline (+) (x :'T)  (y:'T) : 'T = x + y

    let inline (*) (x :'T)  (y:'T) : 'T = x * y

    let inline (/) (x :'T)  (y:'T) : 'T = x / y

    let inline (-) (x :'T)  (y:'T) : 'T = x - y

    let inline (~-) (x :'T) : 'T = -x

    let inline ( ** ) (x :'T)  (y:'T) : 'T = x ** y
