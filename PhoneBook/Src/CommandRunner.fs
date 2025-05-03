module CommandRunner

open System

type Command =
    | Exit
    | AddEntry of string
    | GetName of string
    | GetNumber of string
    | Save of string
    | ReadFromFile of string
    | Print
    | Help

let parse (input: string) =
    match
        [ "add ", AddEntry
          "exit", (fun _ -> Exit)
          "print", (fun _ -> Print)
          "get name ", GetName
          "get number ", GetNumber
          "save ", Save
          "read ", ReadFromFile
          "help", (fun _ -> Help) ]
        |> Seq.tryFind (fun (str, _) -> input.StartsWith str)
    with
    | None -> None
    | Some(str, command) -> Some(command (input |> Seq.skip str.Length |> Seq.skipWhile ((=) ' ') |> String.Concat))

let helpMessage =
    "List of avaliable commands:\n"
    + "exit - exit the phone book\n"
    + "add <phone> <name> - add entry to the book\n"
    + "get name <phone number> - get name of the phone number owner\n"
    + "get number <name> - get number of the person\n"
    + "save <path to file> - save the phone book to file\n"
    + "read <path to file> - read the phone book from file\n"
    + "print - print all entries to stdout\n"

let runCommand (book: PhoneBook.PhoneBook) (command: Command) =
    match command with
    | AddEntry text ->
        let numberPart = text |> Seq.takeWhile Char.IsDigit |> String.Concat

        let namePart =
            text |> Seq.skipWhile Char.IsDigit |> Seq.skipWhile ((=) ' ') |> String.Concat

        let newbook = PhoneBook.addEntry book (PhoneBook.Entry(namePart, numberPart))
        Console.WriteLine "Entry was added"
        newbook, "Entry was added"
    | Exit ->
        Console.WriteLine "Leaving the phonebook..."
        exit 0
    | GetNumber text ->
        match book |> Seq.tryFind (fun x -> text = x.Name) with
        | None -> book, "No such entry in the book"
        | Some(Value = x: PhoneBook.Entry) -> book, $"phone number of {x.Name} is {x.Number}"
    | GetName text ->
        match book |> Seq.tryFind (fun x -> text = x.Number) with
        | None -> book, "No such entry in the book"
        | Some(Value = x: PhoneBook.Entry) -> book, $"{x.Number} is a number of {x.Name}"
    | Print ->
        PhoneBook.print (new IO.StreamWriter(Console.OpenStandardOutput())) book
        book, ""
    | Save text ->
        use writer = new IO.StreamWriter(text.ToString())
        PhoneBook.print writer book
        book, "Phonebook was saved"
    | ReadFromFile text ->
        let rec fileReadLoop (reader: IO.StreamReader) (book: PhoneBook.PhoneBook) =
            let line = reader.ReadLine()

            if not (line = "") then
                fileReadLoop
                    reader
                    (PhoneBook.addEntry
                        book
                        (PhoneBook.Entry(
                            (line |> Seq.skipWhile Char.IsDigit |> Seq.skipWhile ((=) ' ')).ToString(),
                            (line |> Seq.takeWhile Char.IsDigit).ToString()
                        )))

        fileReadLoop (IO.File.OpenText(text.ToString())) book
        book, "Phonebook was read from file"
    | Help -> book, helpMessage
