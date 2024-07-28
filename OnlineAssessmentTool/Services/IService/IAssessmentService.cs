using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;

public interface IAssessmentService
{
    Task<Assessment> GetAssessmentByIdAsync(int assessmentId);
    Task<Assessment> CreateAssessmentAsync(AssessmentDTO assessmentDTO);
    Task DeleteAssessmentAsync(int assessmentId);
}
