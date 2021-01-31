using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Networking;

public class AdHandler : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    //private string gameId = "3994016";
#elif UNITY_ANDROID
    private string gameId = "3994017";
#endif

    public bool TestMode = true;
    public string VideoPlacementId;
    public string BannerPlacementId;

    private bool hasConnection = false;
    private bool videoReady = false;
    private bool bannerReady = false;
    private GameplayManager _mgmt;

    private void Start()
    {
        _mgmt = GetComponent<GameplayManager>();
        Advertisement.AddListener(this);
        Advertisement.Initialize (gameId, TestMode);
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
                    hasConnection = true;
                }
            }
        }
    }

    public void PlayVideoAd()
    {
        if (videoReady && hasConnection)
        {
            Advertisement.Banner.Hide();
            Advertisement.Show(VideoPlacementId);
        }
    }
    public void ShowBannerAd()
    {
        if (bannerReady && hasConnection && !Advertisement.isShowing)
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(BannerPlacementId);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        if(placementId == VideoPlacementId)
        {
            videoReady = true;
        } else if(placementId == BannerPlacementId)
        {
            bannerReady = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if(placementId == VideoPlacementId)
        {
            _mgmt.OnNewGameStarted();
        }
    }
}
