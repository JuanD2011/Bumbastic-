using UnityEngine;

public class CrowIdle : StateMachineBehaviour
{
    [SerializeField] float flyingVel;
    [SerializeField] InGame inGame;
    [SerializeField] AnimatorOverrideController animatorOverrideController;
    AnimatorOverrideController original;

    float t = 0f;
    Vector3 dir;

    Vector3 posPowerUpBox = new Vector3(0, -1.4f, 0);

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

        if (GameManager.manager.powerUp.transform.parent == animator.transform)
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
        else if (Mathf.Round(dir.x) == 0 && Mathf.Round(dir.z) == 1 && !GameManager.manager.powerUp.gameObject.activeInHierarchy)
        {
            GameManager.manager.powerUp.Collider.enabled = false;
            GameManager.manager.powerUp.transform.parent = animator.transform;
            GameManager.manager.powerUp.transform.position = animator.transform.position + posPowerUpBox;
            GameManager.manager.powerUp.gameObject.SetActive(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("DropPU", false);
    }
}
