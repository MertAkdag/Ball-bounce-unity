using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class adsbanneradmob : MonoBehaviour
{
    public static adsbanneradmob instance;

    public BannerView _bannerAd;
    // Test Banner Id si 
    private string _bannerAdId = "ca-app-pub-8095700878802270/5319799422";

    public void Start()
    {
        showBannerAd();
        MobileAds.Initialize(initStatus => { });
    }

    // BANNERAD START
    public void showBannerAd()
    {
        requestBannerAd();

        // Yüklenen banner reklamını göstermek için aşağıdaki kodu kullanıyoruz.
        _bannerAd.Show();
    }

    public void requestBannerAd()
    {
        _bannerAd = new BannerView(_bannerAdId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();

        // Burada banner reklamımızın AdMobdan yüklüyoruz ve göstermek için hazır hale getiriyoruz.
        _bannerAd.LoadAd(adRequest);
    }
    // BANNERAD END
}
