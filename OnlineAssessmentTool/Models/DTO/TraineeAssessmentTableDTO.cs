namespace OnlineAssessmentTool.Models.DTO
{
    public class TraineeAssessmentTableDTO
    {
        public string TraineeName { get; set; }
        public string IsPresent { get; set; }
        public int Score { get; set; }
    }

    public enum AttendanceStatus
    {
        Completed,
        Absent
    }

}
