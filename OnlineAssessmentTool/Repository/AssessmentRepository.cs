/*using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
       
        *//*public AssessmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync()
        {
            return await _context.Assessments
                                 .Include(a => a.Questions)
                                 .ThenInclude(q => q.QuestionOptions)
                                 .ToListAsync();
        }
        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task AddAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentId == 0) 
            {
                await _context.Assessments.AddAsync(assessment);
            }
            await _context.SaveChangesAsync();
        }


       
        public async Task UpdateAssessmentAsync(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            await _context.SaveChangesAsync();
        }

       

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }

       *//*

        private readonly ApplicationDbContext _context;
        public AssessmentRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }


    }
}
*/


using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Repository
{
    public class AssessmentRepository : Repository<Assessment>, IAssessmentRepository
    {
        private readonly APIContext _context;

        public AssessmentRepository(APIContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            return await _context.Assessments.Include(a => a.Questions)
                                             .ThenInclude(q => q.QuestionOptions)
                                             .FirstOrDefaultAsync(a => a.AssessmentId == id);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await GetAssessmentByIdAsync(assessmentId);
            if (assessment != null)
            {
                _context.Assessments.Remove(assessment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AssessmentOverviewDTO>> GetAllAssessmentOverviewsAsync()
        {
            return await (from sa in _context.ScheduledAssessments
                          join a in _context.Assessments on sa.AssessmentId equals a.AssessmentId
                          join t in _context.Trainers on a.CreatedBy equals t.UserId
                          join u in _context.Users on t.UserId equals u.UserId
                          join ats in _context.AssessmentScores on sa.ScheduledAssessmentId equals ats.ScheduledAssessmentId
                          group new { sa, a, t, u, ats } by new
                          {
                              sa.AssessmentId,
                              a.AssessmentName,
                              sa.ScheduledDate,
                              Trainer = u.Username
                          } into g
                          select new AssessmentOverviewDTO
                          {
                              AssessmentName = g.Key.AssessmentName,
                              Date = g.Key.ScheduledDate,
                              Trainer = g.Key.Trainer,
                              NumberOfAttendees = g.Count(),
                              MaximumScore = 100, // Replace with a value from the database if applicable
                              HighestScore = g.Max(x => x.ats.AvergeScore),
                              LowestScore = g.Min(x => x.ats.AvergeScore)
                          }).ToListAsync();
        }


        public async Task<IEnumerable<TraineeScoreDTO>> GetLowPerformersByAssessmentIdAsync(int scheduledAssessmentId)
        {
            var lowPerformers = await (
                from ats in _context.AssessmentScores
                where ats.ScheduledAssessmentId == scheduledAssessmentId
                join t in _context.Trainees on ats.TraineeId equals t.TraineeId
                join u in _context.Users on t.UserId equals u.UserId
                orderby ats.AvergeScore ascending
                select new TraineeScoreDTO
                {
                    TraineeName = u.Username,
                    Score = ats.AvergeScore
                }
            )
            .Take(5)
            .ToListAsync();

            return lowPerformers;
        }

        public async Task<IEnumerable<TraineeScoreDTO>> GetHighPerformersByAssessmentIdAsync(int scheduledAssessmentId)
        {
            var highPerformers = await (
                from u in _context.Users
                join t in _context.Trainees on u.UserId equals t.UserId
                join s in _context.AssessmentScores on t.TraineeId equals s.TraineeId
                join sa in _context.ScheduledAssessments on s.ScheduledAssessmentId equals sa.ScheduledAssessmentId
                where s.ScheduledAssessmentId == scheduledAssessmentId
                group new { u, s } by new { u.UserId, u.Username } into g
                select new TraineeScoreDTO
                {
                    TraineeName = g.Key.Username,
                    Score = g.Sum(x => x.s.AvergeScore)
                }
                into result
                orderby result.Score descending
                select result
            ).Take(5).ToListAsync();

            return highPerformers;
        }

        public async Task<List<AssessmentTableDTO>> GetAssessmentTable()
        {
            var result = from a in _context.Assessments
                         join sa in _context.ScheduledAssessments on a.AssessmentId equals sa.AssessmentId
                         join b in _context.batch on sa.BatchId equals b.batchid
                         select new AssessmentTableDTO
                         {
                             AssessmentName = a.AssessmentName,
                             BatchName = b.batchname,
                             CreatedOn = a.CreatedOn,
                             ScheduledDate = sa.ScheduledDate,
                             Status = sa.Status.ToString()
                         };

            return await result.ToListAsync();
        }
    }
}
