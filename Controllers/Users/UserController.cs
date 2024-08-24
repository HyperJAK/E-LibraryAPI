using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace ELib_IDSFintech_Internship.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserService _service;
        private readonly ILogger<UserController> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "User";


        public UserController(ILogger<UserController> logger, UserService service)
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

        [HttpPost("api/verifyUser")]
        public async Task<IActionResult> VerifyUser(VerificationRequest verificationObject)
        {
            _logger.LogInformation($"Verifying a {_logName}, Controller Layer");

            try
            {
                var user = await _service.VerifyUser(verificationObject);

                if (user == null)
                {
                    _logger.LogWarning($"No {_logName} to verify");
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while verifying a {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("api/create")]
        public async Task<IActionResult> Create(User newObject)
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
        public async Task<IActionResult> Update(User modifiedObject)
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

        //prefferably replace userId with session id or with an object that takes both
        [HttpPost("api/borrowBook")]
        public async Task<IActionResult> BorrowBook(BorrowBookRequest request)
        {
            _logger.LogInformation($"Creating a {_logName}, Controller Layer");

            try
            {
                var result = await _service.BorrowBook(request);
                
                switch (result)
                {
                    //subscription needed
                    case -3:
                        {
                            return Ok(new { status = 433, message = "Subscription needed" });
                        }

                    //no book found
                    case -2:
                        {
                            return Ok(new { status = 432, message = "Error, No such book was found" });
                        }
                        
                        // no user found
                    case -1:
                        {
                            return Ok(new { status = 431, message = "User was not found, please login" });
                        }

                    default:
                        //acceptable
                        if (result > 0)
                        {
                            return Ok(new { message = "Book was successfully borrowed" });
                        }
                        else
                        {
                            return BadRequest();
                        }

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
