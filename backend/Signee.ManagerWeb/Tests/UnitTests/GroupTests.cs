using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Legacy;
using Signee.Domain.Entities.Display;
using Signee.Domain.Entities.Group;
using Signee.Services.Areas.Group.Services;
using Signee.Services.Areas.Display.Services;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Domain.RepositoryContracts.Areas.Display;

namespace Signee.ManagerWeb.Tests.UnitTests;

public class GroupTests
{
    [TestFixture]
    public class GroupServiceTests
    {
        [Test]
        public async Task AddDisplayToGroup_CorrectlyAddsDisplay()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var displayRepositoryMock = new Mock<IDisplayRepository>();

            var groupService = new GroupService(groupRepositoryMock.Object, displayRepositoryMock.Object);

            var groupId = "your-group-id";
            var displayId = "your-display-id";

            var group = new Group() { Id = groupId };
            var display = new Display() { Id = displayId };

            groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
            displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);

            // Act
            await groupService.AddDisplayToGroup(groupId, displayId);

            // Fetch the updated group and display from the repository
            var updatedGroup = await groupRepositoryMock.Object.GetByIdAsync(groupId);
            var updatedDisplay = await displayRepositoryMock.Object.GetByIdAsync(displayId);

            // Assert
            groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Once);
            ClassicAssert.IsTrue(updatedGroup.Displays.Contains(updatedDisplay));

        }
    }
}