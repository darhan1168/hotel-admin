using HotelAdministrator.Enums;

namespace HotelAdministrator.Models;

public class HotelRoom : BaseEntity
{
    public NameRoom NameRoom { get; set; }
    public int Seats { get; set; }
}