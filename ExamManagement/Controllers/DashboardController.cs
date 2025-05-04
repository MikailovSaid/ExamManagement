using ExamManagement.Filters;
using ExamManagement.Interfaces;
using ExamManagement.Reports;
using Microsoft.AspNetCore.Mvc;

namespace ExamManagement.Controllers
{
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        private readonly IExamStatisticsRepository _statisticsRepository;
        private readonly IExamRepository _examRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;
        public DashboardController(IExamStatisticsRepository statisticsRepository, IExamRepository examRepository, IStudentRepository studentRepository, ISubjectRepository subjectRepository)
        {
            _statisticsRepository = statisticsRepository;
            _examRepository = examRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
        }
        public async Task<IActionResult> Index(DateTime? date)
        {
            return View(await _statisticsRepository.GetByDateAsync(date ?? DateTime.Now));
        }

        [HttpPost]
        public async Task<IActionResult> ExcelExport()
        {
            var exams = await _examRepository.GetAllAsync();
            var students = await _studentRepository.GetAllAsync();
            var subjects = await _subjectRepository.GetAllAsync();

            ReportService reportService = new();
            var file = reportService.GenerateStudentResultsExcel(exams.ToList(), students.ToList(), subjects.ToList());

            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelReport.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> PdfExport()
        {
            var exams = await _examRepository.GetAllAsync();
            var students = await _studentRepository.GetAllAsync();
            var subjects = await _subjectRepository.GetAllAsync();

            PdfReportService reportService = new();
            var pdfBytes = reportService.GenerateStudentResultsPdf(exams.ToList(), students.ToList(), subjects.ToList());

            return File(pdfBytes, "application/pdf", "StudentResults.pdf");
        }
    }
}
