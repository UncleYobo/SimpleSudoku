using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public TMP_Text Label;
    public Image BackPlate;
    public Color InactiveColor;
    public Dictionary<string, int> TileID = new Dictionary<string, int>();
    public bool IsFilled;
    public List<CheckGroup> CheckGroups = new List<CheckGroup>();

    private GameplayManager _mgmt;
    private bool _isSelectable = true;

    private void Start()
    {
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
    }

    public int CurrentNumber { 
        get { 
            return _currentNumber; 
        } 
        set { 
            Label.text = value.ToString();
            _currentNumber = value;
        } 
    }
    private int _currentNumber;

    public void Show(bool isHiding)
    {
        Label.enabled = isHiding;

        if (isHiding)
        {
            BackPlate.color = InactiveColor;
            _isSelectable = false;
            IsFilled = true;
        }
    }

    public void Select()
    {
        if (_isSelectable)
        {
            _mgmt.SetSelection(this.gameObject);
        }
    }

    public void SetValue(int val)
    {
        CurrentNumber = val;

        if(val != 0)
        {
            Label.enabled = true;
            IsFilled = true;
        } else
        {
            Label.enabled = false;
            IsFilled = false;
        }

        foreach(CheckGroup group in CheckGroups)
        {
            group.PerformCheck();
        }
    }
}
