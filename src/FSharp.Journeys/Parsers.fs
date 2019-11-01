namespace FSharp.Journeys

open FSharp.Journeys.DomainTypes

module Parsers =

    type ParseBuilder() =
        member this.Bind(mv, func) =
            match mv with
            | Ok v -> func v
            | Error message -> Error message
        member this.Return(v) = Ok v
        member this.ReturnFrom(mv) = mv
        member this.For(seqv, func) = 
            let rec innerFor remain func result =
                match remain with
                | [] -> Ok result
                | head::tail -> 
                    let mv = func head
                    match mv with
                    | Ok value -> innerFor tail func (result @ [value])
                    | Error message -> Error message
            innerFor seqv func List.empty
        member this.Yield(mv) = mv

    let parse = ParseBuilder()

    let parsePosition x y =
        let (successX, valueX) = System.Int32.TryParse x
        let (successY, valueY) = System.Int32.TryParse y
        match successX, successY with
        | true, true -> Ok (Position (valueX, valueY))
        | false, _ -> Error (sprintf "Failed to parse position x: %s" x)
        | _, false -> Error (sprintf "Failed to parse position y: %s" y)

    let parseDirection d = 
        match d with
        | "N" -> Ok North
        | "S" -> Ok South
        | "W" -> Ok West
        | "E" -> Ok East
        | _ -> Error (sprintf "Failed to parse direction d: %s" d)

    let parseState x y d =
        parse {
            let! position = parsePosition x y
            let! direction = parseDirection d
            return! Ok (State (position, direction))
        }
        |> function
        | Ok state -> Ok state
        | Error innerMessage -> Error (sprintf "Failed to parse state -> %s" innerMessage)
    
    let parseCommand c =
        match c with
        | "L" -> Ok TurnLeft
        | "R" -> Ok TurnRight
        | "F" -> Ok MoveForward
        | _ -> Error (sprintf "Failed to parse command c: %s" c)

    let parseCommandList cs = 
        parse {
            for c in cs -> parseCommand c
        }
        |> function
        | Ok commands -> Ok commands
        | Error innerMessage -> Error (sprintf "Failed to parse command list -> %s" innerMessage)
    
    let parseStringState (input: string) =
        input.Split(' ')
        |> function
        | [|xStr; yStr; dStr|] -> parseState xStr yStr dStr
        | _ -> Error "Invalid state string"

    let parseStringCommandList (input: string) =
        input.ToCharArray()
        |> Array.toList
        |> List.map (fun c -> sprintf "%c" c)
        |> parseCommandList