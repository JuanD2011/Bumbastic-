using UnityEngine;

[ExecuteInEditMode]
public class ShaderVignette : MonoBehaviour
{
    [SerializeField] Shader shader = null;
    [SerializeField] Texture vignetteMaskTexture = null;
    [SerializeField] private Color vignetteColor = Color.white;

    [Range(0f, 1f)]
    [SerializeField] float colorTreshold = 0.7f;

    [Range(0f, 1f)]
    [SerializeField] float intensity = 0.7f;

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
        Material.SetTexture("_VignetteMask", vignetteMaskTexture);
        Material.SetColor("_Color", vignetteColor);
        Material.SetFloat("_ColorTreshold", colorTreshold);
        //Material.SetFloat("_VignetteIntensity", intensity);
        Material.SetFloat("_TexScale", intensity);
        Graphics.Blit(source, destination, Material);
    }

    private void OnDisable()
    {
        if (material)
        {
            DestroyImmediate(material);
        }
    }
}