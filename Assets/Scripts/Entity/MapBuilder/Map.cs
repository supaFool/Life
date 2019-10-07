using UnityEngine;

public class Map : MonoBehaviour
{
    private Vector3Int m_zoneSize;
    //A zone is a biome grid of the entire map
    private Zone[] m_localZones;
    private int m_minSize = 50;
    private int m_maxSize = 65;


    private void Awake()
    {
        if (m_zoneSize == null)
        {
            var rWidth = Random.Range(m_minSize, m_maxSize);
            var rHeight = Random.Range(m_minSize, m_maxSize);

            //Size of Zone
            m_zoneSize = new Vector3Int(rWidth, rHeight, 0);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string TEST()
    {
        return this.name.ToString() + " is working.";
    }
}
