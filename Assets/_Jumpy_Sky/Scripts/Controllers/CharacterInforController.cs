using UnityEngine;
using CBGames;
using System.Collections;
using System.Linq;

public class CharacterInforController : MonoBehaviour
{
    [Header("The name of this character. This field must be different than others")]
    [Header("Character Information Config")]
    [SerializeField] private string characterName = string.Empty;

    [Header("Price of this character")]
    [SerializeField] private int characterPrice = 0;

    [Header("The material's color of this character when it locked.")]
    [SerializeField] private Color lockColor = Color.gray;

    [Header("The material's color of this character when it unlocked.")]
    [SerializeField] private Color unlockColor = Color.white;

    [Header("Character References")]
    [SerializeField] private MeshFilter[] meshFilters = null;
    [SerializeField] private MeshRenderer[] meshRenders = null;


    /// <summary>
    /// The material of this character.
    /// </summary>
    public Material Material { get { return meshRenders[0].sharedMaterial; } }


    /// <summary>
    /// The name of this character.
    /// </summary>
    public string CharacterName { get { return characterName; } }


    /// <summary>
    /// The sequence number of this character in CharacterContainer
    /// </summary>
    public int SequenceNumber { private set; get; }


    /// <summary>
    /// The unlock of price this character.
    /// </summary>
    public int CharacterPrice { get { return characterPrice; } }


    /// <summary>
    /// Is this character unlocked or not.
    /// </summary>
    public bool IsUnlocked { get { return PlayerPrefs.GetInt(characterName, 0) == 1; } }

    public void OnSetup()
    {
        if (!PlayerPrefs.HasKey(characterName))
        {
            //If characterPrice equals to 0 -> set this character to be unlocked
            PlayerPrefs.SetInt(characterName, (characterPrice == 0) ? 1 : 0);
        }
    }

    public MeshFilter GetMeshFilter(int index)
    {
        return meshFilters[index];
    }

    public MeshRenderer GetMeshRender(int index)
    {
        return meshRenders[index];
    }


    /// <summary>
    /// Set the sequence number of this character.
    /// </summary>
    /// <param name="number"></param>
    public void SetSequenceNumber(int number)
    {
        SequenceNumber = number;
    }

    /// <summary>
    /// Unlock this character and remove coins.
    /// </summary>
    /// <returns></returns>
    public void Unlock()
    {
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.unlock);
        ServicesManager.Instance.RewardCoinManager.RemoveTotalCoins(0.2f, characterPrice);
        PlayerPrefs.SetInt(characterName, 1);
        PlayerPrefs.Save();
    }
}
