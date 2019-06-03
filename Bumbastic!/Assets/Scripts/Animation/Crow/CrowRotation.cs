using System.Collections;
using UnityEngine;

public class CrowRotation : MonoBehaviour
{
    Vector3 antPos;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        antPos = transform.position;
        StartCoroutine(CrowSound());
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Crow_Flying"))
        {
            transform.LookAt(-antPos + (2 * transform.position));
            antPos = transform.position; 
        }
    }

    IEnumerator CrowSound()
    {
        yield return new WaitForSeconds(Random.Range(10, 15f));
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crow, 0.8f);
        StartCoroutine(CrowSound());
    }
}
