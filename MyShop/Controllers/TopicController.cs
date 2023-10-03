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
                var threadListViewModel = new ThreadListViewModel(topics, "Table");
                return View(threadListViewModel);
        }

        [HttpGet]
        public IActionResult CreateTopic(int Id )
        {
            try
            {
                var topic = new Topic
                {
                    RoomId = Id // Set the CategoryId based on the categoryId parameter.
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
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            if (ModelState.IsValid)
            {
                await _topicRepository.Create(topic);
                return RedirectToAction(nameof(TopicTable));
            }
            _logger.LogWarning("[TopicController] Topic creation failed {@topic}", topic);
            return View(topic);
        }

        [HttpGet]
        public async Task<IActionResult> TopicDetails(int topicId)
        {
            var topic = await _topicRepository.GetItemById(topicId);

            if (topic== null)
            {
                _logger.LogError("[TopicController] Topic not found for the TopicId {TopicId:0000}", topicId);
            
            }

            // Send rommet til visningen for topicetaljer
            return View(topic);
        }

        // POST: Topic

        [HttpPost]
        public async Task<IActionResult> UpdateTopic(int topicId, Topic topic)
        {
            if (topicId != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _topicRepository.Update(topic);
                }
                catch
                {
                }

                return RedirectToAction(nameof(TopicTable));
            }

            return View(topic);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            var topic = await _topicRepository.GetItemById(topicId);

            if (topic == null)
            {
                _logger.LogError("[TopicController] topic not found for the TopicId {TopicId:0000}", topicId);
                return BadRequest("Category not found for the CategoryId");
            }

            return View(topic);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedRoom(int topicId)
        {
            bool returnOk = await _topicRepository.Delete(topicId);
            if (!returnOk)
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {TopicId:0000}", topicId);
                return BadRequest("topic deletion failed");

            }
            
                return RedirectToAction(nameof(TopicTable));
            
        }
    }


}

