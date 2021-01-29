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

    private GameplayManager _mgmt;
    private bool _isSelectable = true;

    private void Start()
    {
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
    }

    public int CurrentNumber { get { return CurrentNumber; } set { Label.text = value.ToString(); } }

    public void Show(bool isHiding)
    {
        Label.enabled = isHiding;

        if (isHiding)
        {
            BackPlate.color = InactiveColor;
            _isSelectable = false;
        }
    }

    public void Select()
    {
        if (_isSelectable)
        {
            _mgmt.SetSelection(this.gameObject);
        }
    }
}
