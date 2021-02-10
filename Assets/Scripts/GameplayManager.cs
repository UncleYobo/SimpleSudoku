using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public bool Ads;
    public Generator.DifficultyLevels Difficulty;

    public Tile currentlySelected;
    public Color SelectedColor;

    public CheckGroup[] checkGroups;

    public UIGroup Victory;

    private AdHandler _ads;
    private SaveLoad _saveLoad;

    void Start()
    {
        DontDestroyOnLoad(this);
        _ads = GetComponent<AdHandler>();
        _saveLoad = GetComponent<SaveLoad>();

        if (_saveLoad.HasData())
        {
            LoadPreviousGame();
        }
    }

    public void OnNewGameStarted()
    {
        if (Ads)
        {
            StartCoroutine(TriggerBanner());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadPreviousGame()
    {

    }

    public void NewGame(string difficulty)
    {
        switch (difficulty)
        {
            case ("Easy"):
                Difficulty = Generator.DifficultyLevels.Easy;
                break;
            case ("Medium"):
                Difficulty = Generator.DifficultyLevels.Medium;
                break;
            case ("Hard"):
                Difficulty = Generator.DifficultyLevels.Hard;
                break;
            case ("Expert"):
                Difficulty = Generator.DifficultyLevels.Expert;
                break;
            default:
                Difficulty = Generator.DifficultyLevels.Easy;
                break;
        }

        SceneManager.LoadScene("Game");
    }

    IEnumerator TriggerBanner()
    {
        yield return new WaitForSeconds(3);
        _ads.ShowBannerAd();
    }

    IEnumerator PuzzleComplete()
    {
        yield return new WaitForEndOfFrame();
        _saveLoad.ClearSave();
        Victory.OpenGroup();
    }

    public void SetSelected(Tile selection)
    {
        if (currentlySelected)
        {
            currentlySelected.backPlate.color = Color.white;
            foreach(CheckGroup g in checkGroups)
            {
                g.RemoveHightlight();
            }
        }
        currentlySelected = selection;
        currentlySelected.backPlate.color = SelectedColor;
    }

    public void PerformCheck()
    {
        bool isComplete = true;
        foreach(CheckGroup g in checkGroups)
        {
            if (!g.IsComplete)
            {
                isComplete = false;
                break;
            }
        }

        if (isComplete) StartCoroutine(PuzzleComplete());
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            if (Ads) _ads.PlayVideoAd();

            checkGroups = GameObject.Find("CheckGroups").GetComponentsInChildren<CheckGroup>();
            Victory = GameObject.Find("Victory").GetComponent<UIGroup>();
        }
    }
}
