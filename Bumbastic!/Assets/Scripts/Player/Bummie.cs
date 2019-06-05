using UnityEngine;

public class Bummie : MonoBehaviour
{
    [SerializeField] Transform catapult = null;
    [SerializeField] SkinnedMeshRenderer[] skinnedMeshRenderers = new SkinnedMeshRenderer[0];
    Animator m_Animator = null;
    SphereCollider[] m_sphereColliders = new SphereCollider[0];

    public Transform Catapult { get => catapult; set => catapult = value; }
    public SkinnedMeshRenderer[] SkinnedMeshRenderers { get => skinnedMeshRenderers; set => skinnedMeshRenderers = value; }
    public Animator Animator { get => m_Animator; set => m_Animator = value; }
    public SphereCollider[] SphereColliders { get => m_sphereColliders; set => m_sphereColliders = value; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SphereColliders = GetComponents<SphereCollider>();
    }
}
