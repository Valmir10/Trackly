using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Chat.Queries.GetChatMessages;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryHandlerTests
{
    private readonly IChatMessageRepository _chatMessageRepository = Substitute.For<IChatMessageRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    private GetChatMessagesQueryHandler CreateHandler() => new(_chatMessageRepository, _userRepository);

    [Fact]
    public async Task Handle_ResolvesAuthorInitials()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var author = User.Create(Guid.NewGuid(), "sarah@acme.com", "Sarah", "Kim");
        var message = ChatMessage.Create(Guid.NewGuid(), projectId, author.Id, "Started on #127 today.");
        _chatMessageRepository.GetByScopeAsync(projectId, null, Arg.Any<CancellationToken>())
            .Returns(new List<ChatMessage> { message });
        _userRepository.GetByIdAsync(author.Id, Arg.Any<CancellationToken>()).Returns(author);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetChatMessagesQuery(projectId, null), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].AuthorInitials.Should().Be("SK");
        result[0].Content.Should().Be("Started on #127 today.");
    }

    [Fact]
    public async Task Handle_WithUnknownAuthor_UsesPlaceholderInitials()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var message = ChatMessage.Create(Guid.NewGuid(), projectId, authorId, "hello");
        _chatMessageRepository.GetByScopeAsync(projectId, null, Arg.Any<CancellationToken>())
            .Returns(new List<ChatMessage> { message });
        _userRepository.GetByIdAsync(authorId, Arg.Any<CancellationToken>()).Returns((User?)null);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetChatMessagesQuery(projectId, null), CancellationToken.None);

        // Assert
        result[0].AuthorInitials.Should().Be("?");
    }

    [Fact]
    public async Task Handle_PassesTicketIdThroughToRepository()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticketId = Guid.NewGuid();
        _chatMessageRepository.GetByScopeAsync(projectId, ticketId, Arg.Any<CancellationToken>())
            .Returns(new List<ChatMessage>());
        var handler = CreateHandler();

        // Act
        await handler.Handle(new GetChatMessagesQuery(projectId, ticketId), CancellationToken.None);

        // Assert
        await _chatMessageRepository.Received(1).GetByScopeAsync(projectId, ticketId, Arg.Any<CancellationToken>());
    }
}
