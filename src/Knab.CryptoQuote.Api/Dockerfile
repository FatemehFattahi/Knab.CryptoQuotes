FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "src/"]
COPY ["Directory.Packages.props", "src/"]
COPY ["src/Knab.CryptoQuote.Api/Knab.CryptoQuote.Api.csproj", "src/Knab.CryptoQuote.Api/"]
COPY ["src/Knab.CryptoQuote.Infrastructure/Knab.CryptoQuote.Infrastructure.csproj", "src/Knab.CryptoQuote.Infrastructure/"]
COPY ["src/Knab.CryptoQuote.Application/Knab.CryptoQuote.Application.csproj", "src/Knab.CryptoQuote.Application/"]
COPY ["src/Knab.CryptoQuote.Domain/Knab.CryptoQuote.Domain.csproj", "src/Knab.CryptoQuote.Domain/"]
RUN dotnet restore "src/Knab.CryptoQuote.Api/Knab.CryptoQuote.Api.csproj"
COPY . .
WORKDIR "/src/src/Knab.CryptoQuote.Api"
RUN dotnet build "Knab.CryptoQuote.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Knab.CryptoQuote.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Knab.CryptoQuote.Api.dll"]
