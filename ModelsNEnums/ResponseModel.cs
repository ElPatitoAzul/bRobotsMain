namespace BackRobotTDM.Scripts.SID.Models
{
    public class ResponseModel
    {
        public Guid Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string FechaNac { get; set; }
        public string Cadena { get; set; }
        public string Tipo { get; set; }
        public string Busqueda { get; set; }
        public string CURP { get; set; }
        public string Estado { get; set; }
        public bool Found { get; set; }
        public string Comments { get; set; }

    }
}
