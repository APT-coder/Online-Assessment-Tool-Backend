using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionsRepository _permissionsRepository;

        public RolesController(IRoleRepository roleRepository, IPermissionsRepository permissionsRepository)
        {
            _roleRepository = roleRepository;
            _permissionsRepository = permissionsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            try
            {
                var rolesWithPermissions = await _roleRepository.GetAllAsync();

                if (rolesWithPermissions == null || !rolesWithPermissions.Any())
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "No roles found." } });
                }

                return Ok(new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = rolesWithPermissions });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error retrieving roles from database." } });
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Role not found." }
                    });
                }

                // Extract permission IDs from role's permissions
                var permissionIds = role.Permissions.Select(p => p.Id).ToList();

                var roleDto = new
                {
                    id = role.Id,
                    roleName = role.RoleName,
                    permissionIds = permissionIds
                };

                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = roleDto
                });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error retrieving role from database." }
                });
            }
        }



        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostRole(CreateRoleDTO createRoleDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createRoleDTO.RoleName))
                {
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Role name is required." }
                    });
                }

                if (createRoleDTO.PermissionIds == null || createRoleDTO.PermissionIds.Count == 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = new List<string> { "Role must have at least one permission." }
                    });
                }

                // Fetch existing permissions from the database by IDs
                var existingPermissions = await _permissionsRepository.GetByIdsAsync(createRoleDTO.PermissionIds);

                if (existingPermissions.Count != createRoleDTO.PermissionIds.Count)
                {
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

                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = role
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                //  _logger.LogError(ex, "Error creating role.");

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
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = new List<string> { "Role not found." }
                    });
                }

                // Update role name
                role.RoleName = createRoleDTO.RoleName;

                // Fetch existing permissions from the database
                var existingPermissions = await _permissionsRepository.GetByIdsAsync(createRoleDTO.PermissionIds);

                // Update the role's permissions
                role.Permissions = existingPermissions.ToList();

                // Save changes
                await _roleRepository.UpdateAsync(role);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = new List<string> { "Error updating role." }
                });
            }
        }


        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Role not found." } });
                }

                await _roleRepository.DeleteAsync(role);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error deleting role." } });
            }
        }
    }
}