using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CBGames;
using UnityEngine.SceneManagement;

public class IngameManager : MonoBehaviour
{

    public static IngameManager Instance { private set; get; }
    public static event System.Action<IngameState> GameStateChanged = delegate { };
    public IngameState IngameState
    {
        get
        {
            return ingameState;
        }
        private set
        {
            if (value != ingameState)
            {
                ingameState = value;
                GameStateChanged(ingameState);
            }
        }
    }


    [Header("Enter a number of level to test. Set back to 0 to disable this feature.")]
    [SerializeField] private int testingLevel = 0;



    [Header("Ingame Config")]
    [SerializeField] private float reviveWaitTime = 5f;
    [SerializeField] private int startPlatforms = 10;

    [Header("Levels Config")]
    [SerializeField] private List<LevelConfig> listLevelConfig = null;

    [Header("Ingame References")]
    [SerializeField] private Material backgroundMaterial = null;
    [SerializeField] private Transform completedLevelEffectsTrans = null;
    [SerializeField] private ParticleSystem[] completedLevelEffects = null;


    public float ReviveWaitTime { get { return reviveWaitTime; } }
    public int CurrentLevel { private set; get; }
    public bool IsRevived { private set; get; }



    private IngameState ingameState = IngameState.Ingame_GameOver;
    private List<PlatformCreationData> listPlatformCreationData = new List<PlatformCreationData>();
    private List<PlatformController> listPlatformControl = new List<PlatformController>();
    private List<PlatformColorsConfig> listPlatformColorsConfig = new List<PlatformColorsConfig>();

    private SoundClip background = null;
    private int dataIndex = 0;
    private int jumpSteps = 0;
    private int stepCount = 0;
    private int currentPlatformYAxis = -1;
    private int platformColorConfigIndex = 0;
    private int totalPlatform = 0;
    private int currentPassedPlatform = 0;

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

