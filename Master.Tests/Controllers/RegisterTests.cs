using System;
using NUnit;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Master;
using Master.Models;
using Master.Services;
using Master.Contexts;
using Master.Controllers;
using Master.Repositories;
using Master.Interfaces.Models;
using Master.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

namespace Tests.Controllers
{
    [TestFixture]
    public class RegisterTests
    {
        RegisterController Controller;
        ContractorAccountRepository ContractorAccountRepository;
        IContractorProfileRepository ContractorProfileRepository;
        TokenGenerator TokenGenerator;
        ContractorAccount TrueContractor;
        ContractorAccount FalseEmailContractor;
        ContractorAccount ExistingContractor;        

        public RegisterTests()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            TokenGenerator = new TokenGenerator(config);
            Controller = new RegisterController(ContractorAccountRepository, ContractorProfileRepository, TokenGenerator);
        }

        [OneTimeSetUp]
        public void SetupContractorAccounts()
        {
            TrueContractor = new ContractorAccount
            {
                EmailAddress = "johndoe@example.com",
                Password = "IAmAContractor",
                FirstName = "James",
                LastName = "Bond"
            };
            
            FalseEmailContractor = new ContractorAccount
            {
                EmailAddress = "agsdzs",
                Password = "IAmAContractor",
                FirstName = "Jason",
                LastName = "Bourne"
            };

            ExistingContractor = new ContractorAccount
            {
                EmailAddress = "bourneCoder@example.com",
                Password = "TestPassword",
                FirstName = "Jason",
                LastName = "Bourne"
            };
        }

        [OneTimeTearDown]
        public void RemoveSetupAccount()
        {
            DissertationContext dbContext = new DissertationContext();
            ContractorAccountRepository repo = new ContractorAccountRepository(dbContext);
            repo.DeleteContractorAccount(TrueContractor.EmailAddress);
        }

        [Test]
        public void TrueContractorAccount()
        {
            IActionResult actualResult = Controller.RegisterContractor(TrueContractor);
            var resultContent = actualResult as OkObjectResult;

            Assert.IsNotNull(resultContent);
            Assert.IsInstanceOf(typeof(OkObjectResult), actualResult);
            Assert.AreEqual(200, resultContent.StatusCode);
        }
        
        
        [Test]
        public void FalseEmailContractorAccount()
        {
            IActionResult actualResult = Controller.RegisterContractor(FalseEmailContractor);
            var resultContent = actualResult as BadRequestObjectResult;

            Assert.IsNotNull(resultContent);
            Assert.AreEqual(400, resultContent.StatusCode);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actualResult);
        }
        
        [Test]
        public void ExisitingContractorAccount()
        {
            IActionResult actualResult = Controller.RegisterContractor(ExistingContractor);
            var resultContent = actualResult as BadRequestObjectResult;

            Assert.IsNotNull(resultContent);
            Assert.AreEqual(400, resultContent.StatusCode);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actualResult);
        }
    }
}