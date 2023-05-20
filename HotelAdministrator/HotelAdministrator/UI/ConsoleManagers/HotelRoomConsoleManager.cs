using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers;

public class HotelRoomConsoleManager : ConsoleManager<IHotelRoomService, HotelRoom>, IConsoleManager<HotelRoom>
{
    public HotelRoomConsoleManager(IHotelRoomService hotelRoomService) : base(hotelRoomService)
    {
    }

    public override void PerformOperations()
    {
        Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            { "1", DisplayAllUsers },
        };

        while (true)
        {
            Console.WriteLine("\nUser operations:");
            Console.WriteLine("1. Display all users");
            Console.WriteLine("2. Create a new user");
            Console.WriteLine("3. Update a user");
            Console.WriteLine("4. Delete a user");
            Console.WriteLine("5. Exit");

            Console.Write("Enter the operation number: ");
            string input = Console.ReadLine();

            if (input == "5")
            {
                break;
            }

            if (actions.ContainsKey(input))
            {
                actions[input]();
            }
            else
            {
                Console.WriteLine("Invalid operation number.");
            }
        }
    }
    
    public void DisplayAllUsers()
    {
        Console.WriteLine(1);
    }
}