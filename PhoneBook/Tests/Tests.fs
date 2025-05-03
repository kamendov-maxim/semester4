module Tests

open PhoneBook
open CommandRunner
open System.IO

open NUnit.Framework

let RunCommandTestCases =
    [ AddEntry "89581130987 Pavel", "Entry was added", [], [ Entry("Pavel", "89581130987") ]

      AddEntry "89110310239 Alexey",
      "Entry was added",
      [ Entry("Pavel", "895811309871") ],
      [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ]

      GetNumber "Alexey",
      "phone number of Alexey is 89110310239",
      [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ],
      [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ]

      GetName "895811309871",
      "895811309871 is a number of Pavel",
      [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ],
      [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ]

      ]
    |> List.map (fun (a, b, c, d) -> TestCaseData(a, b, c, d))

[<TestCaseSource("RunCommandTestCases")>]
let TestCommandRun (command: Command) (expectedMessage: string) (before: PhoneBook) (after: PhoneBook) =
    let newbook, message = runCommand before command
    Assert.That(newbook, Is.EqualTo after)
    Assert.That(message, Is.EqualTo expectedMessage)

[<Test>]
let TestSavingEmptyPhoneBook () =
    let tempFilePath = Path.GetTempFileName()

    try
        let book = Empty
        let command = Save tempFilePath

        let _, message = runCommand book command

        Assert.That(message, Is.EqualTo "Phonebook was saved")
        let fileContent = File.ReadAllText tempFilePath
        Assert.That(fileContent, Is.Empty)
    finally
        File.Delete tempFilePath

[<Test>]
let TestSavingMultipleEntries () =
    let tempFilePath = Path.GetTempFileName()
    try
        let book = [ Entry("Alexey", "89110310239"); Entry("Pavel", "895811309871") ]
        let command = Save tempFilePath

        let _, message = runCommand book command

        Assert.That(message, Is.EqualTo "Phonebook was saved")
        let fileContent = File.ReadAllText(tempFilePath).Replace("\r\n", "\n")

        let expectedContent =
            "Name: Alexey Number: 89110310239\nName: Pavel Number: 895811309871\n"

        Assert.That(fileContent, Is.EqualTo expectedContent)
    finally
        File.Delete tempFilePath

let CommandParseTestCases =
    [ "add 89581130987 Pavel", AddEntry "89581130987 Pavel"
      "add 89110310239 Alexey", AddEntry "89110310239 Alexey"
      "get number Alexey", GetNumber "Alexey"
      "get name Pavel", GetName "Pavel"
      "save ../phonebooks/phonebook1", Save "../phonebooks/phonebook1"
      "read ../phonebooks/phonebook2", ReadFromFile "../phonebooks/phonebook2"
      "print", Print
      "exit", Exit ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource("CommandParseTestCases")>]
let TestCommandParse (input: string) (command: Command) =
    Assert.That(parse input, Is.EqualTo(Some command))
