FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Iffley/Iffley.csproj Iffley/
RUN dotnet restore Iffley/Iffley.csproj
COPY . .
WORKDIR /src/Iffley
RUN dotnet build Iffley.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Iffley.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Iffley.dll"]
