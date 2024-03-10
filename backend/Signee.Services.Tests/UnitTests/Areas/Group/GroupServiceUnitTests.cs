// using Moq;
// using NUnit.Framework;
// using NUnit.Framework.Legacy;
// using Signee.Domain.Entities.View;
// using Signee.Domain.Identity;
// using Signee.Domain.RepositoryContracts.Areas.Group;
// using Signee.Services.Areas.Display.Contracts;
// using Signee.Services.Areas.Group.Services;
// using Signee.Services.Areas.User.Contracts;
// using Signee.Services.Areas.View.Contracts;
//
// namespace Signee.Services.Tests.UnitTests.Areas.Group;
//
// using Group = Domain.Entities.Group.Group;
// using Display = Domain.Entities.Display.Display;
// using View = View;
//
// public class GroupServiceUnitTests
// {
//     [TestFixture]
//     public class GroupServiceTests
//     {
//         private readonly Mock<IUserService> _userServiceMock = new();
//         private readonly Mock<IGroupRepository> _groupRepositoryMock = new();
//         private readonly Mock<IDisplayService> _displayServiceMock = new();
//         private readonly Mock<IViewService> _viewServiceMock = new();
//
//         [Test]
//         public async Task AddAsync_CorrectlyAddsGroup()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var group = new Group { Id = "group-id-1", UserId = "user-id-1" };
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync("group-id-1")).ReturnsAsync(group);
//
//             var user = new ApplicationUser { Id = "user-id-1" };
//             _userServiceMock.Setup(repo => repo.GetByIdAsync("user-id-1")).ReturnsAsync(user);
//
//             // Act
//             group.UserId = "user-id-1";
//             await groupService.AddAsync(group);
//
//             // Assert
//             _groupRepositoryMock.Verify(repo => repo.AddAsync(group), Times.Once);
//             ClassicAssert.AreEqual("user-id-1", group.UserId);
//         }
//
//         [Test]
//         public async Task UpdateAsync_CorrectlyUpdatesGroup()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var group = new Group { Id = "group-id-1" };
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync("group-id-1")).ReturnsAsync(group);
//
//             // Act
//             group.Name = "NewName";
//             await groupService.UpdateAsync(group);
//
//             // Assert
//             _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Once);
//             ClassicAssert.AreEqual("NewName", group.Name);
//         }
//
//         [Test]
//         public async Task GetAllAsync_CorrectlyGetAllGroups()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groups = new List<Group>
//             {
//                 new() { Id = "group-id-1" },
//                 new() { Id = "group-id-2" },
//                 new() { Id = "group-id-3" }
//             };
//
//             _groupRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(groups);
//
//             // Act
//             var result = await groupService.GetAllAsync();
//
//             // Assert
//             ClassicAssert.IsTrue(result.Count() == 3);
//         }
//
//         [Test]
//         public async Task GetByIdAsync_CorrectlyGetGroupById()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var group = new Group { Id = "group-id-1" };
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync("group-id-1")).ReturnsAsync(group);
//
//             // Act
//             var result = await groupService.GetByIdAsync("group-id-1");
//
//             // Assert
//             ClassicAssert.AreEqual(group, result);
//         }
//
//         [Test]
//         public async Task GetByIdAsync_ShouldThrowExceptionWhenGroupNotFound()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync("group-id-1")).ReturnsAsync((Group)null);
//
//             // Act + Assert
//             var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             {
//                 await groupService.GetByIdAsync("group-id-1");
//             });
//             Assert.That(exception, Is.Not.Null);
//         }
//
//         [Test]
//         public async Task AddDisplayToGroup_CorrectlyAddsDisplay()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var displayId = "your-display-id";
//
//             var group = new Group { Id = groupId };
//             var display = new Display { Id = displayId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _displayServiceMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//
//             // Act
//             await groupService.AddDisplayToGroupAsync(groupId, displayId);
//
//             // Assert
//             _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Once);
//             ClassicAssert.IsTrue(group?.Displays.Contains(display));
//         }
//
//         [Test]
//         public async Task AddDisplayToGroup_ShouldNotAddAlreadyAddedDisplay()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var displayId = "your-display-id";
//
//             var group = new Group { Id = groupId };
//             var display = new Display { Id = displayId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _displayServiceMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//
//             // Act
//             await groupService.AddDisplayToGroupAsync(groupId, displayId);
//
//             // Assert
//             var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             {
//                 await groupService.AddDisplayToGroupAsync(groupId, displayId);
//             });
//
//             Assert.That(exception, Is.Not.Null);
//             _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Once);
//         }
//
//         [Test]
//         public async Task RemoveDisplayFromGroupAsync_CorrectlyRemovesDisplay()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var displayId1 = "your-display-id1";
//             var displayId2 = "your-display-id2";
//
//             var group = new Group { Id = groupId };
//             var display1 = new Display { Id = displayId1 };
//             var display2 = new Display { Id = displayId2 };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _displayServiceMock.Setup(repo => repo.GetByIdAsync(displayId1)).ReturnsAsync(display1);
//             _displayServiceMock.Setup(repo => repo.GetByIdAsync(displayId2)).ReturnsAsync(display2);
//             await groupService.AddDisplayToGroupAsync(groupId, displayId1);
//             await groupService.AddDisplayToGroupAsync(groupId, displayId2);
//
//             // Act
//             ClassicAssert.IsTrue(group.Displays.Count == 2);
//             await groupService.RemoveDisplayFromGroupAsync(groupId, displayId1);
//
//             // Assert
//             ClassicAssert.IsFalse(group.Displays.Contains(display1));
//             ClassicAssert.IsTrue(group.Displays.Contains(display2));
//             ClassicAssert.IsTrue(group.Displays.Count == 1);
//         }
//
//         [Test]
//         public async Task RemoveDisplayFromGroupAsync_ShouldNotRemoveNotExistingDisplay()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var displayId1 = "your-display-id1";
//
//             var group = new Group { Id = groupId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//
//             // Act + Assert
//             ClassicAssert.IsTrue(group.Displays.Count == 0);
//             var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             {
//                 await groupService.RemoveDisplayFromGroupAsync(groupId, displayId1);
//             });
//             Assert.That(exception, Is.Not.Null);
//
//             _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Exactly(0));
//         }
//
//         [Test]
//         public async Task AddViewToGroupAsync_CorrectlyAddsView()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var viewId = "your-view-id";
//
//             var group = new Group { Id = groupId };
//             var view = new View { Id = viewId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _viewServiceMock.Setup(repo => repo.GetByIdAsync(viewId)).ReturnsAsync(view);
//
//             // Act
//             await groupService.AddViewToGroupAsync(groupId, viewId);
//
//             // Assert
//             ClassicAssert.IsTrue(group?.Views.Contains(view));
//         }
//
//         [Test]
//         public async Task AddViewToGroupAsync_ShouldNotAddAlreadyAddedView()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var viewId = "your-view-id";
//
//             var group = new Group { Id = groupId };
//             var view = new View { Id = viewId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _viewServiceMock.Setup(repo => repo.GetByIdAsync(viewId)).ReturnsAsync(view);
//
//             // Act
//             await groupService.AddViewToGroupAsync(groupId, viewId);
//             view.GroupId = groupId; // Framework does this automatically
//
//             // Assert
//             var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             {
//                 await groupService.AddViewToGroupAsync(groupId, viewId);
//             });
//
//             Assert.That(exception, Is.Not.Null);
//         }
//
//         [Test]
//         public async Task RemoveViewFromGroupAsync_CorrectlyRemovesView()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var viewId1 = "your-view-id1";
//             var viewId2 = "your-view-id2";
//
//             var group = new Group { Id = groupId };
//             var view1 = new View { Id = viewId1 };
//             var view2 = new View { Id = viewId2 };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//             _viewServiceMock.Setup(repo => repo.GetByIdAsync(viewId1)).ReturnsAsync(view1);
//             _viewServiceMock.Setup(repo => repo.GetByIdAsync(viewId2)).ReturnsAsync(view2);
//             await groupService.AddViewToGroupAsync(groupId, viewId1);
//             view1.Group = group;
//             await groupService.AddViewToGroupAsync(groupId, viewId2);
//             view2.Group = group;
//
//             // Act
//             ClassicAssert.IsTrue(group.Views.Count == 2);
//             await groupService.RemoveViewFromGroupAsync(groupId, viewId1);
//
//             // Assert
//             ClassicAssert.IsNull(view1.Group); // Check if view1.Group is null after removal
//             ClassicAssert.IsTrue(group.Views.Contains(view2));
//             ClassicAssert.IsNotNull(view2.Group);
//         }
//
//
//         [Test]
//         public async Task RemoveViewFromGroupAsync_ShouldNotRemoveNotExistingView()
//         {
//             // Arrange
//             var groupService = new GroupService(_userServiceMock.Object, _groupRepositoryMock.Object,
//                 _displayServiceMock.Object, _viewServiceMock.Object);
//
//             var groupId = "your-group-id";
//             var viewId1 = "your-view-id1";
//
//             var group = new Group { Id = groupId };
//
//             _groupRepositoryMock.Setup(repo => repo.GetByIdAsync(groupId)).ReturnsAsync(group);
//
//             // Act + Assert
//             ClassicAssert.IsTrue(group.Views.Count == 0);
//             var exception = Assert.ThrowsAsync<InvalidOperationException>(async () =>
//             {
//                 await groupService.RemoveViewFromGroupAsync(groupId, viewId1);
//             });
//             Assert.That(exception, Is.Not.Null);
//
//             _groupRepositoryMock.Verify(repo => repo.UpdateAsync(group), Times.Exactly(0));
//         }
//     }
// }