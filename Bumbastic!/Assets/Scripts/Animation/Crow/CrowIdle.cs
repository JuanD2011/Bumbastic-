using UnityEngine;

public class CrowIdle : StateMachineBehaviour
{
    [SerializeField] float flyingVel;
    [SerializeField] InGame inGame;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    AnimatorOverrideController original;

    float t = 0f;
    Vector3 dir;

    Vector3 reference = new Vector3(0, -2f, 0);

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        inGame.CrowPos = animator.transform.position;

        if (original == null)
        {
            original = new AnimatorOverrideController(animator.runtimeAnimatorController);
        }

        animator.runtimeAnimatorController = original;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        t += Time.deltaTime;
        animator.transform.parent.eulerAngles = new Vector3(0, t * flyingVel, 0);
        dir = (animator.transform.parent.position - animator.transform.position).normalized;

        if (GameManager.Manager.powerUp.transform.parent == animator.transform)
        {
            if (animator.runtimeAnimatorController != animatorOverrideController)
            {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
            if (Mathf.Round(dir.x) == -1 && Mathf.Round(dir.z) == 0)
            {
                if (!animator.GetBool("DropPU"))
                {
                    animator.SetBool("DropPU", true); 
                } 
            }
        }
        else if (Mathf.Round(dir.x) == 0 && Mathf.Round(dir.z) == 1 && !GameManager.Manager.powerUp.gameObject.activeInHierarchy)
        {
            GameManager.Manager.powerUp.Collider.enabled = false;
            GameManager.Manager.powerUp.transform.parent = animator.transform;
            GameManager.Manager.powerUp.transform.position = animator.transform.position + reference;
            GameManager.Manager.powerUp.gameObject.SetActive(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("DropPU", false);
    }
}
