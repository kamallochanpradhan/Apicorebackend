using AngularCrudApI1.Model;
using AngularCrudApI1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
/*Serilog used form udemy 
 https://www.udemy.com/course/build-rest-apis-with-aspnet-core-web-api-entity-framework/learn/lecture/37067262#overview*/

namespace AngularCrudApI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudenttRepository _student;
        private readonly ILogger<StudentController> logger;

        public StudentController(IStudenttRepository student, ILogger<StudentController> logger)
        {
            _student = student;
            this.logger = logger;
        }

        /*Authorization in ASP.NET Core is controlled with AuthorizeAttribute and
         * its various parameters. In its most basic form, applying the
         * [Authorize] attribute to a controller, action, or Razor Page, 
         * limits access to that component to authenticated users.*/

        /*[AllowAnonymous] bypasses authorization statements.
         * If you combine [AllowAnonymous] and an [Authorize] attribute, 
         * the [Authorize] attributes are ignored. 
         * For example if you apply [AllowAnonymous] at the controller level*/

        /*Overall, the Microsoft.AspNetCore.Authorization namespace provides the
         * necessary components for implementing authorization in your 
         * ASP.NET Core application, allowing you to control access to 
         * resources based on defined policies, roles, or custom requirements.*/

        // here Authorize attribute used to authenticate
        // [Authorize]
        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> Get()
        {
            #region Code Which Will Produce error
            //int x = 0;
            //int y = 5;
            //int result = y / x; // This line will produce an error at runtime
           // Console.WriteLine(result);
            #endregion

            logger.LogInformation("GetStudent Action method Invoked");
            var stdData = await _student.GetStudent();
            return Ok(stdData);

            /*JsonSerializer.Serialize(stdData) will convert the stdData object into json data*/
            logger.LogInformation($"Finished GetStudent Action method all data: {JsonSerializer.Serialize(stdData)}");
        }


        // GET api/<StudentController>/5
        [HttpGet]
        [Route("GetStudentByID/{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            return Ok(await _student.GetStudentByID(Id));
        }


        // POST api/<StudentController>s
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


        [HttpPost]
        [Route("process")]
        public IActionResult ProcessRequest([FromBody] Product requestModel, [FromHeader] string Authorization88, [FromHeader] string ContentType)
        {
            if (requestModel == null)
            {
                return BadRequest(new ErrorResponse { StatusCode = 400, Message = "Request model cannot be null." });
            }
            try
            {
                Console.WriteLine("Authorization88", Authorization88);
                Console.WriteLine("ContentType", ContentType);
            }
        
            catch (Exception ex)
            {
                // Log the exception if needed

                // Return an error response with status code and message
                return StatusCode(500, new ErrorResponse { StatusCode = 500, Message = ex.Message });
            }

            // Do something with the request model and headers
             Console.WriteLine($"Received request with Pname: {requestModel.Pname}, Prid: {requestModel.Prid}");
             Console.WriteLine($"Authorization Header: {authToken}");
             Console.WriteLine($"Content-Type Header: {contentType}");

            // Return a success response
            return Ok("Request processed successfully.");
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
