namespace FSharp.Journeys

open FSharp.Journeys.DomainTypes

module Parsers =

    type ParseBuilder() =
        member __.Bind(mv, func) =
            match mv with
            | Ok v -> func v
            | Error message -> Error message
        member __.Return(v) = Ok v
        member __.ReturnFrom(mv) = mv
        member __is.For(seqv, func) = 
            let rec innerFor remain func result =
                match remain with
                | [] -> Ok result
                | head::tail -> 
                    func head
                    |> function
                        | Ok value -> innerFor tail func (result @ [value])
                        | Error message -> Error message
            innerFor seqv func List.empty
        member __.Yield(mv) = mv

    let parse = ParseBuilder()

    let parsePosition x y =
        let (successX, valueX) = System.Int32.TryParse x
        let (successY, valueY) = System.Int32.TryParse y
        match successX, successY with
        | true, true -> Ok (Position (valueX, valueY))
        | false, _ -> Error (sprintf "Failed to parse X position: %s" x)
        | _, false -> Error (sprintf "Failed to parse Y position: %s" y)

    let parseDirection d = 
        match d with
        | "N" -> Ok North
        | "S" -> Ok South
        | "W" -> Ok West
        | "E" -> Ok East
        | _ -> Error (sprintf "Failed to parse direction: %s" d)

    let parseState x y d =
        parse {
            let! pos = parsePosition x y
            let! dir = parseDirection d
            return! Ok { Position = pos; Direction = dir }
        }
        |> function
            | Ok state -> Ok state
            | Error innerMessage -> Error (sprintf "Failed to parse state -> %s" innerMessage)
    
    let parseCommand c =
        match c with
        | "L" -> Ok TurnLeft
        | "R" -> Ok TurnRight
        | "F" -> Ok MoveForward
        | _ -> Error (sprintf "Failed to parse command: %s" c)

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
            | _ -> Error (sprintf "Invalid state string: %s" input)

    let parseStringCommandList (input: string) =
        input.ToCharArray()
        |> Array.toList
        |> List.map (fun c -> sprintf "%c" c)
        |> parseCommandList