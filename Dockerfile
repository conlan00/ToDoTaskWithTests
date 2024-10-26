# Build Stage 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore "./ToDo/ToDo.csproj" --disable-parallel

RUN dotnet publish "./ToDo/ToDo.csproj" -c release -o /app --no-restore

# Final Stage (Serwer)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app ./

EXPOSE 5041
EXPOSE 7262

ENV ASPNETCORE_URLS="https://+:7262;http://+:5041" \
    ASPNETCORE_ENVIRONMENT="Development"

ENTRYPOINT ["dotnet", "ToDo.dll"]