module Tests

open NUnit.Framework
open Queue

[<Test>]
let ``Enqueued values should be decueued in correct order`` () =
    let queue = Queue<int>.Empty()
    let queue1 = queue.Enqueue 1
    let queue2 = queue1.Enqueue 2
    let queue3 = queue2.Enqueue 3
    let v1, q2 = queue3.Dequeue()
    let v2, q3 = q2.Dequeue()
    let v3, _ = q3.Dequeue()
    Assert.That(v1, Is.EqualTo 1)
    Assert.That(v2, Is.EqualTo 2)
    Assert.That(v3, Is.EqualTo 3)

[<Test>]
let ``Dequeuing empty queue should fail with exception`` () =
    let queue = Queue<int>.Empty()
    let ex = Assert.Throws<System.Exception>(fun () -> queue.Dequeue() |> ignore);
    Assert.That(ex.Message, Is.EqualTo "Queue is empty");


