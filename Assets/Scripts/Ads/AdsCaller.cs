using System;
using UnityEngine;

public class AdsCaller : MonoBehaviour
{
    ///THE PLAYERPOINTS IS JUST FOR DEMO PURPOSES OF REWARDED ADS
    // [SerializeField]private int PlayerPoints = 0;
    public Action RewardGive;

    // Subscribe to the OnRewardedAds_reward event on OnEnable
    private void OnEnable()
    {
        BridgeAdsManager.OnRewardedAds_reward += OnRewardedAdsComplete;
    }

    // Unsubscribe from the OnRewardedAds_reward event on OnDisable
    void OnDisable()
    {
        BridgeAdsManager.OnRewardedAds_reward -= OnRewardedAdsComplete;
    }

    /// YOU ONLY NEED TO REPLACE THIS PART OF THE CODE I.E WHEN THE ADS
    /// ARE SHOWN, IS IT AFTER THE LEVEL COMPLETES OR WHEN USER PRESSES A
    /// PARTICULAR BUTTON AND WHICH ADS SHOULD SHOW UP
    
    /// JUST FOR TEST WHEN Q AND W ARE PRESSED INTERSTITIAL AND REWARDED
    /// ADS ARE SHOWN RESPECTIVELY.
    
    
    // Update loop to check for user input
    // private void Update()
    // {
    //     // If 'Q' is pressed, show interstitial ad
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         ShowInterstitialAds();
    //     }

    //     // If 'W' is pressed, show rewarded ad
    //     if (Input.GetKeyDown(KeyCode.W))
    //     {
    //         ShowRewardedAds();
    //     }
    // }

    // Show an interstitial ad using BridgeAdsManager
    public void ShowInterstitialAds()
    {
        BridgeAdsManager.ShowAds(AdsType.Interstitial);
    }

    // Show a rewarded ad using BridgeAdsManager
    public void ShowRewardedAds()
    {
        BridgeAdsManager.ShowAds(AdsType.Rewarded);
    }
    public void ShowBannerAds()
    {
        BridgeAdsManager.ShowAds(AdsType.Banner);
    }
    // public void HideBanner()
    // {
    //     BridgeAdsManager.HideBanner();
    // }

    // Called when a rewarded ad is completed, increase player points
    //ALSO DETERMINE WHICH REWARD IS GIVEN TO THE PLAYER HERE
    //INCREMENTING THE PLAYERPOINTS HERE IS JUST A DEMO
    private void OnRewardedAdsComplete()
    {
        RewardGive?.Invoke();
    }
}
