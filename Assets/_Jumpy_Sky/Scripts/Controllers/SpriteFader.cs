using System.Collections;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;


    /// <summary>
    /// Scale this object up and fade it out.
    /// </summary>
    public void StartFade()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        transform.eulerAngles = new Vector3(90, 0, 0);
        StartCoroutine(CRFadingAndScale());
    }
    private IEnumerator CRFadingAndScale()
    {
        float t = 0;
        float fadingTime = 0.75f;
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            spriteRenderer.color = Color.Lerp(startColor, endColor, factor);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.5f, factor);
            yield return null;
        }

        transform.localScale = Vector3.zero;
        spriteRenderer.color = startColor;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
