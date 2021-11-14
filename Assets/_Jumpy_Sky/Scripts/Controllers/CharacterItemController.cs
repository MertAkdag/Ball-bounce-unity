using UnityEngine;
using UnityEngine.UI;

public class CharacterItemController : MonoBehaviour
{
    [SerializeField] private Image characterItemRoundImg = null;
    [SerializeField] private Image characterImg = null;
    [SerializeField] private RectTransform blackPanelTrans = null;

    private string charName = string.Empty;
    public void OnSetup(string characterName)
    {
        charName = characterName;
        characterItemRoundImg.color = Color.white;
        Sprite charSp = Resources.Load(characterName, typeof(Sprite)) as Sprite;
        characterImg.overrideSprite = charSp;  
        HandleLockPanel();
    }


    public void OnClick()
    {
        ViewManager.Instance.PlayClickButtonSound();
        CharacterManager.Instance.OnSelectedCharacter(charName);
    }


    public void HandleLockPanel()
    {
        if (PlayerPrefs.GetInt(charName) == 0)
        {
            blackPanelTrans.SetAsLastSibling();
        }
        else
        {
            blackPanelTrans.SetAsFirstSibling();
        }
    }


    public void OnSelect()
    {
        characterItemRoundImg.color = Color.white;
    }

    public void OnDeselect()
    {
        characterItemRoundImg.color = Color.green;
    }
}
