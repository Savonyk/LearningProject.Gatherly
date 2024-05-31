using FluentAssertions;
using Gatherly.Application.Members.Commands.CreateMember;
using Gatherly.Domain.Entities;
using Gatherly.Domain.Errors;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.Shared;
using Gatherly.Domain.ValueObjects;
using Moq;

namespace Gatherly.Application.UnitTests.Members.Commands;
public class CreateMemberCommandHandlerTests
{
    private readonly Mock<IMemberRepository> _memberRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public CreateMemberCommandHandlerTests()
    {
        _memberRepositoryMock = new();
        _unitOfWorkMock = new (); 
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        //Arange
        var command = new CreateMemberCommand("email@test.com", "first", "last");

        _memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateMemberCommandHandler(_memberRepositoryMock.Object, _unitOfWorkMock.Object);
        //Act
        Result<Guid> result = await handler.Handle(command, default);

        //Asert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_Should_CallAddOnRepository_WhenEmailIsUnique()
    {
        var command = new CreateMemberCommand("email@test.com", "first", "last");

        _memberRepositoryMock.Setup(
            x => x.IsEmailUniqueAsync(
                It.IsAny<Email>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateMemberCommandHandler(_memberRepositoryMock.Object, _unitOfWorkMock.Object);
        //Act
        Result<Guid> result = await handler.Handle(command, default);

        //Asert
        _memberRepositoryMock.Verify(
            x => x.Add(It.Is<Member>(m => m.Id == result.Value)),
            Times.Once);
    }
}
