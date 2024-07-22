namespace OnlineAssessmentTool.Models.DTO
{
    public class CreateUserRequestDTO
    {
        public CreateUserDTO CreateUserDTO { get; set; }
        public TrainerDTO TrainerDTO { get; set; }
        public TraineeDTO TraineeDTO { get; set; }
        public List<int> BatchIds { get; set; }
    }
}
