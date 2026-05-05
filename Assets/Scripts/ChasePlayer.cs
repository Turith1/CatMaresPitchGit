using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : StateMachineBehaviour
{
    [SerializeField]
    private EnemyNavMesh enemy;
    [SerializeField]
    private ActionGhosts enemyController;
    [SerializeField]
    private float _agentSpeed = 3.5f;
    [SerializeField]
    private PlayerCapture _playerCapture;
    Transform bestSpot = null;
    float maxDistance = -1;
    public MenuManager _menuManager;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<EnemyNavMesh>();
        enemyController = animator.GetComponentInParent<ActionGhosts>();
        _playerCapture = enemyController.player.GetComponent<PlayerCapture>();
        if (!_playerCapture.canCapture)
        {
            enemy.m_agent.SetDestination(enemyController.player.position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyController.player == null) return;

        if (enemy._menuManager._isPaused && !enemy.m_agent.isStopped)
        {
            enemy.m_agent.isStopped = true;
            return;
        }
        if (!enemy._menuManager._isPaused && enemy.m_agent.isStopped)
            enemy.m_agent.isStopped = false;

        if (!_playerCapture.canCapture)
        {
            if(enemy.m_agent.remainingDistance <= enemy.m_agent.stoppingDistance)
            {
                enemy.m_agent.SetDestination(enemyController.player.position);
            }
        }
        else
        {
            if (_playerCapture.canCapture)
            {
                if (!enemy.m_agent.pathPending && enemy.m_agent.remainingDistance <= enemy.m_agent.stoppingDistance)
                {
                    float maxDistance = 0f;
                    Transform bestSpot = null;

                    foreach (Transform spot in enemy._rotaEnemy)
                    {
                        float distFromPlayer = Vector3.Distance(
                            spot.position,
                            enemyController.player.position);

                        if (distFromPlayer > maxDistance)
                        {
                            maxDistance = distFromPlayer;
                            bestSpot = spot;
                        }
                    }

                    if (bestSpot != null)
                    {
                        NavMeshPath path = new NavMeshPath();

                        if (enemy.m_agent.CalculatePath(bestSpot.position, path) &&
                            path.status == NavMeshPathStatus.PathComplete)
                        {
                            enemy.m_agent.SetDestination(bestSpot.position);
                        }
                    }
                }
            }
        }

        if (!enemyController.IsPlayerInRange())
        {
            enemyController._ronda.m_agent.speed = _agentSpeed;
            animator.SetBool("Chase", false);
            return;
        }
    }
}
