using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AdHandler : MonoBehaviour, IUnityAdsListener
{
//#if UNITY_IOS
    //private string gameId = "3994016";
//#elif UNITY_ANDROID
    private string gameId = "3994017";
//#endif

    public bool TestMode = true;
    public string PlacementId;

    private bool isReady = false;


    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize (gameId, TestMode);
        StartCoroutine(TestForInternet());
    }

    IEnumerator TestForInternet()
    {
        using (UnityWebRequest testRequest = UnityWebRequest.Get("https://www.google.com"))
        {
            yield return testRequest.SendWebRequest();

            if(!testRequest.isNetworkError || !testRequest.isHttpError)
            {
                if(testRequest.downloadedBytes > 0)
                {
                    isReady = true;
                    Debug.Log("Verified connection, showing ad");
                } else
                {
                    LoadNext();
                    Debug.Log("No connection, skipping");
                }
            } else
            {
                LoadNext();
                Debug.Log("No connection, skipping");
            }
        }
    }

    void LoadNext()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnUnityAdsReady(string placementId)
    {
        if(placementId == PlacementId)
        {
            Debug.Log("Ad ready");
            if (isReady)
            {
                Advertisement.Show(PlacementId);
            }
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error, skipped");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad started: " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Ad complete, status: " + showResult.ToString());
        if(placementId == PlacementId)
        {
            LoadNext();
        }
    }
}
