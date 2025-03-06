module PowerList

let PowerList n m =
    let rec f acc m =
        match m with
        | 0 -> acc
        | (m: int) -> f (List.head acc / 2 :: acc) (m - 1)
    if m < n then
        []
    else
        f [ 2 <<< m - 1] (m - n)

for i in PowerList 2 3 do
    printf "%d " i
