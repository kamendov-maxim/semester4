module Tests

open NUnit.Framework

[<TestCase(1, 1)>]
[<TestCase(2, 1)>]
[<TestCase(3, 2)>]
[<TestCase(4, 3)>]
[<TestCase(5, 5)>]
[<TestCase(6, 8)>]
[<TestCase(7, 13)>]
[<TestCase(8, 21)>]
[<TestCase(9, 34)>]
[<TestCase(10, 55)>]
[<TestCase(20, 6765)>]
let FibonacciTest n expected =
    Assert.That(Fibonacci.Fibonacci n, Is.EqualTo expected)

[<TestCase(0)>]
[<TestCase(-1)>]
let FibonacciIncorrectInputTest n =
    Assert.Throws<System.ArgumentException>(fun () -> Fibonacci.Fibonacci n |> ignore)
    |> ignore

[<TestCase(1, 1)>]
[<TestCase(2, 2)>]
[<TestCase(3, 6)>]
[<TestCase(4, 24)>]
[<TestCase(5, 120)>]
[<TestCase(6, 720)>]
[<TestCase(7, 5040)>]
[<TestCase(8, 40320)>]
[<TestCase(9, 362880)>]
[<TestCase(10, 3628800)>]
let FactorialTests n expected =
    Assert.That(Factorial.Factorial n, Is.EqualTo expected)

[<TestCase(0)>]
[<TestCase(-1)>]
let FactorialIncorrectInputTest n =
    Assert.Throws<System.ArgumentException>(fun () -> Factorial.Factorial n |> ignore)
    |> ignore

let ReverseListTestCases =
    [ [], []
      [ 0 ], [ 0 ]
      [ 0; 1 ], [ 1; 0 ]
      [ 1; 2; 3 ], [ 3; 2; 1 ]
      [ 1; 2; 3; 4 ], [ 4; 3; 2; 1 ] ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource("ReverseListTestCases")>]
let ReverseListTests input expected =
    Assert.That(ReverseList.ReverseList input, Is.EqualTo expected)

let FindNumberTestCases =
    [ [ 1; 2; 3; 4; 5 ], 3, Some 2
      [ 1; 2; 3; 4; 5 ], 1, Some 0
      [ 1; 2; 3; 4; 5 ], 5, Some 4
      [ 1; 2; 3; 4; 5 ], 6, None ]
    |> List.map (fun (a, b, c) -> TestCaseData(a, b, c))

[<TestCaseSource("FindNumberTestCases")>]
let FindIndexTest list n (expected: int option) =
    Assert.That(FindNumber.FindNumber list n, Is.EqualTo expected)

let PowerListTestCases =
    [ 2, 5, [ 4; 8; 16; 32 ]; 1, 1, [ 2 ]; 2, 1, [] ]
    |> List.map (fun (a, b, c) -> TestCaseData(a, b, c))

[<TestCaseSource("PowerListTestCases")>]
let PowerListTests n m expected =
    Assert.That(PowerList.PowerList n m, Is.EqualTo expected)
