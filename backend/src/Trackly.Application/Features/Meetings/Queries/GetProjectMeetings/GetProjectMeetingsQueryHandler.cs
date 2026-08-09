using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;

public sealed class GetProjectMeetingsQueryHandler : IRequestHandler<GetProjectMeetingsQuery, IReadOnlyList<MeetingSummaryDto>>
{
    private readonly IMeetingRepository _meetingRepository;

    public GetProjectMeetingsQueryHandler(IMeetingRepository meetingRepository)
    {
        _meetingRepository = meetingRepository;
    }

    public async Task<IReadOnlyList<MeetingSummaryDto>> Handle(GetProjectMeetingsQuery request, CancellationToken cancellationToken)
    {
        var meetings = await _meetingRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        return meetings
            .Select(m => new MeetingSummaryDto(m.Id, m.ProjectId, m.Title, m.ScheduledAt, m.CreatedAt))
            .ToList();
    }
}
