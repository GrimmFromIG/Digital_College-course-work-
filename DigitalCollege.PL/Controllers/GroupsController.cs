using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using DigitalCollege.BLL.Interfaces;
using DigitalCollege.BLL.DTOs;

namespace DigitalCollege.PL.Controllers
{
    [ApiController]
    [Route("api/management")]
    [Authorize(Roles = "Manager")]
    public class GroupsController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public GroupsController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpGet("groups")]
        public IActionResult GetAll()
        {
            var groups = _managementService.GetGroups();
            return Ok(groups);
        }

        [HttpPost("group")]
        public IActionResult Create([FromBody] GroupDto groupDto)
        {
            _managementService.AddGroup(groupDto);
            return Ok();
        }

        [HttpPut("group")]
        public IActionResult Update([FromBody] GroupDto groupDto)
        {
            _managementService.UpdateGroup(groupDto);
            return Ok();
        }

        [HttpDelete("group/{id}")]
        public IActionResult Delete(int id)
        {
            _managementService.DeleteGroup(id);
            return Ok();
        }
    }
}