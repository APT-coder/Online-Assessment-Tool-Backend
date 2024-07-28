using System.Net;

namespace OnlineAssessmentTool.Models.DTO
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Message = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Message { get; set; }
    }
}
