FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY IffleyRoutesRecord/IffleyRoutesRecord.csproj IffleyRoutesRecord/
RUN dotnet restore IffleyRoutesRecord/IffleyRoutesRecord.csproj
COPY . .
WORKDIR /src/IffleyRoutesRecord
RUN dotnet build IffleyRoutesRecord.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish IffleyRoutesRecord.csproj -c Release -o /app

FROM base AS final
ENV ASPNETCORE_URLS http://*:5000
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IffleyRoutesRecord.dll"]
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet IffleyRoutesRecord.dll
