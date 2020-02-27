FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /fsharp-journeys
COPY . /fsharp-journeys
RUN dotnet build