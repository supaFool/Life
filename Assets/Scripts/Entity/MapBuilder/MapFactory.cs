using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : IMap
{
    private MapSettings settings;

    public MapFactory()
    {
        settings = new MapSettings();
    }

    public void Simulate(int[] samples)
    {
        throw new System.NotImplementedException();
    }
}