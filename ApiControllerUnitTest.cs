using NUnit.Framework;
using EFCoreExample.Models;
using EFCoreExample.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExampleUnitTest;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    //MethodName_StateUnderTest_ExpectedBehavior
    public async Task GetInformation__HttpGet_ReturnInformation()
    {
        //Arrange
        //1.Set up In-Memory DB
        var options = new DbContextOptionsBuilder<CompanyContext>()
            .UseInMemoryDatabase(databaseName: "Company")
            .Options;

        // Insert seed data into the database using one instance of the context
        using (var context = new CompanyContext(options))
        {
            var infos = new Information[]
            {
                new Information { Name = "YogiHosting", License = "XXYY", Revenue = 1000, Establshied = Convert.ToDateTime("2014/06/24") },
                new Information{ Name ="Microsoft", License ="XXXX", Revenue = 1000, Establshied = Convert.ToDateTime("2014/07/14") },
                new Information{ Name ="Google", License ="RRRRR", Revenue = 1000, Establshied = Convert.ToDateTime("2019/06/18") },
                new Information{ Name ="Apple", License ="XADFD", Revenue = 1000, Establshied = Convert.ToDateTime("2022/02/02") },
                new Information{ Name ="SpaceX", License ="##@$", Revenue = 1000, Establshied = Convert.ToDateTime("2030/10/01") }
            };

            context.Information.AddRange(infos);
            context.SaveChanges();
        }

        using (var context = new CompanyContext(options))
        {
            var companyController = new CompanyController(context);
            //Act
            var result = await companyController.GetInformation();
            //var okResult = result.Result as ObjectResult;
            //List<Information> infos = (List<Information>)okResult.Value;
            List<Information> infos = result.Value as List<Information>;
            //Assert
            Assert.AreEqual(5, infos.Count);
        }
    }
}
