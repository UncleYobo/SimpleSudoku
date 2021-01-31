using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject CurrentSelection;
    public Color SelectionColor;

    public Generator.GameMode Difficulty;
    public Generator _boardGenerator;
    public CheckGroup[] checkGroups;

    public bool Ads;

    private GameObject _previousSelection;
    private bool _isDirty;
    private AdHandler _ads;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneChanged;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneChanged;
    }

    void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        _ads = GetComponent<AdHandler>();
    }

    public void OnNewGameStarted()
    {
        _boardGenerator = GameObject.Find("Generator").GetComponent<Generator>();
        _boardGenerator.CreateNew(Difficulty);

        StartCoroutine(TriggerBanner());
    }

    public void SortIntoCheckGroups(List<Tile> tileList)
    {
        if(checkGroups.Length == 0 || checkGroups[0] == null)
        {
            checkGroups = GameObject.Find("CheckGroups").GetComponentsInChildren<CheckGroup>();
        }
        foreach(CheckGroup group in checkGroups)
        {
            group.SortGroup(tileList);
        }
    }

    public void PerformCheck()
    {
        bool isSolved = true;

        foreach(CheckGroup group in checkGroups)
        {
            if (!group.Complete)
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved)
        {
            GameObject.Find("Victory").GetComponent<UIGroup>().OpenGroup();
        }
    }

    public void SetSelection(GameObject selection)
    {
        if (CurrentSelection)
        {
            _previousSelection = CurrentSelection;
        }
        CurrentSelection = selection;
        _isDirty = true;
    }

    public void SetValue(int val)
    {
        if (CurrentSelection)
        {
            CurrentSelection.GetComponent<Tile>().SetValue(val);
        }
    }

    private void LateUpdate()
    {
        if (CurrentSelection)
        {
            if (_isDirty)
            {
                if (_previousSelection)
                {
                    _previousSelection.GetComponent<Tile>().BackPlate.color = Color.white;
                }
                CurrentSelection.GetComponent<Tile>().BackPlate.color = SelectionColor;

                _isDirty = false;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void NewGame(string difficulty)
    {
        switch (difficulty)
        {
            case ("Easy"):
                Difficulty = Generator.GameMode.Easy;
                break;
            case ("Medium"):
                Difficulty = Generator.GameMode.Medium;
                break;
            case ("Hard"):
                Difficulty = Generator.GameMode.Hard;
                break;
            case ("Expert"):
                Difficulty = Generator.GameMode.Expert;
                break;
        }

        SceneManager.LoadScene("Game");

        if (Ads)
        {
            _ads.PlayVideoAd();
        }
    }

    IEnumerator TriggerBanner()
    {
        yield return new WaitForSeconds(3);
        _ads.ShowBannerAd();
    }
}
