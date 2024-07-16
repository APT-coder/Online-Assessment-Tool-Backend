using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

        public PermissionController(IPermissionsRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        // GET: api/Permission
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllPermissions()
        {
            try
            {
                var permissions = await _permissionRepository.GetAllAsync();
                return Ok(new ApiResponse { IsSuccess = true, Result = permissions, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        // GET: api/Permission/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetPermission(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);

                if (permission == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }

                return Ok(new ApiResponse { IsSuccess = true, Result = permission, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        // POST: api/Permission
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreatePermission(CreatePermissionDTO createPermissionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
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

                var permission = new Permission
                {
                    PermissionName = createPermissionDto.PermissionName,
                    Description = createPermissionDto.Description
                };

                await _permissionRepository.AddAsync(permission);

                return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, new ApiResponse { IsSuccess = true, Result = permission, StatusCode = HttpStatusCode.Created });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }


        // PUT: api/Permission/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdatePermission(int id, UpdatePermissionDTO updatePermissionDto)
        {
            try
            {
                if (id != updatePermissionDto.Id)
                {
                    return BadRequest(new ApiResponse { IsSuccess = false, Message = new List<string> { "Request id does not match permission id" }, StatusCode = HttpStatusCode.BadRequest });
                }

                var existingPermission = await _permissionRepository.GetByIdAsync(id);

                if (existingPermission == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }

                // Update the properties
                existingPermission.PermissionName = updatePermissionDto.PermissionName;
                existingPermission.Description = updatePermissionDto.Description;

                await _permissionRepository.UpdateAsync(existingPermission);

                return Ok(new ApiResponse { IsSuccess = true, Result = existingPermission, StatusCode = HttpStatusCode.OK });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse { IsSuccess = false, Message = new List<string> { ex.Message }, StatusCode = HttpStatusCode.InternalServerError });
            }
        }

        // DELETE: api/Permission/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeletePermission(int id)
        {
            try
            {
                var permission = await _permissionRepository.GetByIdAsync(id);
                if (permission == null)
                {
                    return NotFound(new ApiResponse { IsSuccess = false, Message = new List<string> { "Permission not found" }, StatusCode = HttpStatusCode.NotFound });
                }

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
