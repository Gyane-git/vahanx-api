FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["VahanX.Api/VahanX.Api.csproj", "VahanX.Api/"]
COPY ["VahanX.Application/VahanX.Application.csproj", "VahanX.Application/"]
COPY ["VahanX.Domain/VahanX.Domain.csproj", "VahanX.Domain/"]
COPY ["VahanX.Infrastructure/VahanX.Infrastructure.csproj", "VahanX.Infrastructure/"]

RUN dotnet restore "VahanX.Api/VahanX.Api.csproj"

COPY . .
WORKDIR "/src/VahanX.Api"
RUN dotnet build "VahanX.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VahanX.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VahanX.Api.dll"]
