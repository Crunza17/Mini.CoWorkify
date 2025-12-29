FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Mini.CoWorkify.API/Mini.CoWorkify.API.csproj", "Mini.CoWorkify.API/"]
COPY ["Mini.CoWorkify.Application/Mini.CoWorkify.Application.csproj", "Mini.CoWorkify.Application/"]
COPY ["Mini.CoWorkify.Domain/Mini.CoWorkify.Domain.csproj", "Mini.CoWorkify.Domain/"]
COPY ["Mini.CoWorkify.Infrastructure/Mini.CoWorkify.Infrastructure.csproj", "Mini.CoWorkify.Infrastructure/"]

RUN dotnet restore "Mini.CoWorkify.API/Mini.CoWorkify.API.csproj"

COPY . .

WORKDIR "/src/Mini.CoWorkify.API"
RUN dotnet publish "Mini.CoWorkify.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mini.CoWorkify.API.dll"]