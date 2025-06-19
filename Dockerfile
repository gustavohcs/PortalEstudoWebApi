# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia o projeto e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia tudo e define as variáveis de ambiente (como a connection string)
COPY . ./

# Executa a migration
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef database update

# Publica a aplicação
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "PortalEstudoWebApi.dll"]
