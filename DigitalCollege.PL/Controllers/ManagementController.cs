using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService) { _managementService = managementService; }

        [HttpPut("teacher")][Authorize(Roles = "Manager")] public IActionResult UpdateTeacher([FromBody] TeacherDto dto) { try { _managementService.UpdateTeacher(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }
        [HttpDelete("teacher/{id}")][Authorize(Roles = "Manager")] public IActionResult DeleteTeacher(int id) { try { _managementService.DeleteTeacher(id); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }

        [HttpPut("student")][Authorize(Roles = "Manager")] public IActionResult UpdateStudent([FromBody] StudentDto dto) { try { _managementService.UpdateStudent(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }
        [HttpDelete("student/{id}")][Authorize(Roles = "Manager")] public IActionResult DeleteStudent(int id) { try { _managementService.DeleteStudent(id); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }

        [HttpPut("discipline")][Authorize(Roles = "Manager")] public IActionResult UpdateDiscipline([FromBody] DisciplineDto dto) { try { _managementService.UpdateDiscipline(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }
        [HttpDelete("discipline/{id}")][Authorize(Roles = "Manager")] public IActionResult DeleteDiscipline(int id) { try { _managementService.DeleteDiscipline(id); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }

        [HttpPost("teacher")][Authorize(Roles = "Manager")] public IActionResult AddTeacher([FromBody] TeacherDto dto) { try { _managementService.AddTeacher(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }
        [HttpPost("student")][Authorize(Roles = "Manager")] public IActionResult AddStudent([FromBody] StudentDto dto) { try { _managementService.AddStudent(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }
        [HttpPost("discipline")][Authorize(Roles = "Manager")] public IActionResult AddDiscipline([FromBody] DisciplineDto dto) { try { _managementService.AddDiscipline(dto); return Ok(); } catch (BusinessLogicException ex) { return BadRequest(ex.Message); } }

        [HttpGet("teachers")]
        [AllowAnonymous]
        public IActionResult GetTeachers([FromQuery] string s = null, [FromQuery] int? disciplineId = null, [FromQuery] string o = null)
            => Ok(_managementService.GetTeachers(s, disciplineId, o));

        [HttpGet("students")]
        [Authorize(Roles = "Manager,Teacher")]
        public IActionResult GetStudents([FromQuery] string s = null, [FromQuery] string g = null, [FromQuery] string o = null)
            => Ok(_managementService.GetStudents(s, g, o));

        [HttpGet("disciplines")]
        [AllowAnonymous]
        public IActionResult GetDisciplines([FromQuery] string s = null, [FromQuery] string o = null, [FromQuery] int? teacherId = null)
            => Ok(_managementService.GetDisciplines(s, o, teacherId));
    }
}
