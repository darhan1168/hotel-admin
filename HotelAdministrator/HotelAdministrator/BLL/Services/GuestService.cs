using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.DAL.Interfaces;
using HotelAdministrator.Models;
using Exception = System.Exception;

namespace HotelAdministrator.BLL.Services;

public class GuestService : GenericService<Guest>, IGuestService
{ 
    private readonly IHotelRoomService _hotelRoomService;
    public GuestService(IRepository<Guest> repository, IHotelRoomService hotelRoomService) : 
        base(repository)
    {
       _hotelRoomService = hotelRoomService;
    }

    public void RegisterGuest(Guest guest)
    {
        try
        {
            Add(guest);
            var rooms = _hotelRoomService.GetAll();
            var specialRooms = rooms.Where(r => r.NameRoom.Equals(guest.HotelRoom.NameRoom)).ToList();
            foreach (var specialRoom in specialRooms)
            {
                specialRoom.Guests.Add(guest);
                specialRoom.AvailableSeats -= 1;
                _hotelRoomService.Update(specialRoom.Id, specialRoom);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to register {guest.Name}. Exception: {ex.Message}");
        }
    }

    public void DeleteGuest(Guid guestId)
    {
        try
        {
            Guest guest = GetById(guestId);
            var rooms = _hotelRoomService.GetAll();
            var specialRooms = rooms.Where(r => r.NameRoom.Equals(guest.HotelRoom.NameRoom)).ToList();
            foreach (var specialRoom in specialRooms)
            {
                foreach (var g in specialRoom.Guests)
                {
                    Delete(g.Id);
                    specialRoom.AvailableSeats += 1;
                }
                
                specialRoom.Guests = new List<Guest>();
                _hotelRoomService.Update(specialRoom.Id, specialRoom);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete guest by {guestId}. Exception: {ex.Message}");
        }
    }

    public List<Guest> GetAllGuest()
    {
        try
        {
            return GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get all guest. Exception: {ex.Message}");
        }
    }

    public List<Guest> GetGuestByHotelRoom(HotelRoom hotelRoom)
    {
        try
        {
            return GetAll().Where(g => g.HotelRoom.Equals(hotelRoom)).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get guest by {hotelRoom}. Exception: {ex.Message}");
        }
    }

    public List<Guest> GetGuestByCheckOut(DateTime checkOut)
    {
        try
        {
            return GetAll().Where(g => g.CheckOut.Equals(checkOut)).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get guest by {checkOut}. Exception: {ex.Message}");
        }
    }

    public Guest GetGuestByNumPassport(int numPassport)
    {
        try
        {
            return GetByPredicate(g => g.NumPassport.Equals(numPassport));
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get guest by {numPassport}. Exception: {ex.Message}");
        }
    }

    public decimal GetCheck(Guid guestId)
    {
        try
        {
            var guest = GetById(guestId);
            var totalDays = (int)(guest.CheckOut - guest.CheckIn).TotalDays;
            return totalDays * guest.HotelRoom.PriceForOneDay;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get check by {guestId}. Exception: {ex.Message}");
        }
    }
}