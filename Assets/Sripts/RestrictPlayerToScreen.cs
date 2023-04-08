using UnityEngine;

public class RestrictPlayerToScreen : MonoBehaviour
{
    public float margin = 0.1f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 playerPosition = mainCamera.WorldToViewportPoint(transform.position);
        playerPosition.x = Mathf.Clamp(playerPosition.x, margin, 1 - margin); 
        playerPosition.y = Mathf.Clamp(playerPosition.y, margin, 1 - margin); 
        Vector3 clampedPosition = mainCamera.ViewportToWorldPoint(playerPosition);
        transform.position = new Vector3(clampedPosition.x, transform.position.y, clampedPosition.z);
    }
}
