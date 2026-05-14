using Microsoft.AspNetCore.Mvc;
using DigitalCollege.BLL.Interfaces;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly IPublicService _publicService;

        public PublicController(IPublicService publicService)
        {
            _publicService = publicService;
        }

        [HttpGet("teachers")]
        public IActionResult GetAllTeachers()
        {
            return Ok(_publicService.GetAllTeachersInfo());
        }

        [HttpGet("disciplines")]
        public IActionResult GetAllDisciplines()
        {
            return Ok(_publicService.GetAllDisciplinesInfo());
        }
    }
}