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
    public interface IShoppingListRepository
    {
        Task<ShoppingList> AddToShoppingList(Guid id, ShoppingListItem item);
        Task<ShoppingList> Create(Guid userId);
        ShoppingList GetShoppingListById(Guid id);
        ShoppingList GetShoppingListByUserId(Guid userId);
    }

    public class ShoppingListRepository : IShoppingListRepository
    {
        private IShoppingContext _dbContext { get; set; }

        public ShoppingListRepository(IShoppingContext context)
        {
            _dbContext = context;
        }


        public ShoppingList GetShoppingListByUserId(Guid userId)
        {
            /* TODO: Handle Multiple Shopping Lists per user.
             * For now, a user will only have one shopping list. This can be expanded
             * later, as the database was created to handle one or more shopping lists per
             * user account */
            var list = _dbContext.ShoppingLists
                .Include(o => o.Items)
                .ThenInclude(o => o.ListItem)
                .Include(o => o.UserAccount)
                .FirstOrDefault(o => o.UserId == userId);

            return list;
        }

        public ShoppingList GetShoppingListById(Guid id)
        {

            var list = _dbContext.ShoppingLists
                .Include(o => o.Items)
                .ThenInclude(o => o.ListItem)
                .Include(o => o.UserAccount)
                .FirstOrDefault(o => o.Id == id);

            return list;
        }

        public async Task<ShoppingList> Create(Guid userId)
        {
            var list = new ShoppingList
            {
                UserId = userId,
                Items = new List<ShoppingListItem>()
            };

            _dbContext.ShoppingLists.Add(list);
            await _dbContext.SaveChangesAsync();

            return list;
        }

        public async Task<ShoppingList> AddToShoppingList(Guid id, ShoppingListItem item)
        {

            var list = _dbContext.ShoppingLists.FirstOrDefault(o => o.Id == id);

            if (list == null)
            {
                throw new KeyNotFoundException("A shopping list with the provided id cannot be found.");
            }

            list.Items.Add(item);

            await _dbContext.SaveChangesAsync();

            return list;
        }


    }
}
