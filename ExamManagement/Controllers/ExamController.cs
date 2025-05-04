using ExamManagement.Entities;
using ExamManagement.Filters;
using ExamManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExamManagement.Controllers
{
    [SessionAuthorize]
    public class ExamController : Controller
    {
        private readonly IExamRepository _examRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IExamAuditLogRepository _examAuditLogRepository;
        public ExamController(IExamRepository examRepository, IStudentRepository studentRepository, ISubjectRepository subjectRepository, IExamAuditLogRepository examAuditLogRepository)
        {
            _examRepository = examRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
            _examAuditLogRepository = examAuditLogRepository;
        }
        public async Task<IActionResult> Index()
        {
            var exams = await _examRepository.GetAllAsync();
            return View(exams);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["StudentId"] = new SelectList(await _studentRepository.GetAllAsync(), "StudentId", "FirstName", "LastName");
            ViewData["SubjectCode"] = new SelectList(await _subjectRepository.GetAllAsync(), "SubjectCode", "SubjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Exam exam)
        {
            ViewData["StudentId"] = new SelectList(await _studentRepository.GetAllAsync(), "StudentId", "FirstName", "LastName");
            ViewData["SubjectCode"] = new SelectList(await _subjectRepository.GetAllAsync(), "SubjectCode", "SubjectName");
            if (!ModelState.IsValid) 
                return View(exam);
            var dbExam = await _examRepository.GetByCompositeKeyAsync(null, exam.SubjectCode, exam.StudentId, exam.ExamDate);
            if (dbExam != null)
            {
                ModelState.AddModelError("ExamDate", "The same student should not be given an exam more than once on the same date for the same subject.");
                return View(exam);
            }
            if ((await _studentRepository.GetByIdAsync(exam.StudentId))!.ClassLevel != (await _subjectRepository.GetByIdAsync(exam.SubjectCode))!.ClassLevel)
            {
                ModelState.AddModelError("StudentId", "The student's class level must match the class level of the subject.");
                ModelState.AddModelError("SubjectCode", "The subject's class level must match the class level of the student.");
                return View(exam);
            }

            await _examRepository.AddAsync(exam);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["StudentId"] = new SelectList(await _studentRepository.GetAllAsync(), "StudentId", "FirstName", "LastName");
            ViewData["SubjectCode"] = new SelectList(await _subjectRepository.GetAllAsync(), "SubjectCode", "SubjectName");
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
                return NotFound();
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Exam exam)
        {
            ViewData["StudentId"] = new SelectList(await _studentRepository.GetAllAsync(), "StudentId", "FirstName", "LastName");
            ViewData["SubjectCode"] = new SelectList(await _subjectRepository.GetAllAsync(), "SubjectCode", "SubjectName");
            if (!ModelState.IsValid)
                return View(exam);
            var dbExam = await _examRepository.GetByCompositeKeyAsync(exam.ExamId, exam.SubjectCode, exam.StudentId, exam.ExamDate);
            if (dbExam != null)
            {
                ModelState.AddModelError("ExamDate", "The same student should not be given an exam more than once on the same date for the same subject.");
                return View(exam);
            }
            if ((await _studentRepository.GetByIdAsync(exam.StudentId))!.ClassLevel != (await _subjectRepository.GetByIdAsync(exam.SubjectCode))!.ClassLevel)
            {
                ModelState.AddModelError("StudentId", "The student's class level must match the class level of the subject.");
                ModelState.AddModelError("SubjectCode", "The subject's class level must match the class level of the student.");
                return View(exam);
            }

            var oldGrade = (await _examRepository.GetByIdAsync(exam.ExamId))!.Grade;
            await _examRepository.UpdateAsync(exam);
            await _examAuditLogRepository.AddAsync(new() { OldGrade = oldGrade, NewGrade = exam.Grade, ChangedAt = DateTime.Now, ExamId = exam.ExamId, ChangedBy = HttpContext.Session.GetString("Username")! });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _examRepository.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _examRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
