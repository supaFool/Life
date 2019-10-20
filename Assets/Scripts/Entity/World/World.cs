using System.Collections.Generic;
using UnityEngine;

public abstract class World : MonoBehaviour
{
    private string m_worldName;
    private List<Region> m_regions;

    public string WorldName { get => m_worldName; set => m_worldName = value; }

    public List<Region> Regions { get => m_regions; }

    public void AddRegion(Region region)
    {
        Regions.Add(region);
    }
    public int AmountOfRegions()
    {
        return m_regions.Count;
    }
}
