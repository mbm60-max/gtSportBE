using Microsoft.AspNetCore.Mvc;
using My.Models;
using PDTools.SimulatorInterfaceTestTool;
namespace My.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Access the username value from the request
            string username = request.Username;
            Program.Username = username;
            // Do something with the username, such as storing it in a database or performing other operations

            // Return a response if needed
            return Ok(new { message = "Login successful" });
        }
    }
}
