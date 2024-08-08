using OnlineAssessmentTool.ServiceRegistry;
using Serilog;
using Serilog.Events;
using FluentValidation.AspNetCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Validations;
using FluentValidation;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
/*Log.Logger = new LoggerConfiguration()
.WriteTo.File("logs\\myapp.log", rollingInterval: RollingInterval.Day)
.CreateLogger();*/

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
);

builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();

builder.Services.AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient(smtpSettings.Host)
    {
        Port = smtpSettings.Port,
        Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
        EnableSsl = true,


        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
    });

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");




app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
