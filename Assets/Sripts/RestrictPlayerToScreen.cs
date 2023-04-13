using UnityEngine;

public class RestrictPlayerToScreen : MonoBehaviour
{
    public float margin = 0.1f;

    void Start()
    {
    }

    void LateUpdate()
    {
        Vector3 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
        playerPosition.x = Mathf.Clamp(playerPosition.x, margin, 1 - margin); 
        playerPosition.y = Mathf.Clamp(playerPosition.y, margin, 1 - margin); 
        Vector3 clampedPosition = Camera.main.ViewportToWorldPoint(playerPosition);
        transform.position = new Vector3(clampedPosition.x, transform.position.y, clampedPosition.z);
    }
}
