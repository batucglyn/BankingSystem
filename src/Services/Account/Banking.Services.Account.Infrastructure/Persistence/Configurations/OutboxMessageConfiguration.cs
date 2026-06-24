using Banking.Services.Account.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Persistence.Configurations
{
    public sealed class OutboxMessageConfiguration
     : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(
            EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.ProcessedAt);

            builder.Property(x => x.Error)
             .HasMaxLength(1000);

            builder.Property(x => x.RetryCount)
                .IsRequired();
        }
    }
}
