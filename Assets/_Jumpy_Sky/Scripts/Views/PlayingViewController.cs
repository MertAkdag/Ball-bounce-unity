using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PlayingViewController : MonoBehaviour {

    [SerializeField] private RectTransform leftBarTrans = null;
    [SerializeField] private Image levelProgressFilterImg = null;
    [SerializeField] private RectTransform currentLevelTxtTrans = null;
    [SerializeField] private Text currentLevelTxt = null;


    public void OnShow()
    {
        ViewManager.Instance.MoveRect(leftBarTrans, leftBarTrans.anchoredPosition, new Vector2(5, leftBarTrans.anchoredPosition.y), 0.5f);
        ViewManager.Instance.MoveRect(currentLevelTxtTrans, currentLevelTxtTrans.anchoredPosition, new Vector2(currentLevelTxtTrans.anchoredPosition.x, 0), 0.5f);

        currentLevelTxt.text = "LEVEL: " + IngameManager.Instance.CurrentLevel.ToString();
    }

    private void OnDisable()
    {        
        leftBarTrans.anchoredPosition = new Vector2(-60, leftBarTrans.anchoredPosition.y);
        currentLevelTxtTrans.anchoredPosition = new Vector2(currentLevelTxtTrans.anchoredPosition.x, 60);
    }




    /// <summary>
    /// Update the level progress UI.
    /// </summary>
    /// <param name="currentPassedPlatform"></param>
    /// <param name="totalPlatform"></param>
    public void UpdateLevelProgressUI(int currentPassedPlatform, int totalPlatform)
    {
        StartCoroutine(CRUpdatingLevelProgress(currentPassedPlatform / (float)totalPlatform));
    }

    private IEnumerator CRUpdatingLevelProgress(float newAmount)
    {
        float t = 0;
        float updatingTime = 0.1f;
        float currentAmount = levelProgressFilterImg.fillAmount;
        while (t < updatingTime)
        {
            t += Time.deltaTime;
            float factor = t / updatingTime;
            levelProgressFilterImg.fillAmount = Mathf.Lerp(currentAmount, newAmount, factor);
            yield return null;
        }
    }

    
}
