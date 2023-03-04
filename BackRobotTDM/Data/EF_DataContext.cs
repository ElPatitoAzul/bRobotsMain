
using Microsoft.EntityFrameworkCore;

namespace BackRobotTDM.Data
{
    public class EF_DataContext : DbContext
    {

        public EF_DataContext(DbContextOptions _option): base(_option) { }

        public DbSet<Modelos.SID.PeticionActaModel> PeticionesActas { get; set; }
        public DbSet<Modelos.Robots.RobotModel> RobotsUsage { get; set; }
        public DbSet<Modelos.Logs.PeticionesActasLogModel> PeticionesActasLog { get; set; }
        public DbSet<Modelos.SAT.PeticionesRFCModel> PeticionesRFC { get; set; }
        public DbSet<Modelos.Usuarios.UsersModel> users { get; set; }
        public DbSet<Modelos.Corte.ActasRegModel> actas_reg { get; set; }

    }
}
