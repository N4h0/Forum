using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Forum.Controllers
{
    public class RoomController : Controller

    {
        private readonly CategoryDbContext _roomDbContext;

        public RoomController(CategoryDbContext roomDbContext)
        {
            _roomDbContext = roomDbContext;
        }

        public async Task<IActionResult>RoomTable()
        {
            List<Room> rooms = _roomDbContext.Rooms.ToList();
            var roomListViewModel = new RoomListViewModel(rooms, "Table");
            return View(roomListViewModel);
        }
        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                _roomDbContext.Rooms.Add(room);
                _roomDbContext.SaveChanges();
                return RedirectToAction(nameof(RoomTable));
            }
            return View(room);
        }
    }
}