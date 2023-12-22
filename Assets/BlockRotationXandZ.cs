using UnityEngine;

public class BlockRotationXandZ : MonoBehaviour
{
    private Transform planeTransform;
    public float heightOffset = 50.0f;
    public float maxHeight = 340.0f;

    void Start()
    {
        planeTransform = GameObject.Find("Plane").transform;
    }

    void Update()
    {
        if (planeTransform != null)
        {
            // Obtenez la position de l'avion
            Vector3 planePosition = planeTransform.position;

            // Ajustez la hauteur en fonction de l'offset
            float targetHeight = planePosition.y + heightOffset;

            // Limitez la hauteur à la valeur maximale
            targetHeight = Mathf.Min(targetHeight, maxHeight);

            // Mettez à jour la position de l'objet en utilisant la nouvelle hauteur
            transform.position = new Vector3(planePosition.x, targetHeight, planePosition.z);
        }
    }
}
