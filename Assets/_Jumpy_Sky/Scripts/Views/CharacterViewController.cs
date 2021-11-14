using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CBGames;

public class CharacterViewController : MonoBehaviour
{
    [SerializeField] private RectTransform topBarTrans = null;
    [SerializeField] private Text totalCoinsTxt = null;
    [SerializeField] private ScrollRect characterItemScroll = null;
    [SerializeField] private GameObject unlockBtn = null;
    [SerializeField] private Text unlockPriceTxt = null;
    [SerializeField] private GameObject selectBtn = null;
    [SerializeField] private GameObject characterItemPrefab = null;

    private List<CharacterInforController> listCharacterInforControl = new List<CharacterInforController>();



    private Dictionary<string, CharacterItemController> dicCharacterItemControl = new Dictionary<string, CharacterItemController>();
    private CharacterInforController currentCharControl = null;
    public void OnShow()
    {
        //Move UI
        ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);       
    }


    private void OnDisable()
    {
        topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 150f);
    }

    private void Update()
    {
        totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
    }





    public void UnlockBtn()
    {
        Debug.Log(currentCharControl.CharacterPrice);

        if (ServicesManager.Instance.CoinManager.TotalCoins >= currentCharControl.CharacterPrice)
        {
            ViewManager.Instance.PlayClickButtonSound();
            currentCharControl.Unlock();
            unlockBtn.SetActive(false);
            selectBtn.SetActive(true);



            foreach (KeyValuePair<string, CharacterItemController> b in dicCharacterItemControl)
            {
                if (b.Key.Equals(currentCharControl.CharacterName))
                {
                    Debug.Log("Basıldı");

                    b.Value.HandleLockPanel();

                }
            }
        }
    }

    public void SelectBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ServicesManager.Instance.CharacterContainer.SetSelectedCharacterIndex(currentCharControl.SequenceNumber);
        ViewManager.Instance.LoadScene("Home", 0.25f);
    }

    public void BackBtn()
    {
        ViewManager.Instance.PlayClickButtonSound();
        ViewManager.Instance.LoadScene("Home", 0.25f);
    }


    public void CreateCharacterItem(string charName)
    {
        if (!dicCharacterItemControl.ContainsKey(charName)) //The dictionary didn't contain the item
        {
            CharacterItemController characterItemControl = Instantiate(characterItemPrefab).GetComponent<CharacterItemController>();
            characterItemControl.transform.SetParent(characterItemScroll.content);
            characterItemControl.transform.localScale = Vector3.one;
            characterItemControl.OnSetup(charName);
            dicCharacterItemControl.Add(charName, characterItemControl);
        }
    }



    public void HandleOnSelectCharacterItem(CharacterInforController charControl)
    {
        foreach(KeyValuePair<string, CharacterItemController> o in dicCharacterItemControl)
        {
            if (o.Key.Equals(charControl.CharacterName))
            {
                o.Value.OnSelect();
                currentCharControl = charControl;

                //Handle buttons
                if (charControl.IsUnlocked)
                {
                    selectBtn.SetActive(true);
                    unlockBtn.SetActive(false);
                }
                else
                {
                    if (ServicesManager.Instance.CoinManager.TotalCoins >= charControl.CharacterPrice)
                    {
                        unlockBtn.SetActive(true);
                        unlockPriceTxt.text = charControl.CharacterPrice.ToString();
                        selectBtn.SetActive(false);
                    }
                    else
                    {
                        unlockBtn.SetActive(true);
                        unlockPriceTxt.text = charControl.CharacterPrice.ToString();
                        selectBtn.SetActive(false);
                    }

                }
            }
            else
            {
                o.Value.OnDeselect();
            }
        }
    }
}

/*
 * 
 *  
        ViewManager.Instance.PlayClickButtonSound();
        currentCharControl.Unlock();
        unlockBtn.SetActive(false);
        selectBtn.SetActive(true);

        foreach (KeyValuePair<string, CharacterItemController> o in dicCharacterItemControl)
        {
            if (o.Key.Equals(currentCharControl.CharacterName))
            {
                Debug.Log("Basıldı");

                o.Value.HandleLockPanel();

            }
        }

*/
