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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>()
               .Property(r => r.Id)
               .UseIdentityColumn();

            modelBuilder.Entity<Role>()
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
                .UsingEntity(j => j.ToTable("RolePermission"));

            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");

            modelBuilder.Entity<Assessment>().HasKey(a => a.AssessmentId);
            modelBuilder.Entity<Question>().HasKey(q => q.QuestionId);
            modelBuilder.Entity<QuestionOption>().HasKey(qo => qo.QuestionOptionId);

            modelBuilder.Entity<Assessment>()
                .HasMany(a => a.Questions)
                .WithOne(q => q.Assessment)
                .HasForeignKey(q => q.AssessmentId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.QuestionOptions)
                .WithOne(qo => qo.Question)
                .HasForeignKey(qo => qo.QuestionId);

            modelBuilder.Entity<Trainer>().ToTable("Trainers");
            modelBuilder.Entity<TrainerBatch>().ToTable("TrainerBatches");

            modelBuilder.Entity<Trainer>()
               .HasOne(t => t.User)
               .WithMany()
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as per your requirement

            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Role)
                .WithMany()
                .HasForeignKey(t => t.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrainerBatch>()
              .HasKey(tb => tb.TrainerBatchId); // Specify the primary key

            // Configure relationships if necessary
            modelBuilder.Entity<TrainerBatch>()
                .HasOne(tb => tb.Trainer)
                .WithMany() // Adjust as per your relationship
                .HasForeignKey(tb => tb.TrainerId);

            modelBuilder.Entity<TrainerBatch>()
                .HasOne(tb => tb.Batch)
                .WithMany() // Adjust as per your relationship
                .HasForeignKey(tb => tb.BatchId);
        }
    }

}
