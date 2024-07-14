using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class BatchController : ControllerBase
    {
        private readonly APIContext _context;

        public BatchController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Batch
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetBatches()
        {
            var batches = await _context.batch.ToListAsync();
            return Ok(batches);
        }

        // GET: api/Batch/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            var batch = await _context.batch.FindAsync(id);

            if (batch == null)
            {
                return NotFound();
            }

            return batch;
        }

        // POST: api/Batch
        [HttpPost]
        public async Task<ActionResult<Batch>> CreateBatch(Batch batch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.batch.Add(batch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBatch), new { id = batch.batchid }, batch);
        }

        // PUT: api/Batch/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(int id, Batch batch)
        {
            if (id != batch.batchid)
            {
                return BadRequest();
            }

            _context.Entry(batch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Batch/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            var batch = await _context.batch.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            _context.batch.Remove(batch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatchExists(int id)
        {
            return _context.batch.Any(e => e.batchid == id);
        }
    }
}


