using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Meetings.Queries.GetMeetingById;

public sealed class GetMeetingByIdQueryHandler : IRequestHandler<GetMeetingByIdQuery, MeetingDto>
{
    private readonly IMeetingRepository _meetingRepository;

    public GetMeetingByIdQueryHandler(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task<MeetingDto> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken)
            ?? throw new NotFoundException(nameof(Meeting), request.MeetingId);

        return new MeetingDto(
            meeting.Id,
            meeting.ProjectId,
            meeting.Title,
            meeting.ScheduledAt,
            meeting.Notes,
            meeting.CreatedById,
            meeting.CreatedAt,
            meeting.UpdatedAt);
    }
}
