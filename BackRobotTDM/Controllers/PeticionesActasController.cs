using BackRobotTDM.Data;
using BackRobotTDM.Enviroments;
using BackRobotTDM.Models;
using BackRobotTDM.Models.PeticionesActasReqs;
using BackRobotTDM.ModelsNEnums;
using BackRobotTDM.mSID;
using BackRobotTDM.Scripts.SID;
using BackRobotTDM.Scripts.SID.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Modelos.Corte;
using Modelos.Logs;
using Modelos.Peticiones;
using Modelos.SID;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using System.Linq;
using System.Xml.Linq;
using Tools = BackRobotTDM.mSID.Tools;

namespace BackRobotTDM.Controllers
{
    [ApiController]
    [Route("api/peticiones/actas")]
    public class PeticionesActasController : Controller
    {
        private readonly EF_DataContext DB;
        private readonly mSID.Main _SID = new mSID.Main();
        private readonly mxSID.Main _mxSID = new mxSID.Main();
        private static GeneralTools.Security _scty = new GeneralTools.Security();
        public PeticionesActasController(EF_DataContext _context) 
        {
            this.DB = _context;
        }

        [Route("user/{UserId}")]
        [HttpGet]
        public async Task<IActionResult> GetPeticiones([FromHeader] string PrivateKey, [FromRoute] int UserId)
        {


            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }
            var sqlCommand = "SELECT * FROM \"PeticionesActas\" WHERE \"UserId\" = {0}  ORDER BY \"CreatedAt\" DESC";
            var _result = await DB.PeticionesActas.FromSqlRaw(sqlCommand, UserId).ToListAsync();
            if (_result == null)
            {
                return NotFound();
            }
            return Ok(_result);
        }


