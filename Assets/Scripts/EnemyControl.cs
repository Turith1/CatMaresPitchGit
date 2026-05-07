using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;
//using Transform = UnityEngine.Transform;

public class EnemyControl : StateMachineBehaviour
{

    EnemyNavMesh _pattrol;
    public float _distance;
    ActionGhosts enemyController;
    [SerializeField] EnemyNavMesh _ronda;
    [SerializeField]
    private float _agentSpeed = 5.5f;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _pattrol = animator.GetComponentInParent<EnemyNavMesh>();
        enemyController = animator.GetComponentInParent<ActionGhosts>();
        _pattrol.Ronda();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_pattrol._menuManager._isPaused && !_pattrol.m_agent.isStopped)
        {
            _pattrol.m_agent.isStopped = true;
            return;
        }
        if (!_pattrol._menuManager._isPaused && _pattrol.m_agent.isStopped)
            _pattrol.m_agent.isStopped = false;

        if (_pattrol.m_agent.remainingDistance <= _pattrol.m_agent.stoppingDistance)
        {
            _pattrol.Ronda();
        }

        if (enemyController.IsPlayerInRange())
        {
            _pattrol.m_agent.speed = _agentSpeed;
            animator.SetBool("Chase", true);
            return;
        }

    }
}
