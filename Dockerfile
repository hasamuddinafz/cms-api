FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY NexusCms.slnx ./
COPY src/NexusCms.Domain/NexusCms.Domain.csproj src/NexusCms.Domain/
COPY src/NexusCms.Application/NexusCms.Application.csproj src/NexusCms.Application/
COPY src/NexusCms.Persistence/NexusCms.Persistence.csproj src/NexusCms.Persistence/
COPY src/NexusCms.Api/NexusCms.Api.csproj src/NexusCms.Api/
RUN dotnet restore src/NexusCms.Api/NexusCms.Api.csproj

COPY . .
RUN dotnet publish src/NexusCms.Api/NexusCms.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "NexusCms.Api.dll"]
