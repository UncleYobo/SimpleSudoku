using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private List<CheckGroup> groups = new List<CheckGroup>();

    public void Start()
    {
        displayText = GetComponentInChildren<TMP_Text>();
        backPlate = GetComponent<Image>();
    }

    public void SortIntoGroups()
    {
        CheckGroup[] allGroups = GameObject.Find("CheckGroups").GetComponentsInChildren<CheckGroup>();
        foreach(CheckGroup group in allGroups)
        {
            switch (group.CheckType)
            {
                case (CheckGroup.CheckGroupType.Row):
                    if(Info.Row == group.RowStart)
                    {
                        group.TilesInGroup.Add(this);
                        groups.Add(group);
                    }
                    break;
                case (CheckGroup.CheckGroupType.Column):
                    if(Info.Col == group.ColStart)
                    {
                        group.TilesInGroup.Add(this);
                        groups.Add(group);
                    }
                    break;
                case (CheckGroup.CheckGroupType.Section):
                    if(Info.Row >= group.RowStart && Info.Row <= group.RowFinish)
                    {
                        if(Info.Col >= group.ColStart && Info.Col <= group.ColFinish)
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
        if(Info.Values.Count > 1)
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
}
