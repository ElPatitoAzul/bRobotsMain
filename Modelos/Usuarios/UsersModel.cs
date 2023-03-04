using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Usuarios
{
    public class UsersModel
    {
        [NotNull]
        public int id { get; set; }
        [AllowNull]
        public string? username { get; set; }
        [AllowNull]
        public string? password { get; set; }
        [AllowNull]
        public string? rol { get; set; }
        [AllowNull]
        public string? type { get; set; }
        [NotNull]
        public double idSuper { get; set; }
        [NotNull]
        public DateOnly? createdAt { get; set; }
        [NotNull]
        public DateTime? updatedAt { get; set;}
        [NotMapped]
        public Object? precios { get; set; }
        [NotNull]
        public Boolean status { get; set; }
        [AllowNull]
        public string? nombre { get; set; }
        [NotMapped]
        public Object? promocion { get; set; }
        [AllowNull]
        public string? servicios { get; set; }
        [AllowNull]
        public string? token { get; set; }
        [AllowNull]
        public string? latitud { get; set; }
        [AllowNull]
        public string? longitud { get; set; }
        [AllowNull]
        public string? ine_path { get; set; }
        [AllowNull]
        public string? domicilio_path { get; set; }
        [AllowNull]
        public string? foto_path { get; set; }
        [AllowNull]
        public string? number_phone { get; set; }
        [AllowNull]
        public int? actas_rest { get; set; }
        [AllowNull]
        public int? actas_limit { get; set; }
        [AllowNull]
        public int? actas_current { get; set; }
        [AllowNull]
        public int? id_update { get; set; }
    }
}
