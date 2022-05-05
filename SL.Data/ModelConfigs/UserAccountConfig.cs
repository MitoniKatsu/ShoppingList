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
    class UserAccountConfig : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("UserAccount");

            builder.Property(o => o.Id)
                .HasDefaultValueSql("NEWID()")
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(o => o.Email)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(o => o.CreatedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(o => o.LastLogin)
                .HasColumnType("datetime")
                .IsRequired(false);

            builder.HasKey(o => o.Id);
        }
    }
}
