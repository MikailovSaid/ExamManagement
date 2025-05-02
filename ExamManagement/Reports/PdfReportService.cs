using ExamManagement.Entities;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace ExamManagement.Reports
{
    public class PdfReportService
    {
        public byte[] GenerateStudentResultsPdf(List<Exam> exams, List<Student> students, List<Subject> subjects)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 10);

            int y = 20;

            gfx.DrawString("Student Exam Results", new XFont("Verdana", 14, XFontStyleEx.Bold), XBrushes.Black, new XPoint(200, y));
            y += 40;

            foreach (var exam in exams)
            {
                var student = students.FirstOrDefault(s => s.StudentId == exam.StudentId);
                var subject = subjects.FirstOrDefault(s => s.SubjectCode == exam.SubjectCode);

                string line = $"Student: {student?.FirstName} {student?.LastName}, Subject: {subject?.SubjectName}, Grade: {exam.Grade}, Date: {exam.ExamDate:dd.MM.yyyy}";
                gfx.DrawString(line, font, XBrushes.Black, new XPoint(40, y));
                y += 20;

                if (y > page.Height - 40)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 20;
                }
            }

            using var stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
    }
}
