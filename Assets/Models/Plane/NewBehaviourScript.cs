using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Verrouille le curseur au centre de l'écran
    }

    void Update()
    {
        // Obtenez l'entrée de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Appliquez la rotation verticale à l'objet de la caméra pour regarder haut et bas
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limitez la rotation pour éviter des renversements

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Appliquez la rotation horizontale au corps du joueur
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
