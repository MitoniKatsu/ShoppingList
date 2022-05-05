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
    class ShoppingListConfig : IEntityTypeConfiguration<ShoppingList>
    {
        public void Configure(EntityTypeBuilder<ShoppingList> builder)
        {
            builder.ToTable("ShoppingList");

            builder.Property(o => o.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.HasKey(o => o.Id);

            builder.HasOne(p => p.UserAccount)
                .WithMany()
                .HasForeignKey(o => o.UserId);
        }
    }
}
