using MediatR;

namespace Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;

public sealed record UpdateMeetingNotesCommand(Guid MeetingId, string Notes) : IRequest;
