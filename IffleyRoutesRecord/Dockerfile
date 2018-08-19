FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY IffleyRoutesRecord/IffleyRoutesRecord.csproj IffleyRoutesRecord/
RUN dotnet restore IffleyRoutesRecord/IffleyRoutesRecord.csproj
COPY . .
WORKDIR /src/IffleyRoutesRecord
RUN dotnet build IffleyRoutesRecord.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish IffleyRoutesRecord.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IffleyRoutesRecord.dll"]
