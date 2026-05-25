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
    public class AcademicController : ControllerBase
    {
        private readonly IAcademicService _academicService;

        public AcademicController(IAcademicService academicService)
        {
            _academicService = academicService;
        }

        [HttpPost("grade")]
        [Authorize(Roles = "Teacher")]
        public IActionResult AssignGrade([FromBody] GradeDto gradeDto)
        {
            try
            {
                _academicService.AssignGrade(gradeDto);
                return Ok();
            }
            catch (BusinessLogicException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("student/{id}/grades")]
        [Authorize(Roles = "Student,Manager")]
        public IActionResult GetStudentGrades(int id)
        {
            return Ok(_academicService.GetStudentGrades(id));
        }

        [HttpGet("discipline/{id}/grades")]
        [Authorize(Roles = "Teacher,Manager")]
        public IActionResult GetDisciplineGrades(int id)
        {
            return Ok(_academicService.GetDisciplineGrades(id));
        }
    }
}