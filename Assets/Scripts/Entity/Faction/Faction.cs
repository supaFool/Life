using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    private List<Faction> m_currentFactionList;
    public Faction()
    {
        InitFactions();
        NameFaction();
    }

    private void NameFaction()
    {
        #region Prefix
        var prefix = Random.Range(0, 100);
        var _faction_name = "";
        if (prefix < 25)
        {
            _faction_name += "The ";
        }

        if (prefix > 25 && prefix < 50)
        {
            _faction_name += "A ";
        }
        #endregion

        Debug.Log(_faction_name);
    }

    private void InitFactions()
    {
        m_currentFactionList = new List<Faction>();
    }

    public List<Faction> CurrentFactionList { get => m_currentFactionList; }
}
