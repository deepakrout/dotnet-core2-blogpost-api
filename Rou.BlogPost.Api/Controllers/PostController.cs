using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
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
                var posts = _postService.GetPosts (0);
                return Ok (posts);
            } catch (Exception ex) {
                //TODO: Log error 
                return BadRequest ();
            }

        }

        [HttpGet ("{id}", Name = "GetPost")]
        public IActionResult GetById (int id) {
            try {
                var item = _postService.GetPosts (id).Where (a => a.PostId == id).FirstOrDefault ();
                if (item == null) {
                    return NotFound ();
                }
                return new ObjectResult (item);
            } catch (Exception ex) {
                //TODO: loggerror
                return BadRequest ();
            }

        }

        [HttpPost]
        [ProducesResponseType (typeof (Post), 201)]
        [ProducesResponseType (typeof (Post), 400)]
        public IActionResult Create ([FromBody] Post item) {
            try {
                if (item == null) {
                    return BadRequest ();
                }
                var newBlog = _postService.CreatePost (item);

                return CreatedAtRoute ("GetPost", new { id = item.PostId }, item);
            } catch (Exception ex) {
                //TODO: log error
                return BadRequest ();
            }

        }
    }
}