module Tests

open NUnit.Framework
open LambdaInterpreter

let LambdaInterpreterTestCases =
    [
      Application (Abstraction ("x", Var "x"), Var "y"), Var "y";
      Application(Abstraction("x", Abstraction("y", Var "x")), Abstraction("y", Var "y")) , Abstraction ("y0", Abstraction ("y", Var "y"));
      Application(Application(Abstraction("x", Abstraction("y", Var "x")), Var "a"), Var "b"), Var "a";
      Application(Abstraction("x", Var "x"), Abstraction("x", Var "x")), Abstraction("x", Var "x")
    ] |> List.map (fun (a, b) -> TestCaseData(a, b))


[<TestCaseSource("LambdaInterpreterTestCases")>]
let Test1 expression expected =
    Assert.That(Reduce expression, Is.EqualTo expected)

