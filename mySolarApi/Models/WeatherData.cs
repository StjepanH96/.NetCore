public class WeatherData
{
    public Coord Coord { get; set; }
    public List<Weather> Weather { get; set; }
    public Main Main { get; set; }
    public Wind Wind { get; set; }
    public Clouds Clouds { get; set; }
    public int Dt { get; set; }
    public Sys Sys { get; set; }
    public string Name { get; set; }
}

public class Coord
{
    public decimal Lon { get; set; }
    public decimal Lat { get; set; }
}

public class Weather
{
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}

public class Main
{
    public decimal Temp { get; set; }
    public decimal FeelsLike { get; set; }
    public decimal TempMin { get; set; }
    public decimal TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
}

public class Wind
{
    public decimal Speed { get; set; }
    public int Deg { get; set; }
}

public class Clouds
{
    public int All { get; set; }
}

public class Sys
{
    public string Country { get; set; }
    public int Sunrise { get; set; }
    public int Sunset { get; set; }
}
