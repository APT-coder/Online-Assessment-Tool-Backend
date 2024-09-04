namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeAverageScoreDto
    {
            public int TraineeId { get; set; }
            public string TraineeName { get; set; }
            public string BatchName { get; set; }
            public double AveragePercentageScore { get; set; }
            public int TotalAssessmentsCompleted { get; set; }
            public double TotalScore { get; set; }
            public DateTime LastAssessmentDate { get; set; }
            public int RankInBatch { get; set; }
            public double BatchAverageScore { get; set; }
    }
}
