# blazor-webapp-auth0
Blazor Web App template using Interactive Auto rendering, using Auth0 for authentication and authorization.

Will try strictly to follow Auth0 documentations and not using any 3rd party packages where possible

## Commands available
### List out available installed dotnet project templates
```
dotnet new list
``` 
### List out the arguments to create blazor web app.
```
dotnet new blazor --help 
```

## Create the blazor app
### Create the sample app - Interactive Auto
```
dotnet new blazor -n Blazor.WebApp.RenderAuto.Auth0 -o samples/int-auto/ -int Auto
```


## References
- [Structuring Dependency Injection In ASP.NET Core The Right Way](https://www.youtube.com/watch?v=tKEF6xaeoig)
- [Creating a C# console application with dependency injection](https://www.youtube.com/watch?v=y4BNWvdgccs)