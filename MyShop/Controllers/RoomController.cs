using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
    public class RoomController : Controller

    {
        private readonly IRoomRepository _roomRepository;
        private readonly ILogger<RoomController> _logger;


        public RoomController(IRoomRepository roomRepository, ILogger<RoomController> logger)
        {
            _roomRepository = roomRepository;
            _logger = logger;
        }

        public async Task<IActionResult> RoomDetails(int Id)
        {

            _logger.LogInformation("RoomDetails called with Room ID: {RoomId}", Id);


            // Get the room from the database based on the room-ID
            var room = await _roomRepository.GetRoomById(Id);

            if (room == null)
             
            {
                _logger.LogError("Room with ID not found Room ID: {RoomId}", Id);
                return NotFound(); //returns 404 if the room is not found
            }
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomRepository.Create(room);
                return RedirectToAction("CategoryDetails", "Category", new {id = room.CategoryId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            _logger.LogError("[RoomController] Room creation failed  {@room}", room);
            return View(room);
        }

        // GET: Room/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateRoom(int Id)
        {
            var room = await _roomRepository.GetRoomById(Id);

            if (room == null)
            {
                _logger.LogError("Error while updating room with ID {Id}", Id);
                return NotFound();
            }

            return View(room);
        }

        // POST: Room
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roomRepository.Update(room);
                }
                catch(Exception e)
                {
                    _logger.LogError("An error occured during creating room",e);

                }
                return RedirectToAction("CategoryDetails", "Category", new { id = room.CategoryId }); //Return to Category/CategoryDetails/CategoryId after create.
            }
            return View(room);
        }

        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteRoom(int Id)
        {
            var room = await _roomRepository.GetRoomById(Id);

            if (room == null)
            {
                _logger.LogError("[RoomController] Room deletion failed for the Romid {RoomId:0000}", Id);
                return BadRequest("Room not found for the RoomId");
            }

            return View(room);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmedRoom(int Id)
        {
            var CategoryId = await _roomRepository.GetCategoryId(Id);
            try
            {
                await _roomRepository.Delete(Id);
                return RedirectToAction("CategoryDetails", "Category", new { id = CategoryId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            catch(Exception e)
            {
                // Handle exceptions, if any
                    _logger.LogError("[RoomController] Room deletion failed for the Romid {RoomId:0000}", e);

                    return RedirectToAction("CategoryDetails", "Category", new { id = CategoryId });
            }

        }
    }
}