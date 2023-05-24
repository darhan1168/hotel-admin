using System.Text;
using HotelAdministrator.UI;
using Microsoft.Extensions.DependencyInjection;
using UI;

namespace HotelAdministrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            var serviceProvider = DependencyRegistration.Register();

            using (var scope = serviceProvider.CreateScope())
            {
                var appManager = scope.ServiceProvider.GetService<AppManager>();
                appManager.Start();
            }
        }
    }
}

