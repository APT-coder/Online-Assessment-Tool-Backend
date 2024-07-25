using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using AutoMapper;

public class AssessmentService : IAssessmentService
{
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IMapper _mapper;

    public AssessmentService(IAssessmentRepository assessmentRepository, IMapper mapper)
    {
        _assessmentRepository = assessmentRepository;
        _mapper = mapper;
    }

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

    public async Task DeleteAssessmentAsync(int assessmentId)
    {
        await _assessmentRepository.DeleteAssessmentAsync(assessmentId);
    }
}
