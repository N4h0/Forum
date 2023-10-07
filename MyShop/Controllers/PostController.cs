﻿using Microsoft.AspNetCore.Mvc;

using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;


        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<IActionResult> PostTable()
        {
            var posts = await _postRepository.GetAll();
            if (posts == null)
            {
                _logger.LogError("[PostController] Post list not found while executing _postRepository.GetAll()");
            }
            var postListViewModel = new PostListViewModel(posts, "Table");
            return View(postListViewModel);
        }

        [HttpGet] //Called when clicking on the CreatePost hypelink under TopicDetails. CreatePost is the name of the method (here) and view (CreatePost.cshtml).
        public IActionResult CreatePost(int topicId) //Create a post with a given Id, which is (should be) passed  when the method is called.
        {
            try
            {
                var postCommentViewModel = new PostCommentViewModel
                {
                    Post = new Post { TopicId = topicId }, //Setting the ViewModels post and comment, and the Post inside the viewmodels TopicId.
                    Comment = new Comment()
                };
                return View(postCommentViewModel); // Returns the view with the initialized post and comment.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a post");
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCommentViewModel postCommentViewModel)
        {
            ModelState.Remove("Topic.Room");
            ModelState.Remove("Post.Topic"); //Decided to remove Topic (nav property) from validation. It is not properly set at this point,
            // and the validation is most important to check user inputs. In addition, having to validate Topics due to the link
            // might cause unnecessary loading time. Same for room.

            if (ModelState.IsValid)
            {
                //Need to figure out how to create the comment here
                await _postRepository.Create(postCommentViewModel.Post);
                return RedirectToAction("TopicDetails", "Topic", new { id = postCommentViewModel.Post.TopicId });//Return to Topic/TopicDetails/TopicId after create.
            }
            _logger.LogWarning("[PostController] Post creation failed {@post}", postCommentViewModel.Post);
            return View(postCommentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostDetails(int Id)
        {
            var post = await _postRepository.GetItemById(Id);

            if (post == null)
            {
                _logger.LogError("[PostController] post not found for the TopicId {PostId:0000}", Id);
                return NotFound("Post not found!");
            }
            // Send rommet til visningen for topicetaljer
            return View(post);
        }

        // POST: Topic
        [HttpPost]
        public async Task<IActionResult> UpdatePost(int postId, Post post)
        {
            if (postId != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postRepository.Update(post);
                }
                catch
                {
                }

                return RedirectToAction(nameof(PostTable));
            }

            return View(post);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var topic = await _postRepository.GetItemById(postId);

            if (topic == null)
            {
                _logger.LogError("[PostController] post not found for the PostId {PostId:0000}", postId);
                return BadRequest("Category not found for the CategoryId");
            }
            return View(topic);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedPost(int postId)
        {
            bool returnOk = await _postRepository.Delete(postId);
            if (!returnOk)
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {TopicId:0000}", postId);
                return BadRequest("topic deletion failed");

            }

            return RedirectToAction(nameof(PostTable));

        }
    }


}

