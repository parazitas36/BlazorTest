using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorTest.Data
{
    public class UniversityDatabase : IUniversityDatabase
    {
        private readonly UniversityContext _dbContext;
        public UniversityDatabase(UniversityContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SeedDb()
        {
            _dbContext.SeedDatabase();
        }

        public UniversityContext GetUniversityDbContext()
        {
            return _dbContext;
        }

        public Student GetStudent(int id)
        {
            return _dbContext.Students.Find(id);
        }

        public List<Student> GetStudents()
        {
            return _dbContext.Students.ToList<Student>();
        }

        public List<Grade> GetStudentGrades(int id, Student student = null)
        {
            // Find student by id
            if(student == null)
            {
                student = _dbContext.Students.Find(id);
            }

            // Get all student's module reports
            List<ModuleReport> reports = _dbContext.ModuleReports
                    .Where(x => x.Student == student)
                    .Include(x => x.Module)
                        .ThenInclude(m => m.Coordinator)
                    .Include(x => x.Lecturer)
                    .ToList();

            // Get all grades mapped with module reports
            List<Grade> grades = _dbContext.Grades
                .Include(x => x.Module)
                .ToList();

            // Filter grades, keep only student's grades
            grades = grades.Where(x => reports
                                    .Where(z => z.Id == x.Module.Id)
                                    .FirstOrDefault() != null)
                            .ToList();
            return grades;
        }

        public async Task UpdateGrades(List<Grade> newGrades)
        {
            _dbContext.UpdateRange(newGrades);
            await _dbContext.SaveChangesAsync();
        }
    }
}
