using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Services.IService;
using OnlineAssessmentTool.Models.DTO;
using static System.Net.WebRequestMethods;

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
            var emailResponse = await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body, emailRequest.Attachments);
            if (emailResponse.Successful)
            {
                return Ok(new { message = "Email sent successfully" });
            }
            else
            {
                return StatusCode(500, "Failed to send Email.");
            }
        }
    }
}
