using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

public class RewardedAdUnit : IAdUnit
{
    readonly string m_id;
    RewardedAd m_ad;
    Action m_onReward;

    // Checks if this Ad is ready
    public bool IsReady => m_ad != null && m_ad.CanShowAd();

    public RewardedAdUnit(string id)
    {
        m_id = id;
    }

    public void Load()
    {
        Destroy();
        RewardedAd.Load(m_id, new AdRequest(), (ad, err) =>
        {
            if (err != null || ad == null)
            {
                Debug.Log("Rewarded failed: " + err);
                return;
            }

            m_ad = ad;
            m_ad.OnAdFullScreenContentClosed += Load; // preload the next one
            m_ad.OnAdFullScreenContentFailed += _ => Load(); // preload the next one
        });
    }

    public void Show() => Show(null);

    public void Show(Action onReward)
    {
        if (!(m_ad != null && m_ad.CanShowAd())) return;
        m_onReward = onReward;
        m_ad.Show(reward =>
        {
            Debug.Log($"Reward earned: {reward.Type} x{reward.Amount}");
            MobileAdsEventExecutor.ExecuteInUpdate(() => m_onReward?.Invoke());
        });
    }

    public void Hide() { }

    public void Destroy()
    {
        m_ad?.Destroy(); m_ad = null;
    }
}