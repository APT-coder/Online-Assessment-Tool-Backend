namespace OnlineAssessmentTool.Models.DTO
{
    public class UpdateUserRequestDTO
    {
        public UpdateUserDTO UpdateUser { get; set; }
        public TrainerDTO Trainer { get; set; }
        public TraineeDTO Trainee { get; set; }
    }
}
