using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingsController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("available-resources")]
    public ActionResult<List<Resource>> GetAvailableResources(
        [FromQuery] DateTime startTime,
        [FromQuery] DateTime endTime,
        [FromQuery] string? resourceType = null)
    {
        var resources = _bookingService.GetAvailableResources(startTime, endTime, resourceType);
        return Ok(resources);
    }

    [HttpPost]
    public ActionResult<Booking> CreateBooking(Booking booking)
    {
        var created = _bookingService.CreateBooking(booking);
        if (created == null) return BadRequest("Resource not available or doesn't exist");
        return CreatedAtAction(nameof(GetBookings), new { id = created.Id }, created);
    }

    [HttpGet]
    public ActionResult<List<Booking>> GetBookings()
    {
        return Ok(_bookingService.GetAllBookings());
    }

    [HttpDelete("{id}")]
    public IActionResult CancelBooking(int id)
    {
        if (!_bookingService.CancelBooking(id))
            return NotFound();
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBooking(int id, [FromBody] UpdateBookingRequest request)
    {
        if (!_bookingService.UpdateBooking(id, request.StartTime, request.EndTime))
            return BadRequest("Booking not found or time slot not available");
        return NoContent();
    }
}

public class UpdateBookingRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}