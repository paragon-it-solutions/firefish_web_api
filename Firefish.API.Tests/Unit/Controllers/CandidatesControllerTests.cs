using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firefish.API.Controllers;
using Firefish.Core.Contracts.Services;
using Firefish.Core.Models.Candidate.Requests;
using Firefish.Core.Models.Candidate.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Firefish.Tests.Controllers
{
    public class CandidatesControllerTests
    {
        private readonly Mock<ICandidateService> _mockCandidateService;
        private readonly CandidatesController _controller;

        public CandidatesControllerTests()
        {
            _mockCandidateService = new Mock<ICandidateService>();
            _controller = new CandidatesController(_mockCandidateService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithCandidates()
        {
            // Arrange
            var expectedCandidates = new List<CandidateListItemResponseModel>
            {
                new CandidateListItemResponseModel { Id = 1, Name = "John Doe" },
                new CandidateListItemResponseModel { Id = 2, Name = "Jane Smith" },
            };
            _mockCandidateService
                .Setup(s => s.GetAllCandidatesAsync())
                .ReturnsAsync(expectedCandidates);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCandidates = Assert.IsAssignableFrom<
                IEnumerable<CandidateListItemResponseModel>
            >(okResult.Value);
            Assert.Equal(expectedCandidates, returnedCandidates);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerErrorWhenExceptionOccurs()
        {
            // Arrange
            _mockCandidateService
                .Setup(s => s.GetAllCandidatesAsync())
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.Get();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains(
                "An error occurred while getting candidates",
                objectResult.Value.ToString()
            );
        }

        [Fact]
        public async Task GetById_ReturnsOkResultWithCandidate()
        {
            // Arrange
            int candidateId = 1;
            var expectedCandidate = new CandidateDetailsResponseModel
            {
                Id = candidateId,
                Name = "John Doe",
            };
            _mockCandidateService
                .Setup(s => s.GetCandidateByIdAsync(candidateId))
                .ReturnsAsync(expectedCandidate);

            // Act
            var result = await _controller.Get(candidateId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCandidate = Assert.IsType<CandidateDetailsResponseModel>(okResult.Value);
            Assert.Equal(expectedCandidate, returnedCandidate);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundWhenCandidateDoesNotExist()
        {
            // Arrange
            int candidateId = 1;
            _mockCandidateService
                .Setup(s => s.GetCandidateByIdAsync(candidateId))
                .ThrowsAsync(new KeyNotFoundException("Candidate not found"));

            // Act
            var result = await _controller.Get(candidateId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.Contains("Candidate Not Found", objectResult.Value.ToString());
        }

        [Fact]
        public async Task Post_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var requestModel = new CandidateModifyRequestModel
            {
                FirstName = "John",
                Surname = "Doe",
            };
            var responseModel = new CandidateDetailsResponseModel { Id = 1, Name = "John Doe" };
            _mockCandidateService
                .Setup(s => s.CreateCandidateAsync(requestModel))
                .ReturnsAsync(responseModel);

            // Act
            var result = await _controller.Post(requestModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(CandidatesController.Get), createdAtActionResult.ActionName);
            Assert.Equal(responseModel.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(responseModel, createdAtActionResult.Value);
        }

        [Fact]
        public async Task Put_ReturnsOkResultWithUpdatedCandidate()
        {
            // Arrange
            int candidateId = 1;
            var requestModel = new CandidateModifyRequestModel
            {
                FirstName = "John",
                Surname = "Doe",
            };
            var responseModel = new CandidateDetailsResponseModel
            {
                Id = candidateId,
                Name = "John Doe",
            };
            _mockCandidateService
                .Setup(s => s.UpdateExistingCandidateAsync(candidateId, requestModel))
                .ReturnsAsync(responseModel);

            // Act
            var result = await _controller.Put(candidateId, requestModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCandidate = Assert.IsType<CandidateDetailsResponseModel>(okResult.Value);
            Assert.Equal(responseModel, returnedCandidate);
        }

        [Fact]
        public async Task Put_ReturnsNotFoundWhenCandidateDoesNotExist()
        {
            // Arrange
            int candidateId = 1;
            var requestModel = new CandidateModifyRequestModel
            {
                FirstName = "John",
                Surname = "Doe",
            };
            _mockCandidateService
                .Setup(s => s.UpdateExistingCandidateAsync(candidateId, requestModel))
                .ThrowsAsync(new KeyNotFoundException("Candidate not found"));

            // Act
            var result = await _controller.Put(candidateId, requestModel);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.Contains("Candidate Not Found", objectResult.Value.ToString());
        }
    }
}
