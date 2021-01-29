using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public GameObject CurrentSelection;
    public Color SelectionColor;

    public Generator.GameMode Difficulty;

    private GameObject _previousSelection;
    private bool _isDirty;
    //--TODO: Set this up the right way
    public Generator _boardGenerator;

    public CheckGroup[] checkGroups;

    private void Start()
    {
        _boardGenerator.CreateNew(Difficulty);
    }

    public void SortIntoCheckGroups(List<Tile> tileList)
    {
        if(checkGroups.Length == 0)
        {
            checkGroups = GetComponentsInChildren<CheckGroup>();
        }
        foreach(CheckGroup group in checkGroups)
        {
            group.SortGroup(tileList);
        }
    }

    public void SetSelection(GameObject selection)
    {
        if (CurrentSelection)
        {
            _previousSelection = CurrentSelection;
        }
        CurrentSelection = selection;
        _isDirty = true;
    }

    public void SetValue(int val)
    {
        if (CurrentSelection)
        {
            CurrentSelection.GetComponent<Tile>().SetValue(val);
        }
    }

    private void LateUpdate()
    {
        if (CurrentSelection)
        {
            if (_isDirty)
            {
                if (_previousSelection)
                {
                    _previousSelection.GetComponent<Tile>().BackPlate.color = Color.white;
                }
                CurrentSelection.GetComponent<Tile>().BackPlate.color = SelectionColor;

                _isDirty = false;
            }
        }
    }
}
