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

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
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

            if (room == null)
            {
                return NotFound(); // Returner 404 hvis rommet ikke finnes
            }

            // Send rommet til visningen for romdetaljer
            return View(room);
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
                }

                return RedirectToAction(nameof(RoomTable));
            }

            return View(room);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            var room = await _roomRepository.GetItemById(roomId);

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
                return RedirectToAction(nameof(RoomTable));
            }
            catch
            {
                // Handle exceptions, if any
                return RedirectToAction(nameof(RoomTable)); //Blir sendt tilbake til room table hvis noe skjer feil 
            }
        }




    }
}