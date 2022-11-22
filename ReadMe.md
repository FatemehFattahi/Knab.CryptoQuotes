## Knab.CryptoQuotes
Knab code-challenge about fetching quotes for an specific Cryptocurrency.

### How to run

**1. Simpler way:**

In the root of the project, run this command:

`docker compose up -d`

After that, navigate to `http://localhost:5000/swagger`

**2. Simple Way:**

In the root of the project, run this command:

`dotnet run --project src\Knab.CryptoQuote.Api\Knab.CryptoQuote.Api.csproj`

After that, navigate to `http://localhost:5000/swagger`

### Run the tests
In the root of the project, run this command:

`dotnet test`