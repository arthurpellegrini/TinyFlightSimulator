using UnityEngine;

public class VisibilitySwitcher : MonoBehaviour
{
    public Material opaqueMaterial;
    public Material transparentMaterial;
    public Camera firstPersonCamera;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        // Assurez-vous que la caméra de première personne est la caméra active
        if (firstPersonCamera == Camera.current)
        {
            meshRenderer.material = transparentMaterial;
        }
        else
        {
            meshRenderer.material = opaqueMaterial;
        }
    }
}
