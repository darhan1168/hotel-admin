using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.BLL.Services;
using HotelAdministrator.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HotelAdministrator.BLL;

public class DependencyRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGuestService, GuestService>();
        services.AddScoped<IHotelRoomService, HotelRoomService>();

        DAL.DependencyRegistration.RegisterRepositories(services);
    }
}