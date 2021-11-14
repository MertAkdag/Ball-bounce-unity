using CBGames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class HomeViewController : MonoBehaviour {

    [SerializeField] private RectTransform topBarTrans = null;
    [SerializeField] private RectTransform gameNameTrans = null;
    [SerializeField] private RectTransform playButtonTrans = null;
    [SerializeField] private RectTransform rewardBtnTrans = null;
    [SerializeField] private RectTransform characterBtnTrans = null;
    [SerializeField] private RectTransform settingBtnTrans = null;
    [SerializeField] private RectTransform rateAppBtnTrans = null;
    [SerializeField] private RectTransform soundButtonsTrans = null;
    [SerializeField] private RectTransform musicButtonsTrans = null;
    [SerializeField] private RectTransform nativeShareBtnTrans = null;
    [SerializeField] private RectTransform leaderboardBtnTrans = null;
    [SerializeField] private RectTransform facebookShareBtnTrans = null;
    [SerializeField] private GameObject soundOnBtn = null;
    [SerializeField] private GameObject soundOffBtn = null;
    [SerializeField] private GameObject musicOnBtn = null;
    [SerializeField] private GameObject musicOffBtn = null;
    [SerializeField] private Text currentLevelTxt = null;
    [SerializeField] private Text totalCoinsTxt = null;
    [SerializeField] private DailyRewardViewController dailyRewardViewControl = null;
    [SerializeField] private LeaderboardViewController leaderboardViewController = null;
    [SerializeField] private RectTransform gizlilikButon = null;
    [SerializeField] private RectTransform removeadsbutton = null;


    private int settingButtonTurn = 1;
    public void OnShow()
    {
        ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);
        ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 1f);
        StartCoroutine(CRShowingBottomButtons());

        dailyRewardViewControl.gameObject.SetActive(false);
        leaderboardViewController.gameObject.SetActive(false);
        settingButtonTurn = 1;

        currentLevelTxt.text = "LEVEL: " + PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK, 1).ToString();

        //Update sound btns
        if (ServicesManager.Instance.SoundManager.IsSoundOff())
        {
            soundOnBtn.gameObject.SetActive(false);
            soundOffBtn.gameObject.SetActive(true);
        }
        else
        {
            soundOnBtn.gameObject.SetActive(true);
            soundOffBtn.gameObject.SetActive(false);
        }

        //Update music btns
        if (ServicesManager.Instance.SoundManager.IsMusicOff())
        {
            musicOffBtn.gameObject.SetActive(true);
            musicOnBtn.gameObject.SetActive(false);
        }
        else
        {
            musicOffBtn.gameObject.SetActive(false);
            musicOnBtn.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
    }


    private void OnDisable()
    {
        topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 100);
        gameNameTrans.localScale = Vector2.zero;

        playButtonTrans.localScale = Vector3.zero;
        rewardBtnTrans.anchoredPosition = new Vector2(rewardBtnTrans.anchoredPosition.x, -200);
        settingBtnTrans.anchoredPosition = new Vector2(settingBtnTrans.anchoredPosition.x, -200);
        characterBtnTrans.anchoredPosition = new Vector2(characterBtnTrans.anchoredPosition.x, -200);

        soundButtonsTrans.anchoredPosition = new Vector2(-150, soundButtonsTrans.anchoredPosition.y);
        musicButtonsTrans.anchoredPosition = new Vector2(-150, musicButtonsTrans.anchoredPosition.y);
        nativeShareBtnTrans.anchoredPosition = new Vector2(150, nativeShareBtnTrans.anchoredPosition.y);
        leaderboardBtnTrans.anchoredPosition = new Vector2(150, leaderboardBtnTrans.anchoredPosition.y);
        facebookShareBtnTrans.anchoredPosition = new Vector2(150, facebookShareBtnTrans.anchoredPosition.y);
    }



    /// <summary>
    /// Handle the Home view when Dailyreward view or Environment view closes.
    /// </summary>
    public void OnSubViewClose()
    {
        ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 1f);
        StartCoroutine(CRShowingBottomButtons());
    }


    private IEnumerator CRShowingBottomButtons()
    {
        ViewManager.Instance.ScaleRect(playButtonTrans, Vector2.zero, Vector2.one, 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(rewardBtnTrans, rewardBtnTrans.anchoredPosition, new Vector2(rewardBtnTrans.anchoredPosition.x, 150), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, 150), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(settingBtnTrans, settingBtnTrans.anchoredPosition, new Vector2(settingBtnTrans.anchoredPosition.x, 150), 0.5f);
    }


    //////////////////////////////////////////////////////////////////////UI Functions


    public void PlayBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Ingame", 0.25f);
    }


    public void RewardBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        StartCoroutine(CRHandleRewardBtn());
    }
    private IEnumerator CRHandleRewardBtn()
    {
        ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.one, Vector2.zero, 0.5f);
        ViewManager.Instance.ScaleRect(playButtonTrans, Vector2.one, Vector2.zero, 0.5f);
        ViewManager.Instance.MoveRect(rewardBtnTrans, rewardBtnTrans.anchoredPosition, new Vector2(rewardBtnTrans.anchoredPosition.x, -200), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, -200), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(settingBtnTrans, settingBtnTrans.anchoredPosition, new Vector2(settingBtnTrans.anchoredPosition.x, -200), 0.5f);
        yield return new WaitForSeconds(0.5f);
        dailyRewardViewControl.gameObject.SetActive(true);
        dailyRewardViewControl.OnShow();
    }


    public void CharacterBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Character", 0.3f);
    }


    public void NativeShareBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.ShareManager.NativeShare();
    }

    public void SettingBtn()
    {
        settingButtonTurn *= -1;
        StartCoroutine(CRHandleSettingBtn());
    }
    private IEnumerator CRHandleSettingBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        if (settingButtonTurn == -1)
        {
            ViewManager.Instance.MoveRect(rateAppBtnTrans, rateAppBtnTrans.anchoredPosition, new Vector2(0, rateAppBtnTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(nativeShareBtnTrans, nativeShareBtnTrans.anchoredPosition, new Vector2(0, nativeShareBtnTrans.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);

            ViewManager.Instance.MoveRect(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(0, soundButtonsTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(leaderboardBtnTrans, leaderboardBtnTrans.anchoredPosition, new Vector2(0, leaderboardBtnTrans.anchoredPosition.y), 0.5f);


            yield return new WaitForSeconds(0.08f);

            ViewManager.Instance.MoveRect(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(0, musicButtonsTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(facebookShareBtnTrans, facebookShareBtnTrans.anchoredPosition, new Vector2(0, facebookShareBtnTrans.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);
             ViewManager.Instance.MoveRect(gizlilikButon, gizlilikButon.anchoredPosition, new Vector2(0, gizlilikButon.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);
            ViewManager.Instance.MoveRect(removeadsbutton, removeadsbutton.anchoredPosition, new Vector2(0, removeadsbutton.anchoredPosition.y), 0.5f);


        }
        else
        {
            ViewManager.Instance.MoveRect(rateAppBtnTrans, rateAppBtnTrans.anchoredPosition, new Vector2(150, rateAppBtnTrans.anchoredPosition.y), 0.5f);

            ViewManager.Instance.MoveRect(nativeShareBtnTrans, nativeShareBtnTrans.anchoredPosition, new Vector2(150, nativeShareBtnTrans.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);

            ViewManager.Instance.MoveRect(soundButtonsTrans, soundButtonsTrans.anchoredPosition, new Vector2(-150, soundButtonsTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(leaderboardBtnTrans, leaderboardBtnTrans.anchoredPosition, new Vector2(150, leaderboardBtnTrans.anchoredPosition.y), 0.5f);
            yield return new WaitForSeconds(0.08f);

            ViewManager.Instance.MoveRect(musicButtonsTrans, musicButtonsTrans.anchoredPosition, new Vector2(-150, musicButtonsTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(facebookShareBtnTrans, facebookShareBtnTrans.anchoredPosition, new Vector2(150, facebookShareBtnTrans.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);
            ViewManager.Instance.MoveRect(gizlilikButon, gizlilikButon.anchoredPosition, new Vector2(150, gizlilikButon.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);
            ViewManager.Instance.MoveRect(removeadsbutton, removeadsbutton.anchoredPosition, new Vector2(-150, removeadsbutton.anchoredPosition.y), 0.5f);

            yield return new WaitForSeconds(0.08f);



        }
    }


    public void ToggleSound()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.SoundManager.ToggleSound();
        if (ServicesManager.Instance.SoundManager.IsSoundOff())
        {
            soundOnBtn.gameObject.SetActive(false);
            soundOffBtn.gameObject.SetActive(true);
        }
        else
        {
            soundOnBtn.gameObject.SetActive(true);
            soundOffBtn.gameObject.SetActive(false);
        }
    }

    public void ToggleMusic()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.SoundManager.ToggleMusic();
        if (ServicesManager.Instance.SoundManager.IsMusicOff())
        {
            musicOffBtn.gameObject.SetActive(true);
            musicOnBtn.gameObject.SetActive(false);
        }
        else
        {
            musicOffBtn.gameObject.SetActive(false);
            musicOnBtn.gameObject.SetActive(true);
        }
    }

    public void FacebookShareBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.ShareManager.FacebookShare();
    }
    public void LeaderboardBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        leaderboardViewController.gameObject.SetActive(true);
        leaderboardViewController.OnShow();
    }
    public void RateAppBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        Application.OpenURL(ServicesManager.Instance.ShareManager.AppUrl);
    }
    public void CloseSettingViewBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
    }

    public void gizlilikBtn()
    {
        Application.OpenURL("https://squrrielgames.blogspot.com/2021/11/privacy-policy.html");

    }


    //public GameObject removeadsbuton;
    //public void removeads()
    //{

    //    if (removeadsbuton != null)
    //    {
    //        Animator animator = removeadsbuton.GetComponent<Animator>();
    //        if(animator != null)
    //        {
    //            bool isOpen = animator.GetBool("open");

    //            animator.SetBool("open", !isOpen);

    //        }


    //    }
    //}


}
