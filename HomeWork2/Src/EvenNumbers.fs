module EvenNumbers

/// Implementation of counter of even numbers in a list that uses List.fold function
let counter1 list =
    List.fold (fun x y -> if y % 2 = 0 then x + 1 else x) 0 list

/// Implementation of counter of even numbers in a list that uses List.map function
let counter2 list =
    List.map (fun x -> x % 2 = 0) list |> List.filter id |> List.length

/// Implementation of counter of even numbers in a list that uses List.filter function
let counter3 list =
    List.filter (fun x -> x % 2 = 0) list |> List.length
