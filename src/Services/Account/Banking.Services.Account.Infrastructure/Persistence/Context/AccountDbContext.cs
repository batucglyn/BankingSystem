using Banking.Services.Account.Application.Abstractions;
using Banking.Services.Account.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Infrastructure.Persistence.Context
{
    public class AccountDbContext : DbContext, IAccountDbContext
    {

        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }



        public DbSet<Domain.Entities.Account> Accounts => Set<Domain.Entities.Account>();

        public DbSet<AccountTransaction> AccountTransactions
         => Set<AccountTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);

        }


    }
}
