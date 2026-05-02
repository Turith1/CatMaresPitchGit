using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponentInParent<EnemyNavMesh>();
        enemyController = animator.GetComponentInParent<ActionGhosts>();
        _playerCapture = enemyController.player.GetComponent<PlayerCapture>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyController.player == null) return;


        if (!_playerCapture.canCapture)
        {
            Debug.Log(!_playerCapture.canCapture);
            enemy.m_agent.SetDestination(enemyController.player.position);
        }
        else if (_playerCapture.canCapture && !enemy.m_agent.pathPending)
        {
            FleeToBestSpot();
        }

        if (!enemyController.IsPlayerInRange())
        {
            enemyController._ronda.m_agent.speed = _agentSpeed;
            animator.SetBool("Chase", false);
            return;
        }
    }

    void FleeToBestSpot()
    {

        foreach (Transform spot in enemy._rotaEnemy)
        {
            // Calcula a distância entre este ponto e o JOGADOR
            float distFromPlayer = Vector3.Distance(spot.transform.position, enemyController.player.position);

            // Queremos o ponto que esteja MAIS LONGE do jogador
            if (distFromPlayer > maxDistance)
            {
                maxDistance = distFromPlayer;
                bestSpot = spot;
            }
        }

        if (bestSpot != null)
        {
            enemy.m_agent.SetDestination(bestSpot.transform.position);
        }
    }
}
