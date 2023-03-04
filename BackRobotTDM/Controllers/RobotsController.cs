using BackRobotTDM.Data;
using BackRobotTDM.Enviroments;
using BackRobotTDM.Models;
using BackRobotTDM.Models.RobotReqs;
using BackRobotTDM.Scripts.SID.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Robots;
using Newtonsoft.Json;

namespace BackRobotTDM.Controllers
{
    [ApiController]
    [Route("api/robots/")]
    public class RobotsController : Controller
    {

        private readonly EF_DataContext DB;
        private static GeneralTools.Security _scty = new GeneralTools.Security();
        public RobotsController(EF_DataContext _context)
        {
            this.DB = _context;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllAsync([FromHeader] string PrivateKey)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }
                var _robots = await DB.RobotsUsage.
                    Select(x => new GetAllRobotsModel
                    {
                        Id = x.Id!,
                        Name = x.Name!,
                        AccessToken = x.AccessToken!,
                        Status = x.Status!,
                        Source = x.Source!,
                        System = x.System!,
                        Limit = x.Limit!,
                        Current = x.Current!
                    })
                    .ToListAsync();
                return Ok(_robots);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
            

        }

        [HttpPut]
        [Route("{name}/updateToken/{token}")]
        public IActionResult UpdateToken([FromHeader] string PrivateKey, [FromRoute] string name, string token)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _robot = DB.RobotsUsage.FirstOrDefault(x => x.Name == name);

                if (_robot != null)
                {
                    _robot.AccessToken = _scty.Encrypt(token);
                    _robot.Status = "Ok";
                    DB.SaveChanges();

                    return Ok("AccessToken Updated");
                }
                else return NotFound("Robot Name Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }            
        }

        [HttpPut]
        [Route("remove/{name}/token")]
        public IActionResult RemoveToken([FromHeader] string PrivateKey, [FromRoute] string name)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _robot = DB.RobotsUsage.FirstOrDefault(x => x.Name == name);

                if (_robot != null)
                {
                    _robot.Status = "Off";
                    DB.SaveChanges();

                    return Ok("Status Updated");
                }
                else return NotFound("Name not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("status/{name}/on")]
        public IActionResult ChangeStatusOk([FromHeader] string PrivateKey, [FromRoute] string name)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _robot = DB.RobotsUsage.FirstOrDefault(x => x.Name == name);

                if (_robot != null)
                {
                    _robot.Status = "Ok";
                    DB.SaveChanges();

                    return Ok("Status Updated");
                }
                else return NotFound("Name not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("{name}/changeLimit")]
        public async Task<IActionResult> ChangeLimit([FromHeader] string PrivateKey, [FromRoute] string name, [FromBody] RobotLimitModel _req)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _robot = await DB.RobotsUsage.Where(d => d.Name == name).FirstOrDefaultAsync();
                if (_robot != null)
                {
                    _robot.Limit = _req.Limit;
                    _robot.Current = _req.Current;
                    await DB.SaveChangesAsync();
                    return Ok("Limite Cambiado");
                }
                return NotFound("Name Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("change/current")]
        public async Task<IActionResult> ChangeCurrent([FromHeader] string PrivateKey)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _robots = await DB.RobotsUsage.Where(d => d.Current != 0).ToListAsync();
                if (_robots != null)
                {
                    foreach (var _robot in _robots)
                    {
                        _robot.Current = 0;
                    }
                    DB.SaveChanges();
                    return Ok("OK");
                }
                return NotFound("Contadores en cero");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddRobot([FromHeader] string PrivateKey, [FromBody] AddRobot _req)
        {
            try
            {
                if (PrivateKey != Variables.PrivateKey)
                {
                    return Unauthorized();
                }

                var _db = new Modelos.Robots.RobotModel()
                {
                    Id = Guid.NewGuid(),
                    Name = _req.Name,
                    Source = _req.Source,
                    System = _req.System,
                    UserId = _req.UserId,
                    Username = _req.Username
                };
                await DB.RobotsUsage.AddAsync(_db);
                await DB.SaveChangesAsync();

                return Ok("Robot Agregado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("{name}/SetCookies")]
        public async Task<IActionResult> SetCookies([FromRoute] string name, [FromBody] Modelos.SAT.Access Accessos)
        {


            var _robot = await DB.RobotsUsage.Where(d => d.Name == name).FirstOrDefaultAsync();
            if (_robot != null)
            {
                var _plain = JsonConvert.SerializeObject(Accessos);
                _robot.AccessToken = _plain;
                _robot.Status = "Ok";
                DB.SaveChanges();
                return Ok();
            }
            else return BadRequest();
        }

    }
}
