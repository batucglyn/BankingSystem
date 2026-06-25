using Banking.Outbox;
using Banking.Services.Customer.Application.Abstractions;
using Banking.Services.Customer.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Infrastructure.Persistence
{
    public sealed class CustomerDbContext : DbContext, ICustomerDbContext, IOutboxDbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Entities.Customer> Customers => Set<Domain.Entities.Customer>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(CustomerDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        }
    }
}