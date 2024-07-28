using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options) { }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Batch> batch { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainerBatch> TrainerBatches { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<ScheduledAssessment> ScheduledAssessments { get; set; }
        public DbSet<TraineeAnswer> TraineeAnswers { get; set; }
        public DbSet<AssessmentScore> AssessmentScores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
              .Property(r => r.Id)
              .UseIdentityColumn();

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    j => j.HasOne<Permission>()
                          .WithMany()
                          .HasForeignKey("PermissionId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Role>()
                          .WithMany()
                          .HasForeignKey("RoleId")
                          .OnDelete(DeleteBehavior.Cascade));

            modelBuilder.Entity<TrainerBatch>()
                 .HasKey(ba => new { ba.Trainer_id, ba.Batch_id });

            modelBuilder.Entity<TrainerBatch>()
                      .HasOne(ba => ba.Trainer)
                      .WithMany(b => b.TrainerBatch)
                      .HasForeignKey(ba => ba.Trainer_id);

            modelBuilder.Entity<TrainerBatch>()
                    .HasOne(ba => ba.Batch)
                    .WithMany(b => b.TrainerBatch)
                    .HasForeignKey(ba => ba.Batch_id);

            modelBuilder.Entity<Users>().ToTable("Users");

            modelBuilder.Entity<Role>().ToTable("Roles");

            modelBuilder.Entity<Trainer>().ToTable("Trainers");

            modelBuilder.Entity<Trainer>()
               .HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Role)
                .WithMany()
                .HasForeignKey(t => t.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
