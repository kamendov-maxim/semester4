module PhoneBook

type Entry =
    struct
        val Name: string
        val Number: string
        new(name: string, number: string) = { Name = name; Number = number }
    end

type PhoneBook = Entry list

let addEntry (book: PhoneBook) (entry: Entry) = entry :: book

let getName (book: PhoneBook) name =
    book |> List.filter (fun x -> x.Name = name) |> List.map _.Name

let getNumber (book: PhoneBook) number =
    book |> List.filter (fun x -> x.Number = number) |> List.map _.Number

let Empty: PhoneBook = []

let print (writer: System.IO.StreamWriter) (book: PhoneBook) =
    let rec loop (writer: System.IO.StreamWriter) (book: PhoneBook) =
        if not book.IsEmpty then
            writer.WriteLine $"Name: {book.Head.Name.ToString()} Number: {book.Head.Number.ToString()}"
            loop writer book.Tail

    loop writer book
