module FindNumber

let FindNumber list n =
    let rec loop list index =
        match list with
        | [] -> None
        | head :: _ when head = n -> Some index
        | _ :: tail -> loop tail (index + 1)
    loop list 0
