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
        while (true)
        {
            Console.WriteLine("\nChoose an operation:");
                Console.WriteLine("1. Task operations");
                Console.WriteLine("2. Project operations");
                Console.WriteLine("3. Exit");
                
                Console.Write("Enter the operation number: ");
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
                        Console.WriteLine("Invalid operation number.");
                        break; 
                }
        }
    }
}