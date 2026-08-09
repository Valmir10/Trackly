using MediatR;

namespace Trackly.Application.Features.Meetings.Commands.CreateMeeting;

public sealed record CreateMeetingCommand(Guid ProjectId, string Title, DateTime ScheduledAt) : IRequest<Guid>;
