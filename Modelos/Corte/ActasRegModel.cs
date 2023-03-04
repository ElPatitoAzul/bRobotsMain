
using System.Diagnostics.CodeAnalysis;

namespace Modelos.Corte
{
    public class ActasRegModel
    {
        [NotNull]
        public int id { get; set; }
        [AllowNull]
        public string? document { get; set; }
        [AllowNull]
        public string? state { get; set; }
        [AllowNull] 
        public string? dataset { get; set; }
        [AllowNull]
        public string? nameinside { get; set; }
        [AllowNull]
        public string? namefile { get; set; }
        [AllowNull]
        public int? level0 { get; set; }
        [AllowNull]
        public double? price0 { get; set; }
        [AllowNull]
        public int? level1 { get; set; }
        [AllowNull]
        public double? price1 { get; set; }
        [AllowNull]
        public int? level2 { get; set; }
        [AllowNull]
        public double? price2 { get; set; }
        [AllowNull]
        public int? level3 { get; set; }
        [AllowNull]
        public double? price3 { get; set; }
        [AllowNull]
        public int? level4 { get; set; }
        [AllowNull]
        public double? price4 { get; set; }
        [AllowNull]
        public int? level5 { get; set; }
        [AllowNull]
        public double? price5 { get; set; }
        [AllowNull]
        public DateOnly? corte { get; set; }
        [AllowNull]
        public bool? send { get; set; }
        [AllowNull]
        public int? idcreated { get; set; }
        [AllowNull]
        public bool? hidden { get; set; }
        [AllowNull]
        public int? idhidden { get; set; }
        [AllowNull]
        public TimeSpan? createdtime { get; set; }
        [AllowNull]
        public int? idtranspose { get; set; }
        [AllowNull]
        public DateTime? createdAt { get; set; }
        [AllowNull]
        public DateTime? updatedAt { get; set; }
        [AllowNull]
        public int? id2 { get; set; }
    }
}