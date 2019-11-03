namespace FSharp.Journeys

module DomainTypes =

    type Position = Position of (int * int)
    
    type Direction = North | East | South | West

    type State = { Position: Position; Direction: Direction }

    type Command = TurnRight | TurnLeft | MoveForward