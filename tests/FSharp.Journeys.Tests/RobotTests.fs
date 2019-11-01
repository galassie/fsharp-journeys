namespace FSharp.Journeys.Tests

open NUnit.Framework
open FSharp.Journeys
open FSharp.Journeys.DomainTypes

type RobotTests() =
    
    [<Test>]
    member this.``Robot should turn left properly``()  =
        [|
            {| InitialState = State (Position (1, 1), North); ExpectedEndState = State (Position (1, 1), West) |}
            {| InitialState = State (Position (5, 6), South); ExpectedEndState = State (Position (5, 6), East) |}
            {| InitialState = State (Position (-10, -20), East); ExpectedEndState = State (Position (-10, -20), North) |}
            {| InitialState = State (Position (50, 14), West); ExpectedEndState = State (Position (50, 14), South) |}
        |]
        |> Array.map (fun aRecord -> {| ActualState = Robot.turnLeft aRecord.InitialState; ExpectedState = aRecord.ExpectedEndState |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedState, aRecord.ActualState))

    [<Test>]
    member this.``Robot should turn right properly``()  =
        [|
            {| InitialState = State (Position (1, 1), North); ExpectedEndState = State (Position (1, 1), East) |}
            {| InitialState = State (Position (5, 6), South); ExpectedEndState = State (Position (5, 6), West) |}
            {| InitialState = State (Position (-10, -20), East); ExpectedEndState = State (Position (-10, -20), South) |}
            {| InitialState = State (Position (50, 14), West); ExpectedEndState = State (Position (50, 14), North) |}
        |]
        |> Array.map (fun aRecord -> {| ActualState = Robot.turnRight aRecord.InitialState; ExpectedState = aRecord.ExpectedEndState |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedState, aRecord.ActualState))

    [<Test>]
    member this.``Robot should moveForward properly``()  =
        [|
            {| InitialState = State (Position (1, 1), North); ExpectedEndState = State (Position (1, 2), North) |}
            {| InitialState = State (Position (5, 6), South); ExpectedEndState = State (Position (5, 5), South) |}
            {| InitialState = State (Position (-10, -20), East); ExpectedEndState = State (Position (-9, -20), East) |}
            {| InitialState = State (Position (50, 14), West); ExpectedEndState = State (Position (49, 14), West) |}
        |]
        |> Array.map (fun aRecord -> {| ActualState = Robot.moveForward aRecord.InitialState; ExpectedState = aRecord.ExpectedEndState |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedState, aRecord.ActualState))

    [<Test>]
    member this.``Robot should change state properly based on command``()  =
        [|
            {| Command = TurnLeft; InitialState = State (Position (1, 1), North); ExpectedEndState = State (Position (1, 1), West) |}
            {| Command = TurnRight; InitialState = State (Position (-10, -20), East); ExpectedEndState = State (Position (-10, -20), South) |}
            {| Command = MoveForward; InitialState = State (Position (50, 14), West); ExpectedEndState = State (Position (49, 14), West) |}
        |]
        |> Array.map (fun aRecord -> {| ActualState = Robot.changeState aRecord.InitialState aRecord.Command; ExpectedState = aRecord.ExpectedEndState |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedState, aRecord.ActualState))

    [<Test>]
    member this.``Robot should reach the final state properly with list of commands``()  =
        [|
            {|
                InitialState = State (Position (1, 1), North);
                Commands = [
                                TurnRight; MoveForward; TurnRight; MoveForward; TurnRight;
                                MoveForward; TurnRight; MoveForward
                            ];
                ExpectedEndState = State (Position (1, 1), North);
            |}
            {|
                InitialState = State (Position (3, 2), North);
                Commands = [
                                MoveForward; TurnRight; TurnRight; MoveForward; TurnLeft;
                                TurnLeft; MoveForward; MoveForward; TurnRight; TurnRight;
                                MoveForward; TurnLeft; TurnLeft
                            ];
                ExpectedEndState = State (Position (3, 3), North);
            |}
            {|
                InitialState = State (Position (0, 3), West);
                Commands = [
                                TurnLeft; TurnLeft; MoveForward; MoveForward; MoveForward;
                                TurnLeft; MoveForward; TurnLeft; MoveForward; TurnLeft;
                            ];
                ExpectedEndState = State (Position (2, 4), South);
            |}
        |]
        |> Array.map (fun aRecord -> {| ActualState = Robot.changeStateIter aRecord.InitialState aRecord.Commands; ExpectedState = aRecord.ExpectedEndState |})
        |> Array.iter (fun aRecord -> Assert.AreEqual(aRecord.ExpectedState, aRecord.ActualState))