using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public float keyboardSensitivity = 2f; // Sensibilité du mouvement du manche pour le clavier
    public float mouseSensitivity = 100f; // Sensibilité du mouvement du manche pour la souris
    public float smoothTime = 0.1f; // Temps de lissage pour le mouvement du manche

    private float targetRotationX;
    private float rotationVelocityX;
    private float rotationVelocityY;

    void Update()
    {
        // Lecture des entrées du clavier
        float verticalInput = Input.GetAxis("Vertical") * keyboardSensitivity;

        // Lecture des mouvements de la souris
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Combinaison des entrées clavier et souris pour le mouvement vertical
        targetRotationX += verticalInput + mouseY;

        // Limite de la rotation du manche
        targetRotationX = Mathf.Clamp(targetRotationX, -10f, 10f);

        // Application d'une interpolation linéaire pour un mouvement lisse
        float newRotationX = Mathf.SmoothDampAngle(transform.localEulerAngles.x, targetRotationX, ref rotationVelocityX, smoothTime);

        // Application de la rotation au manche
        transform.localEulerAngles = new Vector3(newRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
