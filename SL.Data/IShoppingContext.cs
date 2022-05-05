using Microsoft.EntityFrameworkCore;
using SL.Data.Models;

namespace SL.Data
{
    interface IShoppingContext
    {
        DbSet<ListItem> ListItems { get; set; }
        DbSet<ShoppingList> ShoppingLists { get; set; }
        DbSet<UserAccount> UserAccounts { get; set; }
    }
}