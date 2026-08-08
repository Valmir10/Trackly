using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class ChatMessageTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidAuthorId = Guid.NewGuid();

    // -------------------------------------------------------
    // ChatMessage.Create — project scope
    // -------------------------------------------------------

    [Fact]
    public void Create_WithoutTicketId_IsProjectScoped()
    {
        // Act
        var message = ChatMessage.Create(ValidTenantId, ValidProjectId, ValidAuthorId, "Started on #127 today.");

        // Assert
        message.TicketId.Should().BeNull();
        message.ProjectId.Should().Be(ValidProjectId);
        message.Content.Should().Be("Started on #127 today.");
    }

    // -------------------------------------------------------
    // ChatMessage.Create — ticket scope
    // -------------------------------------------------------

    [Fact]
    public void Create_WithTicketId_IsTicketScoped()
    {
        // Arrange
        var ticketId = Guid.NewGuid();

        // Act
        var message = ChatMessage.Create(ValidTenantId, ValidProjectId, ValidAuthorId, "Storybook config is in.", ticketId);

        // Assert
        message.TicketId.Should().Be(ticketId);
        message.ProjectId.Should().Be(ValidProjectId);
    }

    // -------------------------------------------------------
    // ChatMessage.Create — domain event
    // -------------------------------------------------------

    [Fact]
    public void Create_RaisesChatMessageSentEvent()
    {
        // Arrange
        var ticketId = Guid.NewGuid();

        // Act
        var message = ChatMessage.Create(ValidTenantId, ValidProjectId, ValidAuthorId, "Storybook config is in.", ticketId);

        // Assert
        message.DomainEvents.Should().ContainSingle(e => e is ChatMessageSentEvent);

        var domainEvent = message.DomainEvents.Single() as ChatMessageSentEvent;
        domainEvent!.MessageId.Should().Be(message.Id);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.TicketId.Should().Be(ticketId);
        domainEvent.AuthorId.Should().Be(ValidAuthorId);
        domainEvent.Content.Should().Be("Storybook config is in.");
        domainEvent.CreatedAt.Should().Be(message.CreatedAt);
    }

    // -------------------------------------------------------
    // ChatMessage.Create — trimming
    // -------------------------------------------------------

    [Fact]
    public void Create_TrimsContentWhitespace()
    {
        // Act
        var message = ChatMessage.Create(ValidTenantId, ValidProjectId, ValidAuthorId, "  hello  ");

        // Assert
        message.Content.Should().Be("hello");
    }

    // -------------------------------------------------------
    // ChatMessage.Create — invalid input
    // -------------------------------------------------------

    [Fact]
    public void Create_WithEmptyContent_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => ChatMessage.Create(ValidTenantId, ValidProjectId, ValidAuthorId, "");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyAuthorId_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => ChatMessage.Create(ValidTenantId, ValidProjectId, Guid.Empty, "hello");

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
