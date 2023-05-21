using HotelAdministrator.Models;

namespace HotelAdministrator.BLL.Interfaces;

public interface IHotelRoomService : IGenericService<HotelRoom>
{
    void CreateHotelRoom(HotelRoom hotelRoom);

    List<HotelRoom> GetAllRooms();
}