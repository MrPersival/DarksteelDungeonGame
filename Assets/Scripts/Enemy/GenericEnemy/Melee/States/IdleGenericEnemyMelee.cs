using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleGenericEnemyMelee : StateMachineBehaviour
{
    public float secondsToStartPatrolFromIdle = 5f;
    public float distanceToStartLoSCheck = 50f; //LoS = line of sight
    public float distanceToAttack = 1.5f;
    Transform player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /* timer += Time.deltaTime;
        Debug.Log("Setting patroling to true");
        if (timer > secondsToStartPatrolFromIdle)
        {
            animator.SetBool("isPatrolling", true);
        } */
        Debug.DrawRay(animator.transform.position + Vector3.up * 0.5f, player.position - animator.transform.position + Vector3.up * 0.5f, Color.red);
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        if(distanceToPlayer <= distanceToStartLoSCheck) 
        {
            RaycastHit hit;
            //Debug.Log("Sending raycast");
            if (Physics.Raycast(animator.transform.position + Vector3.up * 0.5f, player.position - animator.transform.position + Vector3.up * 0.5f, out hit))
            {
                //Debug.Log("Hit: " + hit.transform.name);
                if(hit.transform == player)
                {
                    if (distanceToPlayer <= distanceToAttack) animator.SetBool("isAttacking", true);
                    else animator.SetBool("isChasing", true);
                }
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
