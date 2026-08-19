using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.ClientRoom.Queries.GetSummary;

public sealed class GetClientRoomSummaryQueryHandler : IRequestHandler<GetClientRoomSummaryQuery, ClientRoomSummaryDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IContractRepository _contractRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IApprovalRepository _approvalRepository;
    private readonly ICurrentClientRoomService _currentClientRoom;

    public GetClientRoomSummaryQueryHandler(
        IProjectRepository projectRepository,
        IContractRepository contractRepository,
        IMilestoneRepository milestoneRepository,
        ITicketRepository ticketRepository,
        IApprovalRepository approvalRepository,
        ICurrentClientRoomService currentClientRoom)
    {
        _projectRepository = projectRepository;
        _contractRepository = contractRepository;
        _milestoneRepository = milestoneRepository;
        _ticketRepository = ticketRepository;
        _approvalRepository = approvalRepository;
        _currentClientRoom = currentClientRoom;
    }

    public async Task<ClientRoomSummaryDto> Handle(GetClientRoomSummaryQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(_currentClientRoom.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), _currentClientRoom.ProjectId);

        var contracts = await _contractRepository.GetByProjectIdAsync(project.Id, cancellationToken);
        var contractDtos = new List<ClientRoomContractDto>(contracts.Count);

        foreach (var contract in contracts)
        {
            var milestones = await _milestoneRepository.GetByContractIdAsync(contract.Id, cancellationToken);
            var milestoneDtos = new List<ClientRoomMilestoneDto>(milestones.Count);

            foreach (var milestone in milestones)
            {
                var tickets = await _ticketRepository.GetByMilestoneIdAsync(milestone.Id, cancellationToken);
                var ticketsDone = tickets.Count(t => t.Status == TicketStatus.Done);
                var progressPercentage = tickets.Count > 0 ? (int)Math.Round(ticketsDone * 100.0 / tickets.Count) : 0;

                var approval = await _approvalRepository.GetByMilestoneIdAsync(milestone.Id, cancellationToken);

                milestoneDtos.Add(new ClientRoomMilestoneDto(
                    milestone.Id,
                    milestone.Title,
                    tickets.Count,
                    ticketsDone,
                    progressPercentage,
                    approval is not null,
                    approval?.ApprovedAt));
            }

            contractDtos.Add(new ClientRoomContractDto(contract.Id, contract.Title, milestoneDtos));
        }

        return new ClientRoomSummaryDto(project.Name, project.Color, contractDtos);
    }
}
