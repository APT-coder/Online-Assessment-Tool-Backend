using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Services;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ILPIntegrationController : ControllerBase
    {
        private readonly ILPIntegrationService _ilpIntegrationService;

        public ILPIntegrationController(ILPIntegrationService ilpIntegrationService)
        {
            _ilpIntegrationService = ilpIntegrationService;
        }

        [HttpGet("GetAverageAndTotalScore")]
        public async Task<IActionResult> GetAverageAndTotalScore(string traineeEmail, int scheduledAssessmentId)
        {
            var result = await _ilpIntegrationService.GetAverageAndTotalScore(traineeEmail, scheduledAssessmentId);
            if (result == (0, 0))
            {
                return NotFound(new { message = "No scores found for the given trainee email and scheduled assessment ID." });
            }
            return Ok(new { AverageScore = result.AverageScore, TotalScore = result.TotalScore });
        }

        [HttpGet("{batchname}")]
        public async Task<IActionResult> GetScheduledAssessmentDetails(string batchname)
        {
            var result = await _ilpIntegrationService.GetScheduledAssessmentDetails(batchname);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
