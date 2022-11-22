1. How long did you spend on the coding assignment? What would you add to your solution if you had
   more time?

I spent less than 6 hours on this assignment.
- In a production environment, if I had more time, I would add caching for quote results, so we could use local rates if they are available instead of requesting 3rd-party APIs every time.
- Instead of Console logs, I would use another logging management system (Serilog Sink) like Seq or ELK in a production environment.
- For acceptance tests, I'd use a tool like SpecFlow.
- Checking the possibility to use non-free plans. With the non-free plan, for example via CoinMarketCap, you can fetch all the currency rates like USD, EUR, and etc with just a single HTTP request.

2. What was the most useful feature that was added to the latest version of your language of choice?
Please include a snippet of code that shows how you've used it.

IMO, it was the combination of `nullable reference types`, `records` and `required` keyword. Null-reference exceptions are no longer a concern (most of the time, if you don't bypass it) since you get an error at compile time instead by enabling `nullable reference types`.  With `records`, you can create immutable structures, and with `required`, you can make a property required when instantiating its type and you don't need to require that parameter from constructor anymore.
```csharp
public class Person
{
    public Person(string firstName, string lastName) =>
        (FirstName, LastName) = (firstName, lastName);

    public required string FirstName { get; init; }
    public required string LastName { get; init; }

    public int? Age { get; set; }
}

// Usage
var person = new Person
{
   FirstName = "Fatemeh",
   LastName = "Fattahi" // If you don't set LastName here, it causes a compilation error.
   Age = 32
};
```

3. To begin with, I investigate where the slow down is: CPU, memory, or a 3rd-party service. In our projects, we implement Metrics using Prometheus+Grafana and Tracing using OpenTelemetry+Zipkin but any other APM tools are fine. Initially, we realize we have performance problems somewhere in the application when using these two.
After finding the problematic endpoint/method, we try to track down the underlying issue using dotMemory/dotTrace if we are able to reproduce that problem in our local environment. If not, we run `dotnet-dump`, `dotnet-gcdump`, or `dotnet-trace` on our production environment pods (or on Sidecar containers), and we're copying the outcome (memory dumps) to our local machine and investigate them using `Perfview`, `dotMemory`, `dotTrace`, or `Visual Studio Profiler`.


4. The last thing that I learned was via ["Async Expert"](https://asyncexpert.com) course by Dotnetos academy, and I learn async internals in .NET and how it is managed in .NET Runtime and OS.


5. I think it was a simple task to understand different skill levels of the candidate, including .NET, Design, OOP, and REST principles. We can improve it and extend it a lot.


6.

```json 
{
   "General": {
      "FirstName": "Fatemeh",
      "LastName": "Fattahi",
      "Gender": "Female",
      "NationalId": "0057485214",
      "BirthDetails": {
         "Date": "28/09/1989",
         "Country": "Iran",
         "City": "Babol"
      }
   },
   "Contact": {
      "PhoneNumbers": [ "+989129464843" ],
      "Emails": [ "ftmfattahi@gmail.com" ],
      "LinkedIn": "https://www.linkedin.com/in/fatemehfattahi"
   },
   "Addresses": [
      {
         "Country": "Iran",
         "City": "Tehran",
         "Street": "12m Valiasr Street",
         "PostalCode": "172423698",
         "Type": "Home"
      },
      {
         "Country": "Iran",
         "City": "Tehran",
         "Street": "Bime Street",
         "PostalCode": "8795234631",
         "Type": "Work"
      }
   ],
   "Education": [
      {
         "Degree": "HighSchool",
         "SchoolName": "Efaf School"
      },
      {
         "Degree": "Bachelor",
         "SchoolName": "Mazandaran University of Science and Technology"
      }
   ],
   "Skills": [
      "C# .NET",
      "Software Architecture",
      "SQL/NoSQL",
      "Software Design"
   ]
}
```
