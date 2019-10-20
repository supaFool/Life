using System.Collections.Generic;

public class Zone : Region
{
    //Testing
    private string m_zoneName;
    private int m_zoneID;
    public int[,] LandTiles;
    public int[,] WaterTiles;

    public Zone(string zoneName)
    {
        ZoneName = zoneName;
        LandTiles = new int[MapSettings.MapWidth, MapSettings.MapHeight];
        WaterTiles = new int[MapSettings.MapWidth, MapSettings.MapHeight];
    }

    public string ZoneName { get => m_zoneName; set => m_zoneName = value; }
}
