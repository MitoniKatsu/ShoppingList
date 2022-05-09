using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SL.Data.Models;
using SL.Domain.DTO;
using SL.Domain.Repositories;
using SL.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Test.ServiceTests
{
    public class UserAccountServiceTests
    {
        private Mock<IUserAccountRepository> _mockRepo { get; set; }

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserAccountRepository>();
        }

        [Test]
        public async Task TestLoginWithId()
        {
            var now = DateTime.UtcNow;
            var expectedAccount = new UserAccount
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "email@test.com",
                CreatedOn = now,
                LastLogin = now
            };

            var updatedAccount = expectedAccount;
            updatedAccount.LastLogin = updatedAccount.LastLogin.AddHours(1);

            _mockRepo.Setup(o => o.GetUserAccountById(expectedAccount.Id)).Returns(expectedAccount);
            _mockRepo.Setup(o => o.UpdateLastLogin(expectedAccount)).ReturnsAsync(updatedAccount);

            var service = new UserAccountService(_mockRepo.Object);

            var result = await service.LoginWithId(expectedAccount.Id);

            
            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (UserAccountDto)response.Value;
            Assert.AreEqual(updatedAccount.Id, value.Id);
            Assert.AreEqual(updatedAccount.Name, value.Name);
            Assert.AreEqual(updatedAccount.Email, value.Email);
            Assert.AreEqual(updatedAccount.CreatedOn < updatedAccount.LastLogin, value.Returning);

        }

        [Test]
        public async Task TestLoginWithIdUserIdInvalid()
        {
            var now = DateTime.UtcNow;
            var expectedAccount = new UserAccount
            {
                Id = Guid.Empty,
                Name = "test",
                Email = "email@test.com",
                CreatedOn = now,
                LastLogin = now
            };

            var service = new UserAccountService(_mockRepo.Object);

            var result = await service.LoginWithId(expectedAccount.Id);


            Assert.NotNull(result);
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
            var response = (BadRequestObjectResult)result;
            Assert.AreEqual(400, response.StatusCode);
            var value = (string)response.Value;
            Assert.AreEqual("The provided userId is not valid.", value);

        }

        [Test]
        public async Task TestLoginWithIduserAccountNotFound()
        {
            var now = DateTime.UtcNow;
            var expectedAccount = new UserAccount
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "email@test.com",
                CreatedOn = now,
                LastLogin = now
            };

            _mockRepo.Setup(o => o.GetUserAccountById(expectedAccount.Id)).Returns((UserAccount)default);

            var service = new UserAccountService(_mockRepo.Object);

            var result = await service.LoginWithId(expectedAccount.Id);


            Assert.NotNull(result);
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }
    }
}
