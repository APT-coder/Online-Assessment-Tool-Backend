using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAssessmentService
{
    /*Task<IEnumerable<AssessmentWithCreatedOnDTO>> GetAllAssessmentsAsync();*/
    Task<Assessment> GetAssessmentByIdAsync(int assessmentId);

    Task<Assessment> CreateAssessmentAsync(AssessmentDTO assessmentDTO);

    /* Task<Assessment> UpdateAssessmentAsync(int assessmentId, AssessmentDTO assessmentDTO);*/

    Task DeleteAssessmentAsync(int assessmentId);

}
