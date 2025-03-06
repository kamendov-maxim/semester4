module ReverseList

let ReverseList list =
    let rec f acc lst =
        match lst with
        | [] -> acc
        | head :: tail -> f (head :: acc) tail
    f [] list
