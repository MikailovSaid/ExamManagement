using ExamManagement.Entities;
using ExamManagement.Filters;
using ExamManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamManagement.Controllers
{
    [SessionAuthorize]
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        public async Task<IActionResult> Index()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return View(subjects);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (!ModelState.IsValid)
                return View(subject);

            await _subjectRepository.AddAsync(subject);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string code)
        {
            var subject = await _subjectRepository.GetByIdAsync(code);
            if (subject == null)
                return NotFound();

            return View(subject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Subject subject)
        {
            if (!ModelState.IsValid)
                return View(subject);

            await _subjectRepository.UpdateAsync(subject);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string code)
        {
            var subject = await _subjectRepository.GetByIdAsync(code);
            if (subject == null)
                return NotFound();

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string code)
        {
            await _subjectRepository.DeleteAsync(code);
            return RedirectToAction(nameof(Index));
        }
    }
}
