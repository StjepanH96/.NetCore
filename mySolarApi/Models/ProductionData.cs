namespace SolarApp.Models
{
public class ProductionData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal Production { get; set; }
    public string Unit { get; set; } = "kW";  
    public string TimeseriesType { get; set; } = string.Empty;

    public int SolarPowerPlantId { get; set; }
    public string? SolarPowerPlantName { get; set; }  


}}