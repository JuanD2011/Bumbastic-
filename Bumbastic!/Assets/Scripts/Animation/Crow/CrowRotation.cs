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
        WaitForSeconds waitForSeconds = new WaitForSeconds(Random.Range(10f, 15f));
        yield return waitForSeconds;
        AudioManager.instance.PlayAudio(AudioManager.instance.audioClips.crow, AudioType.SFx);
        StartCoroutine(CrowSound());
    }
}
