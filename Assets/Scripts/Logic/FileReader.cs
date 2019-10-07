using System.Collections.Generic;
using UnityEngine;

public class FileReader : MonoBehaviour
{

    //Lists of words
    public List<string> MaleFirstName;
    public List<string> FemaleFirstName;
    public List<string> LastName;
    public List<string> Adjs;
    public List<string> Words;
    public List<string> Advs;
    public List<string> Nouns;
    public List<string> Streets;
    public List<string> CityNames;
    public List<string> StateAbbr;
    public List<string> CountyName;
    public List<string> Population;
    public List<string> Zip;

    public List<string> Occupation;
    public List<string> OccDesc;

    //pointers
    public static int CITY = 0;
    public static int CITY_ASCII = 1;
    public static int STATE_ID = 2;
    public static int STATE_NAME = 3;
    public static int COUNTY_FIPS = 4;
    public static int COUNTY_NAME = 5;
    public static int LAT = 6;
    public static int LNG = 7;
    public static int POP = 8;
    public static int POP_PROP = 9;
    public static int POP_DENSITY = 10;
    public static int SOURCE = 11;
    public static int INC = 12;
    public static int TIMEZONE = 13;
    public static int ZIP = 14;
    public static int ID = 15;

    public string[] Map;


    public void Init()
    {
        Debug.Log("Load check");
        #region Load Files


        var m = (TextAsset)Resources.Load("Text/male-first", typeof(TextAsset));
        var _maleNames = m.text.Split('\n');
        foreach (var line in _maleNames)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            MaleFirstName.Add(line);
        }

        var f = (TextAsset)Resources.Load("Text/female-first", typeof(TextAsset));
        var _femaleNames = f.text.Split('\n');

        foreach (var line in _femaleNames)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            FemaleFirstName.Add(line);
        }

        var l = (TextAsset)Resources.Load("Text/last", typeof(TextAsset));
        var _lastNames = l.text.Split('\n');

        foreach (var line in _lastNames)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            LastName.Add(line);
        }

        var aj = (TextAsset)Resources.Load("Text/adj", typeof(TextAsset));
        var _adj = aj.text.Split('\n');

        foreach (var line in _adj)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            Adjs.Add(line);
        }

        var w = (TextAsset)Resources.Load("Text/words", typeof(TextAsset));
        var _word = w.text.Split('\n');

        foreach (var line in _word)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            Words.Add(line);
        }

        var av = (TextAsset)Resources.Load("Text/adv", typeof(TextAsset));
        var _adv = av.text.Split('\n');

        foreach (var line in _adv)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            Advs.Add(line);
        }

        var n = (TextAsset)Resources.Load("Text/nouns", typeof(TextAsset));
        var _noun = n.text.Split('\n');

        foreach (var line in _noun)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            Nouns.Add(line);
        }

        var s = (TextAsset)Resources.Load("Text/street", typeof(TextAsset));
        var _street = s.text.Split('\n');

        //Street suffixes
        foreach (var line in _street)
        {
            line.Replace(" ", string.Empty);
            line.Trim();
            Streets.Add(line);
        }

        //Load Occupation
        var o = (TextAsset)Resources.Load("Text/occupation", typeof(TextAsset));
        Occupation.AddRange(o.text.Split('\n'));

        //Load Map
        //var map = (TextAsset)Resources.Load("Text/uscities", typeof(TextAsset));
        //Map = map.text.Split('\n');


        #endregion
        //temp[i].Replace("\"([^\"]*?)\"", string.Empty);
    }

    private void Awake()
    {
        Init();
    }

    public string GetStreetAddress()
    {
        string _streetAddy = Random.Range(0, 9999).ToString() + " ";

        if (Random.Range(0, 100) < 25)
        {
            _streetAddy = _streetAddy + Capitilize(Adjs[Random.Range(0, Adjs.Count)]) + " ";

        }

        _streetAddy = _streetAddy + Capitilize(Nouns[Random.Range(0, Nouns.Count)]) + " ";


        return _streetAddy + Streets[Random.Range(0, Streets.Count)];

    }

    public string[] GetCityAddress(int pick)
    {
        int counter = 0;
        foreach (var line in Map)
        {
            counter++;
            if (counter == pick)
            {
                var s = line.Split(',');
                return s;
            }
        }
        return null;
    }

    public string GetOccupation(int index)
    {
        return Occupation[index];
    }

    public string GetPlayerName(bool is_male)
    {
        if (is_male)
        {
            return MaleFirstName[Random.Range(0, MaleFirstName.Count)].Trim() + " " + LastName[Random.Range(0, LastName.Count)].Trim();
        }
        else
        {
            return FemaleFirstName[Random.Range(0, FemaleFirstName.Count)].Trim() + " " + LastName[Random.Range(0, LastName.Count)].Trim();
        }
    }

    public string GetWorldName()
    {
        var _worldname = "Land of the ";
        if (Random.Range(0, 100) < 50)
        {
            _worldname += Capitilize(Adjs[Random.Range(0, Adjs.Count)] + " ");
        }

        _worldname += Capitilize(Nouns[Random.Range(0, Nouns.Count)]) + " ";
        return _worldname;
    }

    public string Capitilize(string s)
    {
        return char.ToUpper(s[0]) + s.Substring(1);
    }

}
