using System.Collections.Generic;
using UnityEngine;
using CBGames;

enum BannerAdType
{
    NONE,
    ADMOB,
    UNITY,
}

enum InterstitialAdType
{
    UNITY,
    ADMOB,
}


enum RewardedAdType
{
    UNITY,
    ADMOB,
}

[System.Serializable]
class InterstitialAdConfig
{
    public IngameState GameStateForShowingAd = IngameState.Ingame_GameOver;
    public int GameStateCountForShowingAd = 3;
    public float ShowAdDelay = 0.2f;
    public List<InterstitialAdType> ListInterstitialAdType = new List<InterstitialAdType>();
}


namespace CBGames
{
    public class AdManager : MonoBehaviour
    {

        [Header("Banner Ad config")]
        [SerializeField] private BannerAdType bannerAdType = BannerAdType.NONE;
        [SerializeField] private float showingBannerAdDelay = 0.5f;


        [Header("Interstitial Ad Config")]
        [SerializeField] private List<InterstitialAdConfig> listShowInterstitialAdConfig = new List<InterstitialAdConfig>();

        [Header("Rewarded Video Ad Config")]
        [SerializeField] private float showingRewardedVideoAdDelay = 0.2f;
        [SerializeField] private List<RewardedAdType> listRewardedAdType = new List<RewardedAdType>();

        [Header("Rewarded Coins Config")]
        [SerializeField] private int minRewardedCoins = 5;
        [SerializeField] private int maxRewardedCoins = 50;
        [SerializeField] private float rewardDelay = 0.2f;


        [Header("AdManager References")]
        [SerializeField] private AdmobController admobController = null;
        [SerializeField] private UnityAdController unityAdController = null;

        private List<int> listShowAdCount = new List<int>();
        private RewardedAdType readyAdType = RewardedAdType.UNITY;

        private bool isCalledback = false;
        private bool isRewarded = false;
        private void OnEnable()
        {
            IngameManager.GameStateChanged += GameManager_GameStateChanged;
        }

        private void OnDisable()
        {
            IngameManager.GameStateChanged -= GameManager_GameStateChanged;
        }

        // Use this for initialization
        void Start()
        {
            foreach (InterstitialAdConfig o in listShowInterstitialAdConfig)
            {
                listShowAdCount.Add(o.GameStateCountForShowingAd);
            }

            //Show banner ad
            if (bannerAdType == BannerAdType.ADMOB)
            {
                admobController.LoadAndShowBanner(showingBannerAdDelay);
            }
            else if (bannerAdType == BannerAdType.UNITY)
            {
                unityAdController.ShowBanner(showingBannerAdDelay);
            }


            //Request interstitial ads (unity ads auto requests interstitial)
            foreach (InterstitialAdConfig o in listShowInterstitialAdConfig)
            {
                foreach (InterstitialAdType a in o.ListInterstitialAdType)
                {
                    if (a == InterstitialAdType.ADMOB)
                    {
                        admobController.RequestInterstitial();
                    }
                }
            }

            //Request rewarded video (unity ads auto requests rewarded video)
            foreach (RewardedAdType o in listRewardedAdType)
            {
                if (o == RewardedAdType.ADMOB)
                {
                    admobController.RequestRewardedVideo();
                }
            }
        }

        private void Update()
        {
            if (isCalledback)
            {
                isCalledback = false;
                if (isRewarded)
                {
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Home")) //In Home scene
                    {
                        //Reward coins
                        int coins = Random.Range(minRewardedCoins, maxRewardedCoins) / 5 * 5;
                        ServicesManager.Instance.RewardCoinManager.RewardTotalCoins(coins, rewardDelay);
                    }
                    else
                    {
                        if (IngameManager.Instance.IngameState == IngameState.Ingame_Revive)
                        {
                            IngameManager.Instance.SetContinueGame(); //Revive 
                        }
                        else
                        {
                            //Reward collected coins
                            int collectedCoins = ServicesManager.Instance.CoinManager.CollectedCoins;
                            ServicesManager.Instance.RewardCoinManager.RewardCollectedCoins(collectedCoins, rewardDelay);
                        }
                    }
                }
                else
                {
                    if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("Ingame"))
                    {
                        if (IngameManager.Instance.IngameState == IngameState.Ingame_Revive)
                            IngameManager.Instance.GameOver();
                    }
                }
            }
        }


        private void GameManager_GameStateChanged(IngameState obj)
        {
            for (int i = 0; i < listShowAdCount.Count; i++)
            {
                if (listShowInterstitialAdConfig[i].GameStateForShowingAd == obj)
                {
                    listShowAdCount[i]--;
                    if (listShowAdCount[i] <= 0)
                    {
                        //Reset gameCount 
                        listShowAdCount[i] = listShowInterstitialAdConfig[i].GameStateCountForShowingAd;

                        for (int a = 0; a < listShowInterstitialAdConfig[i].ListInterstitialAdType.Count; a++)
                        {
                            InterstitialAdType type = listShowInterstitialAdConfig[i].ListInterstitialAdType[a];
                            if (type == InterstitialAdType.ADMOB && admobController.IsInterstitialReady())
                            {
                                admobController.ShowInterstitial(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                            else if (type == InterstitialAdType.UNITY && unityAdController.IsInterstitialReady())
                            {
                                unityAdController.ShowInterstitial(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Determines whether rewarded video ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedVideoAdReady()
        {
            for (int i = 0; i < listRewardedAdType.Count; i++)
            {
                if (listRewardedAdType[i] == RewardedAdType.UNITY && unityAdController.IsRewardedVideoReady())
                {
                    readyAdType = RewardedAdType.UNITY;
                    return true;
                }
                else if (listRewardedAdType[i] == RewardedAdType.ADMOB && admobController.IsRewardedVideoReady())
                {
                    readyAdType = RewardedAdType.ADMOB;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Show the rewarded video ad with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedVideoAd()
        {
            if (readyAdType == RewardedAdType.UNITY)
            {
                unityAdController.ShowRewardedVideo(showingRewardedVideoAdDelay);
            }
            else if (readyAdType == RewardedAdType.ADMOB)
            {
                admobController.ShowRewardedVideo(showingRewardedVideoAdDelay);
            }
        }

        public void OnRewardedVideoClosed(bool isFinishedVideo)
        {
            isCalledback = true;
            isRewarded = isFinishedVideo;
        }
    }
}
