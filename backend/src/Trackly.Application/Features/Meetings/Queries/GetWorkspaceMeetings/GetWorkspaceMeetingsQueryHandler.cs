using MediatR;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;

namespace Trackly.Application.Features.Meetings.Queries.GetWorkspaceMeetings;

public sealed class GetWorkspaceMeetingsQueryHandler : IRequestHandler<GetWorkspaceMeetingsQuery, IReadOnlyList<MeetingSummaryDto>>
{
    private readonly IMeetingRepository _meetingRepository;

    public GetWorkspaceMeetingsQueryHandler(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task<IReadOnlyList<MeetingSummaryDto>> Handle(GetWorkspaceMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await _meetingRepository.GetAllAsync(cancellationToken);

        return meetings
            .Select(m => new MeetingSummaryDto(m.Id, m.ProjectId, m.Title, m.ScheduledAt, m.CreatedAt))
            .ToList();
    }
}
