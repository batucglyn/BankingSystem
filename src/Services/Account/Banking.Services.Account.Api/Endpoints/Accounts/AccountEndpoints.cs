using Banking.Services.Account.Application.Features.Accounts.CreateAccount;
using Banking.Services.Account.Application.Features.Accounts.GetAccountById;
using MediatR;

namespace Banking.Services.Account.Api.Endpoints.Accounts
{
    public static class AccountEndpoints
    {
        public static RouteGroupBuilder MapAccountEndpoints(
       this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/accounts")
                .WithTags("Accounts");

            group.MapPost(
                "/",
                async (
                    CreateAccountCommand command,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                    command,
                    cancellationToken);

                    if (!result.IsSuccess)
                    {
                        return Results.BadRequest(
                            new
                            {
                                error = result.ErrorMessage
                            });
                    }

                    return Results.Ok(result.Data);
                })
                .WithName("CreateAccount")
                .Produces<CreateAccountResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);




            group.MapGet(
    "/{id:guid}",
    async (
        Guid id,
        ISender sender,
        CancellationToken cancellationToken) =>
    {
        var result = await sender.Send(
            new GetAccountByIdQuery(id),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.NotFound(
                new
                {
                    error = result.ErrorMessage
                });
        }

        return Results.Ok(result.Data);
    })
    .WithName("GetAccountById")
    .Produces<GetAccountByIdResponse>(
        StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

            return group;
        }



    }
}
