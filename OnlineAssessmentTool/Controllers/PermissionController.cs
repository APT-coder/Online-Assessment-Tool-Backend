using Microsoft.AspNetCore.Mvc;
using System.Net;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Repository.IRepository;
using OnlineAssessmentTool.Models.DTO;

namespace OnlineAssessmentTool.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionsRepository _permissionRepository;
        private readonly ILogger<PermissionController> _logger;
        public PermissionController(IPermissionsRepository permissionRepository, ILogger<PermissionController> logger)
        {
            _permissionRepository = permissionRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllPermissions()
        {
            try
            {
                _logger.LogInformation("Fetching all Permissions");
                var permissions = await _permissionRepository.GetAllAsync();
                return Ok(new ApiResponse { IsSuccess = true, Result = permissions, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetPermission(int id)
        {
            try
            {
                _logger.LogInformation("Fetching permission with ID {permissionId}", id);
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("Permission with ID {permissionId} not found", id);
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }
                return Ok(new ApiResponse { IsSuccess = true, Result = permission, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreatePermission(CreatePermissionDTO createPermissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CreatePermission");
                    List<string> errors = new List<string>();
                    foreach (var state in ModelState.Values)
                    {
                        foreach (var error in state.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    return BadRequest(new ApiResponse { IsSuccess = false, Message = errors, StatusCode = HttpStatusCode.BadRequest });
                }
                _logger.LogInformation("Creating new permission");
                var permission = new Permission
                {
                    PermissionName = createPermissionDto.PermissionName,
                    Description = createPermissionDto.Description
                };

                await _permissionRepository.AddAsync(permission);
                _logger.LogInformation("New permission created");
                return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, new ApiResponse { IsSuccess = true, Result = permission, StatusCode = HttpStatusCode.Created });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdatePermission(int id, UpdatePermissionDTO updatePermissionDto)
        {
            try
            {
                _logger.LogWarning("Permission ID mismatch for Updating Permission");
                if (id != updatePermissionDto.Id)
                {
                    return BadRequest(new ApiResponse { IsSuccess = false, Message = new List<string> { "Request id does not match permission id" }, StatusCode = HttpStatusCode.BadRequest });
                }
                var existingPermission = await _permissionRepository.GetByIdAsync(id);
                if (existingPermission == null)
                {
                    _logger.LogWarning("Permission with ID {permissionId} not found for update", id);
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }
                existingPermission.PermissionName = updatePermissionDto.PermissionName;
                existingPermission.Description = updatePermissionDto.Description;
                _logger.LogInformation("Updating permission with ID {permissionId}", id);
                await _permissionRepository.UpdateAsync(existingPermission);
                return Ok(new ApiResponse { IsSuccess = true, Result = existingPermission, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePermission(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("Permission with ID {PermissionId} not found for deletion", id);
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }
                _logger.LogInformation("Deleting permission with ID {permissionId}", id);
                await _permissionRepository.DeleteAsync(permission);
                return Ok(new ApiResponse { IsSuccess = true, Result = null, StatusCode = HttpStatusCode.NoContent });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }
    }
}
