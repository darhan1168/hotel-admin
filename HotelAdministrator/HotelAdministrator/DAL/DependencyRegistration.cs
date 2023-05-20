using HotelAdministrator.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace HotelAdministrator.DAL;

public class DependencyRegistration
{
    public static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}