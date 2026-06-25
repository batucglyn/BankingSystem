using Banking.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Banking.Services.Customer.Application.Abstractions
{
    public interface ICustomerDbContext
    {
        DbSet<Domain.Entities.Customer> Customers { get; }
        DbSet<OutboxMessage> OutboxMessages { get; }
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken=default);
    }
}
