namespace FSharp.Journeys

module DomainTypes =

    type Position = Position of (int * int)
    
    type Direction = North | East | South | West

    type State = State of (Position * Direction)

    type Command = TurnRight | TurnLeft | MoveForward

    type ChangeState = (State * Command) -> State