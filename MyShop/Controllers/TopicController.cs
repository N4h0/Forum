using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ILogger<TopicController> _logger;


        public TopicController(ITopicRepository topicRepository, ILogger<TopicController> logger)
        {
            _topicRepository = topicRepository;
            _logger = logger;
        }

        //Method for theTopicTable-sida
        public async Task<IActionResult> TopicTable()
        {
            var topics = await _topicRepository.GetAll(); //Getting all topic
            if (topics == null) //If-test and logging if no topics are found
            {
                _logger.LogError("[TopicController] Topic list not found while executing _topicRepository.GetAll()");
            }
            var topicListViewModel = new TopicListViewModel(topics, "Table"); //Creating a new topicListViewModel.
            return View(topicListViewModel); //Returning to topictable with the passed topiclistviewmodel.
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only superadmin can access this method
        //Get method for the createtopic-page with a passed roomId
        public IActionResult CreateTopic(int roomId)
        {
            try
            {
                var topic = new Topic //Creating a new topic and setting the topics RoomId equal to the passed roomId
                {
                    RoomId = roomId
                };
                return View(topic); //Returning the create view with the passed topic.
            }
            catch (Exception ex) //If topic creation failed we log an error and return a 404 not found
            {
                _logger.LogError(ex, "An error occurred while creating a topic");
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Accessible only to SuperAdmin
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Save Topic entity
                    await _topicRepository.Create(topic);
                    //Redirecting to RoomDetails/Room/*Newly created topics Id* on successfull create.
                    return RedirectToAction("RoomDetails", "Room", new { id = topic.RoomId });
                } //On invalid model state, returning to the CreateTopic view and passing the topic. Logging the topicId on 
                _logger.LogInformation("Created a new topic with id {id}", topic.TopicId);
                return View(topic);
            }
            catch (Exception ex) //For any other errors than invalid model state, we log a generi error message and return a 404 not found.
            {
                _logger.LogError(ex, "[TopicController]An error occurred while creating a topic");
                return NotFound();
            }
        }

        [HttpGet] //Displays the details of a topic with a given Id. 
        public async Task<IActionResult> TopicDetails(int Id)
        {
            var topic = await _topicRepository.GetTopicById(Id); //Getting the topicrepo based on id.

            if (topic== null)
            {
                _logger.LogError("[TopicController] Topic not found for the TopicId {TopicId:0000}", Id);
                return NotFound("Topic not found."); //This page will popup only if we try to access a page where topic is null.
            }

            // Sends the given topic to the view.
            return View(topic);
        }

        // GET: Comment/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin.
        public async Task<IActionResult> UpdateTopic(int Id)
        {
            var Topic = await _topicRepository.GetTopicById(Id);//Getting the topicrepo based on id.

            if (Topic == null) //Returning not found if Topic is null
            {
                return NotFound();
            }

            return View(Topic); //If a topic is found, returning the updatedtopic view with the passed topic
        }

        // POST: Topic

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin
        public async Task<IActionResult> UpdateTopic(Topic topic)
        {

            if (ModelState.IsValid) //Checking if modelstate is valid
            {
                try
                {
                    await _topicRepository.Update(topic); //Attempting to update topic.
                }
                catch(Exception e) //Reaching the exception if update of topic failed.
                {
                    _logger.LogError("Could not update topic",e);
                }
                return RedirectToAction("RoomDetails", "Room", new { id = topic.RoomId }); //Redirecting to the Room of the newly updated topic.
            }
            else { _logger.LogError("ModeState is not valid for topic"); } //We only reach this log if the modelstate is not valid. After logging we return the updatetopic view with the topic.
            return View(topic);
        }


        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _topicRepository.GetTopicById(id); //Getting a topic based on id.

            if (topic == null) //Logging error if topic is null
            {
                _logger.LogError("[TopicController] topic not found for the TopicId {Id}", id);
                return BadRequest("Topic not found for the TopicId");
            }

            return View(topic); //returning deleteopic view with the created topic
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only accessible to superadmin
        public async Task<IActionResult> DeleteConfirmedTopic(int Id)
        {
            var RoomId = await _topicRepository.GetRoomId(Id); //Getting the roomid of the topic we are going to delete
            bool returnOk = await _topicRepository.Delete(Id); //Deleting the topic based on id
            if (!returnOk) //Entering the loop if the DAL did not return a true and logging + returning a bad request. 
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {Id}", Id);
                return BadRequest("topic deletion failed");
            }
            return RedirectToAction("RoomDetails", "Room", new { id = RoomId }); //Redirecting to Room/Roomdetails/*RoomId of the deleted topic*
        }
    }
}

