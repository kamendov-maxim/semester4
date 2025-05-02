module Lazy

type ILazy<'a> =
    abstract member Get : unit -> 'a

// Simple lazy function implementation
type SimpleLazy<'a>(supplier : unit -> 'a) =
    let mutable result : Result<'a, exn> option = None
    interface ILazy<'a> with
        member _.Get() =
            match result with
            | Some (Ok value) -> value
            | Some (Error ex) -> raise ex
            | None ->
                try
                    let computed = supplier()
                    result <- Some (Ok computed)
                    computed
                with ex ->
                    result <- Some (Error ex)
                    raise ex

// Thread-safe lazy function implementation
type ConcurrentLazy<'a>(supplier : unit -> 'a) =
    let mutable result : Result<'a, exn> option = None
    let lockObj = obj()
    interface ILazy<'a> with
        member _.Get() =
            match result with
            | Some (Ok value) -> value
            | Some (Error ex) -> raise ex
            | None ->
                lock lockObj (fun () ->
                    match result with
                    | Some (Ok value) -> value
                    | Some (Error ex) -> raise ex
                    | None ->
                        try
                            let computed = supplier()
                            result <- Some (Ok computed)
                            computed
                        with ex ->
                            result <- Some (Error ex)
                            raise ex
                )

// Thread-safe lazy function implementation that does not use locks
type LockFreeLazy<'a>(supplier : unit -> 'a) =
    let mutable result : Result<'a, exn> option = None
    interface ILazy<'a> with
        member _.Get() =
            match result with
            | Some res ->
                match res with
                | Ok value -> value
                | Error ex -> raise ex
            | None ->
                let computedResult =
                    try Ok (supplier()) 
                    with ex -> Error ex
                let current = System.Threading.Interlocked.CompareExchange(&result, Some computedResult, None)
                match current with
                | Some res ->
                    match res with
                    | Ok value -> value
                    | Error ex -> raise ex
                | None ->
                    match computedResult with
                    | Ok value -> value
                    | Error ex -> raise ex
