using OnlineAssessmentTool.Data;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Services.BackgroundServices
{
    public class BatchActiveService : BackgroundService
    {
        private readonly ILogger<PasswordExpiryChecker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public BatchActiveService(ILogger<PasswordExpiryChecker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for inactive batches...");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<APIContext>();
                    var now = DateTime.UtcNow;
                    var year = now.Month;

                    try
                    {
                        var batchesToUpdate = context.batch
                        .Where(a => a.CreatedOn >= now.AddMonths(6))
                        .ToList();

                        foreach (var batch in batchesToUpdate)
                        {
                            batch.isActive = false;
                            context.batch.Update(batch);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex) { }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Check every minute
            }
        }
    }
}
