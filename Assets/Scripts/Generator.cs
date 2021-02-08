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

    List<Tile> spawnedTiles = new List<Tile>();

    public bool IsFinished;

    float timer;
    float rate = 0.01f;

    void Start()
    {
        CreateNew();
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
        }
    }

    void CreateNew()
    {
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
            Debug.Log("Unsolvable");
            SceneManager.LoadScene("Game");
        }

        /*
        if(randomSelection.Info.Values.Count != 0)
        {
            IsFinished = true;
            StartCoroutine(ResetPuzzle());
        } else
        {
            
        }
        */
    }

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
}