using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Infrastructure;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Controllers {
    [Produces ("application/json")]
    [Route ("api/[controller]")]
    public class PostController : Controller {
        private IPostService _postService;
        private readonly ILogger<PostController> _logger;
        public PostController (IPostService postService, ILogger<PostController> logger) {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll () {
            try {
                _logger.LogInformation(LoggingEvents.GetItem,"Getting all posts");
                var posts = _postService.GetPosts (0);
                return Ok (posts);
            } catch (Exception ex) {
                _logger.LogError(LoggingEvents.GetItem,ex,"Error retrieving posts");
                return BadRequest ();
            }

        }

        [HttpGet ("{id}", Name = "GetPost")]
        public IActionResult GetById (int id) {
            try {
                _logger.LogInformation(LoggingEvents.GetItem,"Get post by id {ID}",id);
                var item = _postService.GetPosts (id);
                if (item == null) {
                    _logger.LogInformation(LoggingEvents.GetItem,"Post id: {ID} not found.",id);
                    return NotFound ();
                }
                return new ObjectResult (item);
            } catch (Exception ex) {
                _logger.LogError(LoggingEvents.GetItem,ex,"Error getting Posts by id");
                return BadRequest ();
            }

        }

        [HttpPost]
        [ProducesResponseType (typeof (Post), 201)]
        [ProducesResponseType (typeof (Post), 400)]
        public IActionResult Create ([FromBody] Post item) {
            try {
                _logger.LogInformation(LoggingEvents.GenerateItems,"Creating New Posts");
                if (item == null) {
                    _logger.LogInformation(LoggingEvents.GenerateItems,"NUll or Empty Posts. Not Created");
                    return BadRequest ();
                }
                var newBlog = _postService.CreatePost (item);

                return CreatedAtRoute ("GetPost", new { id = item.PostId }, item);
            } catch (Exception ex) {
                _logger.LogError(LoggingEvents.GenerateItems,ex,"Error Creating new Posts");
                return BadRequest ();
            }

        }
    }
}