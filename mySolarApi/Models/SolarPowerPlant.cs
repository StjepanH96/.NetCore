namespace SolarApp.Models
{
    public class SolarPowerPlant
    {
        public int Id { get; set; }

        public required string Name { get; set; } = string.Empty;

        public decimal InstalledPower { get; set; }
        public DateTime DateOfInstallation { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        
        public int UserId { get; set; }


    }
}