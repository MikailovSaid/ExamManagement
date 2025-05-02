using ClosedXML.Excel;
using ExamManagement.Entities;

namespace ExamManagement.Reports
{
    public class ReportService
    {
        public byte[] GenerateStudentResultsExcel(List<Exam> exams, List<Student> students, List<Subject> subjects)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Student Results");

            worksheet.Cell(1, 1).Value = "Student";
            worksheet.Cell(1, 2).Value = "Subject";
            worksheet.Cell(1, 3).Value = "Grade";
            worksheet.Cell(1, 4).Value = "Exam Date";
            worksheet.Cell(1, 5).Value = "Class Level";

            int row = 2;
            foreach (var exam in exams)
            {
                var student = students.FirstOrDefault(s => s.StudentId == exam.StudentId);
                var subject = subjects.FirstOrDefault(s => s.SubjectCode == exam.SubjectCode);

                worksheet.Cell(row, 1).Value = $"{student?.FirstName} {student?.LastName}";
                worksheet.Cell(row, 2).Value = subject?.SubjectName;
                worksheet.Cell(row, 3).Value = exam.Grade;
                worksheet.Cell(row, 4).Value = exam.ExamDate.ToShortDateString();
                worksheet.Cell(row, 5).Value = student?.ClassLevel;

                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}
