# EntityFrameworkCoreNote
Tips for Creating WebApi with EntityFrameworkCore 

Prerequest: Prepare Sql Server in Docker & run Sql Server in docker by using the following command:    
```
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>' -p 1401:1433 -d mcr.microsoft.com/mssql/server
```
p.s.: the latest sql server asks for strong password combination!!! 

Step 1: Create WebApi project with .NetCore 6 
Install the following 4 packages in Nuget: 
```
Microsoft.EntityFrameworkCore 6.0.8 
Microsoft.EntityFrameworkCore.Tools 6.0.8 
Microsoft.EntityFrameworkCore.Design 6.0.8 (Optional! In fact, when using "Tools", it will include "Design" too)
Microsoft.EntityFrameworkCore.SqlServer 6.0.8 
```
p.s: If you want "Package Manage Console" commands then import EFCore.Tools.    
If you want the CLI tools then import EFCore.Design.    
If you want both then only import EFCore.Tools    

1-1: define required “Model class” first 

1-2: create “DatabaseContext” class, and set up the connection string/settings 
p.s a: the sql connection string in appsetting.json:
```
"ConnectionStrings": { 
    "DefaultConnection": "Data Source=localhost,1401;Persist Security Info=True;Initial Catalog=superherodb;User Id=sa;Password=<putyourpasswordhere>" 
  }, 
```
2023-03-13 update:
if it shows "remote server certificate error", please add "TrustServerCertificate=True" in above connection stringsetting.
 

Then, apply this in program.cs
```
builder.Services.AddDbContext\<DataContext\>(options=> 
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
}); 
```
p.s. b: set up CORS in program.cs (optional for providing access for external use, like Angular frontend)
```
builder.Services.AddCors(options=>options.AddPolicy(name: "SuperHeroOrigins", 
    policy => 
    { 
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader(); 
    })); 
app.UseCors("SuperHeroOrigins"); 
```

1-3:
After setting up the connection string and program.cs, start to create DB migration data:
In terminal, cd project folder, and type the following two commands to initialize table in SqlServer:
```
dotnet ef migrations add initialcreate
dotnet ef database update 
```

1-4: create “Api Controller” with “Model class”.    
if you are using VSCode, you may need to use terminal(command line) to new ApiController, like
```
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator controller -name CustomerController -async -api -m Customer -dc ./Data/ApplicationDataContext -outDir Controllers
```


# Extra points: dotnet command(for VSCode)
```
dotnet new webapi -n MyWebAPICore
dotnet new mvc  
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package --version 1.1.0-msbuild3-final Microsoft.EntityFrameworkCore.Tools 
dotnet restore      
dotnet run
//update local PC's Entity Framework Core tool version
dotnet tool update --global dotnet-ef
```
