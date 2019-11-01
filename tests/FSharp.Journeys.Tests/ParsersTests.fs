namespace FSharp.Journeys.Tests

open NUnit.Framework
open FSharp.Journeys.DomainTypes
open FSharp.Journeys.Parsers

type ParsersTests() =
    
    [<Test>]
    member this.``Parse position should return proper data``()  =
        [|
            {| X = "1"; Y = "2"; ExpectedResult = Ok (Position (1, 2)) |}
            {| X = "-10"; Y = "-9"; ExpectedResult = Ok (Position (-10, -9)) |}
            {| X = "50E"; Y = "-9"; ExpectedResult = Error "Failed to parse position x: 50E" |}
            {| X = "12"; Y = "ASD"; ExpectedResult = Error "Failed to parse position y: ASD" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parsePosition aRecord.X aRecord.Y; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse direction should return proper data``()  =
        [|
            {| Direction = "N"; ExpectedResult = Ok North |}
            {| Direction = "S"; ExpectedResult = Ok South |}
            {| Direction = "W"; ExpectedResult = Ok West |}
            {| Direction = "E"; ExpectedResult = Ok East |}
            {| Direction = "ASD"; ExpectedResult = Error "Failed to parse direction d: ASD" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseDirection aRecord.Direction; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse state should return proper data``()  =
        [|
            {| X = "1"; Y = "2"; Direction = "N"; ExpectedResult = Ok (State (Position (1, 2), North)) |}
            {| X = "50E"; Y = "-9"; Direction = "S"; ExpectedResult = Error "Failed to parse state -> Failed to parse position x: 50E" |}
            {| X = "-10"; Y = "-9"; Direction = "NO"; ExpectedResult = Error "Failed to parse state -> Failed to parse direction d: NO" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseState aRecord.X aRecord.Y aRecord.Direction; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse command should return proper data``()  =
        [|
            {| Command = "F"; ExpectedResult = Ok MoveForward |}
            {| Command = "L"; ExpectedResult = Ok TurnLeft |}
            {| Command = "R"; ExpectedResult = Ok TurnRight |}
            {| Command = "B"; ExpectedResult = Error "Failed to parse command c: B" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseCommand aRecord.Command; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse command list should return proper data``()  =
        [|
            {| Commands = ["F"; "L"; "L"; "R"; "F"]; ExpectedResult = Ok [MoveForward; TurnLeft; TurnLeft; TurnRight; MoveForward] |}
            {| Commands = ["L"; "L"; "F"; "R"]; ExpectedResult = Ok [TurnLeft; TurnLeft; MoveForward; TurnRight] |}
            {| Commands = ["L"; "R"; "L"; "R"; "A"]; ExpectedResult = Error "Failed to parse command list -> Failed to parse command c: A" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseCommandList aRecord.Commands; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse string state should return proper data``()  =
        [|
            {| StringState = "1 1 E"; ExpectedResult = Ok (State (Position (1, 1), East)) |}
            {| StringState = "2 4 S"; ExpectedResult = Ok (State (Position (2, 4), South)) |}
            {| StringState = "24 A"; ExpectedResult = Error "Invalid state string" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseStringState aRecord.StringState; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))

    [<Test>]
    member this.``Parse string command list should return proper data``()  =
        [|
            {| StringCommandList = "FLLRF"; ExpectedResult = Ok [MoveForward; TurnLeft; TurnLeft; TurnRight; MoveForward] |}
            {| StringCommandList = "LLFR"; ExpectedResult = Ok [TurnLeft; TurnLeft; MoveForward; TurnRight] |}
            {| StringCommandList = "LRLRA"; ExpectedResult = Error "Failed to parse command list -> Failed to parse command c: A" |}
        |]
        |> Array.map (fun aRecord -> {| ActualResult = parseStringCommandList aRecord.StringCommandList; ExpectedResult = aRecord.ExpectedResult |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedResult, aRecord.ActualResult))