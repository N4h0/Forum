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
            var roomListViewModel = new RoomListViewModel(rooms, "Table");
            return View(roomListViewModel);
        }  
        
        //detalje for Room 
        public async Task<IActionResult> RoomDetails(int Id)
        {
            // Hent rommet fra databasen basert på rom-IDen
            var room = await _roomRepository.GetItemById(Id);
            var topics = await _topicRepository.GetTopicByRoom(Id);


            if (room == null)
            {
                return NotFound(); //return 404 if the room does not exist
            }
            if (topics == null || !topics.Any())
            {
                topics = new List<Topic>();
            }

            {
                var roomListViewModel = new RoomListViewModel(
                 rooms: new List<Room> { room }, // Add the room to the list
                topicsByRoom: new Dictionary<int, List<Topic>> { { room.RoomId, topics.ToList() } }, // Legg emnene for rommet i dictionary
                currentViewName: "Details" // Set display name
    );

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
            int categoryId = await _roomRepository.Delete(roomId);
            try
            {
                await _roomRepository.Delete(roomId);
                return RedirectToAction("CategoryDetails", "Category", new { id = roomId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            catch
            {
                // Handle exceptions, if any
                return RedirectToAction(nameof(RoomTable)); //Blir sendt tilbake til room table hvis noe skjer feil 
            }

        }
    }
}