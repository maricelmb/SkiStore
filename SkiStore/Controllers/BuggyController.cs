﻿using Microsoft.AspNetCore.Mvc;

namespace SkiStore.Controllers
{
    public class BuggyController: BaseApiController
    {
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("This is not a good request.");
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorised()
        {
            return Unauthorized();
        }
        
        [HttpGet("validation-error")]
        public IActionResult GetValidationError(int id)
        {
            ModelState.AddModelError("Problem1", "This is First error");
            ModelState.AddModelError("Problem2", "This is second error");

            return ValidationProblem();
        }


        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }   
    }
}
