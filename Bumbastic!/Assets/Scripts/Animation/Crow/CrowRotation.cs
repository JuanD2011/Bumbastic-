using UnityEngine;

public class CrowRotation : MonoBehaviour
{
    Vector3 antPos;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        antPos = transform.position;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crow_Flying"))
        {
            transform.LookAt(-antPos + (2 * transform.position));
            antPos = transform.position; 
        }
    }
}
