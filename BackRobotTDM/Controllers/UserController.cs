using BackRobotTDM.Data;
using BackRobotTDM.Enviroments;
using BackRobotTDM.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BackRobotTDM.Controllers
{
    [ApiController]
    [Route("api/users/")]
    public class UserController : Controller
    {
        private readonly NodeAPIs Api = new NodeAPIs();
        public IConfiguration _config;
        public UserController(IConfiguration CONFIGURATION)
        {
            _config = CONFIGURATION;
        }
        //LogIn
        [HttpGet]
        [Route("loadServices")]
        public async Task<IActionResult> LoadMyService([FromHeader] string token)
        {
            var _NodeJWT = Request.Headers.Where(d => d.Key == "token").FirstOrDefault();
            if (_NodeJWT.Key != null)
            {
                var _user = Api.LoadMyServices(_NodeJWT.Value).Result;
                try
                {
                    var _myService = JsonConvert.DeserializeObject<UserModel>(_user);
                    return Ok(_user);
                }
                catch
                {
                    return BadRequest();
                }
            }
            return Conflict();
        }

    }
}
