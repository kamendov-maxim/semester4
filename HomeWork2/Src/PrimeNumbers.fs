module PrimeNumbers

/// Generate infinite prime number sequence
let PrimeNumbers =
    let rec primes generateNextFunc =
        seq {
            yield 2
            yield! generateNextFunc 3
        }
        |> Seq.cache

    let rec generateNext current =
        seq {
            if
                primes generateNext
                |> Seq.takeWhile (fun x -> x <= int (sqrt (float current)))
                |> Seq.forall (fun x -> not (current % x = 0))
            then
                yield current

            yield! generateNext (current + 2)
        }

    primes generateNext
