using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineAssessmentTool.Models;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineAssessmentTool.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetRoles()
        {
            try
            {
                var rolesWithPermissions = await _roleRepository.GetAllRolesAsync();

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
                var role = await _roleRepository.GetRoleAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Role not found." } });
                }

                return Ok(new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = role });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error retrieving role from database." } });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> PostRole(CreateRoleDTO createRoleDTO)
        {
            try
            {
                if (createRoleDTO.Permissions == null || createRoleDTO.Permissions.Count == 0)
                {
                    return BadRequest(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, Message = new List<string> { "Role must have at least one permission." } });
                }

                var role = new Role
                {
                    RoleName = createRoleDTO.RoleName,
                    Permissions = createRoleDTO.Permissions.Select(p => new Permission { PermissionName = p.PermissionName, Description = p.Description }).ToList()
                };

                await _roleRepository.CreateRoleAsync(role);

                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, new ApiResponse { IsSuccess = true, StatusCode = HttpStatusCode.Created, Result = role });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error creating role." } });
            }
        }


        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> PutRole(int id, UpdateRoleDTO updateRoleDTO)
        {
            try
            {
                var role = await _roleRepository.GetRoleAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Role not found." } });
                }

                role.RoleName = updateRoleDTO.RoleName;
                role.Permissions = updateRoleDTO.Permissions.Select(p => new Permission { PermissionName = p.PermissionName, Description = p.Description }).ToList();

                await _roleRepository.UpdateRoleAsync(role);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's error handling strategy
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.InternalServerError, Message = new List<string> { "Error updating role." } });
            }
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteRole(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, Message = new List<string> { "Role not found." } });
                }

                await _roleRepository.DeleteRoleAsync(id);

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