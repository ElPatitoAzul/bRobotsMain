using BackRobotTDM.ModelsNEnums;

namespace BackRobotTDM.Models.PeticionesActasReqs
{
    public class AddPeticionActa
    {
        public string Type { get; set; }
        public string Search { get; set; }
        public string Data { get; set; }
        public string Estado { get; set; }
        public Actas.Preferences Preferences { get; set; }
        public int UserId { get; set; }
        public string UserIp { get; set; }
    }
}
