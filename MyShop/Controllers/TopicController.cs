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

        public TopicController(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IActionResult>TopicTable()
        {
            var topics = await _topicRepository.GetAll();
            var threadListViewModel = new ThreadListViewModel(topics, "Table");
            return View(threadListViewModel);
        }

        [HttpGet]
        public IActionResult CreateTopic(int roomId)
        {
            var topic = new Topic
            {
                RoomId = roomId
            };
            return View(topic);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            if (ModelState.IsValid)
            {
                await _topicRepository.Create(topic);
                return RedirectToAction(nameof(TopicTable));
            }
            return View(topic);
        }

        public async Task<IActionResult> TopicDetails(int topicId)
        {
            var topic = await _topicRepository.GetItemById(topicId);

            if (topic== null)
            {
                return NotFound(); // Returner 404 hvis rommet ikke finnes
            }

            // Send rommet til visningen for romdetaljer
            return View(topic);
        }

        // POST: Topic

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
                return NotFound();
            }

            return View(topic);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedRoom(int topicId)
        {
            try
            {
                await _topicRepository.Delete(topicId);
                return RedirectToAction(nameof(TopicTable));
            }
            catch
            {
                // Handle exceptions, if any
                return RedirectToAction(nameof(TopicTable)); //Blir sendt tilbake til topic table i tilfelle feil 
            }
        }
    }


}

