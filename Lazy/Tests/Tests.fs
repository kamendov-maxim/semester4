module Tests

open System.Threading.Tasks
open System.Threading
open NUnit.Framework
open FsUnit
open Lazy

type ILazyFactory<'a> =
    abstract member Create: (unit -> 'a) -> ILazy<'a>
    abstract member Name: string

let lazyFactories: (ILazyFactory<int> array) =
    [| { new ILazyFactory<int> with
           member _.Create supplier = SimpleLazy supplier :> ILazy<int>
           member _.Name = "SimpleLazy" }
       { new ILazyFactory<int> with
           member _.Create supplier = ConcurrentLazy supplier :> ILazy<int>
           member _.Name = "ConcurrentLazy" }
       { new ILazyFactory<int> with
           member _.Create supplier = LockFreeLazy supplier :> ILazy<int>
           member _.Name = "LockFreeLazy" } |]

[<Test>]
[<TestCaseSource("lazyFactories")>]
let ``Get returns computed value and stores it`` (factory: ILazyFactory<int>) =
    let mutable count = 0
    let supplier () = Interlocked.Increment(&count)
    let lazyInst = factory.Create supplier

    lazyInst.Get() |> should equal 1
    lazyInst.Get() |> should equal 1
    count |> should equal 1

[<Test>]
[<TestCaseSource("lazyFactories")>]
let ``Get propagates exceptions and stores them`` (factory: ILazyFactory<int>) =
    let supplier () =
        raise (System.InvalidOperationException "test exception")

    let lazyInst = factory.Create supplier

    let ex =
        Assert.Throws<System.InvalidOperationException>(fun () -> lazyInst.Get() |> ignore)

    Assert.That(ex.Message, Is.EqualTo "test exception")

[<Test>]
let ``ConcurrentLazy computes value exactly once under contention`` () =
    let mutable count = 0
    let supplier () = Interlocked.Increment(&count)
    let lazyInst = ConcurrentLazy supplier :> ILazy<int>

    Parallel.For(0, 100, fun _ -> lazyInst.Get() |> ignore) |> ignore

    lazyInst.Get() |> should equal 1
    count |> should equal 1

[<Test>]
let ``LockFreeLazy returns consistent value despite redundant computations`` () =
    let mutable count = 0
    let supplier () = Interlocked.Increment(&count)
    let lazyInst = LockFreeLazy supplier :> ILazy<int>

    let results = Array.zeroCreate 100
    Parallel.For(0, 100, fun i -> results.[i] <- lazyInst.Get()) |> ignore

    results |> Array.distinct |> should haveLength 1
    results.[0] |> should equal 1
    count |> should be (greaterThanOrEqualTo 1)

[<Test>]
let ``LockFreeLazy handles exception races correctly`` () =
    let mutable count = 0
    let exMessage = "test"

    let supplier () =
        Interlocked.Increment(&count) |> ignore
        invalidOp exMessage

    let lazyInst = LockFreeLazy supplier :> ILazy<int>

    let actions = Array.init 100 (fun _ -> fun () -> lazyInst.Get() |> ignore)

    // Check exception type and message
    let ex =
        Assert.Throws<System.AggregateException>(fun () ->
            Parallel.ForEach(actions, System.Action<_>(fun action -> action ())) |> ignore)

    ex.InnerExceptions
    |> Seq.forall (fun e -> e :? System.InvalidOperationException && e.Message = exMessage)
    |> should be True

    count |> should be (greaterThanOrEqualTo 1)
