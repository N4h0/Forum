using Microsoft.AspNetCore.Mvc;

using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository; //We need this so that we can create a comment when we create a post.
        private readonly ILogger<PostController> _logger;


        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository; //We need this so that we can create a comment when we create a post.
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
        [Authorize]
        public IActionResult CreatePost(int TopicId) //Create a post with a given Id, which is (should be) passed  when the method is called.
        {
            try
            {
                var postCommentViewModel = new PostCommentViewModel
                {
                    Post = new Post { TopicId = TopicId }, //Setting the ViewModels post and comment, and the Post inside the viewmodels TopicId.
                    Comment = new Comment()
                };
                return View(postCommentViewModel); // Returns the view with the initialized post and comment.
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating a post");
                throw;
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(PostCommentViewModel postCommentViewModel)
        {
            if (ModelState.IsValid)
            {
                //Need to figure out how to create the comment here
                await _postRepository.Create(postCommentViewModel.Post); //Creating post
                _logger.LogError("We're here");
                postCommentViewModel.Comment.PostId = postCommentViewModel.Post.PostId; //Then assigning the Comments postId equal to Posts Postid
                postCommentViewModel.Comment.UserId = postCommentViewModel.Post.UserId; //Then assigning the Comments userId equal to Posts Userid
                await _commentRepository.Create(postCommentViewModel.Comment); //Then I can create the comment.
                _logger.LogError("We're here");
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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdatePost(int Id)
        {
            var Post = await _postRepository.GetItemById(Id);

            if (Post == null)
            {
                _logger.LogError("The post with ID {PostId} was not found.", Id);
                return NotFound();
            }

            return View(Post);
        }

        // POST: Topic
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdatePost(Post post)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _postRepository.Update(post);
                }
                catch(Exception e)
                {
                    //TODO FILL OUT (OR remove???) catch
                    _logger.LogError("An error occurred while updating the post", e);
                }

                return RedirectToAction("TopicDetails", "Topic", new { id = post.TopicId }); ;
            }
            _logger.LogError("Model validation failed for Post");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogError($"Key: {state.Key}, Error: {error.ErrorMessage}");
                }
            }

            return View(post);
        }

        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeletePost(int Id)
        {
            var topic = await _postRepository.GetItemById(Id);

            if (topic == null)
            {
                _logger.LogError("[PostController] post not found for the PostId {PostId:0000}", Id);
                return BadRequest("Category not found for the CategoryId");
            }
            return View(topic);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmedPost(int Id)
        {
            var TopicId = await _postRepository.GetTopicId(Id);
            bool returnOk = await _postRepository.Delete(Id);
            if (!returnOk)
            {
                _logger.LogError("[TopicController] Topic deletion failed for the TopicId {TopicId:0000}", Id);
                return BadRequest("topic deletion failed");

            }

            return RedirectToAction("TopicDetails", "Topic", new { id = TopicId });

        }
    }


}

