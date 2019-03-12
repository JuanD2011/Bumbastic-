﻿using UnityEngine;

public class CrowIdle : StateMachineBehaviour
{
    float t = 0f;
    [SerializeField] float flyingVel;
    Vector3 dir;
    [SerializeField] InGame inGame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        inGame.CrowPos = animator.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        t += Time.deltaTime;
        animator.transform.parent.eulerAngles = new Vector3(0, t * flyingVel, 0);
        dir = animator.transform.parent.position - animator.transform.position;
        dir.Normalize();
        if (animator.transform.childCount == 2 && Mathf.Round(dir.x) == -1 && Mathf.Round(dir.z) == 0)
        {
            animator.SetBool("DropPU", true);
        }
        //else if (Mathf.Round(dir.x) == 0 && Mathf.Round(dir.z) == 1 && !GameManager.instance.powerUp.gameObject.activeInHierarchy)
        //{
        //    GameManager.instance.powerUp.transform.parent = animator.gameObject.transform;
        //    GameManager.instance.powerUp.transform.position = animator.transform.GetChild(0).position;
        //    GameManager.instance.powerUp.gameObject.SetActive(true);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("DropPU", false);
    }
}
