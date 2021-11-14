using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CBGames;

public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance { private set; get; }
    public static event System.Action<PlayerState> PlayerStateChanged = delegate { };

    public PlayerState PlayerState
    {
        get
        {
            return playerState;
        }

        private set
        {
            if (value != playerState)
            {
                value = playerState;
                PlayerStateChanged(playerState);
            }
        }
    }


    private PlayerState playerState = PlayerState.Player_Prepare;


    [Header("Player Config")]
    [SerializeField] private float thresholdSpeed = 100f;
    [SerializeField] private float swipeSmoothTime = 0.08f;
    [SerializeField] private float fallDownVelocity = -60;

    [Header("Player References")]
    [SerializeField] private Transform meshTrans = null;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private MeshFilter meshFilter = null;
    [SerializeField] private LayerMask platformLayer = new LayerMask();
    [SerializeField] private LayerMask centerPlatformLayer = new LayerMask();
    [SerializeField] private LayerMask coinLayer = new LayerMask();

    public float TargetY { private set; get; }

    private Vector3 velocity = Vector3.zero;
    private float firstX = 0;
    private float currentJumpVelocity = 0;
    private int bounceIndex = 0;
    private bool isHitCenter = false;
    private void OnEnable()
    {
        IngameManager.GameStateChanged += GameManager_GameStateChanged;
    }
    private void OnDisable()
    {
        IngameManager.GameStateChanged -= GameManager_GameStateChanged;
    }

    private void GameManager_GameStateChanged(IngameState obj)
    {
        if (obj == IngameState.Ingame_Playing)
        {
            PlayerLiving();
        }
        else if (obj == IngameState.Ingame_CompletedLevel)
        {
            PlayerCompletedLevel();
        }
    }



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

        //Fire event
        PlayerState = PlayerState.Player_Prepare;
        playerState = PlayerState.Player_Prepare;

        //Add other actions here

        //Setup character
        CharacterInforController charControl = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
        meshFilter.mesh = charControl.GetMeshFilter(0).sharedMesh;
        meshRenderer.material = charControl.GetMeshRender(0).sharedMaterial;
    }

    private void Update()
    {
        if (playerState == PlayerState.Player_Living)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstX = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).x;
            }
            else if (Input.GetMouseButton(0))
            {
                float currentX = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)).x;
                float amount = Mathf.Abs(Mathf.Abs(currentX) - Mathf.Abs(firstX));

                if (currentX > firstX)
                {
                    Vector3 newPos = transform.position;
                    newPos += new Vector3(amount * thresholdSpeed * Time.deltaTime, 0, 0);
                    transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, swipeSmoothTime);
                }
                else
                {
                    Vector3 newPos = transform.position;
                    newPos -= new Vector3(amount * thresholdSpeed * Time.deltaTime, 0, 0);
                    transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, swipeSmoothTime);
                }

                firstX = currentX;
            }
        }
        
    }

    private void PlayerLiving()
    {
        //Fire event
        PlayerState = PlayerState.Player_Living;
        playerState = PlayerState.Player_Living;


        if (IngameManager.Instance.IsRevived)
        {
            transform.position = new Vector3(0, TargetY, 0);
            meshTrans.localRotation = Quaternion.identity;
        }
        else
        {
            TargetY = transform.position.y;
        }

        float endY = IngameManager.Instance.GetHigherPlatformPoint() + 2f;
        StartCoroutine(CRJumpUp(endY));
    }

    public void PlayerDied()
    {
        //Fire event
        PlayerState = PlayerState.Player_Died;
        playerState = PlayerState.Player_Died;

        //Add other actions here

        //Play sound effect and create screenshot
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.levelFailed);
        ServicesManager.Instance.ShareManager.CreateScreenshot();
    }


    private void PlayerCompletedLevel()
    {
        //Fire event
        PlayerState = PlayerState.Player_CompletedLevel;
        playerState = PlayerState.Player_CompletedLevel;

        //Add others action here

        //Play sound effect and create screenshot
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.completedLevel);
        ServicesManager.Instance.ShareManager.CreateScreenshot();
    }

    private IEnumerator CRJumpUp(float endY)
    {
        isHitCenter = false;
        float t = 0;
        float jumpingTime = 0.35f;
        float startY = transform.position.y;
        Quaternion startQuater = Quaternion.Euler(meshTrans.localEulerAngles);
        Quaternion endQuater = Quaternion.Euler(new Vector3(25f, 0, 0));
        while (t < jumpingTime)
        {
            t += Time.deltaTime;
            float factor = EasyType.MatchedLerpType(LerpType.EaseOutCubic, t / jumpingTime);
            Vector3 pos = transform.position;
            float newY = Mathf.Lerp(startY, endY, factor);
            pos.y = newY;
            transform.position = pos;
            meshTrans.localRotation = Quaternion.Lerp(startQuater, endQuater, factor);
            yield return null;
        }

        currentJumpVelocity = 0;
        StartCoroutine(CRFallingDown());
    }

    private IEnumerator CRFallingDown()
    {
        while (true)
        {
            transform.position = transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallDownVelocity * Time.deltaTime * Time.deltaTime / 2);

            if (currentJumpVelocity < fallDownVelocity)
                currentJumpVelocity = fallDownVelocity;
            else
                currentJumpVelocity = currentJumpVelocity + fallDownVelocity * Time.deltaTime;

            //Adjust angles
            Vector3 angles = meshTrans.localEulerAngles;
            angles.x -= Time.deltaTime * 200f;
            meshTrans.localEulerAngles = angles;


            if (currentJumpVelocity < 0)
            {
                Collider[] coinColliders = Physics.OverlapSphere(transform.position, meshRenderer.bounds.size.x / 2f, coinLayer);
                if (coinColliders.Length > 0)
                {
                    coinColliders[0].GetComponent<ItemController>().HandleCollidePlayer();
                }

                Collider[] centerPlatformColliders = Physics.OverlapSphere(transform.position, meshRenderer.bounds.size.x / 2f, centerPlatformLayer);
                if (centerPlatformColliders.Length > 0)
                {
                    isHitCenter = true;
                    centerPlatformColliders[0].GetComponent<PlatformCenterController>().HandleCollidePlayer();

                    //Play sound effects
                    ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.bounces[bounceIndex]);
                    bounceIndex = (bounceIndex + 1 >= ServicesManager.Instance.SoundManager.bounces.Length - 1) ? ServicesManager.Instance.SoundManager.bounces.Length - 1 : bounceIndex + 1;
                }

                Collider[] platformColliders = Physics.OverlapSphere(transform.position, meshRenderer.bounds.size.x / 2f, platformLayer);
                if (platformColliders.Length > 0)
                {
                    if (!isHitCenter)
                    {
                        bounceIndex = 0;
                        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.bounces[0]);
                    }


                    TargetY = platformColliders[0].transform.position.y;
                    float endY = TargetY;
                    if (IngameManager.Instance.IsHighestPoint(TargetY))
                    {
                        endY = TargetY + 5f;
                        if (playerState == PlayerState.Player_Living)
                        {
                            IngameManager.Instance.CompletedLevel();
                        }
                    }
                    else
                    {
                        endY = IngameManager.Instance.GetHigherPlatformPoint() + 2f;
                    }
                    IngameManager.Instance.BounceTheClosestPlatform();
                    StartCoroutine(CRJumpUp(endY));
                    yield break;
                }
            }

            yield return null;


            //Check player fall out of the screen
            Vector2 viewPort = Camera.main.WorldToViewportPoint(transform.position);
            if (viewPort.y <= -0.2f)
            {
                PlayerDied();
                IngameManager.Instance.HandlePlayerDied();
                yield break;
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////Public functions
}
