using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroup : MonoBehaviour
{
    public enum CheckGroupType { Row, Column, Section}
    public CheckGroupType CheckType;

    public int RowStart, RowFinish;
    public int ColStart, ColFinish;

    public List<Tile> TilesInGroup = new List<Tile>();

    public void PropogateChanges(int val, Tile currentTile)
    {
        foreach(Tile t in TilesInGroup)
        {
            if(t != currentTile)
            {
                if (t.Info.Values.Contains(val))
                {
                    t.Info.Values.Remove(val);
                }
            }
        }
    }
}
