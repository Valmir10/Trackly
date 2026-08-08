using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Projects.Queries.GetProjects;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Projects.Queries.GetProjects;

public class GetProjectsQueryHandlerTests
{
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();

    private GetProjectsQueryHandler CreateHandler() => new(_projectRepository);

    [Fact]
    public async Task Handle_MapsRepositoryProjectsToDtos()
    {
        // Arrange
        var project = Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());
        _projectRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Project> { project });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Id.Should().Be(project.Id);
        result[0].Name.Should().Be("Frontend redesign");
        result[0].Color.Should().Be("var(--tp-cat-1)");
    }

    [Fact]
    public async Task Handle_WithNoProjects_ReturnsEmptyList()
    {
        // Arrange
        _projectRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Project>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
