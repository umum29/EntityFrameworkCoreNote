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
dotnet aspnet-codegenerator controller -name CustomerController -async -api -m Customer -dc ApplicationDataContext -outDir Controllers
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
#  How to decide Foreign Key    
As a rule of thumb, you should add a foreign key on the **child table** referencing the parent table. In your case it appears that WorkingHoursDetail is the child table and WorkingHours the parent table.

You can identify the **parent table** by asking **which table can exists on its own without the presence of the other table**. In your case WorkingHours can exists without WorkingHoursDetails (but not the other way around, so WorkingHours appears to be the parent table.

Alternatively you can look at the data flow when inserting new records. If both tables are empty, you should have to **insert a row in WorkingHours first** and then a record in WorkingHoursDetail. This makes WorkingHours the parent table and therefore the foreign key should be added on WorkingHoursDetail referencing WorkingHours.
## Decide Foreign Key under one to one relation   
If both tables can exist indepenently, You can put Foreign key to either one.(but not put it at both tables)    
## Decide Foreign Key under one(or Zero) to many relation    
The foreign key goes on the **"many"** side.    
For example, if a sales_order is associated with **at most one** customer, and **a customer can have zero, one or more sales_order**
Then we put customer_id in the **sales_order**(many side) table, as a reference to the (unique) id column in customer table.    

Please reference **Student.cs** content to make **Foreign Key setting** work in Entity Framework Core.
## Decide many-to-many relation
https://github.com/teddysmithdev/pokemon-review-api/blob/master/PokemonReviewApp/Repository/PokemonRepository.cs shows:    
1.  For many to many, we need to create a "join table" for two one-to-many relation, and also set up the relation in OnModelCreating method of dbContext.cs.
2.  In WebApi, for posting(creating) data, we have to create new data in **"join table"**, instead of a individual table.
3.  In Webapi perspective, you can only insert(create) one data at time, and that's why **"_context.Owners.Where(a => a.Id == ownerId).FirstOrDefault();"** prepare the data before saving into DB.
