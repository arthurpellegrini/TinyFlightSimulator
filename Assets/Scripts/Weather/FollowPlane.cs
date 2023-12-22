using UnityEngine;

public class FollowPlane : MonoBehaviour
{
    [SerializeField] Transform planeTransform;
    public float heightOffset = 50.0f;
    public float maxHeight = 340.0f;

    void Update()
    {
        if (planeTransform != null)
        {
            Vector3 planePosition = planeTransform.position;
            
            float targetHeight = planePosition.y + heightOffset;
            
            targetHeight = Mathf.Min(targetHeight, maxHeight);

            transform.position = new Vector3(planePosition.x, targetHeight, planePosition.z);
        }
    }
}
