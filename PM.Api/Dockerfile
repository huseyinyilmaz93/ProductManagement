#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PM.Api/PM.Api.csproj", "PM.Api/"]
RUN dotnet restore "PM.Api/PM.Api.csproj"
COPY . .
WORKDIR "/src/PM.Api"
RUN dotnet build "PM.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PM.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PM.Api.dll"]