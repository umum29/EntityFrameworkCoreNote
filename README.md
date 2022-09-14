# EntityFrameworkCoreNote
Tips for Creating WebApi with EntityFrameworkCore 

Prerequest: Prepare Sql Server in Docker & run Sql Server in docker by using the following command: 
docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>' -p 1401:1433 -d mcr.microsoft.com/mssql/server 
p.s.: the latest sql server asks for strong password combination!!! 

Step 1: Create WebApi project with .NetCore 6 
Install the following 4 packages in Nuget: 
Microsoft.EntityFrameworkCore 6.0.8 
Microsoft.EntityFrameworkCore.Tools 6.0.8 
Microsoft.EntityFrameworkCore.Design 6.0.8 
Microsoft.EntityFrameworkCore.SqlServer 6.0.8 

1-1: define required “Model class” first 

1-2: create “DatabaseContext” class 
In terminal, cd project folder, and type the following two commands to initialize table in SqlServer:
dotnet ef migrations add initialcreate 
dotnet ef database update 

1-3: create “Api Controller” with “Model class” 
p.s a: the sql connection string in appsetting.json: 
"ConnectionStrings": { 
    "DefaultConnection": "Data Source=localhost,1401;Persist Security Info=True;Initial Catalog=superherodb;User Id=sa;Password=<putyourpasswordhere>" 
  }, 

Then, apply this in program.cs 
builder.Services.AddDbContext<DataContext>(options=> 
{ 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
}); 

p.s. b: set up CORS in program.cs 
builder.Services.AddCors(options=>options.AddPolicy(name: "SuperHeroOrigins", 
    policy => 
    { 
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader(); 
    })); 
app.UseCors("SuperHeroOrigins"); 
