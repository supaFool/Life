using UnityEngine;

public abstract class MapTile
{
    //XYZ coords of tile
    private Vector3Int m_tileLocation;
    public Vector3Int TileLocation { get => m_tileLocation; set => m_tileLocation = value; }
}
