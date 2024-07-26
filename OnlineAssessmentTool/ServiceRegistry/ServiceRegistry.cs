using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services;
using OnlineAssessmentTool.Services.IService;
using System.Text.Json.Serialization;

namespace OnlineAssessmentTool.ServiceRegistry
{
    public static class ServiceRegistry
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<IPermissionsRepository, PermissionsRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITrainerRepository, TrainerRepository>();
            services.AddScoped<ITraineeRepository, TraineeRepository>();
            services.AddScoped<ITrainerBatchRepository, TrainerBatchRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IScheduledAssessmentRepository, ScheduledAssessmentRepository>();
            services.AddScoped<IScheduledAssessmentService, ScheduledAssessmentService>();
            services.AddScoped<ITraineeAnswerRepository, TraineeAnswerRepository>();
            services.AddScoped<IAssessmentScoreRepository, AssessmentScoreRepository>();
            services.AddScoped<IAssessmentScoreService, AssessmentScoreService>();
            services.AddScoped<IAssessmentPostService, AssessmentPostService>();

            services.AddAutoMapper(typeof(MappingConfig));

            services.AddDbContext<APIContext>(options =>
            {
                options.UseNpgsql(connectionString).EnableSensitiveDataLogging()
                                  .EnableDetailedErrors();
            });
        }
    }
}
