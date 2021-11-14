using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer = new LayerMask();

    private GameObject currentPlatform = null;
    private Vector3 halfExtents = new Vector3(50, 5, 50);
    private void Update()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, halfExtents, transform.localRotation, platformLayer);
        if (colliders.Length > 0 && colliders[0].gameObject != currentPlatform)
        {
            currentPlatform = colliders[0].gameObject;
            currentPlatform.GetComponent<PlatformController>().DisableObject();
            IngameManager.Instance.CreateNextPlatform();
        }
    }
}
