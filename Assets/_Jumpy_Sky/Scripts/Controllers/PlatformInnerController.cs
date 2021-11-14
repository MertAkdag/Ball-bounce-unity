using System.Collections;
using UnityEngine;

public class PlatformInnerController : MonoBehaviour
{
    private MeshRenderer meshRenderer = null;


    /// <summary>
    /// Setup the color for this object.
    /// </summary>
    /// <param name="newColor"></param>
    public void SetColor(Color newColor)
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = newColor;
    }


    /// <summary>
    /// Change th color of this object from the current color to newColor.
    /// </summary>
    /// <param name="isFade"></param>
    public void ChangeColor(Color newColor)
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(CRChangingColor(newColor));
    }

    private IEnumerator CRChangingColor(Color newColor)
    {
        float t = 0;
        float fadingTime = 0.25f;
        Color startColor = meshRenderer.material.color;
        while (t < fadingTime)
        {
            t += Time.deltaTime;
            float factor = t / fadingTime;
            meshRenderer.material.color = Color.Lerp(startColor, newColor, factor);
            yield return null;
        }
    }


}
