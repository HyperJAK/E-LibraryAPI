using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Books.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Enums;
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
        public async Task<IActionResult> Create([FromBody] BookActionRequest request)
        {
            try
            {
                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (ModelState.IsValid)
                {
                    //if no SessionID in request
                    if (request.SessionID == null)
                    {
                        return Ok(new { status = ResponseType.UserNotLoggedIn, message = "You are not logged in or Session expired please relogin" });
                    }

                    if (request.EntityObject != null)
                    {
                        _logger.LogInformation($"Creating a {_logName}, Controller Layer");
                        var response = await _service.Create(request.EntityObject);

                        return Ok(response);
                    }
                    else
                    {
                        return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to update" });
                    }
                }
                else
                {
                    return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to update" });
                }

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

        [HttpGet("api/getSuggestions/{name}")]
        public async Task<IActionResult> GetSuggestionsByName(string name)
        {
            _logger.LogInformation($"Getting {_logName} suggestions with Name: {name}, Controller Layer");

            try
            {
                var result = await _service.GetSuggestionsByName(name);

                if (result == null)
                {
                    _logger.LogWarning($"No {_logName} suggestions found");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} suggestions with Name: {name}, in Controller Layer");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("api/getSearchResults/{name}")]
        public async Task<IActionResult> GetSearchResultsByName(string name)
        {
            _logger.LogInformation($"Getting {_logName} search results with Name: {name}, Controller Layer");

            try
            {
                var result = await _service.GetSearchResultsByName(name);

                if (result == null)
                {
                    _logger.LogWarning($"No {_logName} search results found");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} search results with Name: {name}, in Controller Layer");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("api/getBooksByGenre/{id}")]
        public async Task<IActionResult> GetBooksByGenre(int id)
        {
            _logger.LogInformation($"Getting {_logName}s by Genre: {id}, Controller Layer");

            try
            {
                var result = await _service.GetBooksByGenre(id);

                if (result == null)
                {
                    _logger.LogWarning($"No {_logName} found in this genre");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName}s by Genre: {id}, in Controller Layer");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPut("api/update")]
        public async Task<IActionResult> Update([FromBody] BookActionRequest request)
        {
            try
            {
                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (ModelState.IsValid)
                {
                    //if no SessionID in request
                    if (request.SessionID == null)
                    {
                        return Ok(new { status = ResponseType.UserNotLoggedIn, message = "You are not logged in or Session expired please relogin" });
                    }

                    if (request.EntityObject != null)
                    {
                        _logger.LogInformation($"Updating a {_logName}, Controller Layer");
                        var response = await _service.Update(request.EntityObject);

                        return Ok(response);
                    }
                    else
                    {
                        return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to update" });
                    }
                }
                else
                {
                    return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to update" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpDelete("api/delete")]
        public async Task<IActionResult> Delete([FromBody] BookActionRequest request)
        {
            try
            {
                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (ModelState.IsValid)
                {
                    //if no SessionID in request
                    if (request.SessionID == null)
                    {
                        return Ok(new { status = ResponseType.UserNotLoggedIn, message = "You are not logged in or Session expired please relogin" });
                    }

                    if (request.Id != null)
                    {
                        _logger.LogInformation($"Deleting a {_logName} with ID: {request.Id}, Controller Layer");
                        var response = await _service.Delete((int)request.Id);

                        return Ok(response);
                    }
                    else
                    {
                        return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to delete" });
                    }
                }
                else
                {
                    return Ok(new { status = ResponseType.NoObjectFound, message = $"The request didn't include an {_logName} to delete" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("api/clearCache")]
        public async Task<IActionResult> ClearCache(string key)
        {
            _logger.LogInformation($"Clearing all cached {_logName}s, Controller Layer");

            try
            {
                var cleared = await _service.ClearCache($"Book_{key}");

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

        [HttpGet("api/borrowedBooks/{userId}")]
        public async Task<IActionResult> GetBorrowedBooks(int userId)
        {
            _logger.LogInformation($"Getting all {_logName}s borrowed by user with ID: {userId}, Controller Layer");

            try
            {
                var result = await _service.GetBorrowedBooks(userId);

                if (result == null)
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

    }
}
