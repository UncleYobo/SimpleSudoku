using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    //--Generation var
    public enum GameMode { Easy, Medium, Hard, Expert }
    public GameMode Difficulty;
    
    int[,] grid = new int[9, 9];
    int hiddenCount;

    //--Board Gameobjects
    public GameObject TileObject;
    public Transform SpawnPoint;
    public List<Tile> spawnedTiles = new List<Tile>();

    private void Start()
    {
        DifficultyScale();
        GenerateGrid();
        
    }

    void Init(ref int[,] grid)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
                
            }
        }
    }

    void ChangeTwoCell(ref int[,] grid, int findValue1, int findValue2)
    {
        int xParam1, yParam1, xParam2, yParam2;
        xParam1 = yParam1 = xParam2 = yParam2 = 0;
        for (int i = 0; i < 9; i+=3)
        {
            for (int k = 0; k < 9; k+=3)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        if(grid[i + j, k + z] == findValue1)
                        {
                            xParam1 = i + j;
                            yParam1 = k + z;
                        }
                        if(grid[i + j, k + z] == findValue2)
                        {
                            xParam2 = i + j;
                            yParam2 = k + z;
                        }
                    }
                }
                grid[xParam1, yParam1] = findValue2;
                grid[xParam2, yParam2] = findValue1;
            }
        }
    }

    void UpdateGrid(ref int[,] grid, int shuffleLevel)
    {
        for (int repeat = 0; repeat < shuffleLevel; repeat++)
        {
            System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
            System.Random rand2 = new System.Random(Guid.NewGuid().GetHashCode());
            ChangeTwoCell(ref grid, rand.Next(1, 9), rand2.Next(1, 9));
        }
    }

    void DifficultyScale()
    {
        switch (Difficulty)
        {
            case GameMode.Easy:
                hiddenCount = 30;
                break;
            case GameMode.Medium:
                hiddenCount = 40;
                break;
            case GameMode.Hard:
                hiddenCount = 50;
                break;
            case GameMode.Expert:
                hiddenCount = 60;
                break;
        }
    }

    public void GenerateGrid()
    {
        Init(ref grid);
        UpdateGrid(ref grid, 10);
        CreateBoard(ref grid);

        void CreateBoard(ref int[,] grid)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    //--Create tile
                    Tile newTile = Instantiate(TileObject, SpawnPoint).GetComponent<Tile>();
                    newTile.gameObject.name = "Tile " + x + " : " + y;
                    //--Assign value from grid[x, y] to internal text
                    newTile.CurrentNumber = grid[x, y];
                    spawnedTiles.Add(newTile);
                }
            }

            //--show/hide tile based on difficulty
            for (int i = spawnedTiles.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i);
                Tile shuffleTileA = spawnedTiles[j];
                Tile shuffleTileB = spawnedTiles[i];
                spawnedTiles.RemoveAt(i);
                spawnedTiles.Insert(i, shuffleTileA);
                spawnedTiles.RemoveAt(j);
                spawnedTiles.Insert(j, shuffleTileB);
            }

            for (int hideIndex = 0; hideIndex < spawnedTiles.Count; hideIndex++)
            {
                if(hideIndex < hiddenCount)
                {
                    spawnedTiles[hideIndex].GetComponent<Tile>().Show(false);
                } else
                {
                    spawnedTiles[hideIndex].GetComponent<Tile>().Show(true);
                }
                
            }
        }
    }
}
