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
    public class ShoppingListRepositoryTests
    {
        private Mock<IShoppingContext> _mockContext { get; set; }

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IShoppingContext>();
        }

        [Test]
        public void TestShoppingListGetByUserid()
        {
            var shoppingLists = new List<ShoppingList>
            {
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                },
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                },
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                }
            };

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(shoppingLists);

            var repo = new ShoppingListRepository(_mockContext.Object);

            var shoppingList = repo.GetShoppingListByUserId(shoppingLists[1].UserId);

            Assert.NotNull(shoppingList);
            Assert.AreEqual(shoppingLists[1].Id, shoppingList.Id);
            Assert.AreEqual(shoppingLists[1].UserId, shoppingList.UserId);
            Assert.AreEqual(shoppingLists[1].Items.Count, shoppingList.Items.Count);
        }

        [Test]
        public void TestShoppingListGetById()
        {
            var shoppingLists = new List<ShoppingList>
            {
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                },
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                },
                new ShoppingList
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Items = new List<ShoppingListItem>()
                }
            };

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(shoppingLists);

            var repo = new ShoppingListRepository(_mockContext.Object);

            var shoppingList = repo.GetShoppingListById(shoppingLists[1].Id);

            Assert.NotNull(shoppingList);
            Assert.AreEqual(shoppingLists[1].Id, shoppingList.Id);
            Assert.AreEqual(shoppingLists[1].UserId, shoppingList.UserId);
            Assert.AreEqual(shoppingLists[1].Items.Count, shoppingList.Items.Count);
        }

        [Test]
        public async Task TestShoppingListCreate()
        {
            var expected = new ShoppingList
            {
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList>());

            var repo = new ShoppingListRepository(_mockContext.Object);

            var shoppingList = await repo.Create(expected.UserId);

            Assert.NotNull(shoppingList);
            Assert.AreEqual(expected.UserId, shoppingList.UserId);
            Assert.AreEqual(expected.Items.Count, shoppingList.Items.Count);
        }

        [Test]
        public async Task TestShoppingListAddToShoppingList()
        {
            var initialList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            var newItem = new ShoppingListItem
            {
                ListItemId = Guid.NewGuid(),
                ShoppingListId = initialList.Id
            };

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { initialList });

            var initialCount = initialList.Items.Count();
            var repo = new ShoppingListRepository(_mockContext.Object);

            var shoppingList = await repo.AddToShoppingList(initialList.Id, newItem);

            Assert.NotNull(shoppingList);
            Assert.AreEqual(initialList.Id, shoppingList.Id);
            Assert.AreEqual(initialCount + 1, shoppingList.Items.Count);
            Assert.AreEqual(newItem.ListItemId, shoppingList.Items.ToList()[0].ListItemId);
            Assert.AreEqual(newItem.ShoppingListId, shoppingList.Items.ToList()[0].ShoppingListId);
        }

        [Test]
        public void TestShoppingListAddToShoppingListShoppingListNotFound()
        {
            var newItem = new ShoppingListItem
            {
                ListItemId = Guid.NewGuid(),
                ShoppingListId = Guid.NewGuid()
            };

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList>());

            var repo = new ShoppingListRepository(_mockContext.Object);

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.AddToShoppingList(newItem.ShoppingListId, newItem));

            Assert.AreEqual("A shopping list with the provided id cannot be found.", result.Message);
        }
    }
}
