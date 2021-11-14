using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using CBGames;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { private set; get; }

    private List<CharacterInforController> listCharacterInforControl = new List<CharacterInforController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }



    private void Start()
    {
        //Load the view
        ViewManager.Instance.OnLoadingSceneDone(SceneManager.GetActiveScene().name);

        //Create characters
        int currentCharacterIndex = Mathf.Clamp(ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex, 0, ServicesManager.Instance.CharacterContainer.CharacterInforControllers.Length - 1);
        for (int i = 0; i < ServicesManager.Instance.CharacterContainer.CharacterInforControllers.Length; i++)
        {
            //Instantiate characters
            GameObject characterPrefab = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[i].gameObject;
            CharacterInforController charInforControl = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity).GetComponent<CharacterInforController>();
            charInforControl.SetSequenceNumber(i);
            charInforControl.OnSetup();
            charInforControl.gameObject.SetActive(false);
            charInforControl.transform.SetParent(transform);
            listCharacterInforControl.Add(charInforControl);

            ViewManager.Instance.CharacterViewController.CreateCharacterItem(charInforControl.CharacterName);
        }

        
        CharacterInforController currentCharInforControl = listCharacterInforControl[currentCharacterIndex];
        currentCharInforControl.gameObject.SetActive(true);
        ViewManager.Instance.CharacterViewController.HandleOnSelectCharacterItem(currentCharInforControl);
    }


    private void Update()
    {
        transform.eulerAngles += Vector3.up * 10f * Time.deltaTime;
    }


    public void OnSelectedCharacter(string charName)
    {
        foreach (CharacterInforController o in listCharacterInforControl)
        {
            if (o.CharacterName.Equals(charName))
            {
                o.gameObject.SetActive(true);
                ViewManager.Instance.CharacterViewController.HandleOnSelectCharacterItem(o);
            }
            else
            {
                o.gameObject.SetActive(false);
            }
        }
    }
}
