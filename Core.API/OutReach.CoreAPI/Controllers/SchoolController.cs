using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Models.Response;
using OutReach.CoreBusiness.Repository;
using OutReach.CoreSharedLib.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutReach.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class SchoolController : Controller
    {
        private SchoolRepository _schoolRepository;
        public SchoolController(SchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        [AllowAnonymous]
        [HttpPost("GetAllSchool")]
        public async Task<ActionResult> GetAllSchool([FromQuery] GridRequest<SchoolRequest> gridRequest)
        {
            var result = await _schoolRepository.GetAllSchool(gridRequest);

            return Ok(result);
            
        }   

        [AllowAnonymous]
        [HttpGet("GetSchoolById")]
        public async Task<ActionResult> GetSchoolById(int id)
        {
            var result = await _schoolRepository.GetSchoolById(id);

            return Ok(result);

        }

      
        [AllowAnonymous]
        [HttpPost("AddSchool")]
        public async Task<IActionResult> AddSchool(School createSchool)
        {

            return Ok(await _schoolRepository.AddSchool(createSchool));

        }
        [Authorize]
        [AllowAnonymous]
        [HttpPost("UpdateSchool")]
        public async Task<IActionResult> UpdateSchool(School school)
        {

            return Ok(await _schoolRepository.UpdateSchool(school));

        }
        [Authorize]
        [AllowAnonymous]
        [HttpPost("DeleteSchool")]
        public async Task<IActionResult> DeleteSchool(int id,Boolean status)
        {

            return Ok(await _schoolRepository.DeleteSchool(id, status));

        }

    }
}
