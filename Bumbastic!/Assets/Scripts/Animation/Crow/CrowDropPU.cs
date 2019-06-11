using UnityEngine;

public class CrowDropPU : StateMachineBehaviour
{
    [SerializeField] float timeToDrop = 0f;
    [SerializeField] InGame inGame = null;
    float distance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        distance = Vector3.Distance(animator.transform.position, inGame.posDropPU.position);
        Vector3 dir = (inGame.posDropPU.position - animator.transform.position).normalized;
        animator.transform.LookAt(dir);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.crow, 0.8f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, inGame.posDropPU.position, (Time.deltaTime * distance)/timeToDrop);
        if (animator.transform.position == inGame.posDropPU.position)
        {
            if (HotPotatoManager.HotPotato.PowerUp.transform.parent != null)
            {
                HotPotatoManager.HotPotato.PowerUp.transform.eulerAngles = Vector3.zero;
                HotPotatoManager.HotPotato.PowerUp.transform.position = inGame.posDropPU.position;
                HotPotatoManager.HotPotato.PowerUp.Collider.enabled = true;
                HotPotatoManager.HotPotato.PowerUp.transform.parent = null;
                AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.powerUpBoxDropped, 0.6f);
            }

            if (!animator.GetBool("PUDropped"))
            {
                animator.SetBool("PUDropped", true); 
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("PUDropped", false);
    }
}
