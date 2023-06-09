

using System.Text.RegularExpressions;
using HotelAdministrator.BLL.Interfaces;
using HotelAdministrator.Enums;
using HotelAdministrator.Models;
using UI.Interfaces;

namespace UI.ConsoleManagers;

public class GuestConsoleManager : ConsoleManager<IGuestService, Guest>, IConsoleManager<Guest>
{
    private readonly HotelRoomConsoleManager _hotelRoomConsoleManager;
    
    public GuestConsoleManager(IGuestService guestService, HotelRoomConsoleManager hotelRoomConsoleManager) 
        : base(guestService)
    {
        _hotelRoomConsoleManager = hotelRoomConsoleManager;
    }

    public override void PerformOperations()
    {
        Dictionary<string, Action> actions = new Dictionary<string, Action>
        {
            { "1", AddGuest },
            { "2", DisplayGuest },
            { "3", DeleteGuest }
        };

        while (true)
        {
            Console.WriteLine("\nВиберіть дію, яку треба зробити:");
            Console.WriteLine("1. Додати гостя");
            Console.WriteLine("2. Пошук гостей за ознакою");
            Console.WriteLine("3. Выселення гостя");
            Console.WriteLine("4. Вийти");

            Console.Write("Відповідь: ");
            string input = Console.ReadLine();

            if (input == "4")
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

    public void AddGuest()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Введіть ім'я");
            Console.Write("Відповідь: ");
            string name = Console.ReadLine();
            NameOrSurnameValid(name);
            name = char.ToUpper(name[0]) + name.Substring(1);
            
            Console.WriteLine("Введіть прізвище");
            Console.Write("Відповідь: ");
            string surname = Console.ReadLine();
            NameOrSurnameValid(surname);
            surname = char.ToUpper(surname[0]) + surname.Substring(1);

            Console.WriteLine("Введіть номер паспорту");
            Console.Write("Відповідь: ");
            string numPassport = Console.ReadLine();
            CheckNumberPassport(numPassport);

            var checkOut = GetDateTime();
            var hotelRoom = GetHotelRoom();

            var newGuest = new Guest()
            {
                Id = Guid.NewGuid(),
                Name = name,
                SurName = surname,
                CheckIn = DateTime.Now.Date,
                CheckOut = checkOut,
                HotelRoom = hotelRoom,
                NumPassport = numPassport
            };

            _service.RegisterGuest(newGuest);
            Console.WriteLine("Гость додан в базу");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add guest. Exception: {ex.Message}");
        }
    }
    
    public void DisplayGuest()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("\nВиберіть дію, яку треба зробити:");
            Console.WriteLine("1. Список усіх гостей");
            Console.WriteLine("2. Пошук гостя за номером паспорту");
            Console.WriteLine("3. Пошук гостей за датою виїзду");

