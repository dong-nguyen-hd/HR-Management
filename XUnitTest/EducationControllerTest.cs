using System;
using Xunit;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using HR_Management.Controllers;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Mapping.Education;
using HR_Management.Domain.Services.Communication;
using HR_Management.Domain.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HR_Management.Resources.Education;

namespace XUnitTest
{
    public class EducationControllerTest : IDisposable
    {
        // Use mock
        Mock<IEducationRepository> mockRepo;
        Mock<IEducationService> mockService;
        
        IMapper mapper;

        ModelToResourceProfile mapModelToRes;
        ResourceToModelProfile mapResToModel;

        MapperConfiguration configuration;

        public EducationControllerTest()
        {
            mockRepo = new Mock<IEducationRepository>();
            mockService = new Mock<IEducationService>();

            mapModelToRes = new ModelToResourceProfile();
            mapResToModel = new ResourceToModelProfile();

            configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(mapModelToRes);
                cfg.AddProfile(mapResToModel);
            });

            mapper = configuration.CreateMapper();
        }
        // Using for clean up code
        public void Dispose()
        {
            mockRepo = null;
            mockService = null;
            mapper = null;
            configuration = null;
            mapModelToRes = null;
            mapResToModel = null;
        }

        #region Resource
        /// <summary>
        /// Resource using for test
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        private List<Education> Resource(bool isEmpty = false)
        {
            var listTemp = new List<Education>();
            if (!isEmpty)
            {
                listTemp.Add(new Education
                {
                    Id = 1,
                    CollegeName = "1",
                    Major = "1",
                    OrderIndex = 1,
                    Status = true,
                    StartDate = new DateTime(2020, 12, 12),
                    EndDate = new DateTime(2021, 12, 12),
                    PersonId = 1
                });

                listTemp.Add(new Education
                {
                    Id = 2,
                    CollegeName = "2",
                    Major = "2",
                    OrderIndex = 2,
                    Status = true,
                    StartDate = new DateTime(2020, 12, 12),
                    EndDate = new DateTime(2021, 12, 12),
                    PersonId = 7
                });

                listTemp.Add(new Education
                {
                    Id = 3,
                    CollegeName = "3",
                    Major = "3",
                    OrderIndex = 3,
                    Status = true,
                    StartDate = new DateTime(2020, 12, 12),
                    EndDate = new DateTime(2021, 12, 12),
                    PersonId = 7
                });
            }
            return listTemp;
        }

        /// <summary>
        /// Resource using for test
        /// </summary>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        private async Task<EducationResponse> GetResource(int personId, bool isEmpty = false)
        {
            // Get data from Resource
            var listTemp = Resource(isEmpty);
            // Person not exist any record
            if (isEmpty) return new EducationResponse(listTemp);
            // Get record with PersonId
            List<Education> resource = new List<Education>();
            foreach (var item in listTemp)
            {
                if (item.PersonId == personId)
                    resource.Add(item);
            }
            if (resource.Count == 0)
            {
                var educationResponse = new EducationResponse($"PersonId '{personId}' not exist.");
                return educationResponse;
            }
            else
            {
                var educationResponse = new EducationResponse(resource);
                return educationResponse;
            }
        }

        /// <summary>
        /// Resource using for test
        /// </summary>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        private async Task<EducationResponse> CreateResource(CreateEducationResource education, bool isSuccess = true)
        {
            if(!isSuccess) return new EducationResponse("Error");

            Education tempEducation = new Education()
            {
                Id = 1,
                CollegeName = education.CollegeName,
                Major = education.Major,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                Status = true,
                PersonId = 1
            };
            var temp = new EducationResponse(tempEducation);

            return temp;
        }

        /// <summary>
        /// Resource using for test
        /// </summary>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        private async Task<EducationResponse> UpdateResource(int id, UpdateEducationResource education, bool isSuccess = true)
        {
            if (!isSuccess) return new EducationResponse("Error");
            if (id != 1) return new EducationResponse("Education not exist.");
            Education tempEducation = new Education()
            {
                Id = 1,
                CollegeName = education.CollegeName,
                Major = education.Major,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                Status = true,
                PersonId = 1
            };
            return new EducationResponse(tempEducation);
        }

        /// <summary>
        /// Resource using for test
        /// </summary>
        /// <param name="isEmpty"></param>
        /// <returns></returns>
        private async Task<EducationResponse> DeleteResource(int id, bool isEmpty = false)
        {
            // Get data from Resource
            var listTemp = Resource(isEmpty);
            // Get record with PersonId
            List<Education> resource = new List<Education>();
            foreach (var item in listTemp)
            {
                if (item.Id == id)
                    resource.Add(item);
            }
            if (resource.Count == 0)
            {
                var educationResponse = new EducationResponse("Education not exist.");
                return educationResponse;
            }
            else
            {
                var educationResponse = new EducationResponse(resource);
                return educationResponse;
            }
        }
        #endregion

        #region Get method
        [Fact]
        public void GetAllWithPersonIdAsync_Returns200OK_WhenIsSuccessful()
        {
            // Arrange
            mockService.Setup(sv => sv.ListAsync(1)).Returns(GetResource(1));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.GetAllWithPersonIdAsync(1);
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllWithPersonIdAsync_Returns400BadRequest_WhenPersonIdNotExist()
        {
            // Arrange
            mockService.Setup(sv => sv.ListAsync(0)).Returns(GetResource(0));

            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.GetAllWithPersonIdAsync(0);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllWithPersonIdAsync_ReturnsOneItem_WhenDBHasOneResource()
        {
            // Arrange
            mockService.Setup(sv => sv.ListAsync(1)).Returns(GetResource(1));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.GetAllWithPersonIdAsync(1);
            // Assert
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<EducationResource>;
            Assert.Single(commands);
        }

        [Fact]
        public void GetAllWithPersonIdAsync_ReturnsTwoItems_WhenDBHasTwoResources()
        {
            // Arrange
            mockService.Setup(sv => sv.ListAsync(7)).Returns(GetResource(7));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.GetAllWithPersonIdAsync(7);
            // Assert
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<EducationResource>;
            Assert.Equal(2, commands.Count);
        }

        [Fact]
        public void GetAllWithPersonIdAsync_ReturnsEmpty_WhenDBIsEmpty()
        {
            // Arrange
            mockService.Setup(sv => sv.ListAsync(0)).Returns(GetResource(0, true));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.GetAllWithPersonIdAsync(0);
            // Assert
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<EducationResource>;
            Assert.Empty(commands);
        }
        #endregion

        #region Post method
        [Fact]
        public void CreateEducationAsync_Returns201_WhenIsSuccessful()
        {
            // Arrange
            mockService.SetReturnsDefault(CreateResource(new CreateEducationResource()));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.CreateEducationAsync(new CreateEducationResource());
            // Assert
            var obj = result.Result as ObjectResult;
            var statusCode = obj.StatusCode;
            Assert.Equal(201, statusCode);
        }

        [Fact]
        public void CreateEducationAsync_Returns400BadRequest_WhenIsFailed()
        {
            // Arrange
            mockService.SetReturnsDefault(CreateResource(new CreateEducationResource(), false));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.CreateEducationAsync(new CreateEducationResource());
            // Assert
            var obj = result.Result as ObjectResult;
            var statusCode = obj.StatusCode;
            Assert.Equal(400, statusCode);
        }

        [Fact]
        public void CreateEducationAsync_ReturnsObject_WhenIsSuccessful()
        {
            // Arrange
            CreateEducationResource tempCreateEducationResource = new CreateEducationResource()
            {
                CollegeName = "1",
                Major = "1",
                StartDate = new DateTime(2020, 12, 12),
                EndDate = new DateTime(2021, 12, 12),
                PersonId = 1
            };
            mockService.SetReturnsDefault(CreateResource(tempCreateEducationResource));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.CreateEducationAsync(tempCreateEducationResource);
            // Assert
            var obj = result.Result as ObjectResult;
            var value = obj.Value as EducationResource;
            Assert.Equal("1", value.CollegeName);
        }
        #endregion

        #region Put method
        [Fact]
        public void UpdateEducationAsync_Returns200OK_WhenIsSuccessful()
        {
            // Arrange
            mockService.SetReturnsDefault(UpdateResource(1, new UpdateEducationResource()));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.UpdateEducationAsync(1, new UpdateEducationResource());
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateEducationAsync_Returns400BadRequest_WhenIsFailed()
        {
            // Arrange
            mockService.SetReturnsDefault(UpdateResource(1, new UpdateEducationResource(), false));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.UpdateEducationAsync(1, new UpdateEducationResource());
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateEducationAsync_Returns400BadRequest_WhenIdIsNotExistent()
        {
            // Arrange
            mockService.SetReturnsDefault(UpdateResource(3, new UpdateEducationResource()));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.UpdateEducationAsync(3, new UpdateEducationResource());
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void UpdateEducationAsync_ReturnsObjectUpdated_WhenIsSuccessful()
        {
            // Arrange
            UpdateEducationResource tempUpdateEducationResource = new UpdateEducationResource()
            {
                CollegeName = "1",
                Major = "1",
                StartDate = new DateTime(2020, 12, 12),
                EndDate = new DateTime(2021, 12, 12),
            };

            mockService.SetReturnsDefault(UpdateResource(1, tempUpdateEducationResource));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.UpdateEducationAsync(1, new UpdateEducationResource());
            // Assert
            var obj = result.Result as ObjectResult;
            var value = obj.Value as EducationResource;
            Assert.Equal("1", value.CollegeName);
        }
        #endregion

        #region Delete method
        [Fact]
        public void DeleteEducationAsync_Returns200OK_WhenIsSuccessful()
        {
            // Arrange
            mockService.Setup(sv => sv.DeleteAsync(1)).Returns(DeleteResource(1));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.DeleteEducationAsync(1);
            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void DeleteEducationAsync_Returns400BadRequest_WhenIdNotExistent()
        {
            // Arrange
            mockService.Setup(sv => sv.DeleteAsync(5)).Returns(DeleteResource(5));
            var controller = new EducationController(mockService.Object, mapper);
            // Act
            var result = controller.DeleteEducationAsync(5);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        #endregion
    }
}
