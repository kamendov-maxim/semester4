module Sequences

/// Sequence that generates alternating numbers 1, -1, 1, -1, ...
let alternatingSequence =
    seq {
        while true do
            yield! [ 1; -1 ]
    }

/// Sequence based on previous sequence that generates numbers like 1, -2, 3, -4, 5, -6, ...
let alternatingSequence2 =
    let rec loop n =
        seq {
            yield n
            yield! loop (n + 1)
        }

    (alternatingSequence, loop 1) ||> Seq.map2 (fun x y -> x * y)
