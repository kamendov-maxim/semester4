module Tests

open NUnit.Framework
open FsUnit
open FsCheck

[<SetUp>]
let Setup () = ()

[<Test>]
let CompareCounters1and2 () =
    Check.QuickThrowOnFailure(fun x -> EvenNumbers.counter1 x = EvenNumbers.counter2 x)

[<Test>]
let CompareCounters2and3 () =
    Check.QuickThrowOnFailure(fun x -> EvenNumbers.counter2 x = EvenNumbers.counter3 x)

let EvenNumbersTestCases =
    List.allPairs
        [ EvenNumbers.counter1; EvenNumbers.counter2; EvenNumbers.counter3 ]
        [ [ 1; 2; 3; 4; 5; 6 ], 3
          [ 2; 4; 6; 8 ], 4
          [ 1; 1; 1; 1; 1 ], 0
          [ -2; -4 ], 2
          [], 0 ]
    |> List.map (fun (a, (b, c)) -> TestCaseData(a, b, c))

[<TestCaseSource("EvenNumbersTestCases")>]
let EvenNumberTests (func: int list -> int) list expected =
    Assert.That(func list, Is.EqualTo expected)

let TreeMapTestCases =
    [ (fun x -> x * 2),
      BinaryTree.Node(
          1,
          BinaryTree.Node(2, BinaryTree.Leaf, BinaryTree.Leaf),
          BinaryTree.Node(3, BinaryTree.Leaf, BinaryTree.Leaf)
      ),
      BinaryTree.Node(
          2,
          BinaryTree.Node(4, BinaryTree.Leaf, BinaryTree.Leaf),
          BinaryTree.Node(6, BinaryTree.Leaf, BinaryTree.Leaf)
      ) ]
    |> List.map (fun (a, b, c) -> TestCaseData(a, b, c))

[<TestCaseSource("TreeMapTestCases")>]
let TreeMapTests (f: int -> int) input expected =
    BinaryTree.map f input |> should equal expected


let SyntaxTreeTestCases =
    [ SyntaxTree.Mul(SyntaxTree.Add(SyntaxTree.Var 5, SyntaxTree.Var 6), SyntaxTree.Var 4), 44
      SyntaxTree.Sub(
          SyntaxTree.Add(SyntaxTree.Mul(SyntaxTree.Var 2, SyntaxTree.Var 3), SyntaxTree.Var 4),
          SyntaxTree.Var 5
      ),
      5
      SyntaxTree.Var 1, 1 ]
    |> List.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource("SyntaxTreeTestCases")>]
let SyntaxTreeTests node expected =
    SyntaxTree.eval node |> should equal expected

let PrimeNumbersTestCases =
    [ [ 2; 3; 5; 7; 11; 13; 17; 19; 23; 29; 31; 37; 41; 43; 47; 53; 59; 61; 67; 71; 73; 79; 83; 89; 97; 101; 103; 107;
    109; 113; 127; 131; 137; 139; 149; 151; 157; 163; 167; 173; 179; 181; 191; 193; 197; 199; 211; 223; 227; 229; 233;
    239; 241; 251; 257; 263; 269; 271; 277; 281; 283; 293 ] ]
    |> List.map (fun a -> TestCaseData a)

[<TestCaseSource("PrimeNumbersTestCases")>]
let PrimeNumbersTest expected =
    Assert.That(PrimeNumbers.PrimeNumbers |> Seq.take 62, Is.EquivalentTo expected)
