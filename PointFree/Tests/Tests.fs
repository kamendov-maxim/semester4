module Tests

open PointFree
open FsCheck
open NUnit.Framework

[<Test>]
let ``All implementations should be equivalent`` () =
    Check.Quick(fun x l -> func x l = func' x l)
    Check.Quick(fun x l -> func' x l = func'' x l)
    Check.Quick(fun x l -> func'' x l = funcPointFree x l)
