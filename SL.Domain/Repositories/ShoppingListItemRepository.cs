using Microsoft.EntityFrameworkCore;
using SL.Data;
using SL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Domain.Repositories
{
    public interface IShoppingListItemRepository
    {
        Task<ShoppingListItem> Create(Guid shoppingListId, Guid listItemId);
        Task Delete(Guid shoppingListItemId);
        Task<ShoppingListItem> ToggleComplete(Guid shoppingListItemId);
    }

    public class ShoppingListItemRepository : IShoppingListItemRepository
    {
        private IShoppingContext _dbContext { get; set; }

        public ShoppingListItemRepository(IShoppingContext context)
        {
            _dbContext = context;
        }

        public async Task<ShoppingListItem> Create(Guid shoppingListId, Guid listItemId)
        {
            var shoppingList = _dbContext.ShoppingLists.FirstOrDefault(o => o.Id == shoppingListId);
            if (shoppingList == null)
            {
                throw new KeyNotFoundException("ShoppingList with the provided Id cannot be found.");
            }

            var listItem = _dbContext.ListItems.FirstOrDefault(o => o.Id == listItemId);
            if (listItem == null)
            {
                throw new KeyNotFoundException("ListItem with the provided Id cannot be found.");
            }


            var shoppingListItem = new ShoppingListItem
            {
                ShoppingListId = shoppingListId,
                ListItemId = listItemId,
                Complete = false
            };

            _dbContext.ShoppingListItems.Add(shoppingListItem);

            await _dbContext.SaveChangesAsync();

            return shoppingListItem;
        }

        public async Task<ShoppingListItem> ToggleComplete(Guid shoppingListItemId)
        {
            var shoppingListItem = _dbContext.ShoppingListItems.FirstOrDefault(o => o.Id == shoppingListItemId);

            if (shoppingListItem == null)
            {
                throw new KeyNotFoundException("ShoppingListItem with the provided Id cannot be found.");
            }

            shoppingListItem.Complete = !shoppingListItem.Complete;

            await _dbContext.SaveChangesAsync();

            return shoppingListItem;
        }

        public async Task Delete(Guid shoppingListItemId)
        {
            var shoppingListItem = _dbContext.ShoppingListItems.FirstOrDefault(o => o.Id == shoppingListItemId);

            if (shoppingListItem == null)
            {
                throw new KeyNotFoundException("ShoppingListItem with the provided Id cannot be found.");
            }

            _dbContext.ShoppingListItems.Remove(shoppingListItem);

            await _dbContext.SaveChangesAsync();
        }
    }
}
