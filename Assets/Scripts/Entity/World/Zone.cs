public class Zone : Region
{
    //Testing
    private string m_zoneName;

    private int m_zoneID;
    public int[,] LandTiles;
    public int[,] WaterTiles;

    public Zone(string zoneName, MapSettings settings)
    {
        ZoneName = zoneName;
        LandTiles = new int[settings.Width, settings.Height];
        WaterTiles = new int[settings.Width, settings.Height];
    }

    public string ZoneName { get => m_zoneName; set => m_zoneName = value; }
}