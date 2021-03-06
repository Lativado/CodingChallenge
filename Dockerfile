FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5003
EXPOSE 5004

ENV ASPNETCORE_URLS=http://+:5003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CodingChallenge.csproj", "./"]
RUN dotnet restore "CodingChallenge.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CodingChallenge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodingChallenge.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodingChallenge.dll"]
