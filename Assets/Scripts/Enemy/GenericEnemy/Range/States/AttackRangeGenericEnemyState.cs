using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeGenericEnemyState : StateMachineBehaviour
{
    Transform player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector3.Distance(player.position, animator.transform.position) >= animator.GetFloat("distanceToAttack"))
        {
            animator.SetBool("isChasing", true);
        }
        if (Vector3.Distance(player.position, animator.transform.position) <= animator.GetFloat("distanceToRunBack"))
        {
            animator.SetBool("isRunningAway", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isAttacking", false);
    }
}
