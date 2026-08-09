using MediatR;

namespace Trackly.Application.Features.Decisions.Commands.CreateDecision;

public sealed record CreateDecisionCommand(Guid MeetingId, string Text) : IRequest<Guid>;
