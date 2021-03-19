module helloworld.Issue.Validations

open System
open helloworld.Issue.types

let validateRequired prop propName =
    match prop with
    | "" -> Error("Property " + propName + " is required.")
    | _ -> Ok prop

let validateCreateIssueInput (input: CreateIssueInput) =
    let validationResults =
        [ validateRequired input.name "name"
          validateRequired input.desc "desc"
          validateRequired input.desc "desc" ]
        |> List.choose (fun res ->
            match res with
            | Error msg -> Some msg
            | Ok _ -> None)
        |> (fun filtered ->
            match filtered with
            | [] -> Ok input
            | _ ->
                (Environment.NewLine, filtered)
                |> String.Join
                |> Error)

    validationResults