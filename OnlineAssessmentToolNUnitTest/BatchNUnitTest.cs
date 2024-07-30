using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using OnlineAssessmentTool.Controllers;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace OnlineAssessmentToolNUnitTest
{
    [TestFixture]
    public class BatchNUnitTest
    {
        private Mock<IBatchRepository> _batchRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<BatchController>> _loggerMock;
        private BatchController _controller;

        [SetUp]
        public void Setup()
        {
            _batchRepositoryMock = new Mock<IBatchRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BatchController>>();
            _controller = new BatchController(_batchRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetBatches_ShouldReturnBatches_WhenBatchesExist()
        {
            // Arrange
            var batches = new List<Batch>
            {
                new Batch { batchid = 1, batchname = "Batch 1" },
                new Batch { batchid = 2, batchname = "Batch 2" }
            };
            _batchRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(batches);
            _mapperMock.Setup(m => m.Map<IEnumerable<Batch>>(It.IsAny<IEnumerable<Batch>>())).Returns(batches);

            // Act
            var result = await _controller.GetBatches();
            var actionResult = result.Result as OkObjectResult;
            var apiResponse = actionResult?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult?.StatusCode);
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(batches, apiResponse.Result);
        }

        [Test]
        public async Task GetBatch_ShouldReturnBatch_WhenBatchExists()
        {
            // Arrange
            var batchId = 1;
            var batch = new Batch { batchid = batchId, batchname = "Batch 1" };
            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(batchId)).ReturnsAsync(batch);
            _mapperMock.Setup(m => m.Map<Batch>(It.IsAny<Batch>())).Returns(batch);

            // Act
            var result = await _controller.GetBatch(batchId);
            var actionResult = result.Result as OkObjectResult;
            var apiResponse = actionResult?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult?.StatusCode);
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(batch, apiResponse.Result);
        }

        [Test]
        public async Task CreateBatch_ShouldReturnCreated_WhenBatchIsValid()
        {
            // Arrange
            var createBatchDto = new CreateBatchDTO { batchname = "New Batch" };
            var batch = new Batch { batchid = 1, batchname = "New Batch" };
            _mapperMock.Setup(m => m.Map<Batch>(createBatchDto)).Returns(batch);
            _batchRepositoryMock.Setup(repo => repo.AddAsync(batch)).Returns(Task.FromResult(true));
            _mapperMock.Setup(m => m.Map<CreateBatchDTO>(batch)).Returns(createBatchDto);

            // Act
            var result = await _controller.CreateBatch(createBatchDto);
            var actionResult = result.Result as CreatedAtActionResult;
            var apiResponse = actionResult?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.Created, actionResult?.StatusCode);
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(true, apiResponse.IsSuccess);
            Assert.AreEqual(createBatchDto, apiResponse.Result);
            Assert.Contains("Batch created successfully", apiResponse.Message);
        }

        [Test]
        public async Task UpdateBatch_ShouldReturnOk_WhenBatchIsUpdated()
        {
            // Arrange
            var batchId = 1;
            var updateBatchDto = new UpdateBatchDTO { BatchId = batchId, batchname = "Updated Batch" };
            var batch = new Batch { batchid = batchId, batchname = "Batch" };
            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(batchId)).ReturnsAsync(batch);
            _mapperMock.Setup(m => m.Map(updateBatchDto, batch)).Returns(batch);
            _batchRepositoryMock.Setup(repo => repo.UpdateAsync(batch)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<Batch>(batch)).Returns(batch);

            // Act
            var result = await _controller.UpdateBatch(batchId, updateBatchDto);
            var actionResult = result as OkObjectResult;
            var apiResponse = actionResult?.Value as ApiResponse;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual((int)HttpStatusCode.OK, actionResult?.StatusCode);
            Assert.IsNotNull(apiResponse);
            Assert.AreEqual(true, apiResponse.IsSuccess);
            Assert.AreEqual(batch, apiResponse.Result);
            Assert.Contains("Batch updated successfully", apiResponse.Message);
        }

        [Test]
        public async Task DeleteBatch_ShouldReturnNoContent_WhenBatchIsDeleted()
        {
            // Arrange
            var batchId = 1;
            var batch = new Batch { batchid = batchId, batchname = "Batch to Delete" };
            _batchRepositoryMock.Setup(repo => repo.GetByIdAsync(batchId)).ReturnsAsync(batch);
            _batchRepositoryMock.Setup(repo => repo.DeleteAsync(batch)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteBatch(batchId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
