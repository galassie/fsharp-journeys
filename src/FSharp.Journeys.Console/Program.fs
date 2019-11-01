// Learn more about F# at http://fsharp.org

open System
open System.IO
open FSharp.Journeys
open FSharp.Journeys.Parsers

let testJourney journeyNumber lineInitialState lineCommands lineExpectedFinalState = 
    printfn "\n\n❗️Start Journey n. %i❗️" journeyNumber
    parse {
        let! initialState = parseStringState lineInitialState
        printfn "\nInitial robot state: %A" initialState
        
        let! commands = parseStringCommandList lineCommands
        printfn "\nCommands: %A" commands

        let! expectedFinalState = parseStringState lineExpectedFinalState
        printfn "\nExpected final robot state: %A" expectedFinalState

        let actualFinalState = Robot.changeStateIter initialState commands
        printfn "\nActual final robot state: %A" actualFinalState

        return (expectedFinalState = actualFinalState)
    }
    |> function
    | Ok success -> printfn "\nTest result: %s" (if success then "PASS 🎉" else "FAIL ❌")
    | Error message -> printfn "\nError on journey => %s" message

let rec testJourneys journeyNumber inputLines =
    match inputLines with
    | lineInitialState::lineCommands::[lineExpectedFinalState] ->
        testJourney journeyNumber lineInitialState lineCommands lineExpectedFinalState
    | lineInitialState::lineCommands::lineExpectedFinalState::[empty] when String.IsNullOrWhiteSpace(empty) ->
        testJourney journeyNumber lineInitialState lineCommands lineExpectedFinalState
    | lineInitialState::lineCommands::lineExpectedFinalState::empty::nextJourneys when String.IsNullOrWhiteSpace(empty) ->
        testJourney journeyNumber lineInitialState lineCommands lineExpectedFinalState
        testJourneys (journeyNumber + 1) nextJourneys
    | _ -> printfn "\n\nSomething went wrong while reading the file.. 💀"


[<EntryPoint>]
let main argv =
    printfn "FSHARP JOURNEYS 🤖"
    
    let lines = Array.toList <| File.ReadAllLines("input.txt")
    testJourneys 1 lines
    0