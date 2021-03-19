module helloworld.tests.common

open Expecto
open System

let simpleTest =
  
  test "A simple test" {
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"
  }
    
  test "A simple test" {
    let expected = 4
    Expect.equal expected (2+2) "2+2 = 4"
  }
  
  test "LIST.CHOOSE - multiple result types concat to single string" {
    let results = [Error "error"; Error "error2"]
    
    let expected = results |> List.choose (fun res ->
               match res with
                     | Error msg -> Some msg
                     | Ok _ -> None)
                                  |> (fun filtered -> match filtered with
                                          | [] -> "ok result"
                                          | _ ->  (",", filtered) |> String.Join)
                                  
    Expect.equal expected "error,error2" "Should return equal strings."
  }
  
  test "LIST.SEQUENCE_RESULT_A multiple result types into first result Error - short circuit" {
    let results = [Error "error";Error "error2"; Ok "ok"]
    
    let short = results |> List.sequenceResultA
    
    let expected = match short with
                   | Ok results -> "ok"
                   | Error e -> e
                                  
    Expect.equal expected "error" "Should return equal strings."
  }
  
  
  test "LIST.TRAVERSE_RESULT_A multiple result types into first result Error - short circuit" {
    let results = ["error2";"ok";"error"]
    
    let resultFn s = match s with
                     | "error" | "error2" -> Error "error msg"
                     | _ -> Ok "ok"
    
    let short = results |> List.traverseResultA resultFn
    
    let expected = match short with
                   | Ok results -> "ok"
                   | Error e -> e
                                  
    Expect.equal expected "error msg" "Should return equal strings."
  }
  
  test "LIST.TRAVERSE_ASYNC_A multiple async map results into single async list" {
    let origin = ["error2";"ok";"error"]
    
    let resultFn (s:string) = async{
        let res = s
        return res 
    }
    
    let short = origin |> List.traverseAsyncA resultFn |> Async.RunSynchronously
    
    let expected =["error2";"ok";"error"]
                                
    Expect.equal expected origin "Should return equal lists of strings."
  }
  
  