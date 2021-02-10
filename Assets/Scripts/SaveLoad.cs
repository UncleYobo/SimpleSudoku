using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public void SaveState()
    {
        PlayerPrefs.SetInt("InGame", 1);

        GameObject[] tileObjs = GameObject.FindGameObjectsWithTag("Tile");
        string saveData = "";
        foreach(GameObject go in tileObjs)
        {
            Tile tileData = go.GetComponent<Tile>();

        }

    }
    public void LoadState()
    {

    }
    public void ClearSave()
    {
        PlayerPrefs.SetInt("InGame", 0);
    }

    public bool HasData()
    {
        bool data = false;
        if (PlayerPrefs.HasKey("InGame"))
        {
            if(PlayerPrefs.GetInt("InGame") == 1)
            {
                data = true;
            }
        }
        return data;
    }
}
