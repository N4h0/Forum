using System;
using System.Collections.Generic;
using Forum.Models;



namespace Forum.ViewModels
{
    public class CreateTopicViewModel
    {
        public int RoomId { get; set; }
        public string TopicName { get; set; }
        

        public CreateTopicViewModel(int roomId, string topicName)
        {
            RoomId = roomId;
            TopicName = topicName;
        }

        public CreateTopicViewModel() { }


    }
}
