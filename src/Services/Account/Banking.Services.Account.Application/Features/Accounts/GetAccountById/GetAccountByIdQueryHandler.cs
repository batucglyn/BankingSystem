using Banking.Services.Account.Application.Abstractions;
using Banking.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Services.Account.Application.Features.Accounts.GetAccountById;

public sealed class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<GetAccountByIdResponse>>
{
    private readonly IAccountDbContext _context;

    public GetAccountByIdQueryHandler(IAccountDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetAccountByIdResponse>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {

        var account = await _context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);



        if(account is null)
        {
            return Result<GetAccountByIdResponse>
               .Failure("Account not found");
        }


        var response = new GetAccountByIdResponse(
       account.Id,
       account.AccountNumber,
       account.IBAN,
       account.Balance.Amount,
       account.Balance.Currency
   );

        return Result<GetAccountByIdResponse>
            .Success(response);





    }
}

