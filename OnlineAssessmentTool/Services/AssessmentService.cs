using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AssessmentService : IAssessmentService
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IMapper _mapper;

    public AssessmentService(IAssessmentRepository assessmentRepository, IMapper mapper)
    {
        _assessmentRepository = assessmentRepository;
        _mapper = mapper;
    }

    /* public async Task<IEnumerable<AssessmentWithCreatedOnDTO>> GetAllAssessmentsAsync()
     {
         var assessments = await _assessmentRepository.GetAllAssessmentsAsync();
         return assessments.Select(a => new AssessmentWithCreatedOnDTO
         {
             Assessment = _mapper.Map<AssessmentDTO>(a),
             CreatedOn = a.CreatedOn
         });
     }*/

    public async Task<Assessment> GetAssessmentByIdAsync(int assessmentId)
    {
        return await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
    }



    public async Task<Assessment> CreateAssessmentAsync(AssessmentDTO assessmentDTO)
    {
        var assessment = _mapper.Map<Assessment>(assessmentDTO);
        assessment.CreatedOn = DateTime.UtcNow;
        await _assessmentRepository.AddAsync(assessment);
        return assessment;
    }



    /*public async Task<Assessment> UpdateAssessmentAsync(int assessmentId, AssessmentDTO assessmentDTO)
    {
        var existingAssessment = await _assessmentRepository.GetAssessmentByIdAsync(assessmentId);
        if (existingAssessment != null)
        {
            existingAssessment.AssessmentName = assessmentDTO.AssessmentName;
            existingAssessment.CreatedBy = assessmentDTO.CreatedBy;
           _assessmentRepository.UpdateAssessmentAsync(existingAssessment);
            return existingAssessment;
        }

        return null;
    }*/


    public async Task DeleteAssessmentAsync(int assessmentId)
    {
        await _assessmentRepository.DeleteAssessmentAsync(assessmentId);
    }


}
