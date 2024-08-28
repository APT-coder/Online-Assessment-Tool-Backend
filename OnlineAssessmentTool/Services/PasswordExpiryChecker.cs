using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Services
{
    public class PasswordExpiryChecker : BackgroundService
    {
        private readonly ILogger<PasswordExpiryChecker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PasswordExpiryChecker(ILogger<PasswordExpiryChecker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for expired passwords...");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<APIContext>();
                    var now = DateTime.UtcNow;
                    var currentDate = now.Date;
                    var currentTime = now.TimeOfDay;

                    try
                    {
                        var passwordsToUpdate = context.Trainers
                        .Where(a => a.LastPasswordReset >= now.AddDays(30))
                        .ToList();

                        foreach (var user in passwordsToUpdate)
                        {
                            user.IsActive = false;
                            context.Trainers.Update(user);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex) { }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }
    }
}
