namespace Trackly.Application.Common.Interfaces;

// Mirrors ICurrentTenantService's shape exactly. Populated from the
// ClientRoom-scheme claims (Api/Auth/ClientRoomAuthenticationHandler) —
// ApproveMilestoneCommandHandler and GetClientRoomSummaryQueryHandler
// depend on this, never on a client-supplied projectId.
public interface ICurrentClientRoomService
{
    Guid AccessId { get; }
    Guid ProjectId { get; }
}
