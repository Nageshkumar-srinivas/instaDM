# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY instaDM.csproj ./
RUN dotnet restore

# Copy the rest of the app
COPY . ./

# [Optional] Run build
RUN dotnet build instaDM.csproj --configuration Release --output /app/build

# Run publish to prepare final output
RUN dotnet publish instaDM.csproj --configuration Release --output /app/publish --no-restore

# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "instaDM.dll"]
