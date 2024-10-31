using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGenericMelleeEnemy : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isHitAnimationPlaying", true);
        animator.SetBool("isHit", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isHitAnimationPlaying", false);
    }
}
