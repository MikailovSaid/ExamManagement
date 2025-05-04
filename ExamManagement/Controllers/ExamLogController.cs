using ExamManagement.Filters;
using ExamManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamManagement.Controllers
{
    [SessionAuthorize]
    public class ExamLogController : Controller
    {
        private readonly IExamAuditLogRepository _examAuditLogRepository;
        private readonly IExamRepository _examRepository;
        public ExamLogController(IExamAuditLogRepository examAuditLogRepository, IExamRepository examRepository)
        {
            _examAuditLogRepository = examAuditLogRepository;
            _examRepository = examRepository;
        }
        public async Task<IActionResult> Index(int? examId = null)
        {
            ViewData["ExamId"] = new SelectList(await _examRepository.GetAllAsync(), "ExamId", "ExamId");
            var examLogs = await _examAuditLogRepository.GetAllAsync(examId);
            return View(examLogs);
        }
    }
}
