using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdUnit : IAdUnit
{
    readonly string m_id;
    InterstitialAd m_ad;

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

            m_ad = ad;
            m_ad.OnAdFullScreenContentClosed += Load;
            m_ad.OnAdFullScreenContentFailed += _ => Load();
        });
    }

    public void Show()
    {
        if (IsReady)
        {
            m_ad.Show();
        }
    }

    public void Hide() { } // not applicable to interstitials

    public void Destroy()
    {
        m_ad?.Destroy();
        m_ad = null;
    }
}