using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class Ads : MonoBehaviour, IUnityAdsListener
{
    private static Ads instance;

    private const string ID = "3739541", VIDEO = "rewardedVideo", BANNER = "banner";
    private const bool TEST_MODE = false;

    private int money;
    private Text text;

    public static Ads getInstance()
    {
        if (instance == null)
        {
            instance = new Ads();
            Advertisement.Initialize(ID, TEST_MODE);
        }
        return instance;
    }

    public IEnumerator StartAd(int money, Text text)
    {
        Advertisement.AddListener(this);
        this.money = money;
        this.text = text;

        while (!Advertisement.IsReady(VIDEO))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Show(VIDEO);
    }

    public IEnumerator StartBanner()
    {
        while (!Advertisement.IsReady(BANNER))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BANNER);
    }

    public void DestroyBanner()
    {
        Advertisement.Banner.Hide(false);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // if x3
            // money *= 2;
            text.text = (int.Parse(text.text) + money).ToString();
            PlayerData.getInstance().money += money;
            PlayerData.getInstance().Save();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            text.text = (int.Parse(text.text) + money/2).ToString();
            PlayerData.getInstance().money += money;
            PlayerData.getInstance().Save();
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }

        Advertisement.RemoveListener(this);
    }

    public void OnUnityAdsReady(string placementId) { }
    public void OnUnityAdsDidError(string message) { }
    public void OnUnityAdsDidStart(string placementId) { }

}
  