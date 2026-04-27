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
    /*private void Start()
    {
        Rigidbody EnemyRb = GetComponent<Rigidbody>();
    }*/
    void Update()
    {
        /*RaycastHit hit;
        if (Physics.SphereCast(EnemyRb, 5, transform.forward, out hit, 5))
        {


        }*/
        // float enemyVision = 0;



    }

    void OnDrawGizmos()
    {
        // Set the color for the Gizmos
        Gizmos.color = Color.blue;

        // Draw the sphere at the origin of the cast
        Gizmos.DrawWireSphere(transform.position, 5);

        // Draw the path of the spherecast
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);

        // If the SphereCast hit something, draw a sphere at the hit point
        /*if (_hasHit)
        {
            Gizmos.color = Color.red; // Change color for the hit sphere
            Gizmos.DrawWireSphere(hit.point, 5); // Draw sphere at the hit point
            Gizmos.DrawLine(transform.position, hit.point); // Draw a line to the hit point
        }*/
    }

    private void CreateEnemy()
    {
        if (player != null)
        {
            //GameObject randomGhost = _ghostPref[Random.Range(0, _ghostPref.Length)];
            //Vector3 randomPosition = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;
            Vector3 randomPosition = _spawnPoints[0].position;

            GameObject fantasmas = Instantiate(_ghostPref[0], transform.position, Quaternion.identity);
            Debug.Log(fantasmas.transform.position);
            fantasmas.GetComponent<EnemyNavMesh>()._rotaEnemy = _wayPoints;
            BottleEffect bottleEffect = fantasmas.GetComponent<BottleEffect>();
            bottleEffect._player = player;
            //timer = 0;
        }
    }
}
