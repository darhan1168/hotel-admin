using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.Enums;
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
            { "1", AddHotelRoom },
            { "2", DisplayAllRooms }
        };

        while (true)
        {
            Console.WriteLine("\nВиберіть дію, яку треба зробити:");
            Console.WriteLine("1. Додати кімнату у базу");
            Console.WriteLine("2. Пошук усіх номерів");
            Console.WriteLine("3. Вихід");

            Console.Write("Відповідь: ");
            string input = Console.ReadLine();

            if (input == "3")
            {
                break;
            }

            if (actions.ContainsKey(input))
            {
                actions[input]();
            }
            else
            {
                Console.WriteLine("Немає такої операції");
            }
        }
    }

    public void AddHotelRoom()
    {
        try
        {
            Console.Clear();
            var rooms = GetAll().ToList();
            var nameRoom = GetNameRoom();
            foreach (var room in rooms)
            {
                if (room.NameRoom.Equals(nameRoom))
                {
                    throw new Exception("Номер вже додан в базу");
                }
            }
            
            var seats = GetSeats(nameRoom);
            var price = GetPrice(nameRoom);
            _service.CreateHotelRoom(new HotelRoom()
            {
                Id = Guid.NewGuid(),
                NameRoom = nameRoom,
                Seats = seats,
                AvailableSeats = seats,
                Guests = new List<Guest>(),
                PriceForOneDay = price
            });
            Console.WriteLine("Кімната добавлена в базу");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add room. Exception: {ex.Message}");
        }
    }

    public void DisplayAllRooms()
    {
        try
        {
            Console.Clear();
            var rooms = _service.GetAllRooms();
            int index = 1;
            if (rooms.Count == 0)
            {
                throw new Exception("Кімнат ще не має");
            }

            foreach (var room in rooms)
            {
                Console.WriteLine($"{index} - Назва: {room.NameRoom}, Усього місць: {room.Seats}, Кол-во вільних місць: {room.AvailableSeats}, " +
                                  $"Ціна: {room.PriceForOneDay}, Гості:");
                if (room.Guests.Count == 0)
                {
                    Console.WriteLine("ще не має");
                }
                else
                {
                    foreach (var guest in room.Guests)
                    {
                        Console.WriteLine($"Ім'я: {guest.Name}, Прізвище: {guest.SurName}, В'їзд: {guest.CheckIn}, Виїзд: {guest.CheckOut}," +
                                          $"Номер паспорту: {guest.NumPassport}");
                    }
                }
                
                index++;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to display room. Exception: {ex.Message}");
        }
    }

    private NameRoom GetNameRoom()
    {
        Console.Clear();
        Dictionary<string, NameRoom> names = new Dictionary<string, NameRoom>
        {
            { "1", NameRoom.SingleRoom },
            { "2", NameRoom.DoubleRoom },
            { "3", NameRoom.TwinRoom },
            { "4", NameRoom.QueenRoom },
            { "5", NameRoom.KingRoom }
        };
        Console.WriteLine("Виберіть назву кімнати (1 - SingleRoom, 2 - DoubleRoom, 3 - TwinRoom, " +
                          "4 - QueenRoom , 5 - KingRoom)");
        Console.Write("Відповідь: ");
        string answer = Console.ReadLine();

        if (names.ContainsKey(answer))
        {
            
            return names[answer];
        }
        else
        {
            throw new Exception("Немає такої операції");
        }
    }

    private int GetSeats(NameRoom nameRoom)
    {
        Dictionary<NameRoom, int> seats = new Dictionary<NameRoom, int>
        {
            { NameRoom.SingleRoom, 1 },
            { NameRoom.DoubleRoom, 2 },
            { NameRoom.TwinRoom, 3 },
            { NameRoom.QueenRoom, 4 },
            { NameRoom.KingRoom, 5 }
        };

        return seats[nameRoom];
    }

    private decimal GetPrice(NameRoom nameRoom)
    {
        Dictionary<NameRoom, int> prices = new Dictionary<NameRoom, int>
        {
            { NameRoom.SingleRoom, 300 },
            { NameRoom.DoubleRoom, 500 },
            { NameRoom.TwinRoom, 1000 },
            { NameRoom.QueenRoom, 2000 },
            { NameRoom.KingRoom, 5000 }
        };

        return prices[nameRoom];
    }
}