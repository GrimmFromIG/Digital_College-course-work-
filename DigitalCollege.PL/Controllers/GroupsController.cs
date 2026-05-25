using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs; // Виправляє помилку CS0246

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/management")]
    public class GroupsController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public GroupsController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        // GET: api/management/groups
        [HttpGet("groups")]
        public IActionResult GetAll()
        {
            var groups = _managementService.GetGroups();
            return Ok(groups);
        }

        // POST: api/management/group
        [HttpPost("group")]
        public IActionResult Create([FromBody] GroupDto groupDto)
        {
            _managementService.AddGroup(groupDto);
            return Ok();
        }

        // PUT: api/management/group
        [HttpPut("group")]
        public IActionResult Update([FromBody] GroupDto groupDto)
        {
            _managementService.UpdateGroup(groupDto);
            return Ok();
        }

        // DELETE: api/management/group/{id}
        [HttpDelete("group/{id}")]
        public IActionResult Delete(int id)
        {
            _managementService.DeleteGroup(id);
            return Ok();
        }
    }
}