            Console.Write("Відповідь: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    DisplayList(_service.GetAll());
                    break;
                case "2":
                    Console.WriteLine("Введить номер паспорта");
                    string numPassport = Console.ReadLine();
                    
                    DisplayGuest(_service.GetGuestByNumPassport(numPassport));
                    break;
                case "3":
                    var checkOut = GetDateTime();
                    
                    DisplayList(_service.GetGuestByCheckOut(checkOut));
                    break;
                default:
                    throw new Exception("Немає такої операції");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to display guest. Exception: {ex.Message}");
        }
    }

    public void DeleteGuest()
    {
        try
        {
            Console.Clear();
            var guests = _service.GetAll();
            DisplayList(guests);
            
            Console.WriteLine("Якого гостя треба виселити:");
            Console.Write("Відповідь: ");
            int answer = Int32.Parse(Console.ReadLine());
            var deleteGuest = guests[answer - 1];
            var check = _service.GetCheck(deleteGuest.Id);
            
            
            _service.DeleteGuest(deleteGuest.Id);

            Console.WriteLine($"Чєк за {(int)(deleteGuest.CheckOut - deleteGuest.CheckIn).TotalDays} днів - {check}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete guest. Exception: {ex.Message}");
        }
    }

    private DateTime GetDateTime()
    {
        Console.WriteLine("Введіть день вид'їзду");
        Console.Write("Відповідь: ");
        int day = int.Parse(Console.ReadLine());
        
        Console.WriteLine("Введіть місяць вид'їзду");
        Console.Write("Відповідь: ");
        int month = int.Parse(Console.ReadLine());

        Console.WriteLine("Введіть рік вид'їзду");
        Console.Write("Відповідь: ");
        int year = int.Parse(Console.ReadLine());
        
        DateTime inputDate = new DateTime(year, month, day);
    
        if (inputDate > DateTime.Now)
        {
            return inputDate;
        }
        else
        {
            Console.WriteLine("Ви ввели некоректну дату. Будь ласка, введіть дату, яка більша за поточну.");
            return GetDateTime();
        }
    }

    private HotelRoom GetHotelRoom()
    {
        var rooms = _hotelRoomConsoleManager.GetAll().ToList();
        if (rooms.Count == 0)
        {
            throw new Exception("Номерів ще не має");
        }
        
        var availableRooms = rooms.Where(r => r.AvailableSeats > 0).ToList();
        if (availableRooms.Count == 0)
        {
            throw new Exception("Порожніх номерів не має");
        }

        Console.WriteLine("Список доступних номерів:");
        var index = 1;
        foreach (var availableRoom in availableRooms)
        {
            Console.WriteLine($"{index} - Назва: {availableRoom.NameRoom}, Кол-во вільних місць: {availableRoom.AvailableSeats}, " +
                              $"Ціна: {availableRoom.PriceForOneDay}");
            index++;
        }
        
        Console.Write("Виберіть:");
        int input = Convert.ToInt32(Console.ReadLine());

        return availableRooms[input - 1];
    }

    private void DisplayList(List<Guest> guests)
    {
        Console.Clear();
        if (guests.Count == 0)
        {
            throw new Exception("Гостей ще не має");
        }
        
        int index = 1;
        foreach (var guest in guests)
        {
            Console.WriteLine($"{index} - Ім'я: {guest.Name}, Прізвище: {guest.SurName}, В'їзд: {guest.CheckIn}, Виїзд: {guest.CheckOut}," +
            $"Номер паспорту: {guest.NumPassport}, Номер: {guest.HotelRoom.NameRoom}");
            index++;
        }
    }

    private void DisplayGuest(Guest guest)
    {
        Console.Clear();
        if (guest is null)
        {
            throw new Exception("Такого гостя не має");
        }
        
        Console.WriteLine($"Ім'я: {guest.Name}, Прізвище: {guest.SurName}, В'їзд: {guest.CheckIn}, Виїзд: {guest.CheckOut}," +
                          $"Номер паспорту: {guest.NumPassport}, Номер: {guest.HotelRoom.NameRoom}");
    }

    private void CheckNumberPassport(string numPassport)
    {
        string pattern = "^[a-zA-Z0-9]+$";
        Match match = Regex.Match(numPassport, pattern);

        var guests = _service.GetAll();
        foreach (var guest in guests)
        {
            if (guest.NumPassport.Equals(numPassport))
            {
                throw new Exception("Така людина вже є в базі");
            }
        }
        
        if (!match.Success)
        {
            throw new Exception("Для номера паспорта треба використовувати англ букви та цифри");
        }
    }
    
    private void NameOrSurnameValid(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception("Недопустиме значення");
        }
        
        foreach (char c in name)
        {
            if (!char.IsLetter(c) || c == ' ')
            {
                throw new Exception("Містяться цифри або пробели");
            }
        }
    }

    public void AddStartValues()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{typeof(Guest).Name}.json");
        if (File.ReadAllText(filePath).Length > 0 && File.ReadAllText(filePath) != "[]")
        {
            return;
        }
        
        var rooms = _hotelRoomConsoleManager.GetAll().ToList();
        if (rooms.Count == 0)
        {
            throw new Exception("Номерів ще не має");
        }
        
        var availableRooms1 = rooms.Where(r => r.AvailableSeats > 0).ToList();
        if (availableRooms1.Count == 0)
        {
            throw new Exception("Порожніх номерів не має");
        }

        _service.RegisterGuest(new Guest()
        {
            Id = Guid.NewGuid(),
            Name = "Дарина",
            SurName = "Ханина",
            CheckIn = DateTime.Now.Date,
            CheckOut = new DateTime(2023, 5, 27),
            HotelRoom = availableRooms1[0],
            NumPassport = "1A23V45"
        });
        
        _service.RegisterGuest(new Guest()
        {
            Id = Guid.NewGuid(),
            Name = "Олег",
            SurName = "Вінник",
            CheckIn = DateTime.Now.Date,
            CheckOut = new DateTime(2023, 6, 27),
            HotelRoom = availableRooms1[1],
            NumPassport = "1A23745"
        });

        for (var i = 0; i < 5; i++)
        {
            var availableRooms = rooms.Where(r => r.AvailableSeats > 0).ToList();
            if (availableRooms.Count == 0)
            {
                throw new Exception("Порожніх номерів не має");
            }
            _service.RegisterGuest(new Guest()
            {
                Id = Guid.NewGuid(),
                Name = "Дарина" + $"{i}",
                SurName = "Ханина",
                CheckIn = DateTime.Now.Date,
                CheckOut = new DateTime(2023, 5, 27),
                HotelRoom = availableRooms[0],
                NumPassport = "1A23V45" + $"{i}"
            });
        }
    }
}