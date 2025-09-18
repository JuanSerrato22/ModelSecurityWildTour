using Microsoft.AspNetCore.Mvc;
using Utilities.Results;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult HandleServiceResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess)
            {
                return result.Data == null ? NoContent() : Ok(new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
                });
            }

            if (result.Errors.Contains("RESOURCE_NOT_FOUND") || result.Message.Contains("no fue encontrada"))
            {
                return NotFound(new
                {
                    success = false,
                    message = result.Message,
                    errors = result.Errors
                });
            }

            if (result.Errors.Any(e => e.Contains("VALIDATION")) || result.Message.Contains("validación"))
            {
                return BadRequest(new
                {
                    success = false,
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = result.Message,
                errors = result.Errors
            });
        }

        protected IActionResult HandleServiceResult(ServiceResult result)
        {
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    success = true,
                    message = result.Message
                });
            }

            if (result.Errors.Contains("RESOURCE_NOT_FOUND") || result.Message.Contains("no fue encontrada"))
            {
                return NotFound(new
                {
                    success = false,
                    message = result.Message,
                    errors = result.Errors
                });
            }

            if (result.Errors.Any(e => e.Contains("VALIDATION")) || result.Message.Contains("validación"))
            {
                return BadRequest(new
                {
                    success = false,
                    message = result.Message,
                    errors = result.Errors
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = result.Message,
                errors = result.Errors
            });
        }

        protected IActionResult Created<T>(ServiceResult<T> result, string actionName = "", object? routeValues = null)
        {
            if (result.IsSuccess)
            {
                var response = new
                {
                    success = true,
                    message = result.Message,
                    data = result.Data
                };

                if (!string.IsNullOrEmpty(actionName))
                {
                    return CreatedAtAction(actionName, routeValues, response);
                }

                return StatusCode(201, response);
            }

            return HandleServiceResult(result);
        }
    }
}