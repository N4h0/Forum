using System;
using System.Collections.Generic;
using Forum.Models;


namespace Forum.ViewModels
{
    public class RoomListViewModel
    {
        public IEnumerable<Room> Rooms { get; set; }
        public Dictionary<int, List<Topic>> TopicsByRoom { get; set; }
        public string? CurrentViewName;
        private string v;

        public RoomListViewModel(IEnumerable<Room> rooms, Dictionary<int, List<Topic>> topicsByRoom, string? currentViewName)
        {
            Rooms = rooms;
            TopicsByRoom = topicsByRoom;
            CurrentViewName = currentViewName;
        }

        public RoomListViewModel(IEnumerable<Room> rooms)
        {
            Rooms = rooms;
        }
    }
}