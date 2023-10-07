using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Forum.Controllers
{
    public class RoomController : Controller

    {
        private readonly IRoomRepository _roomRepository;
        private readonly ITopicRepository _topicRepository;

        public RoomController(IRoomRepository roomRepository, ITopicRepository topicRepository)
        {
            _roomRepository = roomRepository;
            _topicRepository = topicRepository;
        }

        public async Task<IActionResult> RoomTable()
        {
            var rooms = await _roomRepository.GetAll();
            var roomListViewModel = new RoomListViewModel(rooms);
            return View(roomListViewModel);
        }  
        
        //details for room
        public async Task<IActionResult> RoomDetails(int Id)
        {
            // Get the room from the database based on the room-ID
            var room = await _roomRepository.GetItemById(Id);
            var topics = await _topicRepository.GetTopicByRoom(Id);


            if (room == null)
            {
                return NotFound(); //returns 404 if the room is not found
            }
            if (topics == null || !topics.Any())
            {
                topics = new List<Topic>();
            }

            {
                var roomListViewModel = new RoomListViewModel(
                 rooms: new List<Room> { room }, // Add the room to the list
                topicsByRoom: new Dictionary<int, List<Topic>> { { room.RoomId, topics.ToList() } }, // Legg emnene for rommet i dictionary
                currentViewName: "Details");  // Set display name
                return View(room);
            }
        }

        [HttpGet]
        public IActionResult CreateRoom(int categoryId)  //la til at man kan lage forum 

        {
            var room = new Room
            {
                CategoryId = categoryId // Set the CategoryId based on the categoryId parameter.
            };
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomRepository.Create(room);
                return RedirectToAction("CategoryDetails", "Category", new {id = room.CategoryId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            return View(room);
        }

        // GET: Room/
        [HttpGet]
        public async Task<IActionResult> UpdateRoom(int roomId)
        {
            var room = await _roomRepository.GetItemById(roomId);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room
        public async Task<IActionResult> UpdateRoom(int roomId, Room room)
        {
            if (roomId != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roomRepository.Update(room);
                }
                catch
                {
                    //TODO fill out this catch
                }

                return RedirectToAction("CategoryDetails", "Category", new { id = room.CategoryId }); //Return to Category/CategoryDetails/CategoryId after create.
            }

            return View(room);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int Id)
        {
            var room = await _roomRepository.GetItemById(Id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedRoom(int roomId)
        {
            try
            {
                await _roomRepository.Delete(roomId);
                return RedirectToAction("CategoryDetails", "Category", new { id = roomId}); //Return to Category/CategoryDetails/CategoryId after create. TODO fiks
            }
            catch
            {
                // Handle exceptions, if any
                return RedirectToAction(nameof(RoomTable)); //Blir sendt tilbake til room table hvis noe skjer feil 
            }

        }
    }
}