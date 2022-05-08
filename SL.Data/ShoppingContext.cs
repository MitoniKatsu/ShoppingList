using Microsoft.EntityFrameworkCore;
using SL.Data.ModelConfigs;
using SL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SL.Data
{
    public class ShoppingContext : DbContext, IShoppingContext
    {
        public ShoppingContext()
        {
        }

        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
        {
        }

        public virtual DbSet<ListItem> ListItems { get; set; }
        public virtual DbSet<ShoppingList> ShoppingLists { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new UserAccountConfig())
                .ApplyConfiguration(new ListItemConfig())
                .ApplyConfiguration(new ShoppingListConfig())
                .ApplyConfiguration(new ShoppingListItemConfig());

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
