open MiniCrawler

let helpMessage = "Usage: miniCrawler <url>\n"

[<EntryPoint>]
let main args =
    if args.Length = 0 then
        printfn "%s" helpMessage
        1
    else
        let results = crawl args[0] |> Async.RunSynchronously

        if results[0].Size = -1 then
            printfn "Unable to download main page (%s)" args[0]
            1
        else
            printfn "List of links found on that page:"

            for i in results do
                if i.Size = -1 then
                    printfn "Url: %s\nPage size: page downloading failed\n" i.Url
                else
                    printfn "Url: %s\nPage size: %d\n" i.Url i.Size

            0
