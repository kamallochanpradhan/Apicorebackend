using AngularCrudApI1.Model;

namespace AngularCrudApI1.Repository
{
    public interface IStudenttRepository
    {
        Task<IEnumerable<Student>> GetStudent();
        Task<Student> GetStudentByID(int ID);
        Task<Student> InsertStudent(Student objDepartment);
        Task<Student> UpdateStudent(Student objDepartment);
        bool DeleteStudent(int ID);
    }
}
