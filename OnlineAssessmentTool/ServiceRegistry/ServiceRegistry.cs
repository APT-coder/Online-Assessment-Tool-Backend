using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Services;
using OnlineAssessmentTool.Services.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Encodings;
using OnlineAssessmentTool.Models.DTO;
using System;
using System.Net.Mail;
using System.Text.Json.Serialization;
using Npgsql;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineAssessmentTool.Services.BackgroundServices;

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
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder => builder
                        .WithOrigins("http://localhost:4201", "http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            DotNetEnv.Env.Load();


            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(option => {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                         "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                         "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                         "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
               {
                   {
                       new OpenApiSecurityScheme
                       {
                           Reference = new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                           },
                           Scheme = "oauth2",
                           Name = "Bearer",
                           In = ParameterLocation.Header,
                       },
                       new List<string>()
                   }
               });

            });

            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWTSecretKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireTrainerOrAdminRole", policy =>
                    policy.RequireRole("Trainer", "Admin"));
                options.AddPolicy("TraineePolicy", policy => policy.RequireRole("Trainee"));
            });

            var connectionString = Environment.GetEnvironmentVariable("POSTGRESQL_CONNECTION_STRING");

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
            services.AddScoped<IIlpRepository, IlpIntegrationRepository>();
            services.AddScoped<ILPIntegrationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddAutoMapper(typeof(MappingConfig));

            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            services.AddDbContext<APIContext>(options =>
            {
                options.UseNpgsql(connectionString).EnableSensitiveDataLogging()
                                  .EnableDetailedErrors();
            });

            services.AddHostedService<AssessmentStatusUpdater>();
            services.AddHostedService<PasswordExpiryChecker>();
            services.AddHostedService<BatchActiveService>();

            services.AddMemoryCache();
        }
    }
}       
