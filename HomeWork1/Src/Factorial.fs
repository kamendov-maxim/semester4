module Factorial

let Factorial n = 
    if n <= 0 then invalidArg "n" "n should be greater than 0"
    let rec f answer steps = 
        match steps with
        | 0 -> answer
        | steps -> f (answer * steps) (steps - 1)
    f 1 n
    

