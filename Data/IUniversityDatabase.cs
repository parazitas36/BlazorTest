using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;

namespace BlazorTest.Data
{
    public interface IUniversityDatabase
    {
        Task AssignLecturerAndStudentToModule(ModuleReport report);
        UserAccount? GetByUsername(string userName);
        List<Course> GetCourses();
        Lecturer GetLecturer(UserAccount id);
        List<Lecturer> GetLecturers();
        List<Module> GetModules();
        Student GetStudent(int id);
        List<Grade> GetStudentGrades(int id, Student student = null);
        List<Student> GetStudents();
        UniversityContext GetUniversityDbContext();
        List<ModuleReport> GetModuleReportsForLecture(Lecturer lecturer);
        Task PostCourse(Course course);
        Task PostLecturer(Lecturer lecturer);
        Task PostModule(Module module);
        Task PostStudent(Student student);
        Task PostGrades(List<Grade> newGrades);
        Task SeedDb();
        Task UpdateGrades(List<Grade> newGrades);
        Task UpdateModuleReports(List<ModuleReport> updateReports);
    }
}