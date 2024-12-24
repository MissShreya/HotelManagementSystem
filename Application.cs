using System;
public class Room
{
    public int RoomNumber { get; set; }
    public required string Type { get; set; }
    public required bool IsOccupied { get; set; }

    public Guest ? OccupyingGuest { get; set; } 

    public void PrintRoomDetail()
    {
        string occupancyStatus = IsOccupied ? "Yes, it is occupied." : "No, it is not occupied.";
        Console.WriteLine($"Room Number: {RoomNumber}, Type: {Type}, Occupancy Status: {occupancyStatus} ");
         if (IsOccupied && OccupyingGuest != null)
        {
            Console.WriteLine($"Occupied by: {OccupyingGuest.GuestName}");
        }
    }
}
public class Hotel
{
    public required string HotelName { get; set; }
    public required List<Room> RoomList { get; set; }
    public void PrintHotelDetail()
    {
        Console.WriteLine($"Hotel Name: {HotelName}");
        Console.WriteLine("Room Details:");
        foreach (var room in RoomList)
        {
            room.PrintRoomDetail(); // Calls the method for each room
        }
    }
}

public class Guest
{
    public required string GuestName{get;set;}
    public void PrintGuestDetail(){
        Console.WriteLine($"Guest name is : {GuestName}");
    }
}