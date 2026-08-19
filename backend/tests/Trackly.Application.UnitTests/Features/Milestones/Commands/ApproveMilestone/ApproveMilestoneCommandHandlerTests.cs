using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Milestones.Commands.ApproveMilestone;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Milestones.Commands.ApproveMilestone;

public class ApproveMilestoneCommandHandlerTests
{
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly IApprovalRepository _approvalRepository = Substitute.For<IApprovalRepository>();
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentClientRoomService _currentClientRoom = Substitute.For<ICurrentClientRoomService>();

    private ApproveMilestoneCommandHandler CreateHandler() =>
        new(_milestoneRepository, _approvalRepository, _ticketRepository, _unitOfWork, _currentTenant, _currentClientRoom);

    private static Milestone ExistingMilestone(Guid projectId) =>
        Milestone.Create(Guid.NewGuid(), projectId, Guid.NewGuid(), "Milestone 2", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidToken_ApprovesMilestone_ReturnsApprovalId()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var milestone = ExistingMilestone(projectId);
        _currentClientRoom.ProjectId.Returns(projectId);
        _currentClientRoom.AccessId.Returns(Guid.NewGuid());
        _currentTenant.TenantId.Returns(milestone.TenantId);
        _milestoneRepository.GetByIdForProjectAsync(milestone.Id, projectId, Arg.Any<CancellationToken>()).Returns(milestone);
        _approvalRepository.GetByMilestoneIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns((Approval?)null);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new ApproveMilestoneCommand(milestone.Id), CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _approvalRepository.Received(1).AddAsync(
            Arg.Is<Approval>(a => a.MilestoneId == milestone.Id && a.ProjectId == projectId),
            Arg.Any<CancellationToken>());
    }

    // -------------------------------------------------------
    // The single most important test in this move.
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenMilestoneBelongsToADifferentProjectThanTheToken_ThrowsNotFoundException()
    {
        // Arrange — a client-room token for Project A must never approve a
        // milestone belonging to Project B, even within the same tenant.
        // GetByIdForProjectAsync is the enforcement point: it returns null
        // for a project mismatch, exactly as the real repository would.
        var tokenProjectId = Guid.NewGuid();
        var milestoneId = Guid.NewGuid();
        _currentClientRoom.ProjectId.Returns(tokenProjectId);
        _milestoneRepository.GetByIdForProjectAsync(milestoneId, tokenProjectId, Arg.Any<CancellationToken>()).Returns((Milestone?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new ApproveMilestoneCommand(milestoneId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _approvalRepository.DidNotReceive().AddAsync(Arg.Any<Approval>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenMilestoneAlreadyApproved_ThrowsConflictException()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var milestone = ExistingMilestone(projectId);
        var existingApproval = Approval.Create(milestone.TenantId, projectId, milestone.Id, Guid.NewGuid());
        _currentClientRoom.ProjectId.Returns(projectId);
        _milestoneRepository.GetByIdForProjectAsync(milestone.Id, projectId, Arg.Any<CancellationToken>()).Returns(milestone);
        _approvalRepository.GetByMilestoneIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(existingApproval);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new ApproveMilestoneCommand(milestone.Id), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
        await _approvalRepository.DidNotReceive().AddAsync(Arg.Any<Approval>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ClearsBlockedByMilestoneOnDownstreamTickets_BeforeSingleSaveChangesCall()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var milestone = ExistingMilestone(projectId);
        var blockedTicket = Ticket.Create(milestone.TenantId, projectId, "Ship it", Guid.NewGuid());
        blockedTicket.SetBlockedByMilestone(milestone.Id);
        _currentClientRoom.ProjectId.Returns(projectId);
        _currentClientRoom.AccessId.Returns(Guid.NewGuid());
        _currentTenant.TenantId.Returns(milestone.TenantId);
        _milestoneRepository.GetByIdForProjectAsync(milestone.Id, projectId, Arg.Any<CancellationToken>()).Returns(milestone);
        _approvalRepository.GetByMilestoneIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns((Approval?)null);
        _ticketRepository.GetBlockedByMilestoneAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { blockedTicket });
        var handler = CreateHandler();

        // Act
        await handler.Handle(new ApproveMilestoneCommand(milestone.Id), CancellationToken.None);

        // Assert
        blockedTicket.BlockedByMilestoneId.Should().BeNull();
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
