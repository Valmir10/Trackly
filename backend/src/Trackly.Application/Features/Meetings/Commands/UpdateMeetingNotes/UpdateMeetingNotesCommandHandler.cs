using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;

public sealed class UpdateMeetingNotesCommandHandler : IRequestHandler<UpdateMeetingNotesCommand>
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMeetingNotesCommandHandler(IMeetingRepository meetingRepository, IUnitOfWork unitOfWork)
    {
        _meetingRepository = meetingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateMeetingNotesCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken)
            ?? throw new NotFoundException(nameof(Meeting), request.MeetingId);

        meeting.UpdateNotes(request.Notes);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
