#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProjectsNetwork.Data/ProjectsNetwork.Data.csproj", "ProjectsNetwork.Data/"]
RUN dotnet restore "ProjectsNetwork.Data/ProjectsNetwork.Data.csproj"
COPY . .
WORKDIR "/src/ProjectsNetwork.Data"
RUN dotnet build "ProjectsNetwork.Data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectsNetwork.Data.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectsNetwork.Data.dll"]