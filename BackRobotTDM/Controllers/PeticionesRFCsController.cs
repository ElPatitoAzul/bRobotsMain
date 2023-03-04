using BackRobotTDM.Data;
using BackRobotTDM.Enviroments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Corte;
using Modelos.Peticiones;
using Modelos.SAT;
using Newtonsoft.Json;
using System;

namespace BackRobotTDM.Controllers
{
    [ApiController]
    [Route("api/peticiones/rfcs")]
    public class PeticionesRFCsController : Controller
    {

        private readonly EF_DataContext DB;
        private readonly SAT.Main _sat = new SAT.Main();

        public PeticionesRFCsController(EF_DataContext _context)
        {
            this.DB = _context;
        }

        [HttpGet]
        [Route("user/{UserId}")]
        public IActionResult GetMyReqs([FromHeader] int UserId)
        {
            var _result = DB.PeticionesRFC.Where(d => d.UserId == UserId).ToListAsync().Result;
            if (_result != null)
            {
                return Ok(_result);
            }
            else return NotFound();
        }

        [HttpPost]
        [Route("/curp/new")]
        public async Task<IActionResult> ByCurp([FromBody] string CURP)
        {
            var _robot = await DB.RobotsUsage.Where(d => d.Source == "rfcs" && d.System == "sat" && d.AccessToken != null && d.Status == "Ok" ).FirstOrDefaultAsync();
            if (_robot != null)
            {
                var _access = JsonConvert.DeserializeObject<Modelos.SAT.Access>(_robot.AccessToken!);

                var _result = await _sat.SearchByCurp(_access?.Cookie!, _access?.ViewState!, CURP);

                return Ok(_result);
            }
            else return BadRequest();

        }


        [HttpPost]
        [Route("fisica/new")]
        public async Task<IActionResult> FisicPerson([FromBody] Requests DATA)
        {
            var _robot = await DB.RobotsUsage.Where(d => d.Source == "rfcs" && d.System == "sat" && d.AccessToken != null && d.Status == "Ok").FirstOrDefaultAsync();

            if (_robot != null)
            {
                lock (_robot)
                {
                    var _peticion = new Modelos.SAT.PeticionesRFCModel();
                    var _req = new Modelos.SAT.Work();
                    var _access = JsonConvert.DeserializeObject<Modelos.SAT.Access>(_robot.AccessToken!);

                    _req.COOKIES = _access?.Cookie!;
                    _req.VIEW_STATE = _access?.ViewState!;
                    _req.CURP = DATA.CURP;
                    _req.RFC = DATA.RFC;
                    _req.UserId = DATA.UserId;
                    _req.Id = Guid.NewGuid();
                    var seed = Environment.TickCount;
                    var random = new Random(seed);
                    int n = random.Next(1, 4);
                    Thread.Sleep(n * 1000);
                    var _result = _sat.byDataFisica(_req);

                    _peticion.Search = DATA.RFC != "" || DATA.RFC != null ? "RFC" : "CURP";
                    _peticion.CURP = DATA.CURP != "" || DATA.CURP != null ? DATA.CURP: _result.CURP;
                    _peticion.RFC = DATA.RFC != "" || DATA.RFC != null ? DATA.RFC : _result.RFC;
                    _peticion.CreatedAt = DateTime.UtcNow;
                    _peticion.Id = _req.Id;
                    _peticion.UserId = _req.UserId;
                    _peticion.UserIp = DATA.UserIp;
                    _peticion.Type = "FISICA";
                    _peticion.RobotTaken = _robot.Name;

                    if (!_result.ValidToken)
                    {
                        _robot.AccessToken = null;
                        _robot.Status = "Off";
                        DB.SaveChanges();
                        return BadRequest("Token caducado");
                    }
                    else if (!_result.Found)
                    {
                        _robot.AccessToken = JsonConvert.SerializeObject(_result.NewToken);
                        DB.SaveChanges();

                        _peticion.Comments = "No se han encontrado resultados de búsqueda.";

                        DB.PeticionesRFC.Add(_peticion);
                        return NotFound(new
                        {
                            message = "No se han encontrado resultados de búsqueda."
                        });
                    }

                    else {
                        _robot.AccessToken = JsonConvert.SerializeObject(_result.NewToken);
                        DB.SaveChanges();

                        _peticion.RegId = _result.CorteId;
                        _peticion.Nombres = _result.Names;
                        _peticion.Apellidos = _result.Apellidos;
                        _peticion.Comments = "Descargado";
                        _peticion.Filename = $"{_req.Id}-{_result.RFC}.pdf";
                        _peticion.Ciudad = _result.CIUDAD;
                        _peticion.Estado = _result.Estado;
                        _peticion.CURP = _result.CURP;
                        _peticion.RFC = _result.RFC;
                        DB.PeticionesRFC.Add(_peticion);
                        DB.SaveChanges();
                        return Ok(new
                        {
                            _peticion.Id,
                            _result.CURP,
                            Nombres = _result.Names,
                            _result.Apellidos,
                            _result.RFC,
                            _result.CIUDAD,
                            _result.Estado
                        });
                    } 
                }

            }
            else return NotFound(new {
                message = "Sin sistema"
                        });
        }

