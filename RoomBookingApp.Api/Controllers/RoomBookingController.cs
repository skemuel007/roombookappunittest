using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RoomBookingApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomBookingController : ControllerBase
    {
        private IRoomBookingRequestProcessor _roomBookingRequestProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor roomBookingRequest)
        {
            _roomBookingRequestProcessor = roomBookingRequest;
        }

        /// <summary>
        /// Book a room
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if ( ModelState.IsValid )
            {
               var result = _roomBookingRequestProcessor.BookRoom(request);
                if (result.Flag == Core.Enums.BookingResultFlag.Success)
                {
                    return Ok(result);
                }

                ModelState.AddModelError(nameof(RoomBookingRequest.Date),
                    "No rooms available for given date");
            }

            return BadRequest(ModelState);
        }
    }
}

