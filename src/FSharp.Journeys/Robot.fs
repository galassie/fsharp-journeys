namespace FSharp.Journeys

open FSharp.Journeys.DomainTypes

[<RequireQualifiedAccess>]
module Robot =
    
    let turnRight state =
        match state with
        | State (pos, North) -> State (pos, East)
        | State (pos, East) -> State (pos, South)
        | State (pos, South) -> State (pos, West)
        | State (pos, West) -> State (pos, North)

    let turnLeft state =
        match state with
        | State (pos, North) -> State (pos, West)
        | State (pos, West) -> State (pos, South)
        | State (pos, South) -> State (pos, East)
        | State (pos, East) -> State (pos, North)

    let moveForward state =
        match state with
        | State (Position (x, y), North) -> State (Position (x, y+1), North)
        | State (Position (x, y), South) -> State (Position (x, y-1), South)
        | State (Position (x, y), West) -> State (Position (x-1, y), West)
        | State (Position (x, y), East) -> State (Position (x+1, y), East)               

    let changeState state command =
        match command with
        | TurnRight -> turnRight state
        | TurnLeft -> turnLeft state
        | MoveForward -> moveForward state

    let rec changeStateIter state commands =
        match commands with
        | command::tail -> changeStateIter (changeState state command) tail
        | [] -> state