        [HttpPost]
        [Route("moral/new")]
        public IActionResult MoralPerson(MoralRequest DATA)
        {
            return Ok("Moral");
        }

        [HttpGet]
        [Route("download/{id}")]
        public IActionResult Download([FromRoute] Guid id)
        {
            var _peticion = DB.PeticionesRFC.Where(d => d.Id == id).FirstOrDefaultAsync().Result;
            if (_peticion != null)
            {
                _peticion.Downloaded = true;
                DB.SaveChangesAsync().Wait();

                var _path = @$"cache/rfcs/{_peticion.Filename}";
                if (System.IO.File.Exists(_path))
                {
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(_path);
                    var _b64 = Convert.ToBase64String(pdfBytes);
                    return Ok(new { b64 = _b64 });
                }
                else return NotFound();
            }
            else return NotFound("NULL");
        }

        [HttpPut]
        [Route("update/deadline/{date}")]
        public IActionResult UpdateDeadline([FromRoute] string date)
        {
            var _result = DB.PeticionesRFC.Where(d => d.Deadline == null || d.Deadline.Equals(null)).ToListAsync().Result;
            try
            {
                foreach (var p in _result)
                {
                    p.Deadline = date;
                }
                DB.SaveChanges();
                return Ok("OK");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("regId/{guId}/")]
        public IActionResult GetRegId([FromRoute] Guid guId, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var request = DB.PeticionesRFC.FirstOrDefault(d => d.Id == guId);

            if (request != null)
            {
                return Ok(request.RegId);
            }
            else return NotFound("Not Found");
        }  

        [HttpPut]
        [Route("transpose/rfc/{id}/{userId}")]
        public IActionResult TransposeRFC([FromRoute] Guid id, [FromRoute] int userId, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var _result = DB.PeticionesRFC.Where(d => d.Id == id).FirstOrDefaultAsync().Result;

            if (_result != null)
            {
                _result.TransposeId = userId;
                DB.SaveChanges();
                return Ok("OK");
            }
            else return NotFound("Not Found");
        }

        [HttpGet]
        [Route("myDates/{UserId}")]
        public IActionResult GetMyDates([FromRoute] int UserId)
        {
            var dates = DB.PeticionesRFC.
                Where(d => d.UserId == UserId).
                GroupBy(g => g.Deadline).
                Select(p => new DeadlineModel { Deadline = p.Key! }).
                OrderByDescending(x => x.Deadline)
                .ToListAsync().Result;

            if (dates == null)
            {
                return NotFound();
            }
            return Ok(dates);
        }

        [HttpGet]
        [Route("myReqs/{UserId}/{date}")]
        public IActionResult GetMyCorte([FromRoute] int UserId, [FromRoute] string date, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var myReqs = DB.PeticionesRFC.
                Where(d => d.UserId == UserId && d.Deadline == (date == "null" ? null : date)).
                Select(p => new MyReqsModel
                {
                    Id = p.Id!,
                    Type = p.Type!,
                    Search = p.Search!,
                    CURP = p.CURP!,
                    Nombres = p.Nombres!,
                    Apellidos = p.Apellidos!,
                    RFC = p.RFC!,
                    Ciudad = p.Ciudad!,
                    Estado = p.Estado!,
                    UserId = p.UserId!,
                    Comments = p.Comments!,
                    TransposeId = p.TransposeId!,
                    Downloaded = p.Downloaded!,
                    CreatedAt = p.CreatedAt!
                }).
                OrderByDescending(x => x.CreatedAt).
                ToListAsync().Result;
            if (myReqs.Count != 0)
            {
                return Ok(myReqs);
            }
            else return NotFound("Not Found");

        }

        //[HttpGet]
        //[Route("/init")]
        //public async Task<IActionResult> Init()
        //{
        //    var _robotRfc = await DB.RobotsUsage.Where(d => d.Source == "rfcs" && d.System == "sat" && d.AccessToken != null).FirstOrDefaultAsync();
        //    if (_robotRfc != null)
        //    {
        //        var _s = _uint.Init(_robotRfc?.AccessToken!);
        //        Console.WriteLine(_s);
        //        return Ok(_s);
        //    }
        //    else return BadRequest();


        //}
    }
}
