using UnityEngine;

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{
    [SerializeField] Shader shader = null;

    [Range(-1, 2)]
    [SerializeField] float contrast = 0f;

    [Range(-255, 255)]
    [SerializeField] float brightness = 0f;
    private Material material;

    Material Material
    {
        get
        {
            if (material == null)
            {
                material = new Material(shader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }
            return material;
        }
    }

    private void Start()
    {
        if (!shader || !shader.isSupported)
        {
            enabled = false;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Material.SetFloat("_Contrast", contrast);
        Material.SetFloat("_Brightness", brightness);
        Graphics.Blit(source, destination, material);
    }

    private void OnDisable()
    {
        if (material)
        {
            DestroyImmediate(material);
        }
    }
}
