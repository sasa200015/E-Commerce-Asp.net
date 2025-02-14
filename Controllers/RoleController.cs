using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> role;
        private readonly GeneralRes res;
        public RoleController(RoleManager<IdentityRole> role, GeneralRes res)
        {
            this.role = role;
            this.res = res;
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddRole(Roles ReqModel)
        {
            if (ModelState.IsValid)
            {

                IdentityRole roles = new IdentityRole();
                roles.Name = ReqModel.RoleName;
                IdentityResult result = await role.CreateAsync(roles);
                if (result.Succeeded)
                {
                    res.Message = "Success";
                    res.Data = roles;
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("RoleError", item.Description);
                }
            }
            res.Message = "fail";
            res.Data = ModelState;
            return StatusCode(StatusCodes.Status400BadRequest, res);
        }
    }
}
