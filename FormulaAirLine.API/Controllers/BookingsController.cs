using FormulaAirLine.API.Services;
using FormulaAirLine.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirLine.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMessageProducer _messageProducer;
    private readonly ILogger<BookingsController> _logger;
    public static readonly List<Booking> _bookings = new();

    public BookingsController(
        IMessageProducer messageProducer, 
        ILogger<BookingsController> logger)
    {
        _logger = logger;
        _messageProducer = messageProducer;
    }

    [HttpPost]
    public IActionResult CreatingBooking(Booking newBooking)
    {
        if(!ModelState.IsValid) return BadRequest();

        _bookings.Add(newBooking);
        _messageProducer.SendingMessage<Booking>(newBooking);

        return Ok();
    }
}
