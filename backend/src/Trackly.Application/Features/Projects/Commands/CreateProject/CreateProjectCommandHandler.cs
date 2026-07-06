using MediatR;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Projects.Commands.CreateProject;

public sealed class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentUserService _currentUser;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentUserService currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Project.Create(
            _currentTenant.TenantId,
            request.Name,
            request.Color,
            _currentUser.UserId,
            request.Description);

        await _projectRepository.AddAsync(project, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
