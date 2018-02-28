using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Infrastructure;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Controllers {

    [Produces ("application/json")]
    [Route ("api/[controller]")]
    public class BlogController : Controller {
        private IBlogService _blogService;
        private readonly ILogger<BlogController> _logger;

        public BlogController (IBlogService blogService, ILogger<BlogController> logger) {
            _blogService = blogService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll () {
            try {
                _logger.LogInformation (LoggingEvents.GetItem, "Getting all blogs");
                var blogs = _blogService.GetBlogs (0);
                return Ok (blogs);
            } catch (Exception ex) {
                _logger.LogError (LoggingEvents.GetItem, ex, "Error when getting all blogs");
                return BadRequest ();
            }

        }

        [HttpGet ("{id}", Name = "GetBlog")]
        public IActionResult GetById (int id) {
            try {
                _logger.LogInformation (LoggingEvents.GetItem, "Getting blog {ID}", id);
                var item = _blogService.GetBlogs (id).FirstOrDefault();
                if (item == null) {
                    _logger.LogWarning (LoggingEvents.GetItem, "Unable to find blog id {ID}", id);
                    return NotFound ();
                }
                return new ObjectResult (item);
            } catch (Exception ex) {
                _logger.LogError (LoggingEvents.GetItem, ex, "Error when getting blog id {ID}", id);
                return BadRequest ();
            }

        }

        [HttpPost]
        [ProducesResponseType (typeof (Blog), 201)]
        [ProducesResponseType (typeof (Blog), 400)]
        public IActionResult Create ([FromBody] Blog item) {
            try {
                _logger.LogInformation (LoggingEvents.GenerateItems, "Creating new blog at {RequestTime}", DateTime.Now);
                if (item == null) {
                    _logger.LogWarning (LoggingEvents.GenerateItems, "Attepted to create null blog");
                    return BadRequest ();
                }
                var newBlog = _blogService.CreateBlog (item);

                return CreatedAtRoute ("GetBlog", new { id = item.BlogId }, item);
            } catch (Exception ex) {
                _logger.LogError (LoggingEvents.GenerateItems, ex, "Error creating new blog at {RequestTime}", DateTime.Now);
                return BadRequest ();
            }

        }

        [HttpPut ("{id}")]
        public IActionResult Put (int id, [FromBody] Blog blog) {
            try {
                if (blog == null || blog.BlogId != id) {
                    _logger.LogWarning (LoggingEvents.UpdateItemNotFound, "Cannot update Null blog");
                    Console.WriteLine("Can't update null blog");
                    return BadRequest ();
                }
                _logger.LogInformation ("Updating blog id {ID}", id);
                Console.WriteLine("Updating blog id {0}", id);
                _blogService.UpdateBlog (blog);
                return Ok ();
            } catch (Exception ex) {
                _logger.LogError (LoggingEvents.UpdateItem, ex, "Error updating blog");
                return BadRequest ();
            }

        }

        [HttpDelete ("{id}")]
        public IActionResult Delete (int id) {
            try {
                if (id == 0) {
                    _logger.LogWarning (LoggingEvents.DeleteItem, "Invalid blog id {ID}", id);
                    return BadRequest ();
                }
                var blog = _blogService.GetBlogs (id).FirstOrDefault ();

                if (blog == null) {
                    _logger.LogWarning (LoggingEvents.DeleteItem, "Cannot find blog to delete");
                    return NotFound ();
                }
                _logger.LogInformation (LoggingEvents.DeleteItem, "Deleting blog {ID}", id);
                _blogService.DeleteBlog (blog);
                return Ok ();
            } catch (Exception ex) {
                _logger.LogError (LoggingEvents.DeleteItem, ex, "Error deleting blog");
                return BadRequest ();

            }

        }

    }
}