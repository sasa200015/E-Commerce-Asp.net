using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration config;
        private readonly GeneralRes res;

        public UserController(UserManager<User> UserManager, IConfiguration config, GeneralRes res)
        {
            userManager = UserManager;
            this.config = config;
            this.res = res;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] Register modelReq)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = new User();
                    user.Email = modelReq.Email;
                    user.UserName = modelReq.User_Name;
                    user.PhoneNumber = modelReq.Phone_Number;
                    IdentityResult result = await userManager.CreateAsync(user, modelReq.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                        res.Message = "Success";
                        res.Data = new { 
                        user.Email,
                        user.UserName,
                        user.PhoneNumber,
                        user.PasswordHash
                        };
                        return StatusCode(StatusCodes.Status200OK, res);
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Error", item.Description);
                    }
                }
                catch (Exception ex)
                {
                    res.Message = "An unexpected error occurred.";
                    res.Data = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError, res);
                }
            }
            var modelStateErrors = ModelState
                .Where(ms => ms.Value.Errors.Any()) // Only include entries with errors
                .ToDictionary(
                    ms => ms.Key, // Keep the key (field name)
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToList() // Only include error messages
                 );
            res.Message = "Fail";
            res.Data = modelStateErrors;
            return StatusCode(StatusCodes.Status400BadRequest, res);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] Login ReqModel)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(ReqModel.Email);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, ReqModel.Password);
                    if (found)
                    {

                        List<Claim> UserClaims = new List<Claim>();
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        UserClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        var userRole = await userManager.GetRolesAsync(user);

                        foreach (var role in userRole)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SymmetricSecurityKey key = new SymmetricSecurityKey(
                            Encoding.UTF8.
                            GetBytes(config["JWT:SecritKey"]));

                        SigningCredentials credentials = new SigningCredentials(
                            key,
                            SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: config["JWT:IssuerIP"],
                            audience: config["JWT:AudienceIP"],
                            expires: DateTime.Now.AddMinutes(20),
                            claims: UserClaims,
                            signingCredentials: credentials

                            );
                        //split the Date and time to HH:mm:ss
                        Token generated = new Token();
                        DateTime spliter = DateTime.Now.AddMinutes(20);
                        string dateTimeString = spliter.ToString("yyyy-MM-ddTHH:mm:ss");
                        string[] splitResult = dateTimeString.Split('T');
                        generated.token = new JwtSecurityTokenHandler().WriteToken(token);
                        generated.expires = splitResult[1];
                        res.Message = "Success";
                        res.Data = generated;

                        return StatusCode(StatusCodes.Status200OK, res);
                    }
                }
                ModelState.AddModelError("UserName", "UserName or Password invalid");
                res.Message = "Fail";
                res.Data = ModelState["UserName"].Errors[0].ErrorMessage;
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }
            res.Message = "Fail";
            res.Data = ModelState;
            return StatusCode(StatusCodes.Status400BadRequest, res);

        }
    }
}
