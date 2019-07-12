using UnityEngine;

public class Bummie : MonoBehaviour
{
    [SerializeField] Transform catapult = null;
    Animator m_Animator = null;
    SphereCollider[] m_sphereColliders = new SphereCollider[0];

    public Transform Catapult { get => catapult; set => catapult = value; }
    public Animator Animator { get => m_Animator; set => m_Animator = value; }
    public SphereCollider[] SphereColliders { get => m_sphereColliders; set => m_sphereColliders = value; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SphereColliders = GetComponents<SphereCollider>();
    }
}
