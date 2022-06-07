using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;

namespace BlazorTest.Data
{
    public interface IUniversityDatabase
    {
        UniversityContext GetUniversityDbContext();
        Student GetStudent(int id);
        List<Student> GetStudents();
        List<Grade> GetStudentGrades(int id, Student student = null);
        Task UpdateGrades(List<Grade> newGrades);
        Task SeedDb();
    }
}