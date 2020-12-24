### Local Development

Setup local development by running the following commands in CMD on Windows

```
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p PG12345!
dotnet dev-certs https --trust
```

Run PaymentGateway service dependencies:

```
docker-compose -f docker-compose.deps.yml -f docker-compose.services.yml up
```



### Database

Update database:

```
dotnet ef database update -c ApplicationContext -s src\Checkout.PaymentGateway.Api\Checkout.PaymentGateway.Api.csproj -p src\Checkout.PaymentGateway.Infrastructure\Checkout.PaymentGateway.Infrastructure.csproj
```

Add new migration:

```
dotnet ef migrations add <MIGRATION_NAME> -c ApplicationContext -s src\Checkout.PaymentGateway.Api\Checkout.PaymentGateway.Api.csproj -p src\Checkout.PaymentGateway.Infrastructure\Checkout.PaymentGateway.Infrastructure.csproj -o Storage\Migrations
```

### Build

Build docker image:

```
docker build -f src\Checkout.PaymentGateway.Api\Dockerfile .
```