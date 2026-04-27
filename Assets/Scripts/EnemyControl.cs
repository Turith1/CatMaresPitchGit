using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;

public class EnemyControl : StateMachineBehaviour
{

    EnemyNavMesh _pattrol;
    public float _distance;
    ActionGhosts enemyController;
    [SerializeField] EnemyNavMesh _ronda;
    


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        _pattrol = animator.GetComponentInParent<EnemyNavMesh>();
        enemyController = animator.GetComponentInParent<ActionGhosts>();
        _pattrol.Ronda();

        /*if(enemyController == null && _pattrol != null)
        {
            _pattrol.enabled = true;            
            _target = enemyController.targetRb;
            _distance = enemyController._distanceEnemy;
        }*/
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyController.IsPlayerInRange())
        {
            animator.SetBool("Chase", true);
            return;
        }

        if (_pattrol.m_agent.remainingDistance <= _pattrol.m_agent.stoppingDistance)
        {
            _pattrol.Ronda();
        }

        /*if (!enemyController._ronda.m_agent.pathPending && enemyController._ronda.m_agent.remainingDistance < 1)
        {
            _pattrol.Ronda();
        }*/
        /* if (_moveEnemy != null && _target != null)
         {
             animator.SetBool("Chase", true);
             _pattrol.enabled = false;
             // Se a distância for maior que a distância de perseguição, volta ao estado de Patrulha.
            /* if ((float)new Vector3.Distance(distancia.position, _target.position) > _distance)
             {
                 animator.SetBool("Chase", false);
             }   */

        /* else
         {
             animator.SetBool("Chase", false);
             _pattrol.enabled = true;
             _ronda.enabled = true;
             Debug.Log(_pattrol);
         }*/

        /* if (_pattrol = null)
         {
             animator.SetBool("Chase", true);
         }*/
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //  
    //}



    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
