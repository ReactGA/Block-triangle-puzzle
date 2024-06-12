using UnityEngine;
// using CrazyGames;
using System;

public class AdsManager : MonoBehaviour
{
    static AdsManager Instance;
    // public static Action ON_MIDWARE_ADS_FINISHED,ON_MIDWARE_ADS_ERROR;
    // public static Action ON_REWARDED_ADS_FINISHED,ON_REWARDED_ADS_ERROR;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }else
            Destroy(this);
    }

    public static void ShowMidWareAds(){
        // CrazyAds.Instance.beginAdBreak(Instance.OnMidAdsFinised,Instance.OnMidAdsError);
    }
    public static void ShowRewardedAds(){
        // CrazyAds.Instance.beginAdBreak(Instance.OnRewardedAdsFinised,Instance.OnRewardedAdsError);
    }

    void OnMidAdsFinised(){
        // ON_MIDWARE_ADS_FINISHED?.Invoke();
    }
    void OnMidAdsError(){
        // ON_MIDWARE_ADS_ERROR?.Invoke();
    }
    void OnRewardedAdsFinised(){
        // ON_REWARDED_ADS_FINISHED?.Invoke();
    }
    void OnRewardedAdsError(){
        // ON_REWARDED_ADS_ERROR?.Invoke();
    }
}
