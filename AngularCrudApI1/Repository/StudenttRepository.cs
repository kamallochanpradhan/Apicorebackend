using AngularCrudApI1.Model;
using Microsoft.EntityFrameworkCore;

namespace AngularCrudApI1.Repository
{
    //testing
    public class StudenttRepository : IStudenttRepository
    {
        private readonly MyAngularDataContext _appDBContext;

        public StudenttRepository(MyAngularDataContext context)
        {
            _appDBContext = context ??
           throw new ArgumentNullException(nameof(context));

        }
        public bool DeleteStudent(int ID)
        {
            bool result = false;
            var department = _appDBContext.Students.Find(ID);
            if (department != null)
            {
                _appDBContext.Entry(department).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }


        public async Task<IEnumerable<Student>> GetStudent()
        {
            return await _appDBContext.Students.ToListAsync();

        }


        public async Task<Student> GetStudentByID(int ID)
        {
            return await _appDBContext.Students.FindAsync(ID);
        }

        public async Task<Student> InsertStudent(Student objDepartment)
        {
            _appDBContext.Students.Add(objDepartment);
            await _appDBContext.SaveChangesAsync();
            return objDepartment;
        }

        public async Task<Student> UpdateStudent(Student objDepartment)
        {
            _appDBContext.Entry(objDepartment).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return objDepartment;
        }
    }
}
