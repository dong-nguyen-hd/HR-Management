FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /usr/src/app/be

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /usr/src/app/be
COPY --from=build-env /usr/src/app/be/out .

ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]