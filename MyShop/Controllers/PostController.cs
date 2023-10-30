using Microsoft.AspNetCore.Mvc;

using Forum.DAL;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;


namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<PostController> _logger;
        private readonly UserManager<IdentityUser> _userManager;



        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, ILogger<PostController> logger, UserManager<IdentityUser> userManager)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository; 
            _logger = logger;
            _userManager = userManager;
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
            var time = DateTime.Now; //Creating a variable "time" To be used as time for both the first posted
            postCommentViewModel.Post.PostTime = time;
            postCommentViewModel.Comment.CommentTime = time; //Setting commentime
            if (ModelState.IsValid)
            {
                var UserName = _userManager.GetUserName(User);
                postCommentViewModel.Post.UserName = UserName;
                await _postRepository.Create(postCommentViewModel.Post); //Creating post
                postCommentViewModel.Comment.PostId = postCommentViewModel.Post.PostId; //Then assigning the Comments postId equal to Posts Postid
                postCommentViewModel.Comment.UserName = UserName; //Then assigning the Comments postId equal to Posts Postid
                var serializedModel = JsonSerializer.Serialize(postCommentViewModel, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                });
                _logger.LogInformation("Attempting to create a new post and comment with the following data: {PostCommentViewModel}", serializedModel);
                await _commentRepository.Create(postCommentViewModel.Comment); //Then I can create the comment.
                return RedirectToAction("TopicDetails", "Topic", new { id = postCommentViewModel.Post.TopicId });//Return to Topic/TopicDetails/TopicId after create.
            }

            _logger.LogWarning("[PostController] Post creation failed {@post}", postCommentViewModel.Post);
            return View(postCommentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PostDetails(int Id)
        {
            var post = await _postRepository.GetPostById(Id);

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
            var Post = await _postRepository.GetPostById(Id);

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
            var topic = await _postRepository.GetPostById(Id);

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

