using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Number : MonoBehaviour
{
    public int Value;

    GameplayManager _mgmt;

    void Start()
    {
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
    }

    public void SetValue()
    {
        _mgmt.currentlySelected.SetValue(Value);
    }
}
