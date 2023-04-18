using AngularCrudApI1.Model;
using AngularCrudApI1.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudenttRepository _student;

        public StudentController(IStudenttRepository student)
        {
            _student = student;
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _student.GetStudent());
        }


        // GET api/<StudentController>/5
        [HttpGet]
        [Route("GetStudentByID/{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            return Ok(await _student.GetStudentByID(Id));
        }


        // POST api/<StudentController>
        [HttpPost]
        [Route("InsertStudent")]
        public async Task<IActionResult> Post(Student std)
        {
            var result = await _student.InsertStudent(std);
            if (result.Id == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something Went Wrong");
            }

            return Ok("Added Successfully");
        }


        // PUT api/<StudentController>/5
        [HttpPut]
        [Route("UpdateStudent")]
        public async Task<IActionResult> Put([FromBody] Student std)
        {
            await _student.UpdateStudent(std);
            return Ok("Updated Successfully");
        }


        // DELETE api/<StudentController>/5
        [HttpDelete]
        [Route("DeleteStudent")]
        public async Task<bool> Delete(int id)
        {
            var result = _student.DeleteStudent(id);

            if (!result)
            {
                return false;
            }
            return true;
        }
    }
}
