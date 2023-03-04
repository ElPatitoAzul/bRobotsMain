using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Logs
{
    public class PeticionesActasLogModel
    {
        [NotNull]
        public Guid Id { get; set; }

        [AllowNull]
        public string? RequestStatus { get; set; }

        [AllowNull]
        public string? RobotStatus { get; set; }

        [AllowNull]
        public string? ResultStatus { get; set; }

        [NotNull]
        public DateTime CreatedAt { get; set; }

        [AllowNull]
        public string? Deadline { get; set; }


    }
}
