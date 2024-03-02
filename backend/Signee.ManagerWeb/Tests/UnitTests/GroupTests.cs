using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Legacy;
using Signee.Domain.Entities.Display;
using Signee.Domain.Entities.Group;
using Signee.Services.Areas.Group.Services;
using Signee.Domain.RepositoryContracts.Areas.Group;
using Signee.Domain.RepositoryContracts.Areas.Display;
using Signee.Domain.RepositoryContracts.Areas.User;
using Signee.Domain.RepositoryContracts.Areas.View;

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
            var userRepositortyMock = new Mock<IUserRepository>();
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var displayRepositoryMock = new Mock<IDisplayRepository>();
            var viewRepositoryMock = new Mock<IViewRepository>();

            var groupService = new GroupService(userRepositortyMock.Object, groupRepositoryMock.Object, displayRepositoryMock.Object, viewRepositoryMock.Object);

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