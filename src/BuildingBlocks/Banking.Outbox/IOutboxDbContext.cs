using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Outbox
{

    public interface IOutboxDbContext
    {
        DbSet<OutboxMessage> OutboxMessages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
