using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.DAL.Interfaces;
using HotelAdministrator.Enums;
using HotelAdministrator.Models;

namespace HotelAdministrator.BLL.Services;

public class HotelRoomService : GenericService<HotelRoom>, IHotelRoomService
{
    public HotelRoomService(IRepository<HotelRoom> repository) : 
        base(repository)
    {
    }

    public void CreateHotelRoom(HotelRoom hotelRoom)
    {
        try
        {
            Add(hotelRoom);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create {hotelRoom}. Exception: {ex.Message}");
        }
    }

    public List<HotelRoom> GetAllRooms()
    {
        try
        {
            return GetAll();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get all hotel rooms. Exception: {ex.Message}");
        }
    }
}