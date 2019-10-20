using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    #region Vars
    private MapSettings m_mapSettings;
    private MapTile m_mapTile;
    //private Map m_map;
    private Zone m_currentZone; 

    [SerializeField]
    private Tilemap TopMap;
    [SerializeField]
    private Tilemap BottomMap;
    [SerializeField]
    private Tile TopTile;
    [SerializeField]
    private Tile BottomTile;

    //Spawn Settings
    [Range(1, 100)]
    public int m_activeChance;

    [Range(1, 8)]
    public int BirthLimit;

    [Range(1, 8)]
    public int DeathLimit;

    [Range(1, 10)]
    public int Samples;
    private int counter = 0;

    private int[,] m_finalMap;
    public Vector3Int MapSize;

    public int GridSize { get => m_finalMap.Length; }

    #endregion

    #region UI Vars
    public Text SamplesText;
    #endregion

    #region Overrides
    private void Awake()
    {
        m_mapSettings = new MapSettings();
        m_currentZone = new Zone("TestZone");
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            Simulate(Samples);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearMap(true);
        }
    }
    #endregion

    public void Simulate(int samples)
    {
        int c = 0;
        Debug.Log("Building");
        ClearMap(false);



        if (m_finalMap == null)
        {
            Debug.Log("Creating Grid");
            m_finalMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
            InitPos();
        }

        for (int i = 0; i < Samples; i++)
        {
            m_finalMap = GetTilePos(m_finalMap);
        }

        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                if (m_finalMap[x, y] == 1)
                {
                    c++;
                    TopMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), TopTile)
                }

                if (m_finalMap[x, y] == 0)
                {
                    BottomMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), BottomTile);
                }
            }
        }
    }

    private int[,] GetTilePos(int[,] tempMap)
    {
        int[,] newMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
        int neighbor;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                neighbor = 0;
                foreach (var b in bounds.allPositionsWithin)
                {
                    if (b.x == 0 && b.y == 0) continue;
                    if (x + b.x >= 0 && x + b.x < MapSettings.MapWidth && y + b.y >= 0 && y + b.y < MapSettings.MapHeight)
                    {
                        neighbor += tempMap[x + b.x, y + b.y];
                    }
                    else
                    {
                        //Draw Border
                        neighbor++;
                    }
                }
                if (tempMap[x, y] == 1)
                {
                    if (neighbor < DeathLimit) newMap[x, y] = 0;
                    else
                    {
                        newMap[x, y] = 1;
                    }
                }
                if (tempMap[x, y] == 0)
                {
                    if (neighbor > BirthLimit) newMap[x, y] = 1;
                    else
                    {
                        newMap[x, y] = 0;
                    }
                }
            }
        }

        return newMap;
    }

    private void InitPos()
    {
        Debug.Log("InitPos, Map Width = " + MapSettings.MapWidth);
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                m_finalMap[x, y] = Random.Range(1, 101) < m_activeChance ? 1 : 0;
            }
        }
    }

    private void ClearMap(bool v)
    {
        TopMap.ClearAllTiles();
        BottomMap.ClearAllTiles();

        if (v)
        {
            Debug.Log("Finished Clearing all");
            m_finalMap = null;
        }
    }

    private string NameZone(int zoneID)
    {


        return null;
    }
}
