module Queue

/// Queue class
type Queue<'a>(front: 'a list, back: 'a list) =
    let front = front
    let back = back

    /// Used to create empty queue
    static member Empty() = Queue([], [])

    /// Add element to the queue
    member _.Enqueue(e: 'a) = Queue(e :: front, back)

    /// Get last element from the queue. If queue is empty when decueued it throws an exception
    member _.Dequeue() =
        match front, back with
        | [], [] -> failwith "Queue is empty"
        | fs, b :: bs -> b, Queue(fs, bs)
        | fs, [] ->
            let revFront = List.rev fs
            revFront.Head, Queue([], revFront.Tail)
