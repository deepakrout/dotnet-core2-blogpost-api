using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Controllers {

    [Produces ("application/json")]
    [Route ("api/[controller]")]
    public class BlogController : Controller {
        private IBlogService _blogService;
        private readonly ILogger<BlogController> _logger;

        public BlogController (IBlogService blogService) {
            _blogService = blogService;
        }

        [HttpGet]
        public IActionResult GetAll () {
            try {
                var blogs = _blogService.GetBlogs (0);
                return Ok (blogs);
            } catch (Exception ex) {
                //TODO: Log error 
                return BadRequest ();
            }

        }

        [HttpGet ("{id}", Name = "GetBlog")]
        public IActionResult GetById (int id) {
            try {
                var item = _blogService.GetBlogs (id).Where (a => a.BlogId == id).FirstOrDefault ();
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
        [ProducesResponseType (typeof (Blog), 201)]
        [ProducesResponseType (typeof (Blog), 400)]
        public IActionResult Create ([FromBody] Blog item) {
            try {
                if (item == null) {
                    return BadRequest ();
                }
                var newBlog = _blogService.CreateBlog (item);

                return CreatedAtRoute ("GetBlog", new { id = item.BlogId }, item);
            } catch (Exception ex) {
                //TODO: log error
                return BadRequest ();
            }

        }

    }
}