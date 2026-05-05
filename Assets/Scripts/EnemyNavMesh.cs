using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
   [SerializeField ] public Transform m_transform;
    [SerializeField]public NavMeshAgent m_agent;
    public Transform[] _rotaEnemy;
    //[SerializeField] private bool _estaEmRonda = true;
    [SerializeField] public int _rotaAtual = 0;
    public MenuManager _menuManager;

    public void Start()    
    {
        m_agent = GetComponent<NavMeshAgent>();
        //Ronda();
    }

    public void Ronda()
    {
        _rotaAtual = Random.Range(0, _rotaEnemy.Length);
        m_agent.destination = _rotaEnemy[_rotaAtual].position;
        //Quando o inimigo chegar no way point, mandar para o novo point aleatório do array.
    }

    /*public void FixedUpdate()
    {
        //Debug.Log((_rotaEnemy[_rotaAtual].position - transform.position).magnitude);
        if(m_agent.remainingDistance <= m_agent.stoppingDistance && m_agent.destination == _rotaEnemy[_rotaAtual].position)
        {
            Ronda();
        }
    }*/
}
