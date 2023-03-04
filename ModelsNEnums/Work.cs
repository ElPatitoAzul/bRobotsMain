using BackRobotTDM.ModelsNEnums;

namespace BackRobotTDM.Models.PeticionesActasReqs
{
    public class Work
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Search { get; set; }
        public string Data { get; set; }
        public string Estado { get; set; }
        public Actas.Preferences Preferences { get; set; }
        public int Level0 { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }

    }
}
