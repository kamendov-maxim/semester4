module Tests

open NUnit.Framework
open Rounding

let TestCases =
    [ rounding 3 {
          let! a = 2.0 / 12.0
          let! b = 3.5
          return a / b
      },
      0.047

      rounding 3 {
          let! a = 6.3 * 18.55
          let! b = 0.056
          let! c = 15.008
          let! d = 2.111
          return a * b + c - d
      },
      19.441 ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource("TestCases")>]
let ``Workflow should give correct answers`` (expression: float) (answer: float) =
    Assert.Pass()
    Assert.That(expression, Is.EqualTo(answer).Within 0.001)
