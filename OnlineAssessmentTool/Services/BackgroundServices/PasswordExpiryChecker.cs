using OnlineAssessmentTool.Data;

namespace OnlineAssessmentTool.Services.BackgroundServices
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
                        var trainerPasswordsToUpdate = context.Trainers
                        .Where(a => a.LastPasswordReset >= now.AddDays(30))
                        .ToList();

                        var traineePasswordsToUpdate = context.Trainees
                        .Where(a => a.LastPasswordReset >= now.AddDays(30))
                        .ToList();

                        foreach (var user in trainerPasswordsToUpdate)
                        {
                            user.IsActive = false;
                            context.Trainers.Update(user);
                        }

                        foreach (var user in traineePasswordsToUpdate)
                        {
                            user.IsActive = false;
                            context.Trainees.Update(user);
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
