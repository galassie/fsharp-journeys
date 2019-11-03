# FSharp Journeys

A coding challenge based on the one defined by Mike Hadlow [github](https://github.com/mikehadlow/Journeys).

This implementation is entirely written in F# and it's a dotnet core 3.0 solution.

## Build on your machine

If you want to build this soltion, execute the following commands:

``` shell
git clone https://github.com/galassie/fsharp-journeys.git
cd fsharp-journeys
dotnet build
```

If you want to run the tests, execute the following command:

``` shell
dotnet test
```

## Run the console application

To run the application and see the output, execute the following commands from the root folder of the repository:

``` shell
cd ./src/FSharp.Journeys.Console
dotnet run
```

The journeys are defined in the input.txt file.

Most of those are correct:

``` shell
❗Start Journey n. 1❗

Initial robot state: { Position = Position (1, 1)
  Direction = East }

Commands: [TurnRight; MoveForward; TurnRight; MoveForward; TurnRight; MoveForward;
 TurnRight; MoveForward]

Expected final robot state: { Position = Position (1, 1)
  Direction = East }

Actual final robot state: { Position = Position (1, 1)
  Direction = East }

Test result: PASS 🎉
```

I've also added two more wrong journeys to check the output (one with parsing errors and the other with mismatch expected-actual final robot state):

``` shell
❗️Start Journey n. 4❗️

Parsing error ⛔️
Failed to parse state -> Failed to parse direction: A


❗️Start Journey n. 5❗️

Initial robot state: { Position = Position (0, 3)
  Direction = West }

Commands: [TurnLeft; TurnLeft; MoveForward; MoveForward; MoveForward; TurnLeft;
 MoveForward; TurnLeft; MoveForward; TurnLeft]

Expected final robot state: { Position = Position (2, 9)
  Direction = East }

Actual final robot state: { Position = Position (2, 4)
  Direction = South }

Test result: FAIL ❌
```

## Final notes

Suggestions on how to improve the code are more than welcome! 😊  
I'm not by any means an F# expert so feel free to contact me or open PRs if you notice something that could have been done better!
