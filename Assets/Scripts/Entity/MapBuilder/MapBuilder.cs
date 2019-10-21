using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    [SerializeField]
    private Grid MapGrid;
    //How much space to add between grids After map is built
    private float m_cellGap = 3f;
    private int m_iterations = 0;

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
    private Tilemap TreeMap;
    [SerializeField]
    private Tile TopTile;
    [SerializeField]
    private Tile TreeTile;
    [SerializeField]
    private Tile BottomTile;

    private bool running;

    //Spawn Settings
    [Range(1, 100)]
    public int m_activeChance;

    [Range(1, 8)]
    public int BirthLimit;

    [Range(1, 8)]
    public int DeathLimit;

    [Range(1, 10)]
    public int Samples;

    [Range(1, 100)]
    public int ForestActiveChance;

    [Range(1, 8)]
    public int ForestBirthLimit;

    [Range(1, 8)]
    public int ForestDeathLimit;

    [Range(1, 10)]
    public int ForestSamples;
    private int counter = 0;

    private int[,] m_terrainMap;
    private int[,] m_treeMap;
    public Vector3Int MapSize;

    public int GridSize { get => m_terrainMap.Length; }

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
            Simulate();
            running = true;
            //SimulateTrees(ForestSamples);
        }

        if (Input.GetMouseButtonDown(1))
        {
            ClearMap(true);
            running = false;
            MapGrid.cellGap = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_iterations++;
            MapGrid.cellGap = new Vector3(m_cellGap, m_cellGap, 0);
        }
    }
    #endregion

    public void SimulateTerrain(int samples)
    {
        for (int i = 0; i < samples; i++)
        {
            m_terrainMap = BuildTerrain(m_terrainMap);
        }

        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                m_treeMap[x, y] = 0;

                if (m_terrainMap[x, y] == 1)
                {
                    TopMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), TopTile);

                    m_treeMap[x, y] = 9;
                }

                if (m_terrainMap[x, y] == 0)
                {
                    BottomMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), BottomTile);
                    m_treeMap[x, y] = 0;
                }
            }
        }
    }

    public void SimulateTrees(int samples)
    {
        for (int i = 0; i < samples; i++)
        {
            m_treeMap = BuildForest(m_treeMap);
        }
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                Debug.Log(m_treeMap[x, y]);

                if (m_treeMap[x, y] == 3)
                {
                    TreeMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), TreeTile);
                }
            }
        }
    }

    public void Simulate()
    {
        Debug.Log("Building Terrain");
        ClearMap(false);

        if (m_terrainMap == null)
        {
            Debug.Log("Creating Grid");
            m_terrainMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
            m_treeMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
            InitPos();
        }

        SimulateTerrain(Samples);
        SimulateTrees(ForestSamples);
    }

    private int[,] BuildTerrain(int[,] tempMap)
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
    private int[,] BuildForest(int[,] tempMap)
    {
        int[,] newMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
        int neighbor;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);



        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                //Init Treemap
                if (m_treeMap[x, y] == 9)
                {
                    m_treeMap[x, y] = Random.Range(1, 101) < ForestActiveChance ? 3 : 4;
                }
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
                        //neighbor++;
                    }
                }
                if (tempMap[x, y] == 3)
                {
                    if (neighbor < ForestDeathLimit) newMap[x, y] = 4;
                    else
                    {
                        newMap[x, y] = 3;
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
                m_terrainMap[x, y] = Random.Range(1, 101) < m_activeChance ? 1 : 0;
            }
        }
    }
    private void InitTreeMap()
    {
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                //Init Treemap
                if (m_treeMap[x, y] == 9)
                {
                    m_treeMap[x, y] = Random.Range(1, 101) < ForestActiveChance ? 3 : 4;
                }
            }
        }
    }

    private void ClearMap(bool v)
    {
        TopMap.ClearAllTiles();
        BottomMap.ClearAllTiles();
        TreeMap.ClearAllTiles();

        if (v)
        {
            Debug.Log("Finished Clearing all");
            m_terrainMap = null;
        }
    }

    private string NameZone(int zoneID)
    {


        return null;
    }
}
