using ELib_IDSFintech_Internship.Models.Books.Authors;
using ELib_IDSFintech_Internship.Models.Books.Authors.RequestPayloads;
using ELib_IDSFintech_Internship.Models.Users.RequestPayloads;
using ELib_IDSFintech_Internship.Repositories;
using ELib_IDSFintech_Internship.Repositories.Books.Authors;
using ELib_IDSFintech_Internship.Services.Books;
using ELib_IDSFintech_Internship.Services.Enums;
using ELib_IDSFintech_Internship.Services.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ELib_IDSFintech_Internship.Controllers.Books
{
    [ApiController]
    [Route("[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly BookAuthorService _service;
        private readonly ILogger<BookAuthorController> _logger;
        private readonly SessionManagementService _sessionManager;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "BookAuthor";

        public BookAuthorController(ILogger<BookAuthorController> logger, BookAuthorService service, SessionManagementService sessionManager)
        {
            _logger = logger;
            _service = service;
            _sessionManager = sessionManager;
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
        public async Task<IActionResult> Create(AuthorActionRequest request)
        {
            _logger.LogInformation($"Creating a {_logName}, Controller Layer");

            try
            {
                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var countCreated = await _service.Create(request.EntityObject);

                if (countCreated == null)
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
        public async Task<IActionResult> GetById(UserActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Getting a single {_logName} with ID: {request.Id}, Controller Layer");

                var result = await _service.GetById((int)request.Id);

                if (result == null)
                {
                    _logger.LogWarning($"No {_logName} found");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting {_logName} with ID: {request.Id}");
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPut("api/update")]
        public async Task<IActionResult> Update(AuthorActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Updating a {_logName}, Controller Layer");

                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (ModelState.IsValid)
                {
                    //if no SessionID in request
                    if(request.SessionID == null)
                    {
                        return Ok(new { status = ResponseType.UserNotLoggedIn, message = "You are not logged in or Session expired please relogin" });
                    }

                    if (request.EntityObject != null)
                    {
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
        public async Task<IActionResult> Delete(UserActionRequest request)
        {
            try
            {
                _logger.LogInformation($"Deleting a {_logName} with ID: {request.Id}, Controller Layer");

                //first thing we do is validate wether our object is valid based on the rules that we provided in the Class that it belongs to
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else if(request.Id != null)
                {
                    var countDeleted = await _service.Delete((int)request.Id);

                    if (countDeleted == null || countDeleted.Value <= 0)
                    {
                        _logger.LogWarning($"No {_logName} deleted");
                        return NotFound();
                    }

                    return Ok(countDeleted);
                }
                else
                {
                    //replace this with the ResponseType thing like in BorrowBook
                    return BadRequest();
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting {_logName} with ID: {request.Id}");
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
