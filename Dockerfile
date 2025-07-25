# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use the .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy built output from build stage
COPY --from=build /app/out ./

# Expose default port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "instaDM.dll"]
