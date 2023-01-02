using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rabbit.API.Interfaces;
using Rabbit.API.Models;

namespace Rabbit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageProducer _messagePrdoucer;
        public static readonly List<Reservation> Reservations=new List<Reservation>();
        public MessagesController(IMessageProducer messagePrdoucer)
        {
            _messagePrdoucer = messagePrdoucer;
        }
        [HttpPost]
        public IActionResult MakeReservation(Reservation r)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Reservations.Add(r);
            _messagePrdoucer.SendMessage<Reservation>(r);
            return Ok();
        }
    }
}
