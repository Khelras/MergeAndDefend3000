using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdUnit : IAdUnit
{
    readonly string m_id;
    InterstitialAd m_ad;
    Action m_onFinished; // Finished Action Callback

    // Checks if this Ad is ready
    public bool IsReady => m_ad != null && m_ad.CanShowAd();

    public InterstitialAdUnit(string id)
    {
        m_id = id;
    }

    public void Load()
    {
        Destroy();
        InterstitialAd.Load(m_id, new AdRequest(), (ad, err) =>
        {
            if (ad == null || err != null)
            {
                Debug.Log("Interstitial failed: " + err);
                return;
            }

            Debug.Log("[Interstitial] Loaded");
            m_ad = ad;
            m_ad.OnAdFullScreenContentClosed += Load;
            m_ad.OnAdFullScreenContentFailed += _ => Load();
        });
    }

    public void Show() => Show(null);

    public void Show(Action onFinished)
    {
        if (!IsReady)
        {
            Debug.Log("[Interstitial] Not ready");
            onFinished?.Invoke(); // Never block the player
            return;
        }

        m_onFinished = onFinished;
        m_ad.Show();
    }

    void Finish()
    {
        Debug.Log("[Interstitial] Closed / failed, finishing");
        var callback = m_onFinished;
        m_onFinished = null;

        Load();
        callback?.Invoke();
    }

    public void Hide() { } // not applicable to interstitials

    public void Destroy()
    {
        m_ad?.Destroy();
        m_ad = null;
    }
}