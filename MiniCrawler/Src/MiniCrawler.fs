module MiniCrawler

open System.Net.Http
open System.Text.RegularExpressions

let private pattern = @"<a\s+[^>]*?href\s*=\s*[""'](http[^""']*)[""'][^>]*>"

type Result = { Url: string; Size: int }

// Get html of the page behind the link
let private getHtml (client: HttpClient) (url: string) =
    async {
        try
            let! html = client.GetStringAsync url |> Async.AwaitTask
            return Some html
        with _ ->
            return None
    }

// Parse links on a html page with regular expression
let private parseLinks (html: string) =
    Regex.Matches(html, pattern)
    |> Seq.cast<Match>
    |> Seq.map (fun m -> m.Groups.[1].Value.Trim())
    |> Seq.distinct
    |> List.ofSeq

// Get size of a page behind url and wrap results into record
let internal getSize (client: HttpClient) (url: string) =
    async {
        let! page = getHtml client url

        match page with
        | None -> return { Url = url; Size = -1 }
        | Some html -> return { Url = url; Size = html.Length }
    }

// crawl is a wrapper for this function. It is needed for tests with mock HttpClient
let internal analyzePage (client: HttpClient) (url: string) =
    async {
        let! mainPage = getHtml client url

        match mainPage with
        | None -> return Array.create 1 { Url = url; Size = -1 }
        | Some html ->
            let! result = html |> parseLinks |> List.map (getSize client) |> Async.Parallel
            return result
    }

// Download page behind a url, parse all links on it, download them and print sizes of them in characters
let crawl (url: string) =
    use client = new HttpClient()

    async {
        let! results = analyzePage client url
        return results
    }
