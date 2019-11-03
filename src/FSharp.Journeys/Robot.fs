namespace FSharp.Journeys

open FSharp.Journeys.DomainTypes

[<RequireQualifiedAccess>]
module Robot =
    
    let turnRight state =
        match state.Direction with
        | North -> { state with Direction = East }
        | East -> { state with Direction = South }
        | South -> { state with Direction = West }
        | West -> { state with Direction = North }

    let turnLeft state =
        match state.Direction with
        | North -> { state with Direction = West }
        | West -> { state with Direction = South }
        | South -> { state with Direction = East }
        | East -> { state with Direction = North }

    let moveForward state =
        match state.Position, state.Direction with
        | Position (x, y), North -> { state with Position = Position (x, y+1) }
        | Position (x, y), South -> { state with Position = Position (x, y-1) }
        | Position (x, y), West -> { state with Position = Position (x-1, y) }
        | Position (x, y), East -> { state with Position = Position (x+1, y) }               

    let changeState state command =
        match command with
        | TurnRight -> turnRight state
        | TurnLeft -> turnLeft state
        | MoveForward -> moveForward state

    let rec changeStateIter state commands =
        match commands with
        | command::tail -> changeStateIter (changeState state command) tail
        | [] -> state