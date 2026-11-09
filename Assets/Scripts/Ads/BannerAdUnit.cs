using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdUnit : IAdUnit
{
    readonly string m_id;
    BannerView m_view;

    public BannerAdUnit(string id)
    {
        m_id = id;
    }

    public void Load() { } // Nothing to preload as the banner is built on Show

    public void Show()
    {
        // Ad is already Showing
        if (m_view != null) return;

        // Size of the Ad
        AdSize size = AdSize.Banner;

#if UNITY_EDITOR
        // Unity Editor or Non-Android/iOS platforms
        m_view = new BannerView(m_id, size, AdPosition.Bottom);
#else
        float scale = MobileAds.Utils.GetDeviceScale();
        if (scale <= 0f) scale = Screen.dpi > 0f ? Screen.dpi / 160f : 1f;

        // Centre horizontally inside the safe area
        Rect safe = Screen.safeArea;
        int x = Mathf.RoundToInt((safe.x + safe.width * 0.5f) / scale - size.Width * 0.5f);

        // Bottom edge of the safe area, converted to a top-left y origin
        float safeBottomFromTop = Screen.height - safe.y;
        int y = Mathf.RoundToInt(safeBottomFromTop / scale - size.Height);

        m_view = new BannerView(m_id, size, x, y);
#endif

        m_view.LoadAd(new AdRequest());
    }

    public void Hide()
    {
        Destroy();
    }

    public void Destroy()
    {
        m_view?.Destroy();
        m_view = null;
    }
}