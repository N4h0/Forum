using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Microsoft.AspNetCore.Mvc;
using Forum.Models;
using Forum.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Forum.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Forum.Controllers
{
    public class CommentController : Controller
        
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        /*
        public async Task<IActionResult> CommentTable()
        {
            var comments = await _commentRepository.GetAll();
            var commentListViewModel = new CommentListViewModel(comments);
            return View(commentListViewModel);
        }*/

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
    }
}