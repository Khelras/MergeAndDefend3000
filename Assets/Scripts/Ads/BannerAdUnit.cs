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

    public void Load()
    {
        Destroy();
        m_view = new BannerView(m_id, AdSize.Banner, AdPosition.Bottom);
        m_view.LoadAd(new AdRequest());
        m_view.Hide(); // Hidden until a Menu asks for it
    }

    public void Show()
    {
        m_view?.Show();
    }

    public void Hide()
    {
        m_view?.Hide();
    }

    public void Destroy()
    {
        m_view?.Destroy();
        m_view = null;
    }
}