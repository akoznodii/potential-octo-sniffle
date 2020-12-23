### Local Development

Run service dependencies:

```
docker-compose -f docker-compose.deps.yml up
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