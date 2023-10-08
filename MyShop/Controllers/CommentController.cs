using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating a comment");
                throw;
            }
        }
        [HttpPost] //HttpPost is responsible for submitting the form.
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a comment");
                throw;
            }
        }
        // GET: Comment/
        [HttpGet]
        public async Task<IActionResult> UpdateComment(int Id)
        {
            var Comment = await _commentRepository.GetItemById(Id);

            if (Comment == null)
            {
                return NotFound();
            }

            return View(Comment);
        }

        // POST: Comment
        [HttpPost]
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _commentRepository.Update(comment);
                }
                catch
                {
                    //TODO fill out this catch
                }

                return RedirectToAction("PostDetails", "Post", new { id = comment.PostId }); //Return to Post/PostDetails/PostId after create.
            }

            return View(comment);
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> DeleteComment(int Id)
        {
            var Comment = await _commentRepository.GetItemById(Id);

            if (Comment == null)
            {
                return NotFound();
            }

            return View(Comment);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedComment(int Id)
        {
            var PostId = await _commentRepository.GetPostId(Id);
            try
            {
                await _commentRepository.Delete(Id);
                return RedirectToAction("PostDetails", "Post", new { id = PostId }); //Return to Post/PostDetails/PostId after create. TODO fiks
            }
            catch
            {
                // TODO handle exceptions here
                return RedirectToAction("PostDetails", "Post", new { id = PostId }); 
            }

        }
    }
}