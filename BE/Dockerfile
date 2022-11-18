#docker build -t dongnguyenhdkt/hr-management-be .
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY HRManagement.sln ./
COPY API/*.csproj ./API/
COPY Business/*.csproj ./Business/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY UnitTest/*.csproj ./UnitTest/

RUN dotnet restore
COPY . .
WORKDIR /src/Business
RUN dotnet build -c Release -o /app

WORKDIR /src/Infrastructure
RUN dotnet build -c Release -o /app

WORKDIR /src/UnitTest
RUN dotnet build -c Release -o /app

WORKDIR /src/API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "API.dll"]