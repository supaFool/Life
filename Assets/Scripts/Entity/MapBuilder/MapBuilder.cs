using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    [SerializeField]
    private Grid MapGrid;
    //How much space to add between grids After map is built
    private float m_cellGap = 1f;
    private float m_currentCellGap = 0;
    private float m_iterations = 0;
    private bool update = false;
    private float m_iterationMultiplier = 0f;
    private float renderCounter = 0;
    private float renderSpeed = 10000f;
    private bool startRendering = false;

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
    [SerializeField]
    private Tile FiveTile;

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
        renderCounter += renderSpeed * Time.deltaTime;
        Debug.Log(renderCounter);
            if (startRendering)
            {
            m_iterations++;
                UpdateMap();
            
            }
        if (renderCounter >= 1000)
        {
            renderCounter = 0;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startRendering = true;
            update = false;
            Simulate();
            running = true;
            //SimulateTrees(ForestSamples);
        }

        if (Input.GetMouseButtonDown(1))
        {
            startRendering = false;
            ClearMap(true);
            running = false;
            m_iterations = 0;
            m_currentCellGap = 0;
            MapGrid.cellGap = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            for (int i = 0; i < 10; i++)
            {
            UpdateMap();
            }
    }

    private void UpdateMap()
    {
        update = true;
        //m_iterations = m_iterations *= m_iterationMultiplier;
        m_currentCellGap += m_iterations;
        //MapGrid.cellGap = new Vector3((float)m_iterations * m_iterationMultiplier, (float)m_iterations * m_iterationMultiplier, 0);
        MapGrid.cellGap = new Vector3(.75f, .75f, 0);
        //TopMap.size.Set(m_terrainMap.GetLength(0), m_terrainMap.GetLength(1), 0);
        Debug.Log(TopMap.cellGap);

        var oldTerraMap = m_terrainMap;
        var oldForMap = m_treeMap;

        //int[,] newMap = new int[oldTerraMap.GetLength(0) + (int)m_currentCellGap, oldTerraMap.GetLength(1) + (int)m_currentCellGap];
        int[,] newMap = new int[oldTerraMap.GetLength(0), oldTerraMap.GetLength(1)];

        //Init New Map to all 5's
        for (int x = 0; x < newMap.GetLength(0); x++)
        {
            for (int y = 0; y < newMap.GetLength(1); y++)
            {
                newMap[x, y] = 5;
            }
        }

        //Add old map
        for (int x = 0; x < oldTerraMap.GetLength(0); x++)
        {
            for (int y = 0; y < oldTerraMap.GetLength(1); y++)
            {
               // newMap[x + (int)m_currentCellGap, y + (int)m_currentCellGap] = oldTerraMap[x, y];
                newMap[x, y] = oldTerraMap[x, y];
            }
        }

        //TopMap.GetComponent<Tilemap>().size.Scale(new Vector3Int(m_iterations, m_iterations, 0));
        SimulateTerrain(Samples);
            SimulateTrees(ForestSamples);
        for (int i = 0; i < 5; i++)
        {
        }
        // MapGrid.cellGap = CG;
    }
    #endregion

    #region Functions
    private void SimulateTerrain(int samples)
    {
        SampleMap(samples);

        RenderMap();
    }
    private void SampleMap(int samples)
    {
        for (int i = 0; i < samples; i++)
        {
            //Update Terrain map for the amount of samples
            m_terrainMap = BuildTerrain(m_terrainMap, update);
        }
    }
    private void RenderMap()
    {
        for (int x = 0; x < m_terrainMap.GetLength(0); x++)
        {
            for (int y = 0; y < m_terrainMap.GetLength(1); y++)
            {
                m_treeMap[x, y] = 0;

                if (m_terrainMap[x, y] == 1)
                {
                    TopMap.SetTile(new Vector3Int(-x + (m_terrainMap.GetLength(0)) / 2, -y + (m_terrainMap.GetLength(1)) / 2, 0), FiveTile);
                    var r = Random.Range(0, 100);
                    if (r <= 3)
                    {
                    m_treeMap[x, y] = 9;
                    }
                }

                if (m_terrainMap[x, y] == 0)
                {
                    TopMap.SetTile(new Vector3Int(-x + (m_terrainMap.GetLength(0)) / 2, -y + (m_terrainMap.GetLength(1)) / 2, 0), BottomTile);
                    m_treeMap[x, y] = 0;
                }
            }
        }
    }
    private void SimulateTrees(int samples)
    {

        for (int i = 0; i < samples; i++)
        {
            m_treeMap = BuildForest(m_treeMap);
        }
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {

                if (m_treeMap[x, y] == 3)
                {
                    TreeMap.SetTile(new Vector3Int(-x + MapSettings.MapWidth / 2, -y + MapSettings.MapHeight / 2, 0), TopTile);
                }
            }
        }
    }
    private void Simulate()
    {
        Debug.Log("Building Terrain");
        ClearMap(false);
        InitMapGrid();
        SimulateTerrain(Samples);
        SimulateTrees(ForestSamples);
    }
    private void InitMapGrid()
    {
        if (m_terrainMap == null)
        {
            m_terrainMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
            m_treeMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
            InitPos();
        }
    }
    private int[,] BuildTerrain(int[,] tempMap, bool updatemap)
    {
        int[,] updatedMap;
        int neighbor;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);
        #region UpdatedMap
        if (updatemap)
        {
            updatedMap = new int[tempMap.GetLength(0), tempMap.GetLength(1)];
            for (int x = 0; x < tempMap.GetLength(0); x++)
            {
                for (int y = 0; y < tempMap.GetLength(1); y++)
                {
                    if (tempMap[x, y] == 5)
                    {
                        tempMap[x, y] = Random.Range(1, 101) < m_activeChance ? 1 : 0;
                    }

                    neighbor = 0;
                    foreach (var b in bounds.allPositionsWithin)
                    {
                        if (b.x == 0 && b.y == 0) continue;
                        if (x + b.x >= 0 && x + b.x < tempMap.GetLength(0) && y + b.y >= 0 && y + b.y < tempMap.GetLength(1))
                        {
                            neighbor += tempMap[x + b.x, y + b.y];
                        }
                        else
                        {
                            //Draw Border
                            //neighbor++;
                        }
                    }
                    if (tempMap[x, y] == 1)
                    {
                        if (neighbor < DeathLimit) updatedMap[x, y] = 0;
                        else
                        {
                            updatedMap[x, y] = 1;
                        }
                    }
                    if (tempMap[x, y] == 0)
                    {
                        if (neighbor > BirthLimit) updatedMap[x, y] = 1;
                        else
                        {
                            updatedMap[x, y] = 0;
                        }
                    }

                }
            }
        }
        #endregion

        #region Not Update
        else
        {
            updatedMap = new int[m_terrainMap.GetLength(0), m_terrainMap.GetLength(1)];
            for (int x = 0; x < MapSettings.MapWidth; x++)
            {
                for (int y = 0; y < MapSettings.MapHeight; y++)
                {
                    neighbor = 0;
                    foreach (var b in bounds.allPositionsWithin)
                    {
                        if (b.x == 0 && b.y == 0) continue;
                        if (x + b.x >= 0 && x + b.x < m_terrainMap.GetLength(0) && y + b.y >= 0 && y + b.y < m_terrainMap.GetLength(1))
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
                        if (neighbor < DeathLimit) updatedMap[x, y] = 0;
                        else
                        {
                            updatedMap[x, y] = 1;
                        }
                    }
                    if (tempMap[x, y] == 0)
                    {
                        if (neighbor > BirthLimit) updatedMap[x, y] = 1;
                        else
                        {
                            updatedMap[x, y] = 0;
                        }
                    }
                }
            }
        }
        #endregion

        return updatedMap;


    }
    private int[,] BuildForest(int[,] tempMap)
    {
        int[,] newMap = new int[MapSettings.MapWidth, MapSettings.MapHeight];
        int neighbor;
        BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

        #region Build Forest
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

                if (tempMap[x, y] == 4)
                {
                    if (neighbor > BirthLimit) newMap[x, y] = 3;
                    else
                    {
                        newMap[x, y] = 4;
                    }
                }
            }
        }

        #endregion

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
    #endregion
}
