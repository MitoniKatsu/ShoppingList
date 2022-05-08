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
    public class ShoppingListItemRepositoryTests
    {
        private Mock<IShoppingContext> _mockContext { get; set; }

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IShoppingContext>();
        }

        [Test]
        public async Task TestShoppingListItemCreate()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem>());

            var result = await repo.Create(shoppingList.Id, listItem.Id);

            Assert.AreEqual(shoppingList.Id, result.ShoppingListId);
            Assert.AreEqual(listItem.Id, result.ListItemId);
            Assert.False(result.Complete);
        }

        [Test]
        public void TestShoppingListItemCreateShoppingListNotFound()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList>());
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem>());

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.Create(shoppingList.Id, listItem.Id));

            Assert.AreEqual("ShoppingList with the provided Id cannot be found.", result.Message);
        }

        [Test]
        public void TestShoppingListItemCreateListItemNotFound()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem>());
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem>());

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.Create(shoppingList.Id, listItem.Id));

            Assert.AreEqual("ListItem with the provided Id cannot be found.", result.Message);
        }

        [Test]
        public async Task TestShoppingListItemToggleCompleteToTrue()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = listItem.Id,
                Complete = false
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem> { shoppingListItem });

            var result = await repo.ToggleComplete(shoppingListItem.Id);

            Assert.AreEqual(shoppingListItem.Id, result.Id);
            Assert.AreEqual(shoppingList.Id, result.ShoppingListId);
            Assert.AreEqual(listItem.Id, result.ListItemId);
            Assert.True(result.Complete);
        }

        [Test]
        public async Task TestShoppingListItemToggleCompleteToFalse()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = listItem.Id,
                Complete = true
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem> { shoppingListItem });

            var result = await repo.ToggleComplete(shoppingListItem.Id);

            Assert.AreEqual(shoppingListItem.Id, result.Id);
            Assert.AreEqual(shoppingList.Id, result.ShoppingListId);
            Assert.AreEqual(listItem.Id, result.ListItemId);
            Assert.False(result.Complete);
        }

        [Test]
        public void TestShoppingListItemToggleCompleteShoppingListItemNotFound()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = listItem.Id,
                Complete = false
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem> { shoppingListItem });

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.ToggleComplete(Guid.NewGuid()));

            Assert.AreEqual("ShoppingListItem with the provided Id cannot be found.", result.Message);

        }

        [Test]
        public async Task TestShoppingListItemDelete()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = listItem.Id,
                Complete = true
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem> { shoppingListItem });

            await repo.Delete(shoppingListItem.Id);

            try
            {
                _mockContext.Verify(o => o.ShoppingListItems.Remove(It.IsAny<ShoppingListItem>()), Times.Once);
                _mockContext.Verify(o => o.SaveChangesAsync(default), Times.Once);
            }
            catch (MockException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void TestShoppingListItemToggleDeleteShoppingListItemNotFound()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid()
            };
            var listItem = new ListItem
            {
                Id = Guid.NewGuid()
            };
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = listItem.Id,
                Complete = true
            };
            var repo = new ShoppingListItemRepository(_mockContext.Object);

            _mockContext.Setup(o => o.ShoppingLists).ReturnsDbSet(new List<ShoppingList> { shoppingList });
            _mockContext.Setup(o => o.ListItems).ReturnsDbSet(new List<ListItem> { listItem });
            _mockContext.Setup(o => o.ShoppingListItems).ReturnsDbSet(new List<ShoppingListItem> { shoppingListItem });

            try
            {
                _mockContext.Verify(o => o.ShoppingListItems.Remove(It.IsAny<ShoppingListItem>()), Times.Never);
                _mockContext.Verify(o => o.SaveChangesAsync(default), Times.Never);
            }
            catch (MockException ex)
            {
                Assert.Fail(ex.Message);
            }

            var result = Assert.ThrowsAsync<KeyNotFoundException>(async () => await repo.Delete(Guid.NewGuid()));

            Assert.AreEqual("ShoppingListItem with the provided Id cannot be found.", result.Message);

        }
    }
}
