FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 28319
EXPOSE 44304

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY server/server.csproj server/
RUN dotnet restore server/server.csproj
COPY . .
WORKDIR /src/server
RUN dotnet build server.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish server.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ZeusBrowser.Server.dll"]
