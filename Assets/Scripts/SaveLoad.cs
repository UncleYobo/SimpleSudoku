using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Payload
{
    [SerializeField] public string Name;
    [SerializeField] public int Row;
    [SerializeField] public int Col;
    [SerializeField] public int Value;
    [SerializeField] public bool isInteractive;

    [SerializeField]
    public Payload(string _name, int _row, int _col, int _val, bool _isInteractive)
    {
        Name = _name;
        Row = _row;
        Col = _col;
        Value = _val;
        isInteractive = _isInteractive;
    }
}

[Serializable]
public class SaveLoad : MonoBehaviour
{
    public void SaveState()
    {
        PlayerPrefs.SetInt("InGame", 1);

        GameObject[] tileObjs = GameObject.FindGameObjectsWithTag("Tile");
        List<Payload> saveData = new List<Payload>();
        foreach(GameObject go in tileObjs)
        {
            Tile tileData = go.GetComponent<Tile>();
            Payload payload = new Payload(go.name, tileData.Info.Row, tileData.Info.Col, tileData.CurrentValue, tileData.IsInteractive);
            saveData.Add(payload);
        }
        string saveDataJson = JsonHelper.ToJson(saveData.ToArray(), true);
        PlayerPrefs.SetString("SaveData", saveDataJson);
    }
    public void LoadState()
    {
        string saveDataJson = PlayerPrefs.GetString("SaveData");
        Debug.Log(saveDataJson);
        Payload[] saveData = JsonHelper.FromJson<Payload>(saveDataJson);
        GameObject.Find("Generator").GetComponent<Generator>().LoadGame(saveData);
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

[Serializable]
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
