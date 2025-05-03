using ExamManagement.Entities;
using ExamManagement.Interfaces;

namespace ExamManagement.Services.Jobs
{
    public class DailyStatsJob : BackgroundService
    {
        private readonly ILogger<DailyStatsJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DailyStatsJob(IServiceProvider serviceProvider, ILogger<DailyStatsJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                _logger.LogInformation($"DailyStatsJob will start after {delay}");
                await Task.Delay(delay, stoppingToken);

                await RunJobAsync();
            }
        }

        private async Task RunJobAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var examRepo = scope.ServiceProvider.GetRequiredService<IExamRepository>();
            var statRepo = scope.ServiceProvider.GetRequiredService<IExamStatisticsRepository>();
            var logRepo = scope.ServiceProvider.GetRequiredService<ILogRepository>();

            try
            {
                var dateFrom = DateTime.Now.AddDays(-30);
                var allExams = await examRepo.GetAllAsync();
                var recentExams = allExams.Where(e => e.ExamDate >= dateFrom).ToList();

                var groups = recentExams
                    .GroupBy(e => new { e.SubjectCode, e.StudentId })
                    .ToList();

                var statGroups = recentExams
                    .GroupBy(e => new { e.SubjectCode, e.ClassLevel })
                    .ToList();

                foreach (var group in statGroups)
                {
                    var grades = group.Select(e => e.Grade);
                    var stat = new ExamStatistics
                    {
                        SubjectCode = group.Key.SubjectCode,
                        ClassLevel = group.Key.ClassLevel,
                        AverageGrade = grades.Average(),
                        MaxGrade = grades.Max(),
                        MinGrade = grades.Min(),
                        ExamCount = grades.Count(),
                        CalculatedAt = DateTime.Now
                    };

                    await statRepo.AddAsync(stat);
                }

                await logRepo.AddAsync(new Log
                {
                    ServiceName = "DailyStatsJob",
                    RunAt = DateTime.Now,
                    Status = "SUCCESS",
                    Message = $"Processed {statGroups.Count} statistic groups"
                });

                _logger.LogInformation("DailyStatsJob completed successfully.");
            }
            catch (Exception ex)
            {
                await logRepo.AddAsync(new Log
                {
                    ServiceName = "DailyStatsJob",
                    RunAt = DateTime.Now,
                    Status = "FAILED",
                    Message = ex.Message
                });

                _logger.LogError(ex, "DailyStatsJob failed.");
            }
        }
    }
}
