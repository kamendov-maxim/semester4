module Fibonacci

let Fibonacci n =
    if n <= 0 then
        invalidArg "n" "n cannot be negative or zero"

    let rec f next current steps =
        match steps with
        | 0 -> current
        | steps -> f (next + current) next (steps - 1)

    f 1 0 n
