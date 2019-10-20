public abstract class Zone : Region
{
    //Testing
    private string m_zoneName;
    private int m_zoneID;
   

    public Zone(string zoneName)
    {
        ZoneName = zoneName;
    }

    public string ZoneName { get => m_zoneName; set => m_zoneName = value; }
}
