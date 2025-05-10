using Library.Repository;
using Library.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRep userRep;
        AuthServiceModel authServiceModel = new AuthServiceModel();



        public UserController(IUserRep userR)
        {
            userRep = userR;



        }


        [HttpGet("IsUserNameUnique")]
        public async Task<IActionResult> IsUserNameUnique(string userName)
        {

            if (await userRep.IsUserNameUnique(userName))
            {
                return new JsonResult(true);
            }

            return new JsonResult(false);
        }


        [HttpGet("IsEmailUnique")]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var userExists = await userRep.IsEmailUnique(email);
            if (userExists)
            {
                return new JsonResult(true);
            }

            return new JsonResult(false);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            authServiceModel = await userRep.RegisterAsync(model);
            if (!authServiceModel.IsAuthenticated)
                return BadRequest(authServiceModel.Message);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { Message = "Register Successful" });


        }


        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var authService = await userRep.Login(loginModel);
            if (authService.IsAuthenticated)
            {

                return Ok(new { authService.Token, authService.id, authService.Roles });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
        }

        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById(int id)
        {
            if (userRep.GetUserInformaionById(id) == null)
                return BadRequest("User Is Not Found");

            return Ok(userRep.GetUserInformaionById(id));
        }
        [Authorize]
        [HttpPut("EditUser/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromForm] string name, [FromForm] IFormFile? image)
        {

            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdFromToken == null || int.Parse(userIdFromToken) != id)
            {
                return Unauthorized("You are not authorized to edit this user data.");
            }
            {
                if (await userRep.EditUserData(id, name, image))
                    return Ok("User data updated successfully");
                else return BadRequest("Something is wrong");
            }
        }
    }
}
