using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectManager : MonoBehaviour {

    public static EffectManager Instance { private set; get; }

    [SerializeField] private GameObject colorSplashEffectPrefab = null;
    [SerializeField] private GameObject collectCoinEffectPrefab = null;
    [SerializeField] private GameObject circleFadeEffectPrefab = null;

    private List<ParticleSystem> listColorSplashEffect = new List<ParticleSystem>();
    private List<ParticleSystem> listCollectCoinEffect = new List<ParticleSystem>();
    private List<SpriteFader> listCircleFade = new List<SpriteFader>();

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
    /// Play the given particle then disable it 
    /// </summary>
    /// <param name="par"></param>
    /// <returns></returns>
    private IEnumerator CRPlayParticle(ParticleSystem par)
    {
        par.Play();
        yield return new WaitForSeconds(2f);
        par.gameObject.SetActive(false);
    }


    /// <summary>
    /// Play a color splash effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayColorSplashEffect(Vector3 pos)
    {
        //Find in the list
        ParticleSystem cubeSmoke = listColorSplashEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (cubeSmoke == null)
        {
            //Didn't find one -> create new one
            cubeSmoke = Instantiate(colorSplashEffectPrefab, pos, Quaternion.identity).GetComponent<ParticleSystem>();
            cubeSmoke.gameObject.SetActive(false);
            listColorSplashEffect.Add(cubeSmoke);
        }

        cubeSmoke.transform.position = pos;
        cubeSmoke.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(cubeSmoke));
    }


    /// <summary>
    /// Play a collect coin effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCollectCoinEffect(Vector3 pos)
    {
        //Find in the list
        ParticleSystem cubeSmoke = listCollectCoinEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (cubeSmoke == null)
        {
            //Didn't find one -> create new one
            cubeSmoke = Instantiate(collectCoinEffectPrefab, pos, Quaternion.identity).GetComponent<ParticleSystem>();
            cubeSmoke.gameObject.SetActive(false);
            listCollectCoinEffect.Add(cubeSmoke);
        }

        cubeSmoke.transform.position = pos;
        cubeSmoke.gameObject.SetActive(true);
        StartCoroutine(CRPlayParticle(cubeSmoke));
    }


    /// <summary>
    /// Play a circle fade effect at given position.
    /// </summary>
    /// <param name="pos"></param>
    public void PlayCircleFadeEffect(Vector3 pos, Transform parent)
    {
        //Find in the list
        SpriteFader circleFade = listCircleFade.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

        if (circleFade == null)
        {
            //Didn't find one -> create new one
            circleFade = Instantiate(circleFadeEffectPrefab, pos, Quaternion.identity).GetComponent<SpriteFader>();
            circleFade.gameObject.SetActive(false);
            listCircleFade.Add(circleFade);
        }

        circleFade.transform.position = pos;
        circleFade.gameObject.SetActive(true);
        circleFade.transform.SetParent(parent);
        circleFade.StartFade();
    }
}
