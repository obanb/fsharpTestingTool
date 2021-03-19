namespace helloworld.Issue.Effects

module database =
    open MongoDB.Driver
    open helloworld.Issue.types
    open MongoDB.Bson
    
    let getIssues (dbClient: IMongoDatabase) =
        let col = dbClient.GetCollection<Issue> "issues"

        col.Find(Builders.Filter.Empty).ToEnumerable()
        |> List.ofSeq

    let getIssueById (dbClient: IMongoDatabase) (id: string) =
        let col = dbClient.GetCollection<Issue> "issues"

        let filter =
            Builders.Filter.Eq((fun issue -> issue.Id), id)

        let find =
            col.Find(filter).ToEnumerable() |> List.ofSeq

        let res =
            match find with
            | [] -> None
            | _ -> Some find.Head

        res

    let prepareIssueDocument createIssueInput =
        let doc =
            { Id = ObjectId.GenerateNewId().ToString()
              name = createIssueInput.name
              content = createIssueInput.content
              desc = createIssueInput.desc }

        doc

    let createIssue (dbClient: IMongoDatabase) (input: CreateIssueInput) =
        let col = dbClient.GetCollection<Issue> "issues"
        let doc = input |> prepareIssueDocument
        col.InsertOne doc

        input

    let createIssue2 (dbClient: IMongoDatabase) (input: CreateIssueInput): Async<Result<CreateIssueInput, string>> =
        async {
            try
                let col = dbClient.GetCollection<Issue> "issues"
                do input |> prepareIssueDocument |> col.InsertOne

                return Ok input
            with e -> return Error(e.ToString())
        }