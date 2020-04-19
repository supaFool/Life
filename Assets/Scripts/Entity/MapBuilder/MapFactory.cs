using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapFactory : MonoBehaviour
{

    private MapSettings settings;
    private MapBuilder builder;

    public MapFactory()
    {
        settings = new MapSettings();
    }
}