using ELib_IDSFintech_Internship.Models.Books;
using ELib_IDSFintech_Internship.Models.Users;
using ELib_IDSFintech_Internship.Services.Enums;
using ELib_IDSFintech_Internship.Services.Tools;
using ELib_IDSFintech_Internship.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

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
                var (user, sessionId) = await _service.VerifyUser(verificationObject);

                if (user == null || sessionId == null)
                {
                    _logger.LogWarning($"No {_logName} to verify");
                    return NotFound();
                }

                Response.Headers.Append("x-session-id", sessionId);

                return Ok(new { userData = user, status = ResponseType.ResponseSuccess, message = "User Signed in successfully" });
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
                var (user, sessionId) = await _service.Create(newObject);

                if (user == null || sessionId == null)
                {
                    _logger.LogWarning($"No {_logName} Updated");
                    return NotFound();
                }

                Response.Headers.Append("x-session-id", sessionId);

                return Ok(new { userData = user, status = ResponseType.ResponseSuccess, message = "User created successfully" });
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

                if (countUpdated > 0)
                {
                    return Ok(new { status = 200, message = "user successfully updated" });
                }
                else
                {
                    _logger.LogWarning($"No {_logName} Updated");
                    return Ok(new { status = 404, message = "Error, user couldn't be updated" });
                }
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
            _logger.LogInformation($"Adding a book for a {_logName}, Controller Layer");

            try
            {
                var result = await _service.BorrowBook(request);
                
                switch ((ResponseType)result)
                {
                    //Book out of stock
                    case ResponseType.OutOfBook:
                        {
                            return Ok(new { status = ResponseType.OutOfBook, message = "Book out of stock" });
                        }
                    //User already borrowing book
                    case ResponseType.UserAlreadyBorrow:
                        {
                            return Ok(new { status = ResponseType.UserAlreadyBorrow, message = "You are already borrowing this book" });
                        }
                    //subscription needed
                    case ResponseType.SubscriptionNeeded:
                        {
                            return Ok(new { status = ResponseType.SubscriptionNeeded, message = "Subscription needed" });
                        }

                    //no book found
                    case ResponseType.NoObjectFound:
                        {
                            return Ok(new { status = ResponseType.NoObjectFound, message = "Error, No such book was found" });
                        }
                        
                        // no user found
                    case ResponseType.UserNotLoggedIn:
                        {
                            return Ok(new { status = ResponseType.UserNotLoggedIn, message = "User was not found, please login" });
                        }

                    // Success
                    case ResponseType.ResponseSuccess:
                        {
                            return Ok(new { status = ResponseType.ResponseSuccess, message = "Book was successfully borrowed" });
                        }

                    default:
                        return BadRequest();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding book to a {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

        //prefferably replace userId with session id or with an object that takes both
        [HttpPost("api/addSubscription")]
        public async Task<IActionResult> AddSubscription(AddSubscriptionRequest request)
        {
            _logger.LogInformation($"Adding a subscription for a {_logName}, Controller Layer");

            try
            {
                var result = await _service.AddSubscription(request);

                switch ((ResponseType)result)
                {
                    //User already subscribed
                    case ResponseType.UserAlreadySubscribed:
                        {
                            return Ok(new { status = ResponseType.UserAlreadySubscribed, message = "You are already subscribed to this subscription" });
                        }

                    //no subscription found
                    case ResponseType.NoObjectFound:
                        {
                            return Ok(new { status = ResponseType.NoObjectFound, message = "Error, Subscription not found" });
                        }

                    // no user found
                    case ResponseType.UserNotLoggedIn:
                        {
                            return Ok(new { status = ResponseType.UserNotLoggedIn, message = "User was not found, please login" });
                        }

                    // Success
                    case ResponseType.ResponseSuccess:
                        {
                            return Ok(new { status = ResponseType.ResponseSuccess, message = "Successfully subscribed" });
                        }

                    default:
                        return BadRequest();

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding a subscription for a {_logName}");
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
