using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public float keyboardSensitivity = 2f; // Sensibilité du mouvement du manche pour le clavier
    public float mouseSensitivity = 100f; // Sensibilité du mouvement du manche pour la souris
    public float smoothTime = 0.1f; // Temps de lissage pour le mouvement du manche

    private float targetPitch; // Pour le mouvement avant/arrière
    private float targetYaw;   // Pour le mouvement gauche/droite
    private float pitchVelocity;
    private float yawVelocity;

    void Update()
    {
        // Lecture des entrées du clavier
        float verticalInput = Input.GetAxis("Vertical") * keyboardSensitivity;
        float horizontalInput = Input.GetAxis("Horizontal") * keyboardSensitivity;

        // Lecture des mouvements de la souris
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Mise à jour des cibles de rotation
        targetPitch += verticalInput + mouseY;
        targetYaw += horizontalInput + mouseX;

        // Application des limites de rotation
        targetPitch = Mathf.Clamp(targetPitch, -45f, 45f);
        targetYaw = Mathf.Clamp(targetYaw, -45f, 45f);

        // Interpolation lisse des rotations
        float newPitch = Mathf.SmoothDampAngle(transform.localEulerAngles.x, targetPitch, ref pitchVelocity, smoothTime);
        float newYaw = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetYaw, ref yawVelocity, smoothTime);

        // Application des rotations au manche
        transform.localEulerAngles = new Vector3(newPitch, newYaw, 0f);
    }
}
