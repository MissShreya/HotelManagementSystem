﻿using System;
public class Program{
public static void Main(string[] args){
    var guest1= new Guest{GuestName="Shreyanshi Srivastava"};
    var guest2= new Guest{GuestName="Shreya Srivastava"};

    var rooms = new List<Room>
        {
            new Room { RoomNumber = 101, Type = "Single", IsOccupied = true, OccupyingGuest = guest1 },
            new Room { RoomNumber = 102, Type = "Double", IsOccupied = false },
            new Room { RoomNumber = 103, Type = "Suite", IsOccupied = true, OccupyingGuest = guest2 }
        };
    var Hotel= new Hotel{
        HotelName="Taj Hotel",
        RoomList= rooms

    };
    Hotel.PrintHotelDetail();
}
}