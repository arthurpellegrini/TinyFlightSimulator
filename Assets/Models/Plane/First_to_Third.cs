using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera FirstPersonCamera;
    public Camera ThirdPersonCamera;

    void Start()
    {
        // Assurez-vous qu'une cam�ra est active au d�marrage
        FirstPersonCamera.enabled = true;
        ThirdPersonCamera.enabled = false;

        // Assurez-vous qu'un seul AudioListener est actif
        FirstPersonCamera.GetComponent<AudioListener>().enabled = true;
        ThirdPersonCamera.GetComponent<AudioListener>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Bascule entre les cam�ras
            FirstPersonCamera.enabled = !FirstPersonCamera.enabled;
            ThirdPersonCamera.enabled = !ThirdPersonCamera.enabled;

            // Bascule les AudioListeners
            FirstPersonCamera.GetComponent<AudioListener>().enabled = !FirstPersonCamera.GetComponent<AudioListener>().enabled;
            ThirdPersonCamera.GetComponent<AudioListener>().enabled = !ThirdPersonCamera.GetComponent<AudioListener>().enabled;
        }
    }
}
