using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository , ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            // validating  the incoming request using fluent validations 
            // check if user is authentictaed 
          var user =  await  userRepository.AuthenticateAsync
                (loginRequest.Username, loginRequest.Password);
            //
            //if authenticated , generate a token 
            if (user != null)
            {
                // generate a JWT token & send back 
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }
            return BadRequest("Username & Password are incorrect");
        }
    }
}
