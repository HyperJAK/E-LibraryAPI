﻿using ELib_IDSFintech_Internship.Models.Sessions;
using ELib_IDSFintech_Internship.Services.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace ELib_IDSFintech_Internship.Controllers.Sessions
{
    [ApiController]
    [Route("[controller]")]
    public class SessionManagementController : ControllerBase
    {
        private readonly SessionManagementRepository _service;
        private readonly ILogger<SessionManagementController> _logger;

        //conveniently used when was copy pasting from another controller to this, and left behind.
        private readonly string _logName = "Session";


        public SessionManagementController(ILogger<SessionManagementController> logger, SessionManagementRepository service)
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
        public async Task<IActionResult> Create(Session newObject)
        {
            _logger.LogInformation($"Creating a {_logName}, Controller Layer");

            try
            {
                var countCreated = await _service.Create(newObject);

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

        [HttpPost("api/generateSessionId")]
        public async Task<IActionResult> GenerateSessionId(int userId)
        {
            var sessionId = await _service.GenerateSessionId(userId);

            if (sessionId == null)
            {
                return StatusCode(500, "Failed to generate session ID.");
            }

            return Ok(new { SessionId = sessionId });
        }

        [HttpPost("api/compareSessionIds")]
        public async Task<IActionResult> CompareSessionIds(SessionActionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.SessionID) || request.UserId <= 0)
            {
                return BadRequest("Invalid request.");
            }

            var isEqual = await _service.EqualSessionIds(request);

            if (isEqual == null)
            {
                return StatusCode(500, "Failed to compare session IDs.");
            }
            else if (isEqual == false)
            {
                return StatusCode(500, "Expired SessionID");
            }

            return Ok(new { IsEqual = isEqual });
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
        public async Task<IActionResult> Update(Session modifiedObject)
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

    }
}
