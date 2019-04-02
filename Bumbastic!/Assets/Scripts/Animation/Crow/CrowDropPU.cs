﻿using UnityEngine;

public class CrowDropPU : StateMachineBehaviour
{
    [SerializeField] float timeToDrop;
    [SerializeField] InGame inGame;
    float distance;
    Vector3 velocity;
    float smoothTime = 0.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        distance = Vector3.Distance(animator.transform.position, inGame.posDropPU.position);
        Vector3 dir = (inGame.posDropPU.position - animator.transform.position).normalized;
        animator.transform.LookAt(dir);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.MoveTowards(animator.transform.position, inGame.posDropPU.position, (Time.deltaTime * distance)/timeToDrop);
        if (animator.transform.position == inGame.posDropPU.position)
        {
            if (GameManager.manager.powerUp.transform.parent != null) {
                GameManager.manager.powerUp.transform.parent = null;
            }
            animator.SetBool("PUDropped",true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("PUDropped", false);
    }
}
