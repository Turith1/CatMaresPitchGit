using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Ghosts : MonoBehaviour
{
    public GameObject[] _ghostPref;
    public Transform[] _spawnPoints;
    [SerializeField] private Transform[] _wayPoints;

    public Transform player;
    [SerializeField] private PlayerEffectsManager _effectManager;
    [SerializeField] private GameObject _enemyDet;
    [SerializeField] private Rigidbody _enemyDiscance;
    bool _hasHit;
    [SerializeField] private SpherecastCommand _spawnCommand;
    float timer;
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody _targetRb;

    

    public Vector3 EnemyRb { get; private set; }


    private void Awake()
    {
        CreateEnemy();
    }

    void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.blue;

        // Draw the sphere at the origin of the cast
        Gizmos.DrawWireSphere(transform.position, 5);

        // Draw the path of the spherecast
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }

    private void CreateEnemy()
    {
        if (player != null)
        {
            int pos = Random.Range(0, _spawnPoints.Length);
            Vector3 randomPosition = _spawnPoints[pos].position;
            GameObject fantasmas = Instantiate(_ghostPref[0], randomPosition, Quaternion.identity);
            fantasmas.GetComponent<EnemyNavMesh>()._rotaEnemy = _wayPoints;
            BottleEffect bottleEffect = fantasmas.GetComponent<BottleEffect>();
            bottleEffect._player = player;
        }
    }
}
