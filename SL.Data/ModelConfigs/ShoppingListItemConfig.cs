using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Data.ModelConfigs
{
    class ShoppingListItemConfig : IEntityTypeConfiguration<ShoppingListItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItem> builder)
        {
            builder.ToTable("ShoppingListItem");

            builder.Property(o => o.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(o => o.ShoppingListId)
                .IsRequired();
            builder.Property(o => o.ListItemId)
                .IsRequired();

            builder.Property(o => o.Complete)
                .HasDefaultValue(false);

            builder.HasKey(o => o.Id);

            builder.HasOne(p => p.ListItem)
                .WithMany()
                .HasForeignKey(o => o.ListItemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(p => p.ShoppingList)
                .WithMany()
                .HasForeignKey(o => o.ShoppingListId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
