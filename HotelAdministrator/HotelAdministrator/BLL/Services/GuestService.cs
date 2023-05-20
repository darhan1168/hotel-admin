using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.DAL.Interfaces;
using HotelAdministrator.Models;

namespace HotelAdministrator.BLL.Services;

public class GuestService : GenericService<Guest>, IGuestService
{
    public GuestService(IRepository<Guest> repository) : 
        base(repository)
    {
    }
}