using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class AttackingGeneralMeleeEnemyState : StateMachineBehaviour
{
    Transform player;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float angle = Vector3.Angle(animator.transform.forward, player.position - animator.transform.position);
        //Debug.Log(angle);
        if (Vector3.Distance(player.position, animator.transform.position) >= animator.GetFloat("distanceToAttack") || Mathf.Abs(angle) > 10)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isAttacking", false);

    }
}
