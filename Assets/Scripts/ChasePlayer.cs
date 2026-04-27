using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : StateMachineBehaviour
{
    [SerializeField]
    private EnemyNavMesh enemy;
    [SerializeField]
    private ActionGhosts enemyController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<EnemyNavMesh>();
        enemyController = animator.GetComponentInParent<ActionGhosts>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyController.player == null) return;

        if (!enemyController.IsPlayerInRange())
        {
            animator.SetBool("Chase", false);
            return;
        }

        enemy.m_agent.SetDestination(enemyController.player.position);
        if (!enemyController._ronda.m_agent.pathPending && enemyController._ronda.m_agent.remainingDistance < 0.5f)
        {
            enemy.Ronda();
        }
    }
}
