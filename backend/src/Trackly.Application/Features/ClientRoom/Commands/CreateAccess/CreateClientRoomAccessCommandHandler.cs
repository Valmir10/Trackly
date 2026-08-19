using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.ClientRoom.Commands.CreateAccess;

public sealed class CreateClientRoomAccessCommandHandler : IRequestHandler<CreateClientRoomAccessCommand, CreateClientRoomAccessResult>
{
    private readonly IClientRoomAccessRepository _accessRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IClientRoomTokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentUserService _currentUser;

    public CreateClientRoomAccessCommandHandler(
        IClientRoomAccessRepository accessRepository,
        IProjectRepository projectRepository,
        IClientRoomTokenService tokenService,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentUserService currentUser)
    {
        _accessRepository = accessRepository;
        _projectRepository = projectRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<CreateClientRoomAccessResult> Handle(CreateClientRoomAccessCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId);

        var (rawToken, hash) = _tokenService.GenerateToken();

        // Generous expiry — RevokedAt is the real kill switch, checked
        // fresh on every request, not this clock.
        var access = ClientRoomAccess.Create(_currentTenant.TenantId, project.Id, hash, DateTime.UtcNow.AddYears(1), _currentUser.UserId);

        await _accessRepository.AddAsync(access, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateClientRoomAccessResult(access.Id, rawToken);
    }
}
