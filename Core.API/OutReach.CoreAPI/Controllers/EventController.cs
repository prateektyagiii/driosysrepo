using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutReach.CoreBusiness.Entities;
using OutReach.CoreBusiness.Model.Request;
using OutReach.CoreBusiness.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutReach.CoreAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class EventController : Controller
    {

        private EventRepository _eventRepository;

        public EventController(EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [AllowAnonymous]
        [HttpPost("GetAllEvent")]
        public async Task<ActionResult> GetAllEvent([FromQuery] GridRequest<EventRequest> gridRequest)
        {
            var result = await _eventRepository.GetAllEvent(gridRequest);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetEventById")]
        public async Task<ActionResult> GetSchoolById(int id)
        {
            var result = await _eventRepository.GetEventById(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddEvent(Event createEvent)
        {

            return Ok(await _eventRepository.AddEvent(createEvent));

        }

        [AllowAnonymous]
        [HttpGet("GetEventByInterest")]
        public async Task<ActionResult> GetEventByInterest(int id)
        {
            var result = await _eventRepository.GetEventByInterest(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetEventByUser")]
        public async Task<ActionResult> GetEventByUser(int id)
        {
            var result = await _eventRepository.GetEventByUser(id);

            return Ok(result);

        }


        [AllowAnonymous]
        [HttpGet("GetEventBynearBy")]
        public async Task<ActionResult> GetEventBynearBy(int id)
        {
            var result = await _eventRepository.GetEventBynearBy(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetTotalParticipateEventByUser")]
        public async Task<ActionResult> GetTotalParticipateEventByUser(int id)
        {
            var result = await _eventRepository.GetTotalParticipateEventByUser(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("GetUserEventStatus")]
        public async Task<ActionResult> GetUserEventStatus([FromQuery] UserEventStatusRequest userEventStatus)
        {
            var result = await _eventRepository.GetUserEventStatus(userEventStatus);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("AddUserEventChat")]
        public async Task<ActionResult> GetUserEventChat(UserEventChatRequest userEventChatRequest)
        {
            var result = await _eventRepository.AddUserEventChat(userEventChatRequest);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetGroupChatByEvent")]
        public async Task<ActionResult> GetGroupChatByEvent(int id)
        {
            var result = await _eventRepository.GetGroupChatByEvent(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("JoinEvent")]
        public async Task<ActionResult> JoinEvent([FromQuery] UserEventRequest userEventRequest)
        {
            var result = await _eventRepository.JoinEvent(userEventRequest);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("ApproveEvent")]
        public async Task<ActionResult> ApproveEvent([FromQuery] UserEventRequest userEventRequest)
        {
            var result = await _eventRepository.ApproveEvent(userEventRequest);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetMyEventByid/{id}")]
        public async Task<ActionResult> GetMyEventByid(int id)
        {
            var result = await _eventRepository.GetMyEventByid(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetEventUserRequest")]
        public async Task<ActionResult> GetEventUserRequest(int id)
        {
             var result = await _eventRepository.GetEventUserRequest(id);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("DeleteEvent")]
        public async Task<ActionResult> DeleteEvent(int id,Boolean status)
        {
            var result = await _eventRepository.DeleteEvent(id,status);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("LeftEvent")]
        public async Task<ActionResult> LeftEvent(int id,int eventid)
        {
            var result = await _eventRepository.LeftEvent(id, eventid);

            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetEventDetail")]
        public async Task<ActionResult> GetEventDetail(int id)
        {
            var result = await _eventRepository.GetEventDetail(id);

            return Ok(result);
        }




        /*   [AllowAnonymous]
           [HttpPost("EventApproval")]
           public async Task<ActionResult> EventApproval(UserEventRequest userEventRequest)
           {
             //  var result = await _eventRepository.EventApproval(userEventRequest);

               return Ok(result);

           }*/
    }
}