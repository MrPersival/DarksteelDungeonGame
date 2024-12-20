using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingGeneralMeleeEnemyState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    public float distanceToStartLoSCheck = 70f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.DrawRay(animator.transform.position + Vector3.up * 0.5f, player.position - animator.transform.position + Vector3.up * 0.5f, Color.red);
        float distanceToPlayer = Vector3.Distance(player.position, animator.transform.position);
        float angleToPlayer = Vector3.Angle(animator.transform.forward, player.position - animator.transform.position);
        if (distanceToPlayer <= distanceToStartLoSCheck)
        {
            RaycastHit hit;
            //Debug.Log("Sending raycast");
            if (Physics.Raycast(animator.transform.position + Vector3.up * 0.5f, player.position - animator.transform.position + Vector3.up * 0.5f, out hit))
            {
                if (hit.transform == player)
                {
                    if(agent.isActiveAndEnabled) agent.SetDestination(player.position);
                    //Debug.Log("Set destination to player");
                }
                else if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (agent.isActiveAndEnabled) agent.SetDestination(animator.transform.position);
                    animator.SetBool("isChasing", false);
                }
            }
        }

        if (distanceToPlayer <= animator.GetFloat("distanceToAttack"))
        {
            if (Mathf.Abs(angleToPlayer) > 10)
            {
                Vector3 directionToRotate = (player.position - animator.transform.position).normalized;
                Quaternion rotationTarget = Quaternion.LookRotation(directionToRotate);
                animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, rotationTarget, Time.deltaTime * agent.angularSpeed / 10);
            }
            else
            {
                if (agent.isActiveAndEnabled) agent.SetDestination(animator.transform.position);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
            }
        }
        //Debug.Log(agent.destination);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isChasing", false);
    }
}
