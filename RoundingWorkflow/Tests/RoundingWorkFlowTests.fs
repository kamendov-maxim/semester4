module Tests

open NUnit.Framework
open Rounding

let TestCases =
    [ rounding 3 {
          let! a = 2.0 / 12.0
          let! b = 3.5
          return a / b
      },
      0.047,
      3


      rounding 5 {
          let! a = 6.3 * 18.55
          let! b = 0.056
          let! c = 15.008
          let! d = 2.111
          let! z = 6.3 * 18.55 * 0.056 + 15.008 - 2.111
          return a * b + c - d
      },
      19.44144,
      5
      ]
    |> List.map (fun (a, b, c) -> TestCaseData(a, b, c))

let delta digitsAfterPoint = pown 0.1 digitsAfterPoint

[<TestCaseSource("TestCases")>]
let ``Workflow should give correct answers`` (expression: float) (answer: float) (digitsAfterPoint: int)=
    Assert.That(expression, Is.EqualTo(answer).Within (delta digitsAfterPoint))
