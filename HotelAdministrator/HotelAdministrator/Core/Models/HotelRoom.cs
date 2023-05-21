using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.Enums;

namespace HotelAdministrator.Models;

public class HotelRoom : BaseEntity
{
    public NameRoom NameRoom { get; set; }
    public List<Guest> Guests { get; set; }
    public int Seats { get; set; }
    public int AvailableSeats  { get; set; }
    public decimal PriceForOneDay { get; set; }
}