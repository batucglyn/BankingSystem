using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Banking.Services.Account.Domain.Entities;
namespace Banking.Services.Account.Application.Abstractions
{
    public interface IAccountDbContext
    {
        
        DbSet<Domain.Entities.Account> Accounts { get; }
        DbSet<AccountTransaction> AccountTransactions { get; }
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
