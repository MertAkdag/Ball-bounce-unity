using UnityEngine;
using CBGames;

public class ItemController : MonoBehaviour {

    [Header("Item Config")]
    [SerializeField] private float minRotatingSpeed = 150f;
    [SerializeField] private float maxRotatingSpeed = 350f;

    [Header("Item References")]
    [SerializeField] private MeshRenderer meshRender = null;

    private float rotatingSpeed = 0;

    public void Start()
    {
        rotatingSpeed = Random.Range(minRotatingSpeed, maxRotatingSpeed);
    }


    private void Update()
    {
        transform.eulerAngles += Vector3.up * rotatingSpeed * Time.deltaTime;
    }



    /// <summary>
    /// Handle actions when this item collide with the player.
    /// </summary>
    public void HandleCollidePlayer()
    {
        ServicesManager.Instance.CoinManager.AddCollectedCoins(1);
        ServicesManager.Instance.SoundManager.PlayOneSound(ServicesManager.Instance.SoundManager.collectCoin);
        Vector3 pos = transform.position + Vector3.up * (meshRender.bounds.size.y / 2f);
        EffectManager.Instance.PlayCollectCoinEffect(pos);
        transform.SetParent(null);
        gameObject.SetActive(false);
    }



    /// <summary>
    /// Deactivate this object.
    /// </summary>
    public void DisableObject()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
