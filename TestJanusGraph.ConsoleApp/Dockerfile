#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TestJanusGraph.ConsoleApp/TestJanusGraph.ConsoleApp.csproj", "TestJanusGraph.ConsoleApp/"]
RUN dotnet restore "TestJanusGraph.ConsoleApp/TestJanusGraph.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/TestJanusGraph.ConsoleApp"
RUN dotnet build "TestJanusGraph.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestJanusGraph.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestJanusGraph.ConsoleApp.dll"]