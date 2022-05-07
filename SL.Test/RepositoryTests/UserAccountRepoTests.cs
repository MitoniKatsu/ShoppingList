using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SL.Data;
using SL.Data.Models;
using SL.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Test.RepositoryTests
{
    public class UserAccountRepoTests
    {
        private Mock<IShoppingContext> _mockContext { get; set; } 

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IShoppingContext>();
        }

        [Test]
        public void TestUserAccountgetList()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = repo.GetList();

            Assert.NotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            foreach (var a in expected)
            {
                var acct = result.FirstOrDefault(o => o.Id.Equals(a.Id));
                Assert.NotNull(acct);
                Assert.AreEqual(a.Name, acct.Name);
                Assert.AreEqual(a.Email, acct.Email);
                Assert.AreEqual(a.CreatedOn, acct.CreatedOn);
                Assert.AreEqual(a.LastLogin, acct.LastLogin);
            }
        }

        [Test]
        public void TestUserAccountByIdFound()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = repo.GetUserAccountById(expected[1].Id);

            Assert.NotNull(result);
            Assert.AreEqual(expected[1].Id, result.Id);
            Assert.AreEqual(expected[1].Name, result.Name);
            Assert.AreEqual(expected[1].Email, result.Email);
            Assert.AreEqual(expected[1].CreatedOn, result.CreatedOn);
            Assert.AreEqual(expected[1].LastLogin, result.LastLogin);
        }

        [Test]
        public void TestUserAccountByIdNotFound()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = repo.GetUserAccountById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Test]
        public void TestUserAccountByEmailFound()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = repo.GetUserAccountByEmail(expected[1].Email);

            Assert.NotNull(result);
            Assert.AreEqual(expected[1].Id, result.Id);
            Assert.AreEqual(expected[1].Name, result.Name);
            Assert.AreEqual(expected[1].Email, result.Email);
            Assert.AreEqual(expected[1].CreatedOn, result.CreatedOn);
            Assert.AreEqual(expected[1].LastLogin, result.LastLogin);
        }

        [Test]
        public void TestUserAccountByEmailNotFound()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = repo.GetUserAccountByEmail("test@test.com");

            Assert.Null(result);
        }

        [Test]
        public void TestUserAccountByEmailEmptyThrowsException()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = Assert.Throws<InvalidOperationException>(() => repo.GetUserAccountByEmail(""));

            Assert.AreEqual("Unable to get user by email. Provided email is empty.", result.Message);
        }

        [Test]
        public void TestUserAccountByEmailNullThrowsException()
        {
            var expected = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(expected);

            var repo = new UserAccountRepository(_mockContext.Object);

            var result = Assert.Throws<InvalidOperationException>(() => repo.GetUserAccountByEmail(null));

            Assert.AreEqual("Unable to get user by email. Provided email is null.", result.Message);
        }

        [Test]
        public async Task TestUserAccountCreate()
        {
            var name = "new user";
            var email = "new@user.com";

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(new List<UserAccount>());
            _mockContext.Setup(o => o.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            var repo = new UserAccountRepository(_mockContext.Object);

            var newUser = await repo.Create(name, email);

            Assert.AreEqual(name, newUser.Name);
            Assert.AreEqual(email, newUser.Email);
            Assert.AreEqual(newUser.CreatedOn, newUser.LastLogin);
            Assert.AreEqual(Guid.Empty, newUser.Id);
        }

        [Test]
        public async Task TestUserAccountUpdateLastLogin()
        {
            var existing = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(existing);
            _mockContext.Setup(o => o.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            var repo = new UserAccountRepository(_mockContext.Object);

            var oldLastLogin = existing[2].LastLogin;
            var result = await repo.UpdateLastLogin(existing[2]);

            Assert.NotNull(result);
            Assert.AreEqual(existing[2].Id, result.Id);
            Assert.True(oldLastLogin < result.LastLogin);
        }

        [Test]
        public void TestUserAccountUpdateLastLoginUpdatedAccountNull()
        {
            var existing = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(existing);
            _mockContext.Setup(o => o.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            var repo = new UserAccountRepository(_mockContext.Object);

            var result = Assert.ThrowsAsync<InvalidOperationException>(async () => await repo.UpdateLastLogin(null));

            Assert.AreEqual("Unable to update last login. Provided account is null.", result.Message);
        }

        public void TestUserAccountUpdateLastLoginAccountNotFound()
        {
            var existing = new List<UserAccount>
            {
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user1",
                    Email = "user1@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user2",
                    Email = "user2@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                },
                new UserAccount
                {
                    Id = Guid.NewGuid(),
                    Name = "user3",
                    Email = "user3@gmail.com",
                    CreatedOn = DateTime.UtcNow.AddHours(-6),
                    LastLogin = DateTime.UtcNow.AddHours(-6)
                }
            };

            _mockContext.Setup(o => o.UserAccounts).ReturnsDbSet(new List<UserAccount>());
            _mockContext.Setup(o => o.SaveChangesAsync(default)).Returns(Task.FromResult(1));
            var repo = new UserAccountRepository(_mockContext.Object);

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.UpdateLastLogin(null));

            Assert.AreEqual("Unable to update last login. Provided account not found.", result.Message);
        }
    }
}
