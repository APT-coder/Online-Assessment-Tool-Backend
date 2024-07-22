namespace OnlineAssessmentTool.Models
{
    public class TrainerBatch
    {
        public int Trainer_id { get; set; }

        public int Batch_id { get; set; }
        public Trainer Trainer { get; set; }
        public Batch Batch { get; set; }
    }
}
