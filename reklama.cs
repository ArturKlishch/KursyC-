using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class reklama : MonoBehaviour
{
    private RewardedAd rewardedad;
    string adUnitID;
    public void Start()
    {
#if UNITY_ANDROID
          adUnitID = "ca-app-pub-6208002239082055/5287258497";
#elif UNITY_IPHONE
adUnitID = "unexpected_platform";
#endif
        this.rewardedad = new RewardedAd(adUnitID);
        this rewardedad.OnAdLoaded += HandleRewardedADLoaded;
        this.rewardedad.OnAdFailedToLoad += HandleRewardedADFailedToLoad;
        this.rewardedad.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedad.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedad.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedad.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedad.LoadAd(request);
    }
    public void HandleRewardedADLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedADLoaded event had received");
    }
    public void HandleRewardedADFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print("HandleRewardedADFailedToLoad event had received with massage " + args.Message);
    }
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event had received");

    }
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdFailedToShow event had received with massag" + args.Message);
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        CreateAndLoadRewardedAD();
    }
    public void HandleUserEarnedReward (object sender, Reward args)
    {
        wasADWached = true;
    }
    public void WatchAD()
    {
        if (this.rewardedad.IsLoaded())
        {
            this.rewardedad.Show();
        }
    }


}
