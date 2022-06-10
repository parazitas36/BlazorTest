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
            await _dbContext.SeedDatabase();
        }

        public UniversityContext GetUniversityDbContext()
        {
            return _dbContext;
        }

        // Accounts
        public UserAccount? GetByUsername(string userName)
        {
            return _dbContext.Accounts.Where(x => x.UserName == userName).FirstOrDefault();
        }

        // Students
        public List<Student> GetStudents()
        {
            return _dbContext.Students.ToList<Student>();
        }
        public Student GetStudent(int id)
        {
            return _dbContext.Students.Find(id);
        }

        public async Task PostStudent(Student student)
        {
            await _dbContext.AddAsync(student);
            await _dbContext.SaveChangesAsync();
        }

        // Lectures
        public List<Lecturer> GetLecturers()
        {
            return _dbContext.Lecturers.ToList<Lecturer>();
        }
        public Lecturer? GetLecturer(UserAccount acc)
        {
            return _dbContext.Lecturers.Where(x => x.Account == acc).FirstOrDefault();
        }

        public async Task PostLecturer(Lecturer lecturer)
        {
            await _dbContext.AddAsync(lecturer);
            await _dbContext.SaveChangesAsync();
        }

        // Courses
        public List<Course> GetCourses()
        {
            return _dbContext.Courses.ToList<Course>();
        }

        public async Task PostCourse(Course course)
        {
            await _dbContext.AddAsync(course);
            await _dbContext.SaveChangesAsync();
        }

        // Modules
        public List<Module> GetModules()
        {
            return _dbContext.Modules
                .Include(x => x.Coordinator)
                .Include(x => x.Course)
                .ToList();
        }

        public async Task PostModule(Module module)
        {
            await _dbContext.AddAsync(module);
            await _dbContext.SaveChangesAsync();
        }

        // Module reports
        public List<ModuleReport> GetModuleReportsForLecture(Lecturer lecturer)
        {
            var result = _dbContext.ModuleReports
                .Where(x => x.Lecturer == lecturer)
                .Include(x => x.Module)
                .Include(x => x.Student)
                .ToList();
            return result;
        }
        public async Task AssignLecturerAndStudentToModule(ModuleReport report)
        {
            await _dbContext.AddAsync(report);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateModuleReports(List<ModuleReport> updateReports)
        {
            _dbContext.UpdateRange(updateReports);
            await _dbContext.SaveChangesAsync();
        }

        // Grades
        public List<Grade> GetStudentGrades(int id, Student student = null)
        {
            // Find student by id
            if (student == null)
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

        public async Task PostGrades(List<Grade> newGrades)
        {
            await _dbContext.AddRangeAsync(newGrades);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGrades(List<Grade> newGrades)
        {
            _dbContext.UpdateRange(newGrades);
            await _dbContext.SaveChangesAsync();
        }
    }
}
