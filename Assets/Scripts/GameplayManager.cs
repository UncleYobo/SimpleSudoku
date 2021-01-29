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

    private void Start()
    {
        _boardGenerator.CreateNew(Difficulty);
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
