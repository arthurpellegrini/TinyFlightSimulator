using UnityEngine;

[ExecuteInEditMode]
public class GetMainLightDirection : MonoBehaviour
{
   [SerializeField] private Material SkyboxMaterial;

    private void Update()
    {
        SkyboxMaterial.SetVector("_MainLightDirection", transform.forward);
    }
}
