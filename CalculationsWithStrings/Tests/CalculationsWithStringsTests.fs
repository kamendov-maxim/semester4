module Tests

open NUnit.Framework
open CalculationsWithStrings

let TestCases =
    [ calculate {
          let! x = "1"
          let! y = "2"
          let z = x + y
          return z
      },
      Some 3.0

      calculate {
          let! x = "1"
          let! y = "Ъ"
          let z = x + y
          return z
      },
      None ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

let delta = 0.00001

[<TestCaseSource("TestCases")>]
let ``Workflow should give correct answers`` (expression: option<float>) (answer: option<float>) =
    match answer with
    | None -> Assert.That(expression, Is.EqualTo None)
    | Some f -> 
        match expression with
        | None -> Assert.Fail()
        | Some res -> Assert.That(res, Is.EqualTo(f).Within delta)
    Assert.That(expression, Is.EqualTo(answer).Within delta)

