using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CBGames;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { private set; get; }

    [SerializeField] private ItemController coinControlPrefab = null;
    [SerializeField] private List<PlatformSelectionData> listPlatformSelectionData = new List<PlatformSelectionData>();

    private List<ItemController> listCoinControl = new List<ItemController>();
    private List<PlatformController> listPlatformControl = new List<PlatformController>();
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


    /// <summary>
    /// Get an inactive coin object.
    /// </summary>
    /// <returns></returns>
    public ItemController GetCoinControl()
    {
        //Find in the list
        ItemController coinControl = listCoinControl.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if(coinControl == null)
        {
            //Did not find one -> create new one
            coinControl = Instantiate(coinControlPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<ItemController>();
            coinControl.gameObject.SetActive(false);
            listCoinControl.Add(coinControl);
        }

        return coinControl;
    }




    /// <summary>
    /// Get an inactive PlatformController object.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public PlatformController GetPlatformControl(PlatformType type, PlatformSize size)
    {
        //Find in the list
        PlatformController platformControl = listPlatformControl.Where(a => !a.gameObject.activeInHierarchy && (a.PlatformType == type && a.PlatformSize == size)).FirstOrDefault();

        if (platformControl == null)
        {
            //Didn't find one -> create new one
            GameObject prefab = null;
            foreach(PlatformSelectionData o in listPlatformSelectionData)
            {
                if (o.PlatformType == type)
                {
                    prefab = o.ListPlatformControl.Where(a => a.PlatformSize == size).FirstOrDefault().gameObject;
                    break;
                }
            }

            platformControl = Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<PlatformController>();
            platformControl.gameObject.SetActive(false);
            listPlatformControl.Add(platformControl);
        }

        return platformControl;
    }
    
}
