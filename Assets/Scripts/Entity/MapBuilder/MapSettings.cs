public class MapSettings
{
    private string m_worldName;

    //Main Map Size
    public static int MapWidth = 50;
    public static int MapHeight = 50;
    public static int MapZLayers = 0;

    #region ZoneGetters
    //Zone IDS
    public static int OCEAN = 0;
    public static int DESERT = 1;
    public static int FOREST = 2;
    public static int CITY = 3;
    public static int FARMLAND = 4;

    //Zone ID InfoGetter
    public static int ZONE_NAME = 0;
    public static int ZONE_DESC = 1;
    private string[,] name = new string[,]
    {
            { "Ocean", Settings.TODO },
            { "Desert", Settings.TODO },
            { "Forest", Settings.TODO },
            { "City", Settings.TODO },
            {"Farmland", Settings.TODO }

    };
    #endregion


    public string WorldName { get => m_worldName; set => m_worldName = value; }


}
