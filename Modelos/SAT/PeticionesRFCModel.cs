using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.SAT
{
    public class PeticionesRFCModel
    {
        //Auto GUID
        public Guid Id { get; set; }
        //Searcher
        [NotNull]
        public string Type { get; set; }
        [NotNull]
        public string Search { get; set; }
        //Main Request
        public string? CURP { get; set; }
        [AllowNull]
        public string? Nombres { get; set; }
        [AllowNull]
        public string? Apellidos { get; set; }
        [AllowNull]
        public string? RFC { get; set; }
        [AllowNull]
        public string? Ciudad { get; set; }
        [AllowNull]
        public string? Estado { get; set; }
        // User metadata
        [AllowNull]
        public int? UserId { get; set; }
        [AllowNull]
        public string? Comments { get; set; }
        [AllowNull]
        public string? Filename { get; set; }
        [AllowNull]
        public string? Deadline { get; set; }
        [AllowNull]
        public string? UserIp { get; set; }
        [AllowNull]
        public int? TransposeId { get; set; }
        [NotNull]
        public bool Downloaded { get; set; } = false;
        [AllowNull]
        public string? RobotTaken { get; set; }
        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public int? RegId { get; set; }
    }
}
