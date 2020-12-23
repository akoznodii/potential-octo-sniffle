#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Checkout.PaymentGateway.Api/Checkout.PaymentGateway.Api.csproj", "Checkout.PaymentGateway.Api/"]
COPY ["src/Checkout.PaymentGateway.Core/Checkout.PaymentGateway.Core.csproj", "Checkout.PaymentGateway.Core/"]
COPY ["src/Checkout.PaymentGateway.Infrastructure/Checkout.PaymentGateway.Infrastructure.csproj", "Checkout.PaymentGateway.Infrastructure/"]
RUN dotnet restore "Checkout.PaymentGateway.Api/Checkout.PaymentGateway.Api.csproj"
COPY . .
WORKDIR "/src/Checkout.PaymentGateway.Api"
RUN dotnet build "Checkout.PaymentGateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentGateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentGateway.Api.dll"]
