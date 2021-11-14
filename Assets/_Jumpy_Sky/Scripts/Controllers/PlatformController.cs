using System.Collections;
using UnityEngine;
using CBGames;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private PlatformType platformType = PlatformType.SQUARE;
    [SerializeField] private PlatformSize platformSize = PlatformSize.SMALL;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private PlatformInnerController platformInnerControl = null;


    public PlatformType PlatformType { get { return platformType; } }
    public PlatformSize PlatformSize { get { return platformSize; } }





    /// <summary>
    /// Setup PlatformActionData for this object.
    /// </summary>
    /// <param name="data"></param>
    public void SetupPlatformData(PlatformActionData data)
    {
        if (Random.value <= data.MovingPlatformFrequency)
        {
            float leftCorner = transform.position.x - Mathf.Abs(data.MovingLeftDistance);
            float rightCorner = transform.position.x + Mathf.Abs(data.MovingRightDistance);
            StartCoroutine(CRMoving(leftCorner, rightCorner, data.MovingSpeed, data.LerpType));
        }
    }


    /// <summary>
    /// Setup colors for this object.
    /// </summary>
    /// <param name="colorsConfig"></param>
    public void SetupColor(PlatformColorsConfig colorsConfig)
    {
        meshRenderer.material.color = colorsConfig.PlatformColor;
        platformInnerControl.SetColor(colorsConfig.PlatformInnerColor);
    }



    /// <summary>
    /// Create coins base on coinNumber.
    /// </summary>
    /// <param name="coinNumber"></param>
    public void CreateCoin(int coinNumber)
    {
        int coinTemp = 0;
        if (platformSize == PlatformSize.SMALL)
        {
            coinTemp = (coinNumber <= 3) ? coinNumber : 3;
        }
        else if (platformSize == PlatformSize.NORMAL || platformSize == PlatformSize.BIG)
        {
            coinTemp = (coinNumber <= 7) ? coinNumber : 7;
        }

        Vector3 centerPos = transform.position + Vector3.up * (meshRenderer.bounds.size.y / 1.5f);
        if (coinTemp > 0)
        {
            ItemController coinControl = PoolManager.Instance.GetCoinControl();
            coinControl.transform.position = centerPos;
            coinControl.gameObject.SetActive(true);
            coinControl.transform.SetParent(transform);
            coinTemp--;
        }

        int turn = -1;
        for (int i = 1; i <= coinTemp; i++)
        {
            Vector3 dir = (turn < 0) ? Vector3.right : Vector3.left;
            Vector3 pos = centerPos + (dir * 0.65f * i);
            ItemController coinControl = PoolManager.Instance.GetCoinControl();
            coinControl.transform.position = pos;
            coinControl.gameObject.SetActive(true);
            coinControl.transform.SetParent(transform);
        }
    }

    private IEnumerator CRMoving(float leftCorner, float rightCorner, float movingSpeed, LerpType lerpType)
    {
        float movingTime = (rightCorner - leftCorner) / movingSpeed;
        if (Random.value <= 0.5f) //Moving to left corner -> then repeate form left to right
        {
            float currentX = transform.position.x;
            float t = 0;
            float time = ((currentX - leftCorner) * movingTime) / (rightCorner - leftCorner);
            while (t < time)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(lerpType, t / time);
                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(currentX, leftCorner, factor);
                transform.position = pos;
                yield return null;
            }

            while (gameObject.activeInHierarchy)
            {
                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movingTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(leftCorner, rightCorner, factor);
                    transform.position = pos;
                    yield return null;
                }

                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movingTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(rightCorner, leftCorner, factor);
                    transform.position = pos;
                    yield return null;
                }
            }
        }
        else //Moving to right corner -> then repeate form right to left
        {
            float currentX = transform.position.x;
            float t = 0;
            float time = ((rightCorner - currentX) * movingTime) / (rightCorner - leftCorner);
            while (t < time)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(lerpType, t / time);
                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(currentX, rightCorner, factor);
                transform.position = pos;
                yield return null;
            }


            while (gameObject.activeInHierarchy)
            {
                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movingTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(rightCorner, leftCorner, factor);
                    transform.position = pos;
                    yield return null;
                }

                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / movingTime);
                    Vector3 pos = transform.position;
                    pos.x = Mathf.Lerp(leftCorner, rightCorner, factor);
                    transform.position = pos;
                    yield return null;
                }
            }
        }
    }


    /// <summary>
    /// Bounce this platform down and up.
    /// </summary>
    public void Bounce()
    {
        Vector3 circleFadePos = transform.position;
        circleFadePos += Vector3.up * (meshRenderer.bounds.size.y / 2f);
        EffectManager.Instance.PlayCircleFadeEffect(circleFadePos, transform);
        StartCoroutine(CRBouncing());
    }
    private IEnumerator CRBouncing()
    {
        float bouncingTime = 0.1f;
        float t = 0;
        float startY = transform.position.y;
        float endY = startY - 0.4f;

        while (t < bouncingTime)
        {
            t += Time.deltaTime;
            float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
            Vector3 currentPos = transform.position;
            currentPos.y = Mathf.Lerp(startY, endY, factor);
            transform.position = currentPos;
            yield return null;
        }

        t = 0;
        while (t < bouncingTime)
        {
            t += Time.deltaTime;
            float factor = EasyType.MatchedLerpType(LerpType.Liner, t / bouncingTime);
            Vector3 currentPos = transform.position;
            currentPos.y = Mathf.Lerp(endY, startY, factor);
            transform.position = currentPos;
            yield return null;
        }
    }



    /// <summary>
    /// Change the color of this object from the current color to new color using colorsConfig parameter.
    /// </summary>
    /// <param name="colorsConfig"></param>
    /// <param name="isFade"></param>
    public void ChangeColor(PlatformColorsConfig colorsConfig)
    {
        StartCoroutine(CRChangingColor(colorsConfig.PlatformColor));
        platformInnerControl.ChangeColor(colorsConfig.PlatformInnerColor);
    }
    private IEnumerator CRChangingColor(Color newColor)
    {
        float t = 0;
        float fadingTime = 0.15f;
        Color startColor = meshRenderer.material.color;
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            meshRenderer.material.color = Color.Lerp(startColor, newColor, factor);
            yield return null;
        }
    }





    /// <summary>
    /// Deactive this object.
    /// </summary>
    public void DisableObject()
    {
        ItemController[] itemControllers = GetComponentsInChildren<ItemController>();
        foreach(ItemController o in itemControllers)
        {
            o.DisableObject();
        }

        gameObject.SetActive(false);
    }
}
