using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    public GameObject TileObject;
    public Transform SpawnPoint;
    public CanvasGroup LoadingScreen;

    public enum DifficultyLevels { Easy, Medium, Hard, Expert }

    List<Tile> spawnedTiles = new List<Tile>();

    public bool IsFinished;

    int hiddenCount;
    public GameplayManager _mgmt;

    float timer;
    float rate = 0.01f;

    void Start()
    {
        _mgmt = GameObject.FindGameObjectWithTag("MGMT").GetComponent<GameplayManager>();

        if (_mgmt.HasData)
        {

        } else
        {
            CreateNew();
        }

        LoadingScreen.alpha = 1f;
    }

    void Update()
    {
        if (!IsFinished)
        {
            timer += Time.deltaTime;
            if(timer > rate)
            {
                timer = 0f;
                
                AssignValues();
            }
        } else if (IsFinished && spawnedTiles.Count == 0)
        {
            if (LoadingScreen.alpha > 0f) LoadingScreen.alpha -= Time.deltaTime / 2f;
        }
    }

    #region New Game Generation
    void CreateNew()
    {
        DifficultyScale();

        IsFinished = true;

        Init();

        void Init()
        {
            for(int x = 0; x < 9; x++)
            {
                for(int y = 0; y < 9; y++)
                {
                    Tile newTile = Instantiate(TileObject, SpawnPoint).GetComponent<Tile>();
                    newTile.gameObject.name = x + " : " + y;
                    newTile.Info.Row = x;
                    newTile.Info.Col = y;
                    newTile.SortIntoGroups();
                    spawnedTiles.Add(newTile);
                }
            }

            IsFinished = false;
        }

        void DifficultyScale()
        {
            if (!_mgmt)
            {
                _mgmt = GameObject.Find("MGMT").GetComponent<GameplayManager>();
            }

            if (_mgmt.Difficulty == DifficultyLevels.Easy) hiddenCount = 35;
            else if (_mgmt.Difficulty == DifficultyLevels.Medium) hiddenCount = 45;
            else if (_mgmt.Difficulty == DifficultyLevels.Hard) hiddenCount = 55;
            else hiddenCount = 64;
        }
    }

    void AssignValues()
    {
        List<Tile> selectionGroup = ReturnSelectionGroup();

        int randomIndex = UnityEngine.Random.Range(0, selectionGroup.Count - 1);

        Tile randomSelection = selectionGroup[randomIndex];
        spawnedTiles.Remove(randomSelection);

        if (randomSelection.Info.Values.Count != 0)
        {
            int randomNumberFromValues = randomSelection.Info.Values[UnityEngine.Random.Range(0, randomSelection.Info.Values.Count - 1)];
            randomSelection.SetInitialValue(randomNumberFromValues);
            IsFinished = IsFinishedAssigning();
        } else
        {
            SceneManager.LoadScene("Game");
        }

        if (spawnedTiles.Count == 0 && IsFinished)
        {
            GameObject[] allTiles = GameObject.FindGameObjectsWithTag("Tile");
            ShowSolution(allTiles);
            HideRandomTiles(allTiles);
            CheckGroup[] groups = GameObject.Find("CheckGroups").GetComponentsInChildren<CheckGroup>();
            foreach (CheckGroup g in groups)
            {
                g.PerformCheck();
            }

            _mgmt = GameObject.FindGameObjectWithTag("MGMT").GetComponent<GameplayManager>();
        }

        void ShowSolution(GameObject[] tiles)
        {
            string solution = "Solution:\n" ;
            string line = "";
            foreach(GameObject t in tiles)
            {
                line += t.GetComponent<Tile>().CurrentValue.ToString() + " ";

                if(line.Length >= 18)
                {
                    line += "\n";
                    solution += line;
                    line = "";
                }
            }

            Debug.Log(solution);
        }

        void HideRandomTiles(GameObject[] tiles)
        {
            for(int i = 0; i < tiles.Length; i++)
            {
                GameObject tmp;
                int rnd = UnityEngine.Random.Range(0, tiles.Length);
                tmp = tiles[rnd];
                tiles[rnd] = tiles[i];
                tiles[i] = tmp;
            }

            for(int x = 0; x < hiddenCount; x++)
            {
                tiles[x].GetComponent<Tile>().HideValue();
            }
        }
    }

    #endregion

    #region Load Existing

    public void LoadGame(Payload[] saveData)
    {
        List<Payload> payloadList = new List<Payload>();
        foreach(Payload p in saveData)
        {
            payloadList.Add(p);
        }

        payloadList.OrderBy(x => x.Row).ThenBy(x => x.Col);

        foreach(Payload data in payloadList)
        {
            Tile newTile = Instantiate(TileObject, SpawnPoint).GetComponent<Tile>();
            newTile.gameObject.name = data.Name;
            newTile.Info.Row = data.Row;
            newTile.Info.Col = data.Col;
            newTile.IsInteractive = data.isInteractive;
            newTile.CurrentValue = data.Value;
        }
    }

    #endregion

    #region Helpers
    List<Tile> ReturnSelectionGroup()
    {
        int lowestEntropy = FindLowestEntropy();
        List<Tile> selectionGroup = new List<Tile>();

        foreach (Tile t in spawnedTiles)
        {
            if (t.Info.Values.Count == lowestEntropy)
            {
                selectionGroup.Add(t);
            }
        }

        return selectionGroup;
    }
    int FindLowestEntropy()
    {
        int lowest = 9;

        foreach (Tile t in spawnedTiles)
        {
            if (t.Info.Values.Count < lowest)
            {
                lowest = t.Info.Values.Count;
            }
        }

        return lowest;
    }

    bool IsFinishedAssigning()
    {
        bool isFinished = true;

        foreach(Tile t in spawnedTiles)
        {
            if(!t.IsFilled)
            {
                isFinished = false;
                break;
            }
        }

        return isFinished;
    }

    #endregion
}