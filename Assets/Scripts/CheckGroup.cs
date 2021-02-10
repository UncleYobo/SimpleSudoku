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

    public bool IsComplete = false;

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

    public void PerformCheck()
    {
        List<int> tracker = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        foreach(Tile t in TilesInGroup)
        {
            tracker.Contains(t.CurrentValue);
            tracker.Remove(t.CurrentValue);
        }
        if(tracker.Count == 0)
        {
            IsComplete = true;
            Debug.Log(gameObject.name + " complete");
            GameObject.Find("MGMT").GetComponent<GameplayManager>().PerformCheck();
        }
    }

    public void HighlightGroup(Tile t)
    {
        foreach(Tile tile in TilesInGroup)
        {
            tile.HighlightTile(t);
        }
    }

    public void RemoveHightlight()
    {
        foreach(Tile tile in TilesInGroup)
        {
            tile.RemoveHightlight();
        }
    }
}
