module Tests

open BracketChecker
open NUnit.Framework

let BracketSequencsesTestCases =
    [
        "({}[[]{()}])", true
        "", true
        "{]", false
    ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource("BracketSequencsesTestCases")>]
let Test str expected = 
    Assert.That(checker str, Is.EqualTo expected)
