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

        public async Task<IActionResult>TopicTable()
        {
            var topics = await _topicRepository.GetAll();
            if (topics == null)
            {
                _logger.LogError("[TopicController] Topic list not found while executing _topicRepository.GetAll()");
            }
                var threadListViewModel = new TopicListViewModel(topics, "Table");
                return View(threadListViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult CreateTopic(int roomId)
        {
            try
            {
                var topic = new Topic
                {
                    RoomId = roomId
                };
                return View(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a topic");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Save Topic entity
                    await _topicRepository.Create(topic);

                    return RedirectToAction("RoomDetails", "Room", new { id = topic.RoomId });
                }
//TODO ADD LOGGING
                return View(topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a topic");
                throw;
            }
        }

        [HttpGet] //Displays the details of a topic with a given Id. 
        public async Task<IActionResult> TopicDetails(int Id)
        {
            var topic = await _topicRepository.GetTopicById(Id);

            if (topic== null)
            {
                _logger.LogError("[TopicController] Topic not found for the TopicId {TopicId:0000}", Id);
                return NotFound("Topic not found."); //This page will popup only if we try to access a page where topic is null
            }

            // Sends the given topic to the view.
            return View(topic);
        }

        // GET: Comment/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateTopic(int Id)
        {
            var Topic = await _topicRepository.GetTopicById(Id);

            if (Topic == null)
            {
                return NotFound();
            }

            return View(Topic);
        }

        // POST: Topic

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateTopic(Topic topic)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _topicRepository.Update(topic);
                }
                catch(Exception e)
                {
                    _logger.LogError("Could not update topic",e);
                    //TODO: Add catch here
                }
                return RedirectToAction("RoomDetails", "Room", new { id = topic.RoomId });
            }
            else { _logger.LogError("ModeState is not valid for topic"); }
            return View(topic);
        }


        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _topicRepository.GetTopicById(id);

            if (topic == null)
            {
                _logger.LogError("[TopicController] topic not found for the TopicId {TopicId:0000}", id);
                return BadRequest("Topic not found for the TopicId");
            }

            return View(topic);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmedTopic(int Id)
        {
            var RoomId = await _topicRepository.GetRoomId(Id);
            bool returnOk = await _topicRepository.Delete(Id);
            if (!returnOk)
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {TopicId:0000}", Id);
                return BadRequest("topic deletion failed");
            }
            return RedirectToAction("RoomDetails", "Room", new { id = RoomId });
        }
    }
}

