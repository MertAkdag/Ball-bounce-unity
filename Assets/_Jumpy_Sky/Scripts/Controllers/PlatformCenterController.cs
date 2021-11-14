using UnityEngine;
using System.Collections;
using CBGames;

public class PlatformCenterController : MonoBehaviour
{

    private MeshRenderer meshRenderer = null;
    private MeshCollider meshCollider = null;


    private void OnDisable()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();

        meshCollider.enabled = true;
        Color color = meshRenderer.material.color;
        color.a = 1;
        meshRenderer.material.color = color;
        transform.localScale = Vector3.one;
    }


    /// <summary>
    /// Handle actions when this object collide with the player.
    /// </summary>
    public void HandleCollidePlayer()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();

        meshCollider.enabled = false;
        Vector3 pos = transform.position;
        EffectManager.Instance.PlayColorSplashEffect(pos);
        StartCoroutine(CRFadingAndScale());
    }
    private IEnumerator CRFadingAndScale()
    {
        float t = 0;
        float fadingTime = 0.5f;
        Color startColor = meshRenderer.material.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            meshRenderer.material.color = Color.Lerp(startColor, endColor, factor);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, factor);
            yield return null;
        }
    }
}
