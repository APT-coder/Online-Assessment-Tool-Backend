using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Services.IService;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] EmailRequestDTO emailRequest)
        {
            await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent successfully!");
        }
    }
}
