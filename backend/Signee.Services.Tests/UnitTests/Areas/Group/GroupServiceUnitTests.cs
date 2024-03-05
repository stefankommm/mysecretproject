using NUnit.Framework;
using Moq;
using NUnit.Framework.Legacy;
using Signee.Domain.Entities.Display;
using Signee.Services.Areas.Group.Services;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Services.Areas.Display.Contracts;
using Signee.Services.Areas.User.Contracts;
using Signee.Services.Areas.View.Contracts;

namespace Signee.Services.Tests.UnitTests.Areas.Group;
using Group = Domain.Entities.Group.Group;

public class GroupServiceUnitTests
{
    [TestFixture]
    public class GroupServiceTests
    {
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<IGroupRepository> _groupRepositoryMock = new Mock<IGroupRepository>();
        private readonly Mock<IDisplayService> _displaServiceMock = new Mock<IDisplayService>();
        private readonly Mock<IViewService> _viewServiceMock = new Mock<IViewService>();
        
        [Test]
        public async Task AddDisplayToGroup_CorrectlyAddsDisplay()
        {
            // Arrange
            var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object, _displaServiceMock.Object, _viewServiceMock.Object);

            var groupId = "your-group-id";
            var displayId = "your-display-id";

            var group = new Group() { Id = groupId };
            var display = new Display() { Id = displayId };

            _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
            _displaServiceMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);

            // Act
            await groupService.AddDisplayToGroupAsync(groupId, displayId);

            // Assert
            var updatedGroup = await _groupRepositoryMock.Object.GetByIdAsync(groupId);
            var updatedDisplay = await _displaServiceMock.Object.GetByIdAsync(displayId);
            
            _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Once);
            ClassicAssert.IsTrue(updatedGroup?.Displays.Contains(updatedDisplay));
        }
    }
}