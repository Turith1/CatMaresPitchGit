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
        if (targetRb != null)
            player = targetRb.transform;
    }

    /*void Update()
    {
        //Movement
            float direction = (targetRb.position - enemyRb.position).magnitude;
        if (direction <= _distanceEnemy )
        {
            Chase();
        }
        else
        {
            _ronda.Ronda();
        }
    }*/

    public bool IsPlayerInRange()
    {
        if (player == null) return false;
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= _distanceEnemy;
    }

    public bool IsPlayerInAttackRange()
    {
        if (player == null) return false;
        float dist = Vector3.Distance(transform.position, player.position);
        return dist <= _distanceAttack;
    }

}
