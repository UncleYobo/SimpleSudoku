using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGroup : MonoBehaviour
{
    public int RowStart, RowFinish;
    public int ColStart, ColFinish;

    public bool Complete;

    private List<Tile> Group = new List<Tile>();

    public void SortGroup(List<Tile> tileList)
    {
        foreach(Tile tile in tileList)
        {
            int Row;
            tile.TileID.TryGetValue("Row", out Row);
            int Col;
            tile.TileID.TryGetValue("Col", out Col);

            if (Row >= RowStart && Row <= RowFinish && Col >= ColStart && Col <= ColFinish)
            {
                Group.Add(tile);
            }
        }
    }

    public void PerformCheck()
    {
        Complete = GroupIsComplete();
    }

    public bool GroupIsComplete()
    {
        bool isComplete = true;
        List<int> checkList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9};
        
        foreach(Tile tile in Group)
        {
            if (tile.IsFilled)
            {
                if (checkList.Contains(tile.CurrentNumber))
                {
                    checkList.Remove(tile.CurrentNumber);
                } else
                {
                    isComplete = false;
                    break;
                }
            } else
            {
                isComplete = false;
                break;
            }
        }

        return isComplete;
    }
}
