using BackRobotTDM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackRobotTDM.Controllers
{

    [ApiController]
    [Route("api/peticiones/actas/log")]
    public class PeticionesActasLogController : Controller
    {
        private readonly EF_DataContext DB;
        public PeticionesActasLogController(EF_DataContext _context)
        {
            this.DB = _context;
        }

        [HttpPut]
        [Route("update/deadline/{date}")]
        public IActionResult UpdateDeadline([FromRoute] string date)
        {
            var _result = DB.PeticionesActasLog.Where(d => d.Deadline == null || d.Deadline.Equals(null)).ToListAsync().Result;

            try
            {
                foreach (var p in _result)
                {
                    p.Deadline = date;
                }
                DB.SaveChanges();
                return Ok(new { Message = "OK" });
            }
            catch (Exception ex )
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}
