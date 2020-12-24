#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["mocks/Checkout.PaymentProcessorMock.Api/Checkout.PaymentProcessorMock.Api.csproj", "mocks/Checkout.PaymentProcessorMock.Api/"]
RUN dotnet restore "mocks/Checkout.PaymentProcessorMock.Api/Checkout.PaymentProcessorMock.Api.csproj"
COPY . .
WORKDIR "/src/mocks/Checkout.PaymentProcessorMock.Api"
RUN dotnet build "Checkout.PaymentProcessorMock.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.PaymentProcessorMock.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.PaymentProcessorMock.Api.dll"]