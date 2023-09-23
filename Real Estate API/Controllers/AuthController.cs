using DTOModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Mediator.Authentication;

namespace Real_Estate_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// User login.
        /// </summary>
        /// <remarks>
        /// Allows users to authenticate in the application by providing their credentials (email and password).
        /// </remarks>
        /// <param name="credentials">User login credentials.</param>
        /// <returns>A JSON Web Token (JWT) if the credentials are valid, otherwise an unauthorized result.</returns>
        /// <response code="200">Successful login. A valid JWT token is returned.</response>
        /// <response code="401">Invalid credentials. Login is unauthorized.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredentialsDto credentials)
        {

            var token = await _authService.AuthenticateAsync(credentials);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }
    }
}
