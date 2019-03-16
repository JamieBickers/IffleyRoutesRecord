FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 44319
EXPOSE 44367

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["IffleyRoutesRecord/IffleyRoutesRecord.csproj", "IffleyRoutesRecord/"]
COPY ["IffleyRoutesRecord.Logic/IffleyRoutesRecord.Logic.csproj", "IffleyRoutesRecord.Logic/"]
COPY ["IffleyRoutesRecord.Models/IffleyRoutesRecord.Models.csproj", "IffleyRoutesRecord.Models/"]
RUN dotnet restore "IffleyRoutesRecord/IffleyRoutesRecord.csproj"
COPY . .
WORKDIR "/src/IffleyRoutesRecord"
RUN dotnet build "IffleyRoutesRecord.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IffleyRoutesRecord.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "IffleyRoutesRecord.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet IffleyRoutesRecord.dll