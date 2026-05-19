using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs;
using DigitalCollege.BLL.Exceptions;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/management")]
    [Authorize(Roles = "Manager")]
    public class GroupsController : ControllerBase
    {
        private readonly IManagementService _managementService;
        public GroupsController(IManagementService managementService) => _managementService = managementService;

        [HttpGet("groups")]
        public IActionResult GetAll([FromQuery] string s = null) => Ok(_managementService.GetGroups(s));

        [HttpPost("group")]
        public IActionResult Create([FromBody] GroupDto groupDto)
        {
            try { _managementService.AddGroup(groupDto); return Ok(); }
            catch (BusinessLogicException ex) { return BadRequest(ex.Message); }
        }

        [HttpPut("group")]
        public IActionResult Update([FromBody] GroupDto groupDto)
        {
            try { _managementService.UpdateGroup(groupDto); return Ok(); }
            catch (BusinessLogicException ex) { return BadRequest(ex.Message); }
        }

        [HttpDelete("group/{id}")]
        public IActionResult Delete(int id)
        {
            try { _managementService.DeleteGroup(id); return Ok(); }
            catch (BusinessLogicException ex) { return BadRequest(ex.Message); }
        }
    }
}
