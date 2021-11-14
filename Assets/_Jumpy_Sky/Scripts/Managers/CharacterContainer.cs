using UnityEngine;

public class CharacterContainer : MonoBehaviour
{
    public int SelectedCharacterIndex { get { return PlayerPrefs.GetInt(PlayerPrefsKey.SELECTED_CHARACTER_PPK, 0); } }
    public CharacterInforController[] CharacterInforControllers { get { return characterInforControllers; } }
    [SerializeField] private CharacterInforController[] characterInforControllers = null;
    public void SetSelectedCharacterIndex(int index)
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.SELECTED_CHARACTER_PPK, index);
        PlayerPrefs.Save();
    }

}

