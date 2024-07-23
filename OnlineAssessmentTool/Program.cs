using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool;
using System.Text.Json.Serialization;
using OnlineAssessmentTool.Services.IService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IBatchRepository, BatchRepository>();
builder.Services.AddScoped<IPermissionsRepository, PermissionsRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<ITrainerBatchRepository, TrainerBatchRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IScheduledAssessmentRepository, ScheduledAssessmentRepository>();
builder.Services.AddScoped<ITraineeAnswerRepository, TraineeAnswerRepository>();
builder.Services.AddScoped<IAssessmentScoreRepository, AssessmentScoreRepository>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddDbContext<APIContext>(options =>
{
    options.UseNpgsql(connectionString).EnableSensitiveDataLogging() // Enables detailed error messages including sensitive data
                      .EnableDetailedErrors();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
