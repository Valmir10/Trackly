using MediatR;
using Trackly.Application.Features.Contracts.Commands.CreateContract;
using Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

namespace Trackly.Api.Endpoints;

public sealed record CreateContractRequest(string Title);

public static class ContractEndpoints
{
    public static void MapContractEndpoints(this WebApplication app)
    {
        app.MapPost("/api/projects/{projectId:guid}/contracts", async (
            Guid projectId, CreateContractRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateContractCommand(projectId, request.Title);
            var contractId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/contracts/{contractId}", new { Id = contractId });
        }).RequireAuthorization();

        app.MapGet("/api/projects/{projectId:guid}/contracts", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var contracts = await sender.Send(new GetProjectContractsQuery(projectId), cancellationToken);
            return Results.Ok(contracts);
        }).RequireAuthorization();
    }
}
