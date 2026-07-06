using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Chat.Commands.SendChatMessage;

public sealed class SendChatMessageCommandHandler : IRequestHandler<SendChatMessageCommand, Guid>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentUserService _currentUser;

    public SendChatMessageCommandHandler(
        IChatMessageRepository chatMessageRepository,
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentUserService currentUser)
    {
        _chatMessageRepository = chatMessageRepository;
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(SendChatMessageCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId);

        if (request.TicketId.HasValue)
        {
            _ = await _ticketRepository.GetByIdAsync(request.TicketId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Ticket), request.TicketId.Value);
        }

        var message = ChatMessage.Create(
            _currentTenant.TenantId,
            project.Id,
            _currentUser.UserId,
            request.Content,
            request.TicketId);

        await _chatMessageRepository.AddAsync(message, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return message.Id;
    }
}
