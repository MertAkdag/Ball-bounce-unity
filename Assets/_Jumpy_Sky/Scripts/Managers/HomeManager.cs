using UnityEngine.SceneManagement;
using UnityEngine;
using CBGames;
using System.Collections;

public class HomeManager : MonoBehaviour
{

    [SerializeField] private Transform characterTrans = null;
    [SerializeField] private MeshFilter characterMeshFilter = null;
    [SerializeField] private MeshRenderer characterMeshRenderer = null;

    private float fallDownVelocity = -60;
    private float currentJumpVelocity = 0;
    private void Start()
    {
        Application.targetFrameRate = 60;
        ViewManager.Instance.OnLoadingSceneDone(SceneManager.GetActiveScene().name);

        //Setup character
        CharacterInforController charControl = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
        characterMeshFilter.mesh = charControl.GetMeshFilter(0).sharedMesh;
        characterMeshRenderer.material = charControl.GetMeshRender(0).sharedMaterial;

        StartCoroutine(CRJumpUp());

        //Report level to leaderboard
        string username = PlayerPrefs.GetString(PlayerPrefsKey.SAVED_USER_NAME_PPK);
        if (!string.IsNullOrEmpty(username))
        {
            ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
        }
    }

    private IEnumerator CRJumpUp()
    {
        float t = 0;
        float jumpingTime = 0.35f;
        float startY = characterTrans.position.y;
        float endY = 5f;
        Quaternion startQuater = Quaternion.Euler(characterMeshRenderer.transform.localEulerAngles);
        Quaternion endQuater = Quaternion.Euler(new Vector3(25f, 0, 0));
        while (t < jumpingTime)
        {
            t += Time.deltaTime;
            float factor = EasyType.MatchedLerpType(LerpType.EaseOutCubic, t / jumpingTime);
            Vector3 pos = characterTrans.position;
            float newY = Mathf.Lerp(startY, endY, factor);
            pos.y = newY;
            characterTrans.position = pos;
            characterMeshRenderer.transform.localRotation = Quaternion.Lerp(startQuater, endQuater, factor);
            yield return null;
        }

        currentJumpVelocity = 0;

        StartCoroutine(CRFallingDown());
    }

    private IEnumerator CRFallingDown()
    {
        while (true)
        {
            characterTrans.position = characterTrans.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallDownVelocity * Time.deltaTime * Time.deltaTime / 2);

            if (currentJumpVelocity < fallDownVelocity)
                currentJumpVelocity = fallDownVelocity;
            else
                currentJumpVelocity = currentJumpVelocity + fallDownVelocity * Time.deltaTime;

            //Adjust angles
            Vector3 angles = characterMeshRenderer.transform.localEulerAngles;
            angles.x -= Time.deltaTime * 200f;
            characterMeshRenderer.transform.localEulerAngles = angles;


            if (currentJumpVelocity < 0)
            {
                Collider[] platformColliders = Physics.OverlapSphere(characterTrans.position, characterMeshRenderer.bounds.size.x / 2f);
                if (platformColliders.Length > 0)
                {
                    StartCoroutine(CRJumpUp());
                    yield break;
                }
            }

            yield return null;
        }
    }
}
