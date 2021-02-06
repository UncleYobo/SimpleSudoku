using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public bool Ads;
    private AdHandler _ads;

    public void OnNewGameStarted()
    {
        StartCoroutine(TriggerBanner());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void NewGame(string difficulty)
    {
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
