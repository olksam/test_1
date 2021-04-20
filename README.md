# Account Management Backend - Level 3

## Developer Notes

### Summary
In this implementation, I did not perform layering (Data Access, Domain, API, etc.) intentionally. This (micro) service is quite simple and complicating the project structure would be superfluous, in my opinion. Nevertheless, abstractions for Database and Repository were introduced to simplify their testing (personally, I write unit tests only for library code, I prefer to cover the final product with integration tests). Swagger annotations and endpoints are omitted intentionally too (no need).

### Database
The database contains two tables. `Transactions` table for storing transactions and `Accounts` table for storing a pre-calculated balance for each account. On the one hand, this slightly slows down the creation of a new transaction, but on the other hand, the speed of obtaining the balance will not decrease significantly with the growth of the database. 

### ORM
I use **Dapper** for performance and query customization. I planned to use **EF Core** to create a transactions, but I had abandoned this idea to not to add unnecessary complexity to the project.

### Cache
`MemoryCache` is used directly without additional abstractions for two reasons:
* I don't want to complicate the project
* `IMemoryCache` is completely suitable for this scenario. 

### Possible improvements
* To improve the performance and responsiveness of the service, transaction creating request can be queued and executed in the background (background worker or separate consumer service). If necessary, number of consumer services can be increased. 
* Separete Databases can be used to segregate read and write operations in order to improve performance
* **Redis** can be used for distributed cache.
* **ElasticSearch** can be used as log store
* HealthCheck can be more complex (check db, cache or broker connection, for example)

---

**Before you get started, please read [this guide](https://www.notion.so/Get-started-with-your-assignment-dade100d93054a6db1036ce294bdaeb6)** that walks you through how to submit your solution and get help.

### Time limit ⏳

Try not to spend more than **3 hours**.

### The challenge 🎯

Your task is to build a backend service that implements this [API specification](api-specification.yml) that defines a set of operations for creating and reading account transactions. You can use [editor.swagger.io](https://editor.swagger.io/) to visualize the spec.

### The focus areas 🔍
- **Use an SQLite database as the service datastore.** We want to see how you design the database schema and SQL queries for working with the service data. Please use [SQLite](https://www.sqlite.org/index.html) as a DB engine.
- **Create a backend service that implements the provided API.** This will involve the following:
  - Handling invalid HTTP requests;
  - Creating new transactions idempotently;
  - Reading historical transactions;
  - Retreiving the current account balance.
  - Fetching information about the maximum transaction volume.
- **Optimize the GET endpoints for speed.** When designing your service, ensure that the GET endpoints remain fast with the database growing in size.
- **Ensure no lost updates.** When submitting a new transaction, make sure no account balance updates are lost. E.g., when having two concurrent requests updating the same account balance.
- **Minimize the number of SQL queries for fetching max transaction volume.** Try to do it with ideally a single SQL query.
- **Organize your code as a set of low-coupled modules**. Avoid duplication and extract re-usable modules where it makes sense, but don't break things apart needlessly. We want to see that you can create a codebase that is easy to maintain.
- **Document your decisions.** Extend this README.md with info about how to run your application along with any hints that will help us review your submission and better understand the decisions you made.

### The provided boilerplate 🗂
* The [service specification](api-specification.yml) in the Open API format.
* Automated tests to validate your solution. To run locally:
  * Install the required test dependencies with `yarn install`.
  * Update the `apiUrl` (where your app will run) in [cypress.json](cypress.json).
  * Run your app.
  * Run the tests with `yarn run test`.

### Before submitting your solution ⚠️
1. Update the `apiUrl` (where your app will run) in [cypress.json](cypress.json).
2. Update the [`build`](package.json#L5) and [`start`](package.json#L6) scripts in [package.json](package.json) that respectively build and run your application. **[See examples](https://www.notion.so/devskills/Backend-78f49bea524148228f29ceb446157474)**.

---

Made by [DevSkills](https://devskills.co). 

How was your experience? **Give us a shout on [Twitter](https://twitter.com/DevSkillsHQ) / [LinkedIn](https://www.linkedin.com/company/devskills)**.