    // Use this for initialization
    private void Start()
    {
        Application.targetFrameRate = 60;
        ViewManager.Instance.OnLoadingSceneDone(SceneManager.GetActiveScene().name);
        ServicesManager.Instance.CoinManager.ResetCollectedCoins();

        //Setup variables
        IsRevived = false;
        completedLevelEffectsTrans.gameObject.SetActive(false);

        //Set current level
        if (!PlayerPrefs.HasKey(PlayerPrefsKey.SAVED_LEVEL_PPK))
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.SAVED_LEVEL_PPK, 1);
        }

        //Load parameters
        CurrentLevel = (testingLevel != 0) ? testingLevel : PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK);
        foreach (LevelConfig o in listLevelConfig)
        {
            if (o.MinLevel <= CurrentLevel && CurrentLevel < o.MaxLevel)
            {
                //Setup parameters
                jumpSteps = o.JumpSteps;
                backgroundMaterial.SetColor("_TopColor", o.BackgroundTopColor);
                backgroundMaterial.SetColor("_BottomColor", o.BackgroundBottomColor);
                listPlatformColorsConfig = o.ListPlatformColorsConfig;
                background = o.MusicClip;

                //Random elements
                List<int> listTemp = new List<int>();
                while (listTemp.Count < o.ListPlatformConfig.Count)
                {
                    int index = Random.Range(0, o.ListPlatformConfig.Count);
                    if (!listTemp.Contains(index))
                    {
                        listTemp.Add(index);
                    }
                }

                //Create the list of PlatformCreationData
                for (int i = 0; i < listTemp.Count; i++)
                {
                    PlatformConfig config = o.ListPlatformConfig[listTemp[i]];
                    int platformNumber = Random.Range(config.MinPlatform, config.MaxPlatform);
                    for (int a = 0; a <= platformNumber; a++)
                    {
                        PlatformCreationData data = new PlatformCreationData();
                        data.SetPlatformType(config.PlatformType);
                        data.SetPlatformSize(config.PlatformSize);
                        data.SetPlatformDistance(config.PlatformDistance);
                        data.SetXAxisDevitation(Random.Range(config.MinXAxisDevitation, config.MaxXAxisDevitation));
                        data.SetCoinFrequency(config.CoinFrequency);
                        data.SetCoinNumber(Random.Range(config.MinCoinNumber, config.MaxCoinNumber));
                        PlatformActionData platformActionData = new PlatformActionData();
                        platformActionData.SetMovingPlatformFrequency(config.MovingPlatformFrequency);
                        platformActionData.SetMovingLeftDistance(config.MovingLeftDistance);
                        platformActionData.SetMovingRightDistance(config.MovingRightDistance);
                        platformActionData.SetMovingSpeed(config.MovingSpeed);
                        platformActionData.SetLerpType(config.LerpType);
                        data.SetPlatformActionData(platformActionData);
                        listPlatformCreationData.Add(data);
                    }

                    totalPlatform += platformNumber;
                }

                break;
            }
        }

        //Create some platforms first
        startPlatforms = (startPlatforms > listPlatformCreationData.Count) ? listPlatformCreationData.Count : startPlatforms;
        for(int i = 0; i < startPlatforms; i++)
        {
            CreateNextPlatform();
        }

        PlayingGame();
    }



    /// <summary>
    /// Actual start the game (call Ingame_Playing event).
    /// </summary>
    public void PlayingGame()
    {
        //Fire event
        IngameState = IngameState.Ingame_Playing;
        ingameState = IngameState.Ingame_Playing;

        //Other actions

        if (IsRevived)
        {
            ResumeBackgroundMusic(0.5f);

        }
        else
        {
            //Update UI
            ViewManager.Instance.IngameViewController.PlayingViewControl.UpdateLevelProgressUI(currentPassedPlatform, totalPlatform);
            PlayBackgroundMusic(0.5f);
        }
    }


    /// <summary>
    /// Call Ingame_Revive event.
    /// </summary>
    public void Revive()
    {
        //Fire event
        IngameState = IngameState.Ingame_Revive;
        ingameState = IngameState.Ingame_Revive;

        //Add another actions here
        PauseBackgroundMusic(0.5f);
    }


    /// <summary>
    /// Call Ingame_GameOver event.
    /// </summary>
    public void GameOver()
    {
        //Fire event
        IngameState = IngameState.Ingame_GameOver;
        ingameState = IngameState.Ingame_GameOver;

        //Add another actions here
        StopBackgroundMusic(0f);
    }


    /// <summary>
    /// Call Ingame_CompletedLevel event.
    /// </summary>
    public void CompletedLevel()
    {
        //Fire event
        IngameState = IngameState.Ingame_CompletedLevel;
        ingameState = IngameState.Ingame_CompletedLevel;

        //Other actions

        StopBackgroundMusic(0f);
        //ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.completedLevel);

        completedLevelEffectsTrans.transform.position = new Vector3(PlayerController.Instance.transform.position.x, PlayerController.Instance.TargetY, 0);
        completedLevelEffectsTrans.gameObject.SetActive(true);
        foreach (ParticleSystem o in completedLevelEffects)
        {
            o.Play();
        }

        //Save level
        if (testingLevel == 0)
        {
            PlayerPrefs.SetInt(PlayerPrefsKey.SAVED_LEVEL_PPK, PlayerPrefs.GetInt(PlayerPrefsKey.SAVED_LEVEL_PPK) + 1);
        }
    }

    private void PlayBackgroundMusic(float delay)
    {
        StartCoroutine(CRPlayBGMusic(delay));
    }

    private IEnumerator CRPlayBGMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServicesManager.Instance.SoundManager.PlayMusic(background, 0.5f);
    }

    private void StopBackgroundMusic(float delay)
    {
        StartCoroutine(CRStopBGMusic(delay));
    }

    private IEnumerator CRStopBGMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServicesManager.Instance.SoundManager.StopMusic(0.5f);
    }

    private void PauseBackgroundMusic(float delay)
    {
        StartCoroutine(CRPauseBGMusic(delay));
    }

    private IEnumerator CRPauseBGMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServicesManager.Instance.SoundManager.PauseMusic();
    }

    private void ResumeBackgroundMusic(float delay)
    {
        StartCoroutine(CRResumeBGMusic(delay));
    }

    private IEnumerator CRResumeBGMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        ServicesManager.Instance.SoundManager.ResumeMusic();
    }




    /// <summary>
    /// Changing colors of all active platforms 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CRChangingColorOfAllPlatform()
    {
        //Update the color index
        platformColorConfigIndex = (platformColorConfigIndex + 1 == listPlatformColorsConfig.Count) ? 0 : platformColorConfigIndex + 1;

        List<PlatformController> listActivePlatform = new List<PlatformController>();
        foreach (PlatformController o in listPlatformControl)
        {
            if (o.gameObject.activeInHierarchy)
            {
                listActivePlatform.Add(o);
            }
        }

        List<PlatformController> listArrangedPlatform = new List<PlatformController>();
        while (listActivePlatform.Count > 0)
        {
            float minDis = 1000;
            int removedIndex = 0;
            for(int i = 0; i < listActivePlatform.Count; i++)
            {
                float platformY = listActivePlatform[i].transform.position.y;
                if (platformY - PlayerController.Instance.TargetY < minDis)
                {
                    minDis = platformY - PlayerController.Instance.TargetY;
                    removedIndex = i;
                }
            }

            listArrangedPlatform.Add(listActivePlatform[removedIndex]);
            listActivePlatform.RemoveAt(removedIndex);
            yield return null;
        }

        for (int i = 0; i < listArrangedPlatform.Count; i++)
        {
            if (listArrangedPlatform[i].gameObject.activeInHierarchy)
            {
                listArrangedPlatform[i].ChangeColor(listPlatformColorsConfig[platformColorConfigIndex]);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }



    /// <summary>
    /// Play the given particle and disable it.
    /// </summary>
    /// <param name="par"></param>
    /// <returns></returns>
    private IEnumerator CRPlayParticle(ParticleSystem par)
    {
        par.Play();
        yield return new WaitForSeconds(par.main.startLifetimeMultiplier);
        par.gameObject.SetActive(false);
    }



    //////////////////////////////////////Publish functions


    /// <summary>
    /// Continue the game
    /// </summary>
    public void SetContinueGame()
    {
        IsRevived = true;
        PlayingGame();
    }


    /// <summary>
    /// Handle action when player died.
    /// </summary>
    public void HandlePlayerDied()
    {
        if (IsRevived || !ServicesManager.Instance.AdManager.IsRewardedVideoAdReady())
        {
            GameOver();
        }
        else
        {
            Revive();
        }
    }




    /// <summary>
    /// Create the next platform.
    /// </summary>
    public void CreateNextPlatform()
    {
        if (listPlatformCreationData.Count > dataIndex)
        {
            PlatformCreationData data = listPlatformCreationData[dataIndex];

            PlatformController platformControl = PoolManager.Instance.GetPlatformControl(data.PlatformType, data.PlatformSize);
            float distance = 0;
            if (dataIndex == 0)
            {
                distance = data.PlatformDistance;
            }
            else
            {
                float lastY = listPlatformControl[listPlatformControl.Count - 1].transform.position.y;
                distance = lastY + data.PlatformDistance;
            }
            Vector3 pos = Vector3.up * distance;
            pos.x += (dataIndex == 0) ? 0 : data.XAxisDevitation;
            platformControl.transform.position = pos;
            platformControl.gameObject.SetActive(true);
            platformControl.SetupColor(listPlatformColorsConfig[platformColorConfigIndex]);
            if (dataIndex > 0 && dataIndex < listPlatformCreationData.Count -1)
            {
                platformControl.SetupPlatformData(data.PlatformActionData);
                platformControl.CreateCoin(Random.value <= data.CoinFrequency ? data.CoinNumber : 0);
            }
            listPlatformControl.Add(platformControl);
            dataIndex++;
        }
    }


    /// <summary>
    /// Get the closest platform that higher then TargetY of PlayerController.
    /// </summary>
    /// <param name="yAxis"></param>
    /// <returns></returns>
    public float GetHigherPlatformPoint()
    {
        float result = PlayerController.Instance.TargetY;
        float min = 1000f;
        foreach(PlatformController o in listPlatformControl)
        {
            if (o.transform.position.y > PlayerController.Instance.TargetY)
            {
                if (o.transform.position.y - PlayerController.Instance.TargetY < min)
                {
                    min = o.transform.position.y - PlayerController.Instance.TargetY;
                    result = o.transform.position.y;
                }
            }
        }

        return result;
    }



    /// <summary>
    /// Determine whether the given yAxis is the highest point.
    /// </summary>
    /// <param name="yAxis"></param>
    /// <returns></returns>
    public bool IsHighestPoint(float yAxis)
    {
        foreach (PlatformController o in listPlatformControl)
        {
            if (o.transform.position.y > yAxis)
            {
                return false;
            }
        }

        return true;
    }



    /// <summary>
    /// Bounce the closest PlatformController object that lower then the player.
    /// </summary>
    public void BounceTheClosestPlatform()
    {
        PlatformController closestPlatform = null;
        float playerY = PlayerController.Instance.transform.position.y + 0.5f;
        float min = 1000f;
        foreach (PlatformController o in listPlatformControl)
        {
            if (o.transform.position.y < playerY)
            {
                if (playerY - o.transform.position.y < min)
                {
                    min = playerY - o.transform.position.y;
                    closestPlatform = o;
                }
            }
        }

        closestPlatform.Bounce();

        if (Mathf.RoundToInt(closestPlatform.transform.position.y) > currentPlatformYAxis)
        {
            currentPlatformYAxis = Mathf.RoundToInt(closestPlatform.transform.position.y);
            stepCount++;
            if (stepCount == jumpSteps)
            {
                stepCount = 0;
                ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.changeColor);
                StartCoroutine(CRChangingColorOfAllPlatform());
            }

            //Update UI
            currentPassedPlatform++;
            ViewManager.Instance.IngameViewController.PlayingViewControl.UpdateLevelProgressUI(currentPassedPlatform, totalPlatform);
        }
    }
}
