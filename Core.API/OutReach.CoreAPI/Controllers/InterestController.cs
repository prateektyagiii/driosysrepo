using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Entity;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Model.Response;
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
    public class InterestController : Controller
    {
        private InterestRepository _interestRepository;
        public InterestController(InterestRepository interestRepository)
        {
            _interestRepository = interestRepository;
        }

        [AllowAnonymous]
        [HttpPost("GetAllInterest")]
        public async Task<ActionResult> GetAllInterest([FromQuery] GridRequest<InterestRequest> gridRequest)
        {


            var result = await _interestRepository.GetAllInterest(gridRequest);
           
          
                return Ok(result);
            
            
            
        }

        [AllowAnonymous]
        [HttpGet("GetInterestById")]
        public async Task<ActionResult> GetInterestById(int id)
        {
            var result = await _interestRepository.GetInterestById(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetInterestByUserId")]
        public async Task<ActionResult> GetInterestByUserId(int id)
        {
            var result = await _interestRepository.GetInterestByUserId(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("AddInterest")]
        public async Task<IActionResult> AddInterest(Interest createInterest)
        {

            return Ok(await _interestRepository.AddInterest(createInterest));

        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("UpdateInterest")]
        public async Task<IActionResult> UpdateInterest(Interest interest)
        {

            return Ok(await _interestRepository.EditInterest(interest));

        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("DeleteInterest")]
        public async Task<IActionResult> DeleteInterest(int id, Boolean status)
        {

            return Ok(await _interestRepository.DeleteInterest(id, status));

        }

    }
}
