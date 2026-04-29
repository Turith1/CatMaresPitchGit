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

    public GameObject powerUpPrefab;
    public GameObject zinimigo;
    public float _distanceEnemy = 20f;
    public float _distanceAttack = 4f;
    public EnemyNavMesh _ronda;

    public int life = 5;
    public int speed = 1;


    void Awake()
    {
        enemyRb = GetComponent<Rigidbody>();
        _agenteFantasma = GetComponent<NavMeshAgent>();
        GameObject targetRb = GameObject.FindGameObjectWithTag("Player");
        _playerEffects = targetRb.GetComponent<PlayerEffectsManager>();
        if (targetRb != null)
            player = targetRb.transform;
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
