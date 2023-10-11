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

namespace Forum.Controllers
{
    public class CommentController : Controller
        
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        [HttpGet] //HttpGet is responsible for displaying the form
        [Authorize]
        public IActionResult CreateComment(int postId) //CreateCommentView with the spesific postId

        {
            try
            { 
            var comment = new Comment //Creating a new comment
            {
               PostId = postId //setting the postID of the new comment
            };
            return View(comment); //Returning the view with the created comment (with the postID, importantly)
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured while creating a comment");
                throw;
            }
        }
        [HttpPost] //HttpPost is responsible for submitting the form.
        [Authorize]
        public async Task<IActionResult> CreateComment(Comment comment)
        {

            try
            { 
            if (ModelState.IsValid)
            {
                await _commentRepository.Create(comment);
                return RedirectToAction("PostDetails", "Post", new { id = comment.PostId }); //Return to Post/PostDetails/PostId after create.
            }
            _logger.LogWarning("Comment creation failed, ModelState is invalid.");
            return View(comment);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating a comment");
                throw;
            }
        }
        // GET: Comment/
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateComment(int Id)
        {
            var Comment = await _commentRepository.GetItemById(Id);

            if (Comment == null)
            {
                _logger.LogError("Unable to find comment with ID {Id}", Id);
                return NotFound();
            }

            return View(Comment);
        }

        // POST: Comment
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (ModelState.IsValid)
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
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteComment(int Id)
        {
            var Comment = await _commentRepository.GetItemById(Id);

            if (Comment == null)
            {
                _logger.LogWarning("Comment not found when attempting to delete. Comment ID: {CommentId}", Id);
                return NotFound();
            }
            // Log that the view for deleting the comment is being displayed
            _logger.LogInformation("Displaying delete view for Comment ID: {Id}", Id);
            return View(Comment);
        }

        // POST
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmedComment(int Id)
        {
            var PostId = await _commentRepository.GetPostId(Id);
            try
            {
                await _commentRepository.Delete(Id);
                // Log a message that indicate successful deletion of the comment
                _logger.LogInformation("Comment with ID {CommentId} deleted successfully.", Id);

                return RedirectToAction("PostDetails", "Post", new { id = PostId }); //Return to Post/PostDetails/PostId after create. TODO fiks
            }
            catch(Exception e) 
            {
         
                //Log an error message for deleting not working
                _logger.LogError(e, "Error deleting comment with ID {id]", Id);

                return RedirectToAction("PostDetails", "Post", new { id = PostId }); 
            }

        }
    }
}