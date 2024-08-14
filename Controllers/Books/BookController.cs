using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace ELib_IDSFintech_Internship.Controllers.Books
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;
        private readonly ILogger<BookController> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Book";


        public BookController(ILogger<BookController> logger, BookService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("api/data")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"Getting all {_logName}s information, Controller Layer");

            try
            {
                var result = await _service.GetAll();

                if (result == null || result.Count() == 0)
                {
                    _logger.LogWarning($"No {_logName}s found");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all {_logName}s");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("api/create")]
        public async Task<IActionResult> Create(Book newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Controller Layer");

            try
            {
                var countCreated = await _service.Create(newObject);

                if (countCreated == null || countCreated.Value <= 0)
                {
                    _logger.LogWarning($"No {_logName} Updated");
                    return NotFound();
                }

                return Ok(countCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("api/data/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Getting a single {_logName} with ID: {id}, Controller Layer");

            try
            {
                var result = await _service.GetById(id);

                if (result == null)
                {
                    _logger.LogWarning($"No {_logName} found");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} with ID: {id}");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPut("api/update")]
        public async Task<IActionResult> Update(Book modifiedObject)
        {
            _logger.LogInformation($"Updating a {_logName}, Controller Layer");

            try
            {
                var countUpdated = await _service.Update(modifiedObject);

                if (countUpdated == null || countUpdated.Value <= 0)
                {
                    _logger.LogWarning($"No {_logName} Updated");
                    return NotFound();
                }

                return Ok(countUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpDelete("api/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting a {_logName} with ID: {id}, Controller Layer");

            try
            {
                var countDeleted = await _service.Delete(id);

                if (countDeleted == null || countDeleted.Value <= 0)
                {
                    _logger.LogWarning($"No {_logName} deleted");
                    return NotFound();
                }

                return Ok(countDeleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting {_logName} with ID: {id}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("api/clearCache")]
        public async Task<IActionResult> ClearCache()
        {
            _logger.LogInformation($"Clearing all cached {_logName}s, Controller Layer");

            try
            {
                var cleared =  await _service.ClearCache();

                if (!cleared.Value)
                {
                    _logger.LogWarning($"No cached {_logName}s cleared");
                    return NotFound();
                }
                else
                {
                    return Ok();
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while clearing cached {_logName}s");
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
