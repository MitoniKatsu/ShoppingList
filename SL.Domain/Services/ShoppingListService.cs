using Microsoft.AspNetCore.Mvc;
using SL.Data.Models;
using SL.Domain.Enum;
using SL.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.Services
{
    public interface IShoppingListService
    {
        Task<IActionResult> AddToShoppingList(Guid shoppingListId, Guid? itemId, string itemName);
        IActionResult GetAutoCompleteList();
        Task<IActionResult> LoadShoppingList(Guid userId);
        Task<IActionResult> UpdateShoppingListItem(Guid shoppingListItemId, ShoppingListItemAction action);
    }

    public class ShoppingListService : IShoppingListService
    {
        private readonly IListItemRepository _listItemRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IShoppingListItemRepository _shoppingListItemRepository;

        public ShoppingListService(IListItemRepository listItemRepo, IShoppingListRepository shoppingListRepo, IShoppingListItemRepository shoppingListItemRepo)
        {
            _listItemRepository = listItemRepo;
            _shoppingListRepository = shoppingListRepo;
            _shoppingListItemRepository = shoppingListItemRepo;
        }

        public async Task<IActionResult> LoadShoppingList(Guid userId)
        {
            if (userId.Equals(Guid.Empty))
            {
                return new BadRequestObjectResult("The provided userId is not valid.");
            }

            var shoppingList = _shoppingListRepository.GetShoppingListByUserId(userId);

            if (shoppingList == null)
            {
                shoppingList = await _shoppingListRepository.Create(userId);
            }

            return new OkObjectResult(shoppingList);
        }

        public async Task<IActionResult> AddToShoppingList(Guid shoppingListId, Guid? itemId, string itemName)
        {
            var shoppingList = _shoppingListRepository.GetShoppingListById(shoppingListId);

            if (shoppingList == null)
            {
                return new NotFoundResult();
            }
            var shoppingListItem = new ShoppingListItem
            {
                ShoppingListId = shoppingListId,
                Complete = false
            };

            if (itemId != null)
            {
                // list item already exists, add to shopping list
                shoppingListItem.ListItemId = itemId.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(itemName))
                {
                    return new BadRequestObjectResult("The provided item name cannot be empty.");
                }
                // list item does not exist, create it and add to shopping list
                shoppingListItem.ListItem = new ListItem
                {
                    Name = itemName
                };
            }
            shoppingList = await _shoppingListRepository.AddToShoppingList(shoppingList.Id, shoppingListItem);

            return new OkObjectResult(shoppingList);
        }

        public async Task<IActionResult> UpdateShoppingListItem(Guid shoppingListItemId, ShoppingListItemAction action)
        {
            switch (action)
            {
                case ShoppingListItemAction.DELETE:
                    try
                    {
                        await _shoppingListItemRepository.Delete(shoppingListItemId);
                        return new OkResult();
                    }
                    catch (KeyNotFoundException)
                    {
                        return new NotFoundResult();
                    }
                case ShoppingListItemAction.TOGGLE_COMPLETE:
                    try
                    {
                        var shoppingListItem = await _shoppingListItemRepository.ToggleComplete(shoppingListItemId);
                        return new OkObjectResult(shoppingListItem);
                    }
                    catch (KeyNotFoundException)
                    {
                        return new NotFoundResult();
                    }
                default:
                    // this should be unreachable
                    return new BadRequestResult();
            }
        }

        public IActionResult GetAutoCompleteList()
        {
            var list = _listItemRepository.GetList().ToList();

            return new OkObjectResult(list);
        }
    }
}
