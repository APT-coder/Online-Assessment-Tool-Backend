using FluentEmail.Core.Models;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Services.IService
{
    public interface IEmailService
    {
        Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, List<AttachmentDTO> attachments = null);
    }
}
