using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionGhosts : MonoBehaviour
{
    public Rigidbody enemyRb;
    public Rigidbody targetRb;
    public NavMeshAgent _agenteFantasma;
    public Transform player;
    private PlayerEffectsManager _playerEffects;
    private EnemyNavMesh _enemy;

    public GameObject powerUpPrefab;
    public GameObject zinimigo;
    public float _distanceEnemy = 20f;
    public float _distanceAttack = 4f;
    public EnemyNavMesh _ronda;
    public bool _isPersuing;
    public bool _isUpdating;

    public int life = 5;
    public int speed = 1;


    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        _agenteFantasma = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<EnemyNavMesh>();
        GameObject targetRb = GameObject.FindGameObjectWithTag("Player");
        _playerEffects = targetRb.GetComponent<PlayerEffectsManager>();
        if (targetRb != null)
            player = targetRb.transform;
    }

    private void FixedUpdate()
    {
        if (!_isUpdating)
            return;

        if(_isPersuing)
            _agenteFantasma.SetDestination(player.position);
        else
        {
            if (!_agenteFantasma.pathPending && _agenteFantasma.remainingDistance <= _agenteFantasma.stoppingDistance)
            {
                Debug.Log("running");
                float maxDistance = 0f;
                Transform bestSpot = null;

                foreach (Transform spot in _enemy._rotaEnemy)
                {
                    float distFromPlayer = Vector3.Distance(
                        spot.position,
                        player.position);

                    if (distFromPlayer > maxDistance)
                    {
                        maxDistance = distFromPlayer;
                        bestSpot = spot;
                    }
                }

                if (bestSpot != null)
                {
                    NavMeshPath path = new NavMeshPath();

                    if (_agenteFantasma.CalculatePath(bestSpot.position, path) &&
                        path.status == NavMeshPathStatus.PathComplete)
                    {
                        _agenteFantasma.SetDestination(bestSpot.position);
                    }
                }
            }
        }
    }

    public bool IsPlayerInRange()
    {
        if (player == null) return false;
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= _distanceEnemy && !_playerEffects._isInvisible;
    }

    public bool IsPlayerInAttackRange()
    {
        if (player == null) return false;
        float dist = Vector3.Distance(transform.position, player.position);
        Debug.Log(dist);
        return dist <= _distanceAttack;
    }

}
