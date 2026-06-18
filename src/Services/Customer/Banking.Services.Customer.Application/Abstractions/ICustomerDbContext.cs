using Banking.Services.Customer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Customer.Application.Abstractions
{
    public interface ICustomerDbContext
    {
        DbSet<Domain.Entities.Customer> Customers { get; }
        
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken=default);
    }
}
