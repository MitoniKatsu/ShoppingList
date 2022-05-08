using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;
using SL.Data;
using SL.Data.Models;
using SL.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SL.Test.RepositoryTests
{
    public class ListItemRepositoryTests
    {
        private Mock<IShoppingContext> _mockContext { get; set; }

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IShoppingContext>();
        }

        [Test]
        public void TestListItemGetList()
        {
            var expected = new List<ListItem>
            {
                new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "item1"
                },
                new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "item2"
                },
                new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "item3"
                }
            };

            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(expected);

            var repo = new ListItemRepository(_mockContext.Object);

            var result = repo.GetList();

            Assert.NotNull(result);
            Assert.AreEqual(expected.Count, result.Count);
            foreach (var a in expected)
            {
                var acct = result.FirstOrDefault(o => o.Id.Equals(a.Id));
                Assert.NotNull(acct);
                Assert.AreEqual(a.Name, acct.Name);
            }
        }
    }
}
