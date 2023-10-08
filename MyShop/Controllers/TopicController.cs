using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public IActionResult CreateTopic(int roomId)
        {
            try
            {
                var createTopicViewModel = new CreateTopicViewModel
                {
                    RoomId = roomId
                };
                return View(createTopicViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a topic");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(CreateTopicViewModel createTopicViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Convert CreateTopicViewModel til Topic-entity
                    var topic = new Topic
                    {
                        RoomId = createTopicViewModel.RoomId,
                        TopicName = createTopicViewModel.TopicName,
                        // Add other fields necessary for the Topic entity
                    };

                    // Save Topic entity
                    await _topicRepository.Create(topic);

                    return RedirectToAction("RoomDetails", "Room", new { id = topic.RoomId });
                }
                _logger.LogWarning("[TopicController] Topic creation failed {@createTopicViewModel}", createTopicViewModel);
                return View(createTopicViewModel);
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
            var topic = await _topicRepository.GetItemById(Id);


            if (topic== null)
            {
                _logger.LogError("[TopicController] Topic not found for the TopicId {TopicId:0000}", Id);
                return NotFound("Topic not found."); //This page will popup only if we try to access a page where topic is null
            }

            // Sends the given topic to the view.
            return View(topic);
        }

        // POST: Topic

        [HttpPost]
        public async Task<IActionResult> UpdateTopic(int topicId, Topic topic)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _topicRepository.Update(topic);
                }
                catch
                {
                    //TODO: Add catch here
                }
                return RedirectToAction(nameof(TopicTable));
            }
            return View(topic);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _topicRepository.GetItemById(id);

            if (topic == null)
            {
                _logger.LogError("[TopicController] topic not found for the TopicId {TopicId:0000}", id);
                return BadRequest("Topic not found for the TopicId");
            }

            return View(topic);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedTopic(int id)
        {
            bool returnOk = await _topicRepository.Delete(id);
            if (!returnOk)
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {TopicId:0000}", id);
                return BadRequest("topic deletion failed");
            }
            return RedirectToAction(nameof(TopicTable));
        }

    }
}

