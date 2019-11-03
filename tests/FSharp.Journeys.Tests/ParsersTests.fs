namespace FSharp.Journeys.Tests

open NUnit.Framework
open FSharp.Journeys.DomainTypes
open FSharp.Journeys.Parsers

type ParsersTests() =
    
    [<Test>]
    member __.``Parse position should return proper data``()  =
        [|
            {| X = "1"; Y = "2"; ExpectedResult = Ok (Position (1, 2)) |}
            {| X = "-10"; Y = "-9"; ExpectedResult = Ok (Position (-10, -9)) |}
            {| X = "50E"; Y = "-9"; ExpectedResult = Error "Failed to parse X position: 50E" |}
            {| X = "12"; Y = "ASD"; ExpectedResult = Error "Failed to parse Y position: ASD" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parsePosition r.X r.Y; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse direction should return proper data``()  =
        [|
            {| Direction = "N"; ExpectedResult = Ok North |}
            {| Direction = "S"; ExpectedResult = Ok South |}
            {| Direction = "W"; ExpectedResult = Ok West |}
            {| Direction = "E"; ExpectedResult = Ok East |}
            {| Direction = "ASD"; ExpectedResult = Error "Failed to parse direction: ASD" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseDirection r.Direction; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse state should return proper data``()  =
        [|
            {| X = "1"; Y = "2"; Direction = "N"; ExpectedResult = Ok { Position = Position (1, 2); Direction = North } |}
            {| X = "50E"; Y = "-9"; Direction = "S"; ExpectedResult = Error "Failed to parse state -> Failed to parse X position: 50E" |}
            {| X = "-10"; Y = "-9"; Direction = "NO"; ExpectedResult = Error "Failed to parse state -> Failed to parse direction: NO" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseState r.X r.Y r.Direction; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse command should return proper data``()  =
        [|
            {| Command = "F"; ExpectedResult = Ok MoveForward |}
            {| Command = "L"; ExpectedResult = Ok TurnLeft |}
            {| Command = "R"; ExpectedResult = Ok TurnRight |}
            {| Command = "B"; ExpectedResult = Error "Failed to parse command: B" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseCommand r.Command; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse command list should return proper data``()  =
        [|
            {| Commands = ["F"; "L"; "L"; "R"; "F"]; ExpectedResult = Ok [MoveForward; TurnLeft; TurnLeft; TurnRight; MoveForward] |}
            {| Commands = ["L"; "L"; "F"; "R"]; ExpectedResult = Ok [TurnLeft; TurnLeft; MoveForward; TurnRight] |}
            {| Commands = ["L"; "R"; "L"; "R"; "A"]; ExpectedResult = Error "Failed to parse command list -> Failed to parse command: A" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseCommandList r.Commands; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse string state should return proper data``()  =
        [|
            {| StringState = "1 1 E"; ExpectedResult = Ok { Position = Position (1, 1); Direction = East } |}
            {| StringState = "2 4 S"; ExpectedResult = Ok { Position = Position (2, 4); Direction = South } |}
            {| StringState = "24 A"; ExpectedResult = Error "Invalid state string: 24 A" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseStringState r.StringState; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))

    [<Test>]
    member __.``Parse string command list should return proper data``()  =
        [|
            {| StringCommandList = "FLLRF"; ExpectedResult = Ok [MoveForward; TurnLeft; TurnLeft; TurnRight; MoveForward] |}
            {| StringCommandList = "LLFR"; ExpectedResult = Ok [TurnLeft; TurnLeft; MoveForward; TurnRight] |}
            {| StringCommandList = "LRLRA"; ExpectedResult = Error "Failed to parse command list -> Failed to parse command: A" |}
        |]
        |> Array.map (fun r -> {| ActualResult = parseStringCommandList r.StringCommandList; ExpectedResult = r.ExpectedResult |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedResult, r.ActualResult))