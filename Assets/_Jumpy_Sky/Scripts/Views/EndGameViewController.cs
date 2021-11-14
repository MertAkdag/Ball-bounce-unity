using CBGames;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameViewController : MonoBehaviour {

    [SerializeField] private RectTransform topBarTrans = null;
    [SerializeField] private Text nextLevelTxt = null;
    [SerializeField] private Text totalCoinsTxt = null;
    [SerializeField] private Text levelResultTxt = null;
    [SerializeField] private CanvasGroup midBarCanvasGroup = null;
    [SerializeField] private Text collectedCoinsTxt = null;
    [SerializeField] private GameObject doubleCoinsBtn = null;
    [SerializeField] private RectTransform playBtnTrans = null;
    [SerializeField] private Text playTxt = null;
    [SerializeField] private RectTransform shareBtnTrans = null;
    [SerializeField] private RectTransform characterBtnTrans = null;
    [SerializeField] private RectTransform homeBtnTrans = null;

    public void OnShow()
    {
        if (IngameManager.Instance.IngameState == IngameState.Ingame_CompletedLevel)
        {
            levelResultTxt.text = "LEVEL COMPLETED !";
            levelResultTxt.color = Color.green;
            playTxt.text = "CONTINUE";
        }
        else if (IngameManager.Instance.IngameState == IngameState.Ingame_GameOver)
        {
            levelResultTxt.text = "LEVEL FAILED !";
            levelResultTxt.color = Color.red;
            playTxt.text = "REPLAY";
        }

        ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);
        ViewManager.Instance.ScaleRect(levelResultTxt.rectTransform, Vector2.zero, Vector2.one, 0.75f);
        StartCoroutine(CRShowBottomBtns());
        nextLevelTxt.text = "NEXT LEVEL: " + PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK);

        if (ServicesManager.Instance.CoinManager.CollectedCoins > 0)
        {
            midBarCanvasGroup.gameObject.SetActive(true);
            doubleCoinsBtn.SetActive(ServicesManager.Instance.AdManager.IsRewardedVideoAdReady());
            StartCoroutine(CRShowMidBar());
        }
        else
        {
            midBarCanvasGroup.gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        collectedCoinsTxt.text = ServicesManager.Instance.CoinManager.CollectedCoins.ToString();
    }

    private void OnDisable()
    {
        topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 200);
        levelResultTxt.rectTransform.localScale = Vector2.zero;
        midBarCanvasGroup.alpha = 0f;

        playBtnTrans.localScale = Vector3.zero;
        shareBtnTrans.anchoredPosition = new Vector2(shareBtnTrans.anchoredPosition.x, -200);
        characterBtnTrans.anchoredPosition = new Vector2(characterBtnTrans.anchoredPosition.x, -200);
        homeBtnTrans.anchoredPosition = new Vector2(homeBtnTrans.anchoredPosition.x, -200);
    }


    private IEnumerator CRShowBottomBtns()
    {
        ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.zero, Vector2.one, 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(shareBtnTrans, shareBtnTrans.anchoredPosition, new Vector2(shareBtnTrans.anchoredPosition.x, 150), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, 150), 0.5f);
        yield return new WaitForSeconds(0.15f);
        ViewManager.Instance.MoveRect(homeBtnTrans, homeBtnTrans.anchoredPosition, new Vector2(homeBtnTrans.anchoredPosition.x, 150), 0.5f);
    }


    private IEnumerator CRShowMidBar()
    {
        float t = 0;
        float fadingTime = 0.5f;
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            midBarCanvasGroup.alpha = Mathf.Lerp(0f, 1f, factor);
            yield return null;
        }
    }


    public void ClaimBtn()
    {
        Debug.Log(ServicesManager.Instance.CoinManager.CollectedCoins);
        ViewManager.Instance.PlayClickButtonSound();
        midBarCanvasGroup.gameObject.SetActive(false);
        ServicesManager.Instance.RewardCoinManager.RewardTotalCoins(ServicesManager.Instance.CoinManager.CollectedCoins, 0.3f);
    }

    public void DoubleCoinsBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        doubleCoinsBtn.SetActive(false);
        ServicesManager.Instance.AdManager.ShowRewardedVideoAd();
    }

    public void PlayBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Ingame", 0.3f);
    }

    public void ShareBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.ShareManager.NativeShare();
    }

    public void CharacterBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Character", 0.3f);
    }

    public void HomeBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Home", 0.3f);
    }
}
