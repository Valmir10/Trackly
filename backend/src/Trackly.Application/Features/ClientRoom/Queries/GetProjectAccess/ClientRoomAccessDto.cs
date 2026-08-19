namespace Trackly.Application.Features.ClientRoom.Queries.GetProjectAccess;

// Never includes the raw token or its hash — this is a management list,
// not a way to retrieve/re-derive a usable credential.
public sealed record ClientRoomAccessDto(Guid Id, DateTime ExpiresAt, DateTime CreatedAt, DateTime? RevokedAt, bool IsActive);
