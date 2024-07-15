using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models.DTO;


namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BatchController : ControllerBase
    {
        private readonly IBatchRepository _batchRepo;
        private readonly IMapper _mapper;

        public BatchController(IBatchRepository batchRepo, IMapper mapper)
        {
            _batchRepo = batchRepo;
            _mapper = mapper;
        }

        // GET: api/Batch
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetBatches()
        {
            var batches = await _batchRepo.GetAllAsync();
            var response = new ApiResponse
            {
                Result = _mapper.Map<IEnumerable<Batch>>(batches),
                StatusCode = HttpStatusCode.OK
            };
            return Ok(response);
        }

        // GET: api/Batch/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetBatch(int id)
        {
            var batch = await _batchRepo.GetAsync(id);

            if (batch == null)
            {
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound });
            }

            var response = new ApiResponse
            {
                Result = _mapper.Map<Batch>(batch),
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

            var batch = new Batch
            {
                batchname = createbatchDTO.batchname
            };

            await _batchRepo.CreateAsync(batch);
            await _batchRepo.SaveAsync();

            var response = new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = batch,
                Message = { "Batch created successfully" }
            };

            return CreatedAtAction(nameof(GetBatch), new { id = batch.batchid }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(int id, [FromBody] UpdateBatchDTO batchDTO)
        {
            if (id != batchDTO.BatchId)
            {
                return BadRequest(new ApiResponse { StatusCode = HttpStatusCode.BadRequest, Message = { "Batch ID mismatch" } });
            }

            var batch = await _batchRepo.GetAsync(id);
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
                    Result = batch,
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


        // DELETE: api/Batch/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _batchRepo.GetAsync(id);
            if (batch == null)
            {
                return NotFound(new ApiResponse { StatusCode = HttpStatusCode.NotFound, Message = { "Batch not found" } });
            }

            await _batchRepo.RemoveAsync(batch);
            await _batchRepo.SaveAsync();

            return NoContent();
        }
    }
}
