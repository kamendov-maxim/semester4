module Tests

open System.Net.Http
open System.Net
open NUnit.Framework
open System.Threading.Tasks
open FsUnit

type MockHttpMessageHandler(responseGenerator: string -> HttpResponseMessage) =
    inherit HttpMessageHandler()

    member val Generator = responseGenerator

    override this.SendAsync
        (request: HttpRequestMessage, cancellationToken: System.Threading.CancellationToken)
        : Task<HttpResponseMessage> =
        Task.FromResult(this.Generator request.RequestUri.AbsoluteUri)

let createMockClient (response: string) (statusCode: HttpStatusCode) =
    let handler =
        new MockHttpMessageHandler(fun url ->
            new HttpResponseMessage(StatusCode = statusCode, Content = new StringContent(response)))

    new HttpClient(handler)

[<Test>]
let ``analyzePage should process mocked links`` () =
    async {
        let mockHtml =
            """
            <a href="http://test1.com">Link1</a>
            <a href="http://test2.com">Link2</a>
        """

        use client = createMockClient mockHtml HttpStatusCode.OK

        let! results = MiniCrawler.analyzePage client "http://dummy.com"

        results |> should haveLength 2

        results
        |> Array.iter (fun r ->
            r.Url |> should startWith "http://test"
            r.Size |> should greaterThan 0)
    }
    |> Async.RunSynchronously

[<Test>]
let ``should handle HTTP errors correctly`` () =
    async {
        use client = createMockClient "Error" HttpStatusCode.NotFound

        let! result = MiniCrawler.getSize client "http://error.com"

        result.Url |> should equal "http://error.com"
        result.Size |> should equal -1
    }
    |> Async.RunSynchronously

[<Test>]
let ``should process multiple page sizes`` () =
    async {
        let handler =
            { new HttpMessageHandler() with
                override _.SendAsync(request, _) =
                    let content =

                        match request.RequestUri.AbsoluteUri with
                        | "http://main.com/" -> "<a href='http://page1.com'></a><a href='http://page2.com'></a>"
                        | "http://page1.com/" -> String.replicate 100 "a"
                        | "http://page2.com/" -> String.replicate 200 "b"
                        | _ -> ""

                    Task.FromResult(new HttpResponseMessage(Content = new StringContent(content))) }

        use client = new HttpClient(handler)

        let! results = MiniCrawler.analyzePage client "http://main.com"

        results
        |> should
            contain
            { MiniCrawler.Result.Url = "http://page1.com"
              MiniCrawler.Result.Size = 100 }

        results
        |> should
            contain
            { MiniCrawler.Result.Url = "http://page2.com"
              MiniCrawler.Result.Size = 200 }
    }
    |> Async.RunSynchronously
