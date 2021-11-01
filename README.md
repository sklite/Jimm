# Jimmy CMS

A simple CMS with CRUD operations with articles and basic JWT-based authentication model.
Impemented in .NET 5.0 Web Api, aligned with CQRS pattern. Solution was implemented with [MediatR library](https://github.com/jbogard/MediatR "MediatR library").

Consists of 4 projects: <br />
<br /> 1. **[JimmyCms](https://github.com/sklite/Jimm/tree/main/JimmyCms/JimmyCms "JimmyCms")** - API application layer, contains controllers, startup configuration settings, view models.

*UsersController* - controller with Login method to authenticate user and obtain access token.
<br /> *ArticlesController* - controller with CRUD operations upon articles. *Get* operations can be accessed without access token.
All requests can be processed both in JSON and XML format. It depends on the request headers negotiation, set **Accept** request header  to **application/xml** or **application/json** respectively
<br />*Dockerfile* - application can run and debug in docker image.

<br /> 2. **[JimmyCms.Domain](https://github.com/sklite/Jimm/tree/main/JimmyCms/JimmyCms.Domain "JimmyCms.Domain")** - Domain layer, contains MediatR messaging models, handlers, pipelines and validations.

*Commands* - in CQRS pattern, are used for updating data or changing the internal state of the system. We've got Create, Update and Delete article commands. AuthenticateCommand is for user authentication and token issuing.
<br />*Queries* - are used for reading data.
<br />*Pipelines*  - are used for processing workflow, ValidationBehaviour is used for data validation. ResponseBehaviour stands for the correct response code return.
<br />*Validators*  - are validating commands and queries.
Everything here is being orchestrated by the MediatR library, so we dont care much about classes binding.

<br /> 3. **[JimmyCms.Infrastructure](https://github.com/sklite/Jimm/tree/main/JimmyCms/JimmyCms.Infrastructure "JimmyCms.Infrastructure")** - Infrastructure with EF context, migrations and models.
<br />*ArticleContext* - standard database manageable by EF. 
<br />PostgreSQL is being used as default database engine here. 

<br /> 4. **[JimmyCms.Tests](https://github.com/sklite/Jimm/tree/main/JimmyCms/JimmyCms.Tests "JimmyCms.Tests")** - Project with unit tests.

------------
In order to see endpoint documentation, navigate to https://localhost:{port}/swagger/index.html after application startup.
<br /> The solution is also provided with [Postman collection](https://github.com/sklite/Jimm/blob/main/Postman/Articles.postman_collection.json "Postman collection") which contains all API request examples.


