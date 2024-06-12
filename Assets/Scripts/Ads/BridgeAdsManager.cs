using UnityEngine;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Platform;
using InstantGamesBridge.Modules.Advertisement;
using System;

public enum AdsType { Banner, Interstitial, Rewarded }
public class BridgeAdsManager : MonoBehaviour
{
    static BridgeAdsManager instance;
    private AdsType adsType;
    
    //subscribe to this event from another class to reward player after ads completes
    public static Action OnRewardedAds_reward;
    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(instance);
        }else 
            Destroy(instance);
        
        Bridge.platform.SendMessage(PlatformMessage.GameReady);
    }
    void Start()
    {
        Initialize();
    }
    void DeviceType()
    {
        var device = Bridge.device.type;
        Debug.Log(device);
    }
    void Initialize()
    {
        Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;
        Bridge.advertisement.rewardedStateChanged += OnRewardedStateChanged;
        Bridge.advertisement.bannerStateChanged += OnBannerStateChanged;
    }
    private void OnRewardedStateChanged(RewardedState state)
    {
        Debug.Log("Rewarded " + state);
        if(state == RewardedState.Rewarded){
            OnRewardedAds_reward?.Invoke();
        }
    }
    private void OnInterstitialStateChanged(InterstitialState state)
    {
        Debug.Log("Interstitial " + state);
    }
    private void OnBannerStateChanged(BannerState state)
    {
        Debug.Log("Banner " + state);
    }
    public static void ShowAds(AdsType type)
    {
        instance.adsType = type;
        if (instance.adsType == AdsType.Interstitial)
        {
            Bridge.advertisement.ShowInterstitial();
        }
        else if (instance.adsType == AdsType.Rewarded)
        {
            Bridge.advertisement.ShowRewarded();
        }
        else if (instance.adsType == AdsType.Banner)
        {
            Bridge.advertisement.ShowBanner();
        }
    }

    public static void HideBanner(){
        Bridge.advertisement.HideBanner();
    }

}
