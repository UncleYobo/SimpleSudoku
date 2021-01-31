using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyDisplay : MonoBehaviour
{
    public TMP_Text Display;

    private GameplayManager _mgmt;

    private void Start()
    {
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
        Display.text = _mgmt.Difficulty.ToString();
    }
}
