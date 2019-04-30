using UnityEngine;

public class BombHole : MonoBehaviour
{
    [SerializeField] Transform target;
    Renderer m_Renderer;

    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        m_Renderer.material.SetVector("_Target", target.position);
    }
}
