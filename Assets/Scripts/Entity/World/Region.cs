using System.Collections.Generic;

public abstract class Region : World
{
    private string m_regionName;
    private int m_regionID;

    //The list of Zones this Region contains
    private List<Zone> m_zones;

    public List<Zone> Zones { get => m_zones; }

    public string RegionName { get => m_regionName; set => m_regionName = value; }

    public void AddZone(Zone zone)
    {
        Zones.Add(zone);
    }

    public int AmountOfZones()
    {
        return m_zones.Count;
    }
}
