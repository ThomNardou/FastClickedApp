#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Choisis l'image qui sera utilis� 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# D�finis le r�pertoire de travail pour l'image
WORKDIR /app

# Ouvre les ports
EXPOSE 80
EXPOSE 443

# Contruis le conteneur � partir de l'image 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# D�finis le r�p�rtoir de travail pour contruire le projet 
WORKDIR /src

# Copie les fichier csproj dans le dossier "FastClieckWebAppMVC"
COPY ["FastClieckWebAppMVC/FastClieckWebAppMVC.csproj", "FastClieckWebAppMVC/"]

# Execute la commande pour restaurer les d�pendences du projet 
RUN dotnet restore "FastClieckWebAppMVC/FastClieckWebAppMVC.csproj"

# Copie tout les fichier 
COPY . .

# Change de r�p�roire de travail
WORKDIR "/src/FastClieckWebAppMVC"

# reg�n�re le projet et l�ensemble de ses d�pendances
RUN dotnet build "FastClieckWebAppMVC.csproj" -c Release -o /app/build

FROM build AS publish

# Publie l�application et ses d�pendances dans un dossier
RUN dotnet publish "FastClieckWebAppMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

# Change le r�p�rtoir de travail 
WORKDIR /app

# Copie les fichiers publi�s
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastClieckWebAppMVC.dll"]