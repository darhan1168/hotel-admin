namespace HotelAdministrator.Models;

public class Guest : BaseEntity
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public int NumPassport { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public HotelRoom HotelRoom { get; set; }
}