        [HttpPost]
        [Route("new")]
        public IActionResult New([FromBody] New _req)
        {
            var _robot = DB.RobotsUsage.Where(d => d.Status == "Ok" && d.AccessToken != null && d.Source == "actas" && d.Current < d.Limit).ToListAsync().Result;
            if (_robot.Count != 0)
            {
                var seed = Environment.TickCount;
                var random = new Random(seed);
                var value = random.Next(0, _robot.Count);
                var _rUse = _robot[value];
                lock (_rUse)
                {
                    if (_rUse.System == "sid")
                    {
                        var _result = new Modelos.SID.ResponseModel();
                        
                        var _dRobot = new Modelos.Robots.RobotModel()
                        {
                            Current = _rUse.Current,
                            For = null,
                            Id = _rUse.Id,
                            Limit = _rUse.Limit,
                            Name = _rUse.Name,
                            Source = _rUse.Source,
                            Status = _rUse.Status,
                            System = _rUse.System,
                            UserId = _rUse.UserId,
                            Username = _rUse.Username,
                            Version = _rUse.Version,
                            AccessToken = _scty.Decrypt(_rUse.AccessToken!)
                        };

                        if (_dRobot.Version == "2") _result = _mxSID.MAIN(_req, _dRobot).Result;
                        else  _result = _SID.MAIN(_req, _dRobot).Result;

                        var _log = new PeticionesActasLogModel()
                        {
                            Id = _result?.Id != null ? _result.Id : Guid.NewGuid(),
                            RobotStatus = JsonConvert.SerializeObject(_rUse),
                            ResultStatus = JsonConvert.SerializeObject(_result),
                            RequestStatus = JsonConvert.SerializeObject(_req),
                            CreatedAt = DateTime.UtcNow
                        };
                        DB.PeticionesActasLog.Add(_log);
                        if (_result!.Found)
                        {
                            var _solicitud = new PeticionActaModel()
                            {
                                Id = _result.Id,
                                Type = _req.Type,
                                Search = _req.Search,
                                Estado = _result.Estado,
                                CURP = _result.CURP,
                                Cadena = _result.Cadena,
                                UserId = _req.UserId,
                                UserIp = _req.UserIp,
                                Preferences = _req.Preferences,
                                Comments = "Descargado",
                                FechaNac = _result.FechaNac,
                                Nombres = _result.Nombres,
                                Apellidos = _result.Apellidos,
                                CreatedAt = DateTime.UtcNow,
                                RobotTaken = _rUse!.Name,
                                RegId = _result.CorteId
                            };
                            _rUse.Current = (Convert.ToInt32(_rUse.Current) + 1);
                            DB.PeticionesActas.Add(_solicitud);
                            DB.SaveChangesAsync().Wait();
                            return Ok(new
                            {
                                id = _result.Id,
                                nombres = _result.Nombres,
                                apellidos = _result.Apellidos,
                                fechaNac = _result.FechaNac,
                                cadena = _result.Cadena,
                                tipo = _result.Tipo,
                                busqueda = _result.Busqueda,
                                curp = _result.CURP,
                                estado = _result.Estado
                            });
                        }
                        else
                        {
                            if (_result.Comments == "No found")
                            {
                                var _solicitud = new PeticionActaModel()
                                {
                                    Id = _result.Id,
                                    Type = _req.Type,
                                    Search = _req.Search,
                                    Estado = _req.Estado,
                                    CURP = _req.Data!,
                                    Cadena = null,
                                    UserId = _req.UserId,
                                    UserIp = _req.UserIp,
                                    Preferences = _req.Preferences,
                                    CreatedAt = DateTime.UtcNow,
                                    RobotTaken = _rUse.Name,
                                    Comments = $"{_req.Type.ToUpper()} no localizado en la Base de Datos Nacional de Registro Civil Búsqueda por {_req.Search} [ {_req.Data.ToUpper()} ]"
                                };
                                DB.PeticionesActas.Add(_solicitud);
                                DB.SaveChangesAsync().Wait();
                                return Json(new { error = $"{_req.Type.ToUpper()} no localizado en la Base de Datos Nacional de Registro Civil Búsqueda por {_req.Search} [ {_req.Data.ToUpper()} ]" });
                            }
                            else if (_result.Comments == "Token")
                            {
                                _rUse!.AccessToken = null;
                                _rUse!.Status = "Off";
                                DB.SaveChangesAsync().Wait();
                                return Json(new { Error = "Sistema no encontrado" });
                            }
                            else return Json(new { Error = "Acta no encontrada y no descargada" });
                        }
                    }
                    else
                    {
                        //SCRIPT OTROS SISTEMAS
                        return Json(new { Error = "Sistema no encontrado" });
                    }
                }
            }
            else return Json(new { Error = "Sin sistema" });
        }


        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddPeticion([FromHeader] string PrivateKey, New _req)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }
            var paramSQL = "SELECT * FROM \"RobotsUsage\" WHERE \"Status\" = 'Ok' AND \"AccessToken\" IS NOT NULL AND \"Source\" = 'actas' AND \"Current\" < \"Limit\" ";
            var _resultS = await DB.RobotsUsage.FromSqlRaw(paramSQL, "").ToListAsync();

            if (_resultS.Count != 0)
            {

                var seed = Environment.TickCount;
                var random = new Random(seed);
                var value = random.Next(0, _resultS.Count);
                var _robotUsage = _resultS[value];

                if (_robotUsage == null)
                {
                    return BadRequest("SIN ROBOTS DISPONIBLES");
                }

                var _result = new Modelos.SID.ResponseModel();
                var _dRobot = new Modelos.Robots.RobotModel()
                {
                    Current = _robotUsage.Current,
                    For = null,
                    Id = _robotUsage.Id,
                    Limit = _robotUsage.Limit,
                    Name = _robotUsage.Name,
                    Source = _robotUsage.Source,
                    Status = _robotUsage.Status,
                    System = _robotUsage.System,
                    UserId = _robotUsage.UserId,
                    Username = _robotUsage.Username,
                    Version = _robotUsage.Version,
                    AccessToken = _scty.Decrypt(_robotUsage.AccessToken!)
                };

                if (_dRobot.Version == "2")
                {
                    _result = await _mxSID.MAIN(_req, _dRobot);
                }
                else {
                    _result = await _SID.MAIN(_req, _dRobot);
                }

                var _log = new PeticionesActasLogModel()
                {
                    Id = _result?.Id != null ? _result.Id : Guid.NewGuid(),
                    RobotStatus = JsonConvert.SerializeObject(_robotUsage),
                    ResultStatus = JsonConvert.SerializeObject(_result),
                    RequestStatus = JsonConvert.SerializeObject(_req),
                    CreatedAt = DateTime.UtcNow
                };

                await DB.PeticionesActasLog.AddAsync(_log);

                if (_result!.Found)
                {
                    var _solicitud = new PeticionActaModel()
                    {
                        Id = _result.Id,
                        Type = _req.Type,
                        Search = _req.Search,
                        Estado = _result.Estado,
                        CURP = _result.CURP,
                        Cadena = _result.Cadena,
                        UserId = _req.UserId,
                        UserIp = _req.UserIp,
                        Preferences = _req.Preferences,
                        Comments = "Descargado",
                        FechaNac = _result.FechaNac,
                        Nombres = _result.Nombres,
                        Apellidos = _result.Apellidos,
                        CreatedAt = DateTime.UtcNow,
                        RobotTaken = _robotUsage!.Name,
                        RegId = _result.CorteId
                    };

                    _robotUsage.Current = (Convert.ToInt32(_robotUsage.Current) + 1);
                    await DB.PeticionesActas.AddAsync(_solicitud);
                    await DB.SaveChangesAsync();

                    return Ok(new
                    {
                        id = _result.Id,
                        nombres = _result.Nombres ,
                        apellidos = _result.Apellidos ,
                        fechaNac = _result.FechaNac,
                        cadena = _result.Cadena,
                        tipo = _result.Tipo,
                        busqueda = _result.Busqueda,
                        curp = _result.CURP,
                        estado = _result.Estado
                    });
                }
                else
                {
                    if (_result.Comments == "No found")
                    {
                        var _solicitud = new PeticionActaModel()
                        {
                            Id = _result.Id,
                            Type = _req.Type,
                            Search = _req.Search,
                            Estado = _req.Estado,
                            CURP = _req.Data!,
                            Cadena = null,
                            UserId = _req.UserId,
                            UserIp = _req.UserIp,
                            Preferences = _req.Preferences,
                            CreatedAt = DateTime.UtcNow,
                            Comments = $"{_req.Type.ToUpper()} no localizado en la Base de Datos Nacional de Registro Civil Búsqueda por {_req.Search} [ {_req.Data.ToUpper()} ]"
                        };

                        await DB.PeticionesActas.AddAsync(_solicitud);
                        await DB.SaveChangesAsync();
                        return Ok($"{_req.Type.ToUpper()} no localizado en la Base de Datos Nacional de Registro Civil Búsqueda por {_req.Search} [ {_req.Data.ToUpper()} ]");
                    }
                    else if (_result.Comments == "Token")
                    {
                        _robotUsage!.AccessToken = null;
                        _robotUsage!.Status = "Off";
                        await DB.SaveChangesAsync();
                        return BadRequest("TOKEN CADUCADO.");
                    }
                    else return BadRequest();
                }
            }
            else return BadRequest();
        }

        [HttpGet]
        [Route("download/{id}")]
        public async Task<IActionResult> DownloadPeticion([FromHeader] string PrivateKey, [FromRoute] Guid id)
        {
            if (PrivateKey != Variables.PrivateKey)
            {
                return Unauthorized();
            }
            var _result = await DB.PeticionesActas.Where(d => d.Id == id).FirstOrDefaultAsync();
            if (_result != null)
            {
                var _preferences = _result.Preferences;
                _result.Downloaded = true;
                await DB.SaveChangesAsync();


                    var tools = new Tools();

                    var _path = @$"cache/{_result.Id}-{_result.Cadena}.pdf";
                    if (System.IO.File.Exists(_path))
                    {

                        var _tmp = @$"tmp/{_result.Id}/{_result.Id}-{_result.Cadena}.pdf";
                        var _pdf = new mSID.PDFManipulate();
                        _pdf.Enmarcar(_path, id, _tmp);
                        tools._PATHER_(@$"tmp/{_result.Id}/");

                        var _sourceTmp = "";
                        if (_preferences == Modelos.Preferences.Reversado || _preferences == Modelos.Preferences.Foliado)
                        {
                            _sourceTmp = _pdf.PrefsManagement(_tmp, _preferences, _result?.Estado!, _result!.Id, _result?.Cadena!);
                        }
                        else _sourceTmp = _tmp;
                        byte[] pdfBytes = System.IO.File.ReadAllBytes(_sourceTmp);
                        var _b64 = Convert.ToBase64String(pdfBytes);

                        tools._RM_($@"tmp/{id}");
                        return Ok(new { b64 = _b64 });

                    }
                    else return NotFound();
                
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("get/regId/{guId}/")]
        public IActionResult GetRegId([FromRoute] Guid guId, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var request = DB.PeticionesActas.FirstOrDefault(d => d.Id == guId);

            if (request != null)
            {
                return Ok(request.RegId);
            }
            else return NotFound();
        }

        [HttpPut]
        [Route("put/transposeId/{guId}/{userId}")]
        public IActionResult GetRegId([FromRoute] Guid guId, [FromRoute] int userId, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var request =  DB.PeticionesActas.FirstOrDefault(d => d.Id == guId);
            
            if (request != null)
            {
                request.TransposeId = userId;
                DB.SaveChanges();

                return Ok();
            }
            else return NotFound();    
        }

        [HttpPut]
        [Route("update/deadline/{deadline}")]
        public IActionResult ChangeDeadline([FromRoute] string deadline)
        {
            var _result = DB.PeticionesActas.Where(d => d.Deadline == null || d.Deadline.Equals(null)).ToListAsync().Result;
            try
            {
                foreach (var p in _result)
                {
                    p.Deadline = deadline;
                }
                DB.SaveChanges();
                return Ok(new { Message = "OK"});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/get/corte/myDates/{UserId}")]
        public IActionResult GetMyDates([FromRoute] int UserId)
        {
            var dates = DB.PeticionesActas.
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
        [Route("/get/corte/myCorte/{UserId}/{date}")]
        public IActionResult GetMyCorte([FromRoute] int UserId, [FromRoute] string date, [FromHeader] string PrivateKey)
        {
            if (PrivateKey != Variables.PrivateKey || PrivateKey == null)
            {
                return Unauthorized();
            }

            var myCorte = DB.PeticionesActas.
                Where(d => d.UserId == UserId && d.Deadline == (date == "null" ? null : date) ).
                Select(p => new MyCorteModel {
                    Id = p.Id!,
                    Type = p.Type!,
                    Search = p.Search!,
                    CURP = p.CURP!,
                    Nombres = p.Nombres!,
                    Apellidos = p.Apellidos!,
                    FechaNac = p.FechaNac!,
                    Cadena = p.Cadena!,
                    Estado = p.Estado!,
                    Preferences = p.Preferences!,
                    UserId = p.UserId!,
                    Comments = p.Comments!,
                    TransposeId = p.TransposeId!,
                    Downloaded = p.Downloaded!,
                    CreatedAt = p.CreatedAt!
                } ).
                OrderByDescending(x => x.CreatedAt).
                ToListAsync().Result;
            if(myCorte == null)
            {
                return NotFound();
            }
            else return Ok(myCorte);

        }

   
    }
}
