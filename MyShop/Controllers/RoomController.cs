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

        //Method to show the roomtable in the navbar
        public async Task<IActionResult> RoomTable()
        {
            var rooms = await _roomRepository.GetAll(); //getting all rooms
            var roomListViewModel = new RoomListViewModel(rooms); //Creating a view with all rooms
            return View(roomListViewModel); //Returning the created rommListViewModel to the view. 
        }

        //Method seeing the details of a room based on the roomId
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
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmins
        public IActionResult CreateRoom(int categoryId)  //Get method to create a room

        {
                var room = new Room
                {
                    CategoryId = categoryId // Set the CategoryId based on the categoryId parameter.
                };
                return View(room);  
          
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]//Only accessible to superadmins
        public async Task<IActionResult> CreateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomRepository.Create(room);
                return RedirectToAction("CategoryDetails", "Category", new {id = room.CategoryId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            _logger.LogError("[RoomController] Room creation failed  {@room}", room); //Logging error if the modelstate is not valid
            return View(room); //returning the createroom view with the passed room
        }

        // GET: Room/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]//Only accessible to superadmins
        public async Task<IActionResult> UpdateRoom(int Id)
        {
            var room = await _roomRepository.GetRoomById(Id); //getting room based on the passed id

            if (room == null) //Looging if no room is found based on id
            {
                _logger.LogError("Error while updating room with ID {Id}", Id);
                return NotFound(); //returning 404 not found
            }
            return View(room); //Returning the update view with the passed room
        }

        // POST: Room
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]//Only accessible to superadmins
        public async Task<IActionResult> UpdateRoom(Room room)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _roomRepository.Update(room);
                }
                catch(Exception e) //we reach this log if updating a room fail
                {
                    _logger.LogError("An error occured during creating room",e);

                }
                return RedirectToAction("CategoryDetails", "Category", new { id = room.CategoryId }); //Return to Category/CategoryDetails/CategoryId after create.
            }
            return View(room); //Returning the create room view with the created room if the modelstate is invalid
        }

        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]//Only accessible to superadmins
        public async Task<IActionResult> DeleteRoom(int Id)
        {
            var room = await _roomRepository.GetRoomById(Id); //Gerring a room based on the RoomId

            if (room == null) //We log a message and return 404 if no room is found
            {
                _logger.LogError("[RoomController] Room deletion failed for the Romid {Id}", Id);
                return BadRequest("Room not found for the RoomId");
            }
            return View(room); //On a successfull update, returning the deleteroom view with the created room
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]//Only accessible to superadmins
        public async Task<IActionResult> DeleteConfirmedRoom(int Id)
        {
            var CategoryId = await _roomRepository.GetCategoryId(Id); //Getting the categoryId based on the room we are about to deltete
            //We store this so that we can use it to navigate back to the category-page
            try
            {
                await _roomRepository.Delete(Id); //Attempting to delete room from the DB based on id
                return RedirectToAction("CategoryDetails", "Category", new { id = CategoryId}); //Return to Category/CategoryDetails/CategoryId after create.
            }
            catch(Exception e)
            {
                // If deleting the room fails, we reach this and logg the error
                    _logger.LogError("[RoomController] Room deletion failed for the Romid {Id}", Id);
                //On a failed delete we get returned to Category/CategoryDetails/CategoryId.
                return RedirectToAction("CategoryDetails", "Category", new { id = CategoryId });
            }

        }
    }
}