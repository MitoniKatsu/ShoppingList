using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SL.Data.Models;
using SL.Domain.Enum;
using SL.Domain.Repositories;
using SL.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Test.ServiceTests
{
    public class ShoppingListServiceTests
    {
        private Mock<IListItemRepository> _mockListItemRepo { get; set; }
        private Mock<IShoppingListRepository> _mockShoppingListRepo { get; set; }
        private Mock<IShoppingListItemRepository> _mockShoppingListItemRepo { get; set; }

        [SetUp]
        public void Setup()
        {
            _mockListItemRepo = new Mock<IListItemRepository>();
            _mockShoppingListRepo = new Mock<IShoppingListRepository>();
            _mockShoppingListItemRepo = new Mock<IShoppingListItemRepository>();
        }

        [Test]
        public async Task TestLoadShoppingList()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListByUserId(shoppingList.UserId)).Returns(shoppingList);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.LoadShoppingList(shoppingList.UserId);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (ShoppingList)response.Value;
            Assert.AreEqual(shoppingList.Id, value.Id);
            Assert.AreEqual(shoppingList.UserId, value.UserId);
            Assert.AreEqual(shoppingList.Items.Count, value.Items.Count);
        }

        [Test]
        public async Task TestLoadShoppingListEmptyShoppingListNull()
        {
            var newShoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListByUserId(newShoppingList.UserId)).Returns((ShoppingList)default);
            _mockShoppingListRepo.Setup(o => o.Create(newShoppingList.UserId)).ReturnsAsync(newShoppingList);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.LoadShoppingList(newShoppingList.UserId);


            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (ShoppingList)response.Value;
            Assert.AreEqual(newShoppingList.Id, value.Id);
            Assert.AreEqual(newShoppingList.UserId, value.UserId);
            Assert.AreEqual(newShoppingList.Items.Count, value.Items.Count);
        }

        [Test]
        public async Task TestLoadShoppingListEmptyUserId()
        {
            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.LoadShoppingList(Guid.Empty);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
            var response = (BadRequestObjectResult)result;
            Assert.AreEqual(400, response.StatusCode);
            var value = (string)response.Value;
            Assert.AreEqual("The provided userId is not valid.", value);
        }

        [Test]
        public async Task TestAddToShoppingListByItemId()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            var items = new List<ListItem>
            {
                new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "new item"
                }
            };

            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = items[0].Id,
                Complete = false
            };

            var updatedShoppingList = shoppingList;
            updatedShoppingList.Items.Add(shoppingListItem);

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns(shoppingList);
            _mockListItemRepo.Setup(o => o.GetList()).Returns(items);
            _mockShoppingListRepo.Setup(o => o.AddToShoppingList(shoppingList.Id, It.IsAny<ShoppingListItem>())).ReturnsAsync(updatedShoppingList);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, items[0].Id);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (ShoppingList)response.Value;
            Assert.AreEqual(updatedShoppingList.Id, value.Id);
            Assert.AreEqual(updatedShoppingList.UserId, value.UserId);
            Assert.AreEqual(updatedShoppingList.Items.Count, value.Items.Count);
            foreach (var item in updatedShoppingList.Items)
            {
                var foundItem = value.Items.FirstOrDefault(o => o.Id == item.Id);
                Assert.NotNull(foundItem);
                Assert.AreEqual(item.ShoppingListId, foundItem.ShoppingListId);
                Assert.AreEqual(item.ListItemId, foundItem.ListItemId);
            }
        }

        [Test]
        public async Task TestAddToShoppingListByItemIdShoppingListNull()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns((ShoppingList)default);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, Guid.NewGuid());

            Assert.NotNull(result);
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [Test]
        public async Task TestAddToShoppingListByInvalidItemId()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns(shoppingList);
            _mockListItemRepo.Setup(o => o.GetList()).Returns(new List<ListItem>());

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, Guid.NewGuid());

            Assert.NotNull(result);
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
            var response = (BadRequestObjectResult)result;
            Assert.AreEqual(400, response.StatusCode);
            var value = (string)response.Value;
            Assert.AreEqual("The provided itemId is invalid.", value);
        }

        [Test]
        public async Task TestAddToShoppingListByitemName()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            var items = new List<ListItem>
            {
                new ListItem
                {
                    Id = Guid.NewGuid(),
                    Name = "new item"
                }
            };

            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ShoppingListId = shoppingList.Id,
                ListItemId = items[0].Id,
                Complete = false
            };

            var updatedShoppingList = shoppingList;
            updatedShoppingList.Items.Add(shoppingListItem);

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns(shoppingList);
            _mockListItemRepo.Setup(o => o.GetList()).Returns(items);
            _mockShoppingListRepo.Setup(o => o.AddToShoppingList(shoppingList.Id, It.IsAny<ShoppingListItem>())).ReturnsAsync(updatedShoppingList);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, items[0].Name);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (ShoppingList)response.Value;
            Assert.AreEqual(updatedShoppingList.Id, value.Id);
            Assert.AreEqual(updatedShoppingList.UserId, value.UserId);
            Assert.AreEqual(updatedShoppingList.Items.Count, value.Items.Count);
            foreach (var item in updatedShoppingList.Items)
            {
                var foundItem = value.Items.FirstOrDefault(o => o.Id == item.Id);
                Assert.NotNull(foundItem);
                Assert.AreEqual(item.ShoppingListId, foundItem.ShoppingListId);
                Assert.AreEqual(item.ListItemId, foundItem.ListItemId);
            }
        }

        [Test]
        public async Task TestAddToShoppingListByItemNameShoppingListNull()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns((ShoppingList)default);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, Guid.NewGuid());

            Assert.NotNull(result);
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [Test]
        public async Task TestAddToShoppingListByInvalidItemName()
        {
            var shoppingList = new ShoppingList
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Items = new List<ShoppingListItem>()
            };

            _mockShoppingListRepo.Setup(o => o.GetShoppingListById(shoppingList.Id)).Returns(shoppingList);
            _mockListItemRepo.Setup(o => o.GetList()).Returns(new List<ListItem>());

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.AddToShoppingList(shoppingList.Id, string.Empty);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
            var response = (BadRequestObjectResult)result;
            Assert.AreEqual(400, response.StatusCode);
            var value = (string)response.Value;
            Assert.AreEqual("The provided item name cannot be empty.", value);
        }

        [Test]
        public async Task TestUpdateShoppingListItemDelete()
        {
            _mockShoppingListItemRepo.Setup(o => o.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.UpdateShoppingListItem(Guid.NewGuid(), ShoppingListItemAction.DELETE);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkResult), result.GetType());
            var response = (OkResult)result;
            Assert.AreEqual(200, response.StatusCode);
        }

        [Test]
        public async Task TestUpdateShoppingListItemDeleteThrowsKeyNotFound()
        {
            _mockShoppingListItemRepo.Setup(o => o.Delete(It.IsAny<Guid>())).Throws(new KeyNotFoundException());

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.UpdateShoppingListItem(Guid.NewGuid(), ShoppingListItemAction.DELETE);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [Test]
        public async Task TestUpdateShoppingListItemToggleComplete()
        {
            var shoppingListItem = new ShoppingListItem
            {
                Id = Guid.NewGuid(),
                ListItemId = Guid.NewGuid(),
                ShoppingListId = Guid.NewGuid(),
                Complete = false
            };

            var updatedShoppingListItem = shoppingListItem;
            updatedShoppingListItem.Complete = !updatedShoppingListItem.Complete;
            _mockShoppingListItemRepo.Setup(o => o.ToggleComplete(It.IsAny<Guid>())).ReturnsAsync(updatedShoppingListItem);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.UpdateShoppingListItem(Guid.NewGuid(), ShoppingListItemAction.TOGGLE_COMPLETE);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (ShoppingListItem)response.Value;
            Assert.AreEqual(updatedShoppingListItem.Id, value.Id);
            Assert.AreEqual(updatedShoppingListItem.ListItemId, value.ListItemId);
            Assert.AreEqual(updatedShoppingListItem.ShoppingListId, value.ShoppingListId);
            Assert.AreEqual(updatedShoppingListItem.Complete, value.Complete);
        }

        [Test]
        public async Task TestUpdateShoppingListItemToggleCompleteThrowsKeyNotFound()
        {
            _mockShoppingListItemRepo.Setup(o => o.ToggleComplete(It.IsAny<Guid>())).Throws(new KeyNotFoundException());

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = await service.UpdateShoppingListItem(Guid.NewGuid(), ShoppingListItemAction.TOGGLE_COMPLETE);

            Assert.NotNull(result);
            Assert.AreEqual(typeof(NotFoundResult), result.GetType());
        }

        [Test]
        public void TestgetAutoCompleteList()
        {
            var items = new List<ListItem>
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

            _mockListItemRepo.Setup(o => o.GetList()).Returns(items);

            var service = new ShoppingListService(_mockListItemRepo.Object, _mockShoppingListRepo.Object, _mockShoppingListItemRepo.Object);

            var result = service.GetAutoCompleteList();

            Assert.NotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            var response = (OkObjectResult)result;
            Assert.AreEqual(200, response.StatusCode);
            var value = (List<ListItem>)response.Value;
            Assert.AreEqual(items.Count, value.Count);
            foreach (var item in items)
            {
                var foundItem = value.FirstOrDefault(o => o.Id == item.Id);
                Assert.NotNull(foundItem);
                Assert.AreEqual(item.Name, foundItem.Name);
            }
        }
    }
}
