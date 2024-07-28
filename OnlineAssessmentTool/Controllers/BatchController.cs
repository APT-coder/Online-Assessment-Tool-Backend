using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models.DTO;
using System.Net;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BatchController : ControllerBase
    {
        private readonly IBatchRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BatchController> _logger;

        public BatchController(IBatchRepository batchRepo, IMapper mapper, ILogger<BatchController> logger)
        {
            _repository = batchRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetBatches()
        {
            _logger.LogInformation("Fetching all batches");
            var batches = await _repository.GetAllAsync();
            var mappedBatches = _mapper.Map<IEnumerable<Batch>>(batches);
            var response = new ApiResponse
            {
                Result = mappedBatches,
                StatusCode = HttpStatusCode.OK
            };
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetBatch(int id)
        {
            _logger.LogInformation("Fetching batch with ID {BatchId}", id);
            var batch = await _repository.GetByIdAsync(id);
            if (batch == null)
            {
                _logger.LogWarning("Batch with ID {BatchId} not found", id);
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound });
            }
            var mappedBatch = _mapper.Map<Batch>(batch);
            var response = new ApiResponse
            {
                Result = mappedBatch,
                StatusCode = HttpStatusCode.OK
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateBatch([FromBody] CreateBatchDTO createbatchDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CreateBatch");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            try
            {
                _logger.LogInformation("Creating new batch");
                var batch = _mapper.Map<Batch>(createbatchDTO);
                await _repository.AddAsync(batch);
                var response = new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = _mapper.Map<CreateBatchDTO>(batch),
                    Message = { "Batch created successfully" }
                };
                return CreatedAtAction(nameof(GetBatch), new { id = batch.batchid }, response);
            }
            catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
            {
                _logger.LogError(ex, "Duplicate batch ID detected");
                return Conflict(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Conflict,
                    Message = { "Duplicate batch ID detected. Please retry." }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(int id, [FromBody] UpdateBatchDTO batchDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for UpdateBatch");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            if (id != batchDTO.BatchId)
            {
                _logger.LogWarning("Batch ID mismatch for UpdateBatch");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = { "Batch ID mismatch" }
                });
            }
            var batch = await _repository.GetByIdAsync(id);
            if (batch == null)
            {
                _logger.LogWarning("Batch with ID {BatchId} not found for update", id);
                return NotFound(new ApiResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = { "Batch not found" }
                });
            }
            _mapper.Map(batchDTO, batch);
            try
            {
                _logger.LogInformation("Updating batch with ID {BatchId}", id);
                await _repository.UpdateAsync(batch);
                var response = new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = _mapper.Map<Batch>(batch),
                    Message = { "Batch updated successfully" }
                };
                return Ok(response);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id))
                {
                    _logger.LogWarning("Batch with ID {BatchId} not found during concurrency update", id);
                    return NotFound(new ApiResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = { "Batch not found during update" }
                    });
                }
                else
                {
                    _logger.LogError("Concurrency error during batch update");
                    throw;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            _logger.LogInformation("Deleting batch with ID {BatchId}", id);
            var batch = await _repository.GetByIdAsync(id);
            if (batch == null)
            {
                _logger.LogWarning("Batch with ID {BatchId} not found for deletion", id);
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = { "Batch not found" } });
            }
            await _repository.DeleteAsync(batch);
            _logger.LogInformation("Batch with ID {BatchId} deleted successfully", id);
            return NoContent();
        }
    }
}
