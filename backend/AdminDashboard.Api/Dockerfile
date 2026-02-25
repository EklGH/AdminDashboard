# ======== BUILD

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copie csproj :
COPY ["AdminDashboard.Api/AdminDashboard.Api.csproj", "AdminDashboard.Api/"]
COPY ["AdminDashboard.Infrastructure/AdminDashboard.Infrastructure.csproj", "AdminDashboard.Infrastructure/"]
COPY ["AdminDashboard.Domain/AdminDashboard.Domain.csproj", "AdminDashboard.Domain/"]

# Restaure les dépendances :
RUN dotnet restore "AdminDashboard.Api/AdminDashboard.Api.csproj"

# Copie tout le code :
COPY . .

# Build, restore et publish :
WORKDIR "/src/AdminDashboard.Api"
RUN dotnet publish "AdminDashboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false



# ======== RUNTIME

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /src/AdminDashboard.Api

# Copier tout le code
COPY --from=build /app/publish .

# Port de l'application :
EXPOSE 5000

# Démarre l'application :
ENTRYPOINT ["dotnet", "AdminDashboard.Api.dll"]