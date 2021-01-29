using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public TMP_Text Label;

    public int CurrentNumber { get { return CurrentNumber; } set { Label.text = value.ToString(); } }

    public void Show(bool isHiding)
    {
        Label.enabled = isHiding;
    }
}
