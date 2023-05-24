using UI.ConsoleManagers;

namespace UI;

public class AppManager
{
    private readonly GuestConsoleManager _guestConsoleManager;
    private readonly HotelRoomConsoleManager _hotelRoomConsoleManager;

    public AppManager(GuestConsoleManager guestConsoleManager,
        HotelRoomConsoleManager hotelRoomConsoleManager)
    {
        _guestConsoleManager = guestConsoleManager;
        _hotelRoomConsoleManager = hotelRoomConsoleManager;
    }

    public void Start()
    {
        _hotelRoomConsoleManager.AddStartValues();

        while (true)
        {
            Console.WriteLine("\nВітаю вас адміністратор (Виберіть з чим вам потрібно працювати):");
                Console.WriteLine("1. Дія над гостями");
                Console.WriteLine("2. Дія над номерами");
                Console.WriteLine("3. Вийти з програми");
                
                Console.Write("Відповідь: ");
                string input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        _guestConsoleManager.PerformOperations();
                        break;
                    case "2":
                        _hotelRoomConsoleManager.PerformOperations();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Немає такої операції");
                        break; 
                }
        }
    }
}