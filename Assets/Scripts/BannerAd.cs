using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

public class BannerAd : MonoBehaviour, IUnityAdsListener
{
    //#if UNITY_IOS
    //private string gameId = "3994016";
    //#elif UNITY_ANDROID
    private string gameId = "3994017";
    //#endif

    public bool TestMode = true;
    public string PlacementId;

    private bool isReady = false;

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId);
        StartCoroutine(TestForInternet());
    }

    IEnumerator TestForInternet()
    {
        using (UnityWebRequest testRequest = UnityWebRequest.Get("https://www.google.com"))
        {
            yield return testRequest.SendWebRequest();

            if (!testRequest.isNetworkError || !testRequest.isHttpError)
            {
                if (testRequest.downloadedBytes > 0)
                {
                    isReady = true;
                    Debug.Log("Verified connection, showing ad");
                }
            }
        }
    }

    IEnumerator ShowBanner()
    {
        yield return new WaitForSeconds(3);
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show(PlacementId);
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == PlacementId)
        {
            if (isReady)
            {
                StartCoroutine(ShowBanner());
            }
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Issue with ad, banner not shown");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        if(placementId == PlacementId)
        {
            Debug.Log("Showing banner");
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == PlacementId)
        {
            Debug.Log("Ad complete");
        }
    }
}
