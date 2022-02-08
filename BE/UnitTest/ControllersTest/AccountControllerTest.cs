using API.Controllers;
using Business.Communication;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.ControllersTest
{
    public class AccountControllerTest
    {
        [Fact]
        public async Task GetByIdAsync_ReturnOk_WithData()
        {
            // Arrange
            int id = 1;
            var tempResoucre = new AccountResource()
            {
                Id = 1,
                Name = "test"
            };

            var monitor = Mock.Of<IOptionsMonitor<ResponseMessage>>(_ => _.CurrentValue == new ResponseMessage());

            var mockAccountService = new Mock<IAccountService>();
            var mockImageService = new Mock<IImageService>();

            var tempResult = new BaseResponse<AccountResource>(tempResoucre);
            mockAccountService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(tempResult);

            var controller = new AccountController(mockAccountService.Object, mockImageService.Object, monitor);

            // Act
            var result = await controller.GetByIdAsync(id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnObjectExpected_WithData()
        {
            // Arrange
            int id = 1;
            var tempResoucre = new AccountResource()
            {
                Id = 1,
                Name = "test"
            };

            var monitor = Mock.Of<IOptionsMonitor<ResponseMessage>>(_ => _.CurrentValue == new ResponseMessage());

            var mockAccountService = new Mock<IAccountService>();
            var mockImageService = new Mock<IImageService>();

            var tempResult = new BaseResponse<AccountResource>(tempResoucre);
            mockAccountService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync(tempResult);

            var controller = new AccountController(mockAccountService.Object, mockImageService.Object, monitor);

            // Act
            var result = await controller.GetByIdAsync(id);

            // Assert
            var castObject = (result as OkObjectResult).Value as BaseResponse<AccountResource>;
            Assert.Equal(id, castObject.Resource.Id);
            Assert.Equal("test", castObject.Resource.Name);
        }
    }
}
