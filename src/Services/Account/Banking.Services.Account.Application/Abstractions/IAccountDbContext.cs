using Banking.Services.Account.Domain.Entities;
using Banking.Services.Account.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
namespace Banking.Services.Account.Application.Abstractions
{
    public interface IAccountDbContext
    {
        
        DbSet<Domain.Entities.Account> Accounts { get; }
        DbSet<AccountTransaction> AccountTransactions { get; }

        DbSet<OutboxMessage> OutboxMessages { get; }
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken);
    }
}
