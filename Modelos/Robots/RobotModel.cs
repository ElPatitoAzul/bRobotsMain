using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Robots
{
    public class RobotModel
    {
        [NotNull]
        public Guid Id { get; set; }
        [NotNull]
        public string Name { get; set; }
        [AllowNull]
        public string? AccessToken { get; set; }
        [AllowNull]
        public string? Status { get; set; }
        [AllowNull]
        public string? Source { get; set; }
        [AllowNull]
        public string? System { get; set; }
        [AllowNull]
        public string? For { get; set; }
        [AllowNull]
        public int? Limit { get; set; }
        [AllowNull]
        public int? Current { get; set; }
        [AllowNull]
        public string? UserId { get; set; }
        [AllowNull]
        public string? Username { get; set; }
        [AllowNull]
        public string? Version { get; set; }
    }
}
