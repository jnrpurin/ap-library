# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy files and restore
COPY LibraryManagementApp/*.csproj ./LibraryManagementApp/
RUN dotnet restore LibraryManagementApp/LibraryManagementApp.csproj

# Copy code and build
COPY . .
WORKDIR /src/LibraryManagementApp
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
EXPOSE 8081

# Env config
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "LibraryManagementApp.dll"]
