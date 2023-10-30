using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;

namespace Forum.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger, UserManager<IdentityUser> userManager)
        {
            _commentRepository = commentRepository;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet] //HttpGet is responsible for displaying the form.
        [Authorize] //Any user can access this method.
        public IActionResult CreateComment(int postId) //CreateCommentView with the spesific postId.

        {
            try
            { 
            var comment = new Comment //Creating a new comment.
            {
               PostId = postId //setting the postID of the new comment.
            };
            return View(comment); //Returning the view with the created comment.
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while creating a comment");
                throw;
            }
        }
        [HttpPost] //HttpPost is responsible for submitting the form.
        [Authorize] //Any user can access this method.
        public async Task<IActionResult> CreateComment(Comment comment)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                    comment.CommentTime = DateTime.Now; //Setting the comment-time.
                    var UserName = _userManager.GetUserName(User); //Getting the username of the current inlogged user from
                    //UserManager.
                    comment.UserName = UserName; //Setting the comments username.
                    await _commentRepository.Create(comment); //Sending the model to the DAL to create a comment
                return RedirectToAction("PostDetails", "Post", new { id = comment.PostId }); //Return to Post/PostDetails/PostId after create.
            }
            _logger.LogWarning("[CommentController] Comment creation failed, ModelState is invalid."); //This gets printed if comment is not valid.
            return View(comment); //returning to Create comment with the created comment passed
            }
            catch (Exception e) //On any other errors we get sent to this loop
            {
                _logger.LogError(e, "An error occurred while creating a comment");
                throw;
            }
        }
        // GET: Comment/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //Only superadmin can update comments
        public async Task<IActionResult> UpdateComment(int Id)
        {
            var Comment = await _commentRepository.GetCommentById(Id); //Getting the current comment based on the passed id

            if (Comment == null) //Looging error the comment is null.
            {
                _logger.LogError("[CommentController]Unable to find comment with ID {Id}", Id);
                return NotFound();
            }

            return View(Comment); //Returning to UpdateComment with the passed comment.
        }

        // POST: Comment
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only superadmin can access this method.
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (ModelState.IsValid) //Checking if the comment modelstate is valud.
            {
                try
                {
                    await _commentRepository.Update(comment);
                    // Logging for a successful update
                    _logger.LogInformation("Comment updated: {comment}", comment);
                }
                catch(Exception e)
                {
                    //Logging for an error during update
                  _logger.LogError(e, "Error updating comment: {CommentId}", comment);
                }

                return RedirectToAction("PostDetails", "Post", new { id = comment.PostId }); //Return to Post/PostDetails/PostId after create.
            }
            // Logging for an invalid model state
            _logger.LogWarning("Invalid model state when attempting to update comment: {Comment}", comment);

            return View(comment);
        }

        // GET
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] //only superadmin can delete comments
        public async Task<IActionResult> DeleteComment(int Id)
        {
            var Comment = await _commentRepository.GetCommentById(Id); //Getting the comment pased on the passed id.

            if (Comment == null)
            {
                //Logging if the comment we got is null
                _logger.LogWarning("Comment not found when attempting to delete. Comment ID: {CommentId}", Id);
                return NotFound();
            }
            // Log that the view for deleting the comment is being displayed
            _logger.LogInformation("Displaying delete view for Comment ID: {Id}", Id);
            return View(Comment);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] //Only superadmin can use this method
        public async Task<IActionResult> DeleteConfirmedComment(int Id)
        {
            var PostId = await _commentRepository.GetPostId(Id); //Getting postId based on the current commentId
            try
            {
                await _commentRepository.Delete(Id);
                // Log a message that indicate successful deletion of the comment
                _logger.LogInformation("Comment with ID {CommentId} deleted successfully.", Id);

                return RedirectToAction("PostDetails", "Post", new { id = PostId }); //Return to Post/PostDetails/PostId.
            }
            catch(Exception e) 
            {
         
                //Log an error message for deleting not working
                _logger.LogError(e, "Error deleting comment with ID {id]", Id);

                //Redirect to post/Postdetails/PostId on failed delete.
                return RedirectToAction("PostDetails", "Post", new { id = PostId }); 
            }

        }
    }
}