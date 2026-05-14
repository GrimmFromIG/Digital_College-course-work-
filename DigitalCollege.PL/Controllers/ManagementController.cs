using Microsoft.AspNetCore.Mvc;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpPut("teacher")]
        public IActionResult UpdateTeacher([FromBody] TeacherDto dto) { _managementService.UpdateTeacher(dto); return Ok(); }

        [HttpDelete("teacher/{id}")]
        public IActionResult DeleteTeacher(int id) { _managementService.DeleteTeacher(id); return Ok(); }

        [HttpPut("student")]
        public IActionResult UpdateStudent([FromBody] StudentDto dto) { _managementService.UpdateStudent(dto); return Ok(); }

        [HttpDelete("student/{id}")]
        public IActionResult DeleteStudent(int id) { _managementService.DeleteStudent(id); return Ok(); }

        [HttpPut("discipline")]
        public IActionResult UpdateDiscipline([FromBody] DisciplineDto dto) { _managementService.UpdateDiscipline(dto); return Ok(); }

        [HttpDelete("discipline/{id}")]
        public IActionResult DeleteDiscipline(int id) { _managementService.DeleteDiscipline(id); return Ok(); }

        [HttpGet("teachers")]
        public IActionResult GetTeachers([FromQuery] string s = null, [FromQuery] string o = null) => Ok(_managementService.GetTeachers(s, o));

        [HttpGet("students")]
        public IActionResult GetStudents([FromQuery] string s = null, [FromQuery] int? g = null, [FromQuery] string o = null) => Ok(_managementService.GetStudents(s, g, o));

        [HttpGet("disciplines")]
        public IActionResult GetDisciplines([FromQuery] string s = null, [FromQuery] string o = null) => Ok(_managementService.GetDisciplines(s, o));
        
        [HttpPost("teacher")]
        public IActionResult AddTeacher([FromBody] TeacherDto dto) { _managementService.AddTeacher(dto); return Ok(); }
        
        [HttpPost("student")]
        public IActionResult AddStudent([FromBody] StudentDto dto) { _managementService.AddStudent(dto); return Ok(); }
        
        [HttpPost("discipline")]
        public IActionResult AddDiscipline([FromBody] DisciplineDto dto) { _managementService.AddDiscipline(dto); return Ok(); }
    }
}