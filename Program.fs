module helloworld.Program

open System

open Suave
open Suave.Operators
open Suave.Filters
open Suave.Writers
open Suave.Successful

open MongoDB.Driver

open helloworld.tests.config
open helloworld.tests.common

open helloworld.Issue.actions
open helloworld.Common.Database.connections

let setCORSHeaders =
    addHeader "Access-Control-Allow-Origin" "*"
    >=> addHeader "Access-Control-Allow-Headers" "content-type"


let allowCors: WebPart =
    choose [ OPTIONS
             >=> fun context ->
                     context
                     |> (setCORSHeaders >=> Successful.OK "CORS approved") ]

let serverConfig =
    let randomPort = Random().Next(7000, 7999)

    { defaultConfig with
          bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" randomPort ] }

let app (dbClient: IMongoDatabase) =
    choose [ allowCors
             GET
             >=> choose [ path "/issues" >=> getIssueList dbClient
                          pathScan "/issue/%s" (fun id -> getIssue id dbClient)
                          path "/hello" >=> OK "Hello GET" ]
             POST
             >=> choose [ path "/issues/create" >=> createIssue dbClient
                          path "/issues/create2"
                          >=> fun ctx ->
                                  async {
                                      let! res = createIssue2 dbClient ctx
                                      return! res
                                  } ]
             path "/"
             >=> (OK "This will return the base page.") ]


[<EntryPoint>]
let main argv =
    runExpecto argv simpleTest

    startWebServer serverConfig (getInternalMongoClient () |> app)

    0
