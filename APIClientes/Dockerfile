# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY APIClientes/*.csproj ./APIClientes/
RUN dotnet restore ./APIClientes/APIClientes.csproj

COPY APIClientes/. ./APIClientes/
RUN dotnet publish ./APIClientes/APIClientes.csproj -c Release -o /app/publish

# Etapa final (imagen liviana solo con el runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "APIClientes.dll"]