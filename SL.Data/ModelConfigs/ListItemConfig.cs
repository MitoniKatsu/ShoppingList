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
    class ListItemConfig : IEntityTypeConfiguration<ListItem>
    {
        public void Configure(EntityTypeBuilder<ListItem> builder)
        {
            builder.ToTable("ListItem");

            builder.Property(o => o.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasKey(o => o.Id);
        }
    }
}
