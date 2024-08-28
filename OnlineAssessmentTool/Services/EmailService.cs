using FluentEmail.Core;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Services.IService;
using FluentEmail.Core.Models;

namespace OnlineAssessmentTool.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, List<AttachmentDTO> attachments = null)
        {
            var email = _fluentEmail
                .To(toEmail)
                .Subject(subject)
                .Body(body);

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var fileContent = Convert.FromBase64String(attachment.FileContent);
                    var attachmentStream = new MemoryStream(fileContent);

                    // Attach the file stream correctly
                    email = email.Attach(new Attachment
                    {
                        Filename = attachment.FileName,
                        Data = attachmentStream,
                        ContentType = "application/octet-stream" // Adjust content type if needed
                    });
                }
            }

            return await email.SendAsync();
        }
    }
}
