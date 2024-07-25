using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models.DTO;
using System.Net;
using AutoMapper;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BatchController : ControllerBase
    {
        private readonly IBatchRepository _batchRepo;
        private readonly IMapper _mapper;

        public BatchController(IBatchRepository batchRepo, IMapper mapper)
        {
            _batchRepo = batchRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetBatches()
        {
            var batches = await _batchRepo.GetAllAsync();
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
            var batch = await _batchRepo.GetByIdAsync(id);

            if (batch == null)
            {
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
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var batch = _mapper.Map<Batch>(createbatchDTO);

                await _batchRepo.AddAsync(batch);
                await _batchRepo.SaveAsync();

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
            if (id != batchDTO.BatchId)
            {
                return BadRequest(new ApiResponse { StatusCode = HttpStatusCode.BadRequest, Message = { "Batch ID mismatch" } });
            }

            var batch = await _batchRepo.GetByIdAsync(id);
            if (batch == null)
            {
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = { "Batch not found" } });
            }

            _mapper.Map(batchDTO, batch);

            try
            {
                await _batchRepo.UpdateAsync(batch);
                await _batchRepo.SaveAsync();

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
                if (!await _batchRepo.ExistsAsync(id))
                {
                    return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = { "Batch not found during update" } });
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _batchRepo.GetByIdAsync(id);
            if (batch == null)
            {
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = { "Batch not found" } });
            }

            await _batchRepo.DeleteAsync(batch);
            await _batchRepo.SaveAsync();

            return NoContent();
        }
    }
}
