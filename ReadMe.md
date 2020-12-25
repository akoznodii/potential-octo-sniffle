## Payment Gateway API

### Legend

We would like to build a payment gateway, an API based application that will allow a merchant to offer a
way for their shoppers to pay for their product.
Processing a payment online involves multiple steps and entities:

* Shopper: Individual who is buying the product online.
* Merchant: The seller of the product. For example, Apple or Amazon.
* Payment Gateway: Responsible for validating requests, storing card information and forwarding
payment requests and accepting payment responses to and from the acquiring bank.
* Acquiring Bank (Payment Processor): Allows us to do the actual retrieval of money from the shopper’s card and payout to the
merchant. It also performs some validation of the card information and then sends the payment details
to the appropriate 3rd party organization for processing


### Assumptions
 
 The following assumptions made:
 
 * Acquiring Bank (a.k.a Payment Processor) processes payments asynchronously (payment processing time may exceed request timeout), so Payment Processor (PP) sends notifications back to integrating partner (Payment Gateway) with the latest status of a particular payment. Payment Processor sends POST request with JSON payload that have Transaction id and status fields. Payment Gateway implements a webhook endpoint to receive and handle payment notifications.   
 * Payment Gateway also requires its Merchants to implement a webhook endpoint to receive the payment processing result. Additionally, Payment Gateway implements an API endpoint that allows Merchants to get payment details. So Merchants have push-based and pull-based approaches to get information regarding a particular payment.
 * Payment Gateway supports only a few credit card types, few currencies, and there is a limit of the maximum amount.


### Code Repository Structure
 
 * src folder contains Payment Gateway source code
 * tests folder contains Payment Gateway tests
 * mocks folder contains mocked implementation of Payment Processor and Merchant APIs

### Architecture Overview

I've used the Onion/Clean/Hexagonal approach to implement Payment Gateway. Basically, it consists of 3 projects:

* Core - contains domain models (anemic), business code is encapsulated in request/event handlers (a.k.a UseCases), domain transfer objects (DTO) are called as incoming/outgoing request/responses. Core project depends only on 4 packages:
  * MediatR - implements a meditator behavior design pattern and aims to reduce coupling between components and provide the ability to write code that follows S.O. principles from SOLID.
  * Microsoft.Extensions.Options - out of box configuration abstraction
  * Microsoft.Extensions.Logging - out of box logging abstraction.
  * FluentValidation - used for implementing validators for incoming DTOs.
* Infrastructure - location of all third-party adapters that implement port interfaces from Core project. It contains repository implementation (EFCore), payment processor adapter, and merchant notifier adapter.
* Api - an entry point project, implements web API. 

#### Developer Notes
* Payment Gateway depends on an SQL server that is used as the primary data storage to keep payment details, merchants' information, and API client settings.
* Solution uses in-process event notifications (a feature from MediatR package) which is bad for the production environment since it doesn't provide notifications availability in case of any issues. Event notifications should be moved to the out-of-process message broker.
* Payment Gateway API implements api-key authentication which is also not good for the production environment. HMAC secret key authentication is a better option for this case but I had a lack of time to implement it.

### How to run Payment Gateway API locally

All solution dependencies and mocked APIs (Merchant and PaymentProcessor) can be run inside Docker containers. Please follow instructions:

1. Export local development certificate to PFX file and trust it

```
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p PG12345!
dotnet dev-certs https --trust
```

2. Run docker compose command inside the root repository folder via CMD tool:

```
docker-compose -f docker-compose.deps.yml -f docker-compose.services.yml up
```

It will download SQL server container, build mocked API containers and run them.

3. Build and run PaymentGateway.Api project, you should be redirected to Swagger documentation page, by default PaymentGateway will create a database and seed it with a test data (Register API clients and a demo merchant account)

### Demo and Test Cases

Pre-requirements
* Run the docker-compose command 
* Run Checkout.PaymentGateway.Api. 
You will be navigated to Swagger documentation page (https://localhost:9101/swagger/index.html)
Mocked APIs should be available:
* Mocked PaymentProcessor API: https://localhost:9201/swagger/index.html
* Mocked Merchant API: https://localhost:9301/swagger/index.html

Now you can use PaymentGateway API Swagger doc page to send HTTP API requests.

Authenticate your request with test Merchant account api key:

```
API KEY: g6-pmcy3f47xa2bv
```

Use the endpoint Payments POST /v1/payments to initiate payment processing
Example of payload:

```
{
  "amount": 150,
  "currency": "EUR",
  "cardType": "VISA",
  "cardNumber": "4929945484769084",
  "expiryMonth": 2,
  "expiryYear": 2021,
  "cvvNumber": "129"
}
```
You should receive Payment id in the response. You can use it in the endpoint Payments GET /v1/payments/{id} to get payment details.

Checkout the docker-compose CMD output, you should be able to see logged messages about this particular payment processing:

```
checkout.paymentprocessor.mock_1  | info: Checkout.PaymentProcessorMock.Api.Controllers.PaymentController[0]
checkout.paymentprocessor.mock_1  |       Processing payment request: 09700f56aa6c448079d608d8a8ec5f9e. It may take up to 5 seconds...
checkout.paymentprocessor.mock_1  | info: Checkout.PaymentProcessorMock.Api.Controllers.PaymentController[0]
checkout.paymentprocessor.mock_1  |       Complete processing payment request: 09700f56aa6c448079d608d8a8ec5f9e with APPROVED result
checkout.paymentprocessor.mock_1  | info: Checkout.PaymentProcessorMock.Api.Controllers.PaymentController[0]
checkout.paymentprocessor.mock_1  |       Invoking webhook url: http://host.docker.internal:9100/webhooks/fakepp/v1/notifications
checkout.merchant.mock_1          | info: Checkout.MerchantMock.Api.Controllers.NotificationsController[0]
checkout.merchant.mock_1          |       Received notification from payment gateway. PaymentId: 09700f56-aa6c-4480-79d6-08d8a8ec5f9e; Status: Approved; TransactionId: d8c10cea21b04fabb27a7f67275f7113
```

Mocked api will log incoming requests from Payment Gateway.

#### Test Cases

Mocked PaymentProcessor API responds differently by inspecting CVV value of a particular payment

|CVV   |Processor Status   |Gateway Status|
|---|---|---|
|666   |FRAUD   |Declined   |
|403   |DECLINED   |Declined   |
|400   |INVALID_DATA   |Declined   |
|other   |APPROVED   |Approved   |

#### Areas to be Improved

* Use Retry Policy with Circuit Breaker when sending HTTP requests to PaymentProcessor API
* Use Retry Policy when handling webhook callbacks from PaymentProcessor API. It will help to handle a race condition issue with updating the status of a particular payment.
* Replace in-process event notification with out-of-process message broker
* Apply better authentication scheme


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
