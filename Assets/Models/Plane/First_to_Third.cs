using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public Camera ThirdPersonCamera;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            firstPersonCamera.enabled = !firstPersonCamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
        }
    }
}
