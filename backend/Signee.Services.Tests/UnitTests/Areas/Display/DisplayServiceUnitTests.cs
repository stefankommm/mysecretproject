// using System.Net;
// using Moq;
// using NUnit.Framework;
// using NUnit.Framework.Legacy;
// using Signee.Domain.Entities.View;
// using Signee.Domain.RepositoryContracts.Areas.Display;
// using Signee.Services.Areas.Display.Contracts;
// using Signee.Services.Areas.Display.Services;
// using Signee.Services.Areas.Group.Contracts;
// using Signee.Services.Areas.User.Contracts;
// using Signee.Services.Areas.View.Contracts;
//
// namespace Signee.Services.Tests.UnitTests.Areas.Display;
//
// using Display = Domain.Entities.Display.Display;
//
// public class DisplayServiceUnitTests
// {
//     [TestFixture]
//     public class DispalyServiceTests
//     {
//         private readonly Mock<IUserService> _userServiceMock = new();
//         private readonly Mock<IDisplayRepository> _displayRepositoryMock = new();
//         private readonly Mock<IDisplayService> _displayServiceMock = new();
//         private readonly Mock<IViewService> _viewServiceMock = new();
//         private readonly Mock<IGroupService> _groupServiceMock = new();
//
//         [Test]
//         public async Task AddAsync_ShouldCallRepositoryAddAsync()
//         {
//             // Arrange
//             var display = new Display();
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             displayRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Display>()))
//                 .Returns(Task.CompletedTask); // Adjusted setup
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//
//             // Act
//             await displayService.AddAsync(display);
//
//             // Assert
//             displayRepositoryMock.Verify(x => x.AddAsync(display), Times.Once);
//         }
//
//
//         [Test]
//         public async Task UpdateAsync_CorrectlyUpdatesDisplay()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var groupServiceMock = new Mock<IGroupService>();
//             var viewServiceMock = new Mock<IViewService>();
//
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 viewServiceMock.Object, groupServiceMock.Object);
//
//             var displayId = "display-id-1";
//             var display = new Display { Id = displayId };
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//
//             // Act
//             display.Name = "NewName";
//             await displayService.UpdateAsync(display);
//
//             // Assert
//             displayRepositoryMock.Verify(repo => repo.UpdateAsync(display), Times.Once);
//             ClassicAssert.AreEqual("NewName", display.Name);
//         }
//
//
//         [Test]
//         public async Task DeleteByIdAsync_CorrectlyDeletesDisplay()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//
//             var displayId = "display-id-1";
//             var display = new Display { Id = displayId };
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//
//             // Act
//             await displayService.DeleteByIdAsync(displayId);
//
//             // Assert
//             displayRepositoryMock.Verify(repo => repo.GetByIdAsync(displayId), Times.Once);
//
//             displayRepositoryMock.Verify(repo => repo.DeleteByIdAsync(displayId), Times.Once);
//         }
//
//
//         [Test]
//         public async Task DeleteByIdAsync_ThrowsExceptionWhenDisplayNotFound()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//
//             // Setup mock to return null when GetByIdAsync is called with "display-id-1"
//             var displayId = "display-id-1";
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync((Display)null);
//
//             // Act & Assert
//             var ex = Assert.ThrowsAsync<InvalidOperationException>(() =>
//                 displayService.DeleteByIdAsync(displayId));
//
//             ClassicAssert.AreEqual("Display with id: display-id-1 not found => Cannot delete display!", ex.Message);
//         }
//
//
//         [Test]
//         public async Task GetAllAsync_ShouldReturnAllDisplays()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//             var displays = new List<Display>
//             {
//                 new() { Id = "display-id-1" },
//                 new() { Id = "display-id-2" }
//             };
//             displayRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(displays);
//
//             // Act
//             var result = await displayService.GetAllAsync();
//
//             // Assert
//             ClassicAssert.IsTrue(displays.OrderBy(d => d.Id).SequenceEqual(result.OrderBy(d => d.Id)));
//         }
//
//
//         [Test]
//         public async Task GetByIdAsync_ShouldReturnDisplayById()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//             var expectedDisplay = new Display { Id = "display-id-1" };
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync("display-id-1")).ReturnsAsync(expectedDisplay);
//
//             // Act
//             var result = await displayService.GetByIdAsync("display-id-1");
//
//             // Assert
//             ClassicAssert.AreEqual(expectedDisplay.Id, result.Id);
//         }
//
//
//         [Test]
//         public async Task GetByIdAsync_ThrowsExceptionWhenDisplayNotFound()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object, _groupServiceMock.Object);
//             _displayRepositoryMock.Setup(repo => repo.GetByIdAsync("display-id-1")).ReturnsAsync((Display)null);
//
//             // Act & Assert
//             var ex = Assert.ThrowsAsync<InvalidOperationException>(() => displayService.GetByIdAsync("display-id-1"));
//             ClassicAssert.AreEqual("Display with id: display-id-1 not found!", ex.Message);
//         }
//
//         [Test]
//         public async Task GetCurrentViewFromDeviceAsync_ReturnsCurrentView()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var groupServiceMock = new Mock<IGroupService>();
//
//             var displayId = "display-id-1";
//             var viewId = "view-id-1";
//             var ipAddress = IPAddress.Parse("192.168.1.1");
//             var viewPort = "8080";
//             var groupId = "group-id-1";
//
//             var display = new Display { Id = displayId, GroupId = groupId };
//             var expectedView = new View { Id = viewId };
//
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//             groupServiceMock.Setup(service => service.GetCurrentViewAsync(groupId)).ReturnsAsync(expectedView);
//
//             var displayService = new DisplayService(displayRepositoryMock.Object, _viewServiceMock.Object,
//                 groupServiceMock.Object);
//
//             // Act
//             var result = await displayService.GetCurrentViewFromDeviceAsync(displayId);
//
//             // Assert
//             ClassicAssert.AreEqual(expectedView, result);
//             displayRepositoryMock.Verify(repo => repo.UpdateAsync(display), Times.Once);
//             ClassicAssert.AreEqual(ipAddress, display.IpAddress);
//             ClassicAssert.AreEqual(viewPort, display.ViewPort);
//             ClassicAssert.IsNotNull(display.LastOnline);
//         }
//         
//         [Test]
//         public async Task GeneratePairingCodeAsync_ReturnsValidPairingCode()
//         {
//             // Arrange
//             var displayRepositoryMock = new Mock<IDisplayRepository>();
//             var displayService = new DisplayService(displayRepositoryMock.Object,
//                 _viewServiceMock.Object,
//                 _groupServiceMock.Object);
//             var displayId = "display-id-1";
//             var display = new Display { Id = displayId };
//
//             displayRepositoryMock.Setup(repo => repo.GetByIdAsync(displayId)).ReturnsAsync(display);
//
//             // Act
//             var result = await displayService.RegeneratePairingCodeAsync(displayId);
//
//             // Assert
//             displayRepositoryMock.Verify(repo => repo.UpdateAsync(display), Times.Once);
//             ClassicAssert.AreEqual(result, display.PairingCode.ToString());
//         }
//
//     }
// }