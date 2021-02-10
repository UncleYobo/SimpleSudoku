using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyDisplay : MonoBehaviour
{
    public TMP_Text displayText;
    void Start()
    {
        GameplayManager _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
        displayText.text = _mgmt.Difficulty.ToString();
    }
}
