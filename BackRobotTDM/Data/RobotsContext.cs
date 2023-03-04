using BackRobotTDM.Models;
using Microsoft.EntityFrameworkCore;

namespace BackRobotTDM.Data
{
    public class RobotsContext: DbContext
    {
        public RobotsContext(DbContextOptions _options) : base (_options) { }
        public DbSet<RobotsModel> RobotUsage { get; set; }
    }
}
