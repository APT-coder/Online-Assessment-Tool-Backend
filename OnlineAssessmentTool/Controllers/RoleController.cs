using Microsoft.AspNetCore.Mvc;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System.Net;
using System.Linq;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleRepository roleRepository, IPermissionsRepository permissionsRepository, ILogger<RolesController> logger)
        {
            _roleRepository = roleRepository;
            _permissionsRepository = permissionsRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            try
            {
                _logger.LogInformation("Fetching all Roles");
                var rolesWithPermissions = await _roleRepository.GetAllAsync();
                if (rolesWithPermissions == null || !rolesWithPermissions.Any())
                {
                    _logger.LogWarning("No roles found.");
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "No roles found." } });
                }
                return Ok(new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = rolesWithPermissions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all roles.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { ex.Message } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetRole(int id)
        {
            try
            {
                _logger.LogInformation("Fetching Role with ID {roleId}", id);
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("Role with ID {roleId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Role not found." }
                    });
                }
                var permissionIds = role.Permissions.Select(p => p.Id).ToList();
                var fetchedRole = new
                {
                    id = role.Id,
                    roleName = role.RoleName,
                    permissionIds = permissionIds
                };
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = fetchedRole
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the role with ID {roleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostRole(CreateRoleDTO createRoleDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new role with RoleName {roleName}", createRoleDTO.RoleName);

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("ModelState validation failed: {errors}", string.Join(", ", errors));
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = errors
                    });
                }

                if (string.IsNullOrWhiteSpace(createRoleDTO.RoleName))
                {
                    _logger.LogWarning("Role name is required.");
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Role name is required." }
                    });
                }

                if (createRoleDTO.PermissionIds == null || createRoleDTO.PermissionIds.Count == 0)
                {
                    _logger.LogWarning("Role must have at least one permission.");
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Role must have at least one permission." }
                    });
                }

                var existingPermissions = await _permissionsRepository.GetByIdsAsync(createRoleDTO.PermissionIds);
                if (existingPermissions.Count != createRoleDTO.PermissionIds.Count)
                {
                    _logger.LogWarning("Some permissions do not exist.");
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Some permissions do not exist." }
                    });
                }

                var role = new Role
                {
                    RoleName = createRoleDTO.RoleName,
                    Permissions = existingPermissions
                };

                await _roleRepository.AddAsync(role);

                _logger.LogInformation("Created new role with ID {roleId}", role.Id);
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new role.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error creating role." }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutRole(int id, CreateRoleDTO createRoleDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to update Role with ID {roleId}", id);

                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("Role with ID {roleId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Role not found." }
                    });
                }

                role.RoleName = createRoleDTO.RoleName;
                var existingPermissions = await _permissionsRepository.GetByIdsAsync(createRoleDTO.PermissionIds);
                role.Permissions = existingPermissions.ToList();
                await _roleRepository.UpdateAsync(role);

                _logger.LogInformation("Updated Role with ID {roleId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Role with ID {roleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error updating role." }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteRole(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete Role with ID {roleId}", id);

                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("Role with ID {roleId} not found", id);
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Role not found." }
                    });
                }

                await _roleRepository.DeleteAsync(role);

                _logger.LogInformation("Deleted Role with ID {roleId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Role with ID {roleId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error deleting role." }
                });
            }
        }
    }
}
