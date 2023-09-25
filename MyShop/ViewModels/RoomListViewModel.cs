using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class RoomListViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public string? CurrentViewName;

        public RoomListViewModel(IEnumerable<Room> rooms, string? currentViewName)
        {
            Rooms = rooms;
            CurrentViewName = currentViewName;
        }
    }
}