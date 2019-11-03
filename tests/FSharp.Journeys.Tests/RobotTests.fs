namespace FSharp.Journeys.Tests

open NUnit.Framework
open FSharp.Journeys
open FSharp.Journeys.DomainTypes

type RobotTests() =
    
    [<Test>]
    member __.``Robot should turn left properly``()  =
        [|
            {| 
                InitialState = { Position = Position (1, 1); Direction = North }
                ExpectedEndState = { Position = Position (1, 1); Direction = West }
            |}
            {|
                InitialState = { Position = Position (5, 6); Direction = South }
                ExpectedEndState = { Position = Position (5, 6); Direction = East }
            |}
            {|
                InitialState = { Position = Position (-10, -20); Direction = East }
                ExpectedEndState = { Position = Position (-10, -20); Direction = North }
            |}
            {|
                InitialState = { Position = Position (50, 14); Direction = West }
                ExpectedEndState = { Position = Position (50, 14); Direction = South }
            |}
        |]
        |> Array.map (fun r -> {| ActualState = Robot.turnLeft r.InitialState; ExpectedState = r.ExpectedEndState |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedState, r.ActualState))

    [<Test>]
    member __.``Robot should turn right properly``()  =
        [|
            {| 
                InitialState = { Position = Position (1, 1); Direction = North }
                ExpectedEndState = { Position = Position (1, 1); Direction = East }
            |}
            {|
                InitialState = { Position = Position (5, 6); Direction = South }
                ExpectedEndState = { Position = Position (5, 6); Direction = West }
            |}
            {|
                InitialState = { Position = Position (-10, -20); Direction = East }
                ExpectedEndState = { Position = Position (-10, -20); Direction = South }
            |}
            {|
                InitialState = { Position = Position (50, 14); Direction = West }
                ExpectedEndState = { Position = Position (50, 14); Direction =  North }
            |}
        |]
        |> Array.map (fun r -> {| ActualState = Robot.turnRight r.InitialState; ExpectedState = r.ExpectedEndState |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedState, r.ActualState))

    [<Test>]
    member __.``Robot should moveForward properly``()  =
        [|
            {| 
                InitialState = { Position =  Position (1, 1); Direction = North }
                ExpectedEndState = { Position = Position (1, 2); Direction = North }
            |}
            {|
                InitialState = { Position = Position (5, 6); Direction = South }
                ExpectedEndState = { Position = Position (5, 5); Direction = South } 
            |}
            {|
                InitialState = { Position =  Position (-10, -20); Direction = East }
                ExpectedEndState = { Position = Position (-9, -20); Direction = East }
            |}
            {|
                InitialState = { Position =  Position (50, 14); Direction = West }
                ExpectedEndState = { Position =Position (49, 14); Direction = West }
            |}
        |]
        |> Array.map (fun r -> {| ActualState = Robot.moveForward r.InitialState; ExpectedState = r.ExpectedEndState |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedState, r.ActualState))

    [<Test>]
    member __.``Robot should change state properly based on command``()  =
        [|
            {| 
                InitialState = { Position = Position (1, 1); Direction = North }
                Command = TurnLeft
                ExpectedEndState = { Position = Position (1, 1); Direction = West }
            |}
            {|
                InitialState = { Position = Position (-10, -20); Direction = East }
                Command = TurnRight
                ExpectedEndState = { Position = Position (-10, -20); Direction = South }
            |}
            {|
                InitialState = { Position = Position (50, 14); Direction = West }
                Command = MoveForward
                ExpectedEndState = { Position = Position (49, 14); Direction = West }
            |}
        |]
        |> Array.map (fun r -> {| ActualState = Robot.changeState r.InitialState r.Command; ExpectedState = r.ExpectedEndState |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedState, r.ActualState))

    [<Test>]
    member __.``Robot should reach the final state properly with list of commands``()  =
        [|
            {|
                InitialState = { Position = Position (1, 1); Direction = North }
                Commands = [
                                TurnRight; MoveForward; TurnRight; MoveForward; TurnRight;
                                MoveForward; TurnRight; MoveForward
                            ]
                ExpectedEndState = { Position = Position (1, 1); Direction = North }
            |}
            {|
                InitialState = { Position = Position (3, 2); Direction = North }
                Commands = [
                                MoveForward; TurnRight; TurnRight; MoveForward; TurnLeft;
                                TurnLeft; MoveForward; MoveForward; TurnRight; TurnRight;
                                MoveForward; TurnLeft; TurnLeft
                            ]
                ExpectedEndState = { Position = Position (3, 3); Direction = North }
            |}
            {|
                InitialState = { Position = Position (0, 3); Direction = West }
                Commands = [
                                TurnLeft; TurnLeft; MoveForward; MoveForward; MoveForward;
                                TurnLeft; MoveForward; TurnLeft; MoveForward; TurnLeft;
                            ]
                ExpectedEndState = { Position = Position (2, 4); Direction = South }
            |}
        |]
        |> Array.map (fun r -> {| ActualState = Robot.changeStateIter r.InitialState r.Commands; ExpectedState = r.ExpectedEndState |})
        |> Array.iter (fun r -> Assert.AreEqual(r.ExpectedState, r.ActualState))