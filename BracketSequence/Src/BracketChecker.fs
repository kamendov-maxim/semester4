module BracketChecker

let checker str =
    let pairs = [ [ '('; ')' ]; [ '{'; '}' ]; [ '['; ']' ] ]

    let checkIfOpeningOrClose bracket isOpening =
        match pairs |> Seq.tryFind (List.contains bracket) with
        | None -> invalidArg "str" "sequence contains unsupported characters"
        | Some pair -> (pair.Head = bracket) = isOpening

    let getPair bracket =
        match Seq.tryFind (List.contains bracket) pairs with
        | None -> invalidArg "str" "sequence contains unsupported characters"
        | Some [ x; y ] when x = bracket -> y
        | Some [ x; y ] when y = bracket -> x
        | _ -> invalidArg "sdaf" "dsafga"

    let rec loop stack str = 
        match stack, Seq.tryHead str with
        | [], None -> true
        | [], Some y when checkIfOpeningOrClose y false -> false
        | _, Some y when checkIfOpeningOrClose y true -> loop (y::stack) (Seq.tail str)
        | x, Some y when checkIfOpeningOrClose y false ->
            if List.head x = getPair y then
                loop (List.tail x) (Seq.tail str)
            else
                false

    loop [] str
