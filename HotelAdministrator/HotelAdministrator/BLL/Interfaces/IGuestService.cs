using HotelAdministrator.Models;

namespace HotelAdministrator.BLL.Interfaces;

public interface IGuestService : IGenericService<Guest>
{
    void RegisterGuest(Guest guest);

    void DeleteGuest(Guid guestId);
    
    List<Guest> GetAllGuest();

    List<Guest> GetGuestByHotelRoom(HotelRoom hotelRoom);

    List<Guest> GetGuestByCheckOut(DateTime checkOut);
    
    Guest GetGuestByNumPassport(int numPassport);

    decimal GetCheck(Guid guestId);
}