using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    public int Value;

    private GameplayManager _mgmt;

    private void Start()
    {
        _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
    }

    public void SetValue()
    {
        if (!_mgmt)
        {
            GameObject.Find("MGMT").GetComponent<GameplayManager>();
        }
        _mgmt.SetValue(Value);
    }
}
