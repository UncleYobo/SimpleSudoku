using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class TileInfo
{
    public int Row;
    public int Col;
    public List<int> Values = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
}
public class Tile : MonoBehaviour
{
    [SerializeField] public TileInfo Info = new TileInfo();
    public bool IsFilled;
    public TMP_Text displayText;
    public Image backPlate;
    public Color InactiveColor;
    public Color HighlightedColor;

    public bool IsInteractive;

    public int CurrentValue
    {
        get
        {
            return _currentValue;
        }
        set
        {
            _currentValue = value;
            IsFilled = true;
            displayText.text = value.ToString();
        }
    }

    private int _currentValue;
    public List<CheckGroup> groups = new List<CheckGroup>();
    private GameplayManager _mgmt;

    public void Start()
    {
        displayText = GetComponentInChildren<TMP_Text>();
        backPlate = GetComponent<Image>();
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
    }

    public void SortIntoGroups()
    {
        CheckGroup[] allGroups = GameObject.Find("CheckGroups").GetComponentsInChildren<CheckGroup>();
        foreach (CheckGroup group in allGroups)
        {
            switch (group.CheckType)
            {
                case (CheckGroup.CheckGroupType.Row):
                    if (Info.Row == group.RowStart)
                    {
                        group.TilesInGroup.Add(this);
                        groups.Add(group);
                    }
                    break;
                case (CheckGroup.CheckGroupType.Column):
                    if (Info.Col == group.ColStart)
                    {
                        group.TilesInGroup.Add(this);
                        groups.Add(group);
                    }
                    break;
                case (CheckGroup.CheckGroupType.Section):
                    if (Info.Row >= group.RowStart && Info.Row <= group.RowFinish)
                    {
                        if (Info.Col >= group.ColStart && Info.Col <= group.ColFinish)
                        {
                            group.TilesInGroup.Add(this);
                            groups.Add(group);
                        }
                    }
                    break;
            }
        }
    }

    public void SetInitialValue(int val)
    {
        CurrentValue = val;
        backPlate.color = InactiveColor;
        IsInteractive = false;
        PropogateChanges();

        void PropogateChanges()
        {
            Info.Values.Clear();
            Info.Values.Add(CurrentValue);

            foreach (CheckGroup group in groups)
            {
                group.PropogateChanges(CurrentValue, this);
            }
        }
    }

    public void RemoveValue(int val)
    {
        if (Info.Values.Count > 1)
        {
            if (Info.Values.Contains(val))
            {
                Info.Values.Remove(val);
            }
        }

        if (Info.Values.Count == 1)
        {
            CurrentValue = Info.Values[0];
        }
    }

    public void HideValue()
    {
        CurrentValue = -1;
        IsFilled = false;
        IsInteractive = true;
        backPlate.color = Color.white;
        displayText.text = "";
    }

    public void SetValue(int val)
    {
        if(val != -1)
        {
            CurrentValue = val;
            foreach (CheckGroup group in groups)
            {
                group.PerformCheck();
            }
        }
        else
        {
            CurrentValue = -1;
            displayText.text = "";
            IsFilled = false;
        }
    }

    public void SetSelected()
    {
        if (this.IsInteractive) 
        { 
            _mgmt.SetSelected(this);

            foreach (CheckGroup g in groups)
            {
                g.HighlightGroup(this);
            }
        }
    }

    public void HighlightTile(Tile t)
    {
        if(t != this)
        {
            backPlate.color = HighlightedColor;
        }
    }

    public void RemoveHightlight()
    {
        if (this.IsInteractive)
        {
            backPlate.color = Color.white;
        } else
        {
            backPlate.color = InactiveColor;
        }
    }

    #region Constructors and Helpers

    public Tile(string _name, int _row, int _col, int _val, bool _isInteractive)
    {
        this.gameObject.name = _name;
        Info.Row = _row;
        Info.Col = _col;
        CurrentValue = _val;
        IsInteractive = _isInteractive;
    }

    #endregion
}
