module Program

open System
open CommandRunner


let rec loop (book: PhoneBook.PhoneBook) =
    match Console.ReadLine() |> parse with
    | None ->
        Console.WriteLine "Unsupported command"
        loop book
    | Some command -> 
        let newbook, message = runCommand book command
        Console.WriteLine message
        loop newbook

[<EntryPoint>]
let main _ =
    Console.Write helpMessage
    loop []
    0
