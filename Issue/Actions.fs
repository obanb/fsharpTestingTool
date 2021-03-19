namespace helloworld.Issue

module actions =

    open Suave
    open helloworld.Issue.types
    open helloworld.Issue.Effects.database
    open Newtonsoft.Json
    open helloworld.Issue.Validations
    open helloworld.Issue.Utils.convertors

    let getIssueList dbClient httpContext =
        let issues = getIssues dbClient

        let res =
            match issues with
            | [] -> "Empty issue list"
            | _ -> JsonConvert.SerializeObject issues

        Successful.OK res httpContext

    let getIssue id dbClient httpContext =
        let issue = getIssueById dbClient id

        let res =
            match issue with
            | Some iss -> JsonConvert.SerializeObject iss
            | None -> "Issue not found"

        Successful.OK res httpContext

    let createIssue dbClient =
        request (fun req ->
            JsonConvert.DeserializeObject<CreateIssueInput>
                (req.rawForm
                 |> System.Text.ASCIIEncoding.UTF8.GetString)
            |> createIssue dbClient
            |> userRequestToUser
            |> JsonConvert.SerializeObject
            |> Successful.OK)

    let createIssue2 dbClient httpContext =
        async {
            let result =
                deserializeRequest httpContext.request
                |> validateCreateIssueInput
                |> Result.bind (fun res ->
                    createIssue2 dbClient res
                    |> Async.RunSynchronously)

            return
                match result with
                | Error e -> Successful.OK e httpContext
                | Ok issue ->
                    issue
                    |> userRequestToUser
                    |> JsonConvert.SerializeObject
                    |> (fun x -> Successful.OK x httpContext)
        }