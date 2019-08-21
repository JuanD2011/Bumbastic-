using UnityEngine;

public class CrowRotation : MonoBehaviour
{
    Vector3 antPos;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        antPos = transform.position;
        InvokeRepeating("CrowSound", Random.Range(10f, 15f), Random.Range(10f, 15f));
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crow_Flying"))
        {
            transform.LookAt(-antPos + (2 * transform.position));
            antPos = transform.position;
        }
    }

    private void CrowSound() { AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crow, 0.8f); }
}
