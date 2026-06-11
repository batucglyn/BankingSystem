using Banking.Services.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Persistence.Configurations
{
    public sealed class AccountTransactionConfiguration
    : IEntityTypeConfiguration<AccountTransaction>
    {
        public void Configure(
            EntityTypeBuilder<AccountTransaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
            .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();
        }
    }
}
