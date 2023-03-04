using BackRobotTDM.Models;
using Microsoft.EntityFrameworkCore;

namespace BackRobotTDM.Data
{
    public class PeticionesActasContext: DbContext
    {
        public PeticionesActasContext(DbContextOptions _options): base(_options) { }
        public DbSet<PeticionesActasModel> PeticionesActas { get; set; }
    }
}
