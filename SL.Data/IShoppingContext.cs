using Microsoft.EntityFrameworkCore;
using SL.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SL.Data
{
    public interface IShoppingContext
    {
        DbSet<ListItem> ListItems { get; set; }
        DbSet<ShoppingList> ShoppingLists { get; set; }
        DbSet<UserAccount> UserAccounts { get; set; }
        DbSet<ShoppingListItem> ShoppingListItems { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}