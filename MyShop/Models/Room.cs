﻿using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
	public class Room
	{
		public int RoomId { get; set; } //PK
        [Required]
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ. \-]{2,20}", ErrorMessage = "The Name must be numbers or letters and between 2 to 20 characters.")]

        public string RoomName { get; set; } = string.Empty; //Gir ingen mening å kunne opprette et forum uten å gi det et navn
        public int CategoryId { get; set; } //FK


        //Navigaiton property:
        public virtual Category? Category { get; set; } // Can't be zero. Virtual enables lazy loading. 
		public virtual List<Topic>? Topics { get; set; } // One room can have many threads, or zero. ? means it can have zero threads.
	}
}