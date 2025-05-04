using ExamManagement.Data;
using ExamManagement.Interfaces;
using ExamManagement.Repositories;
using ExamManagement.Services.Jobs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(60);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();
builder.Services.AddHostedService<DailyStatsJob>();
builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamAuditLogRepository, ExamAuditLogRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IExamStatisticsRepository, ExamStatisticsRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();


builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}");
});
app.Run();