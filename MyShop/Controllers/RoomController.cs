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

        public async Task<IActionResult>RoomTable()
        {
            var rooms = await _roomRepository.GetAll();
            var roomListViewModel = new RoomListViewModel(rooms, "Table");
            return View(roomListViewModel);
        }
        [HttpGet]
        public IActionResult CreateRoom(int categoryId)
        {
            var room = new Room
            {
                CategoryId = categoryId //setter CategoryID på categoryId parameter

            };
            return View(room);
      
        }

        [HttpPost]
        public async Task< IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomRepository.Create(room);
                return RedirectToAction(nameof(RoomTable)); //Endret fra Table til RoomTable fordi det er den som viser Room
            }
            return View(room);
        }
    }
}