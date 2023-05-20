using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.DAL.Interfaces;
using HotelAdministrator.Models;

namespace HotelAdministrator.BLL.Services;

public class HotelRoomService : GenericService<HotelRoom>, IHotelRoomService
{
    public HotelRoomService(IRepository<HotelRoom> repository) : 
        base(repository)
    {
    }
}