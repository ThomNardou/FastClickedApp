#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Choisis l'image qui sera utilisé 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Définis le répertoire de travail pour l'image
WORKDIR /app

# Ouvre les ports
EXPOSE 80
EXPOSE 443

# Contruis le conteneur à partir de l'image 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Définis le répértoir de travail pour contruire le projet 
WORKDIR /src

# Copie les fichier csproj dans le dossier "FastClieckWebAppMVC"
COPY ["FastClieckWebAppMVC/FastClieckWebAppMVC.csproj", "FastClieckWebAppMVC/"]

# Execute la commande pour restaurer les dépendences du projet 
RUN dotnet restore "FastClieckWebAppMVC/FastClieckWebAppMVC.csproj"

# Copie tout les fichier 
COPY . .

# Change de répéroire de travail
WORKDIR "/src/FastClieckWebAppMVC"

# regénère le projet et l’ensemble de ses dépendances
RUN dotnet build "FastClieckWebAppMVC.csproj" -c Release -o /app/build

FROM build AS publish

# Publie l’application et ses dépendances dans un dossier
RUN dotnet publish "FastClieckWebAppMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

# Change le répértoir de travail 
WORKDIR /app

# Copie les fichiers publiés
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastClieckWebAppMVC.dll"]