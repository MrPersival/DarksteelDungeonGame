using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class RunBackGenericEnemyState : StateMachineBehaviour
{
    float oldNavMeshAgentRotation = 0f;
    NavMeshAgent agent;
    Transform player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.gameObject.GetComponent<NavMeshAgent>();
        oldNavMeshAgentRotation = agent.angularSpeed;
        agent.angularSpeed = 0;
        Debug.Log(agent.destination);
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(animator.transform.position + Vector3.Normalize(animator.transform.position - player.position) * 2, path);
        if (Vector3.Distance(player.position, animator.transform.position) >= animator.GetFloat("distanceToRunBack") || path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            animator.SetBool("isRunningAway", false);
            animator.SetBool("isChasing", true);
            animator.SetBool("isAttacking", false);
            //agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(animator.transform.position + Vector3.Normalize(animator.transform.position - player.position) * 2);
            Debug.Log(agent.destination);
            Debug.Log(agent.pathStatus);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.angularSpeed = oldNavMeshAgentRotation;
    }
}
