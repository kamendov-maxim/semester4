module MiniCrawler

open System.Net.Http
open System.Text.RegularExpressions

let private pattern =
    @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})"

type Result = { Url: string; Size: int }

let private getHtml (client: HttpClient) (url: string) =
    async {
        try
            let! html = client.GetStringAsync url |> Async.AwaitTask
            return Some html
        with _ ->
            return None
    }

let private parseLinks (html: string) =
    Regex.Matches(html, pattern)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value.Trim())
    |> Seq.distinct
    |> List.ofSeq

let private getSize (client: HttpClient) (url: string) =
    async {
        let! page = getHtml client url

        match page with
        | None -> return { Url = url; Size = -1 }
        | Some html -> return { Url = url; Size = html.Length }
    }


let analyzePage (url: string) =
    async {
        use client = new HttpClient()
        let! mainPage = getHtml client url

        match mainPage with
        | None -> return Array.create 1 { Url = url; Size = -1 }
        | Some html ->
            let! result = html |> parseLinks |> List.map (getSize client) |> Async.Parallel
            return result
    }
