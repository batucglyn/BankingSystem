using Banking.Services.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Domain.Entities.Account>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Account> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CustomerId)
                .IsRequired();

            builder.Property(x => x.AccountNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.IBAN)
                .IsRequired()
                .HasMaxLength(34);

            builder.Property(x => x.Balance)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }
}
