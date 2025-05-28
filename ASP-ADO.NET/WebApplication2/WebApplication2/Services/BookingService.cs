using WebApplication2.Models;

public class BookingService
{
    private readonly List<Booking> _bookings = new();
    private readonly List<Resource> _resources = new()
    {
        new Resource { Id = "hotel1", Type = "Hotel", Name = "Hotel A" },
        new Resource { Id = "hotel2", Type = "Hotel", Name = "Hotel B" },
        new Resource { Id = "table1", Type = "Table", Name = "Table 1" },
        new Resource { Id = "table2", Type = "Table", Name = "Table 2" }
    };
    private int _nextId = 1;

    public List<Resource> GetAvailableResources(DateTime startTime, DateTime endTime, string? resourceType = null)
    {
        var bookedResourceIds = _bookings
            .Where(b => b.StartTime < endTime && b.EndTime > startTime)
            .Select(b => b.ResourceId)
            .ToHashSet();

        var availableResources = _resources
            .Where(r => !bookedResourceIds.Contains(r.Id));

        if (!string.IsNullOrEmpty(resourceType))
            availableResources = availableResources.Where(r => r.Type.Equals(resourceType, StringComparison.OrdinalIgnoreCase));

        return availableResources.ToList();
    }

    public Booking? CreateBooking(Booking booking)
    {
        var resource = _resources.FirstOrDefault(r => r.Id == booking.ResourceId);
        if (resource == null) return null;

        var isAvailable = !_bookings.Any(b =>
            b.ResourceId == booking.ResourceId &&
            b.StartTime < booking.EndTime &&
            b.EndTime > booking.StartTime);

        if (!isAvailable) return null;

        booking.Id = _nextId++;
        booking.ResourceType = resource.Type;
        _bookings.Add(booking);
        return booking;
    }

    public bool CancelBooking(int id)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null) return false;

        _bookings.Remove(booking);
        return true;
    }

    public bool UpdateBooking(int id, DateTime startTime, DateTime endTime)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);
        if (booking == null) return false;

        var isAvailable = !_bookings.Any(b =>
            b.Id != id &&
            b.ResourceId == booking.ResourceId &&
            b.StartTime < endTime &&
            b.EndTime > startTime);

        if (!isAvailable) return false;

        booking.StartTime = startTime;
        booking.EndTime = endTime;
        return true;
    }

    public List<Booking> GetAllBookings() => _bookings;
}