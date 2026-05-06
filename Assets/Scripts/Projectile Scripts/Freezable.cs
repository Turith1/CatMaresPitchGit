using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Freezable : MonoBehaviour
{
    [Header("O que desativar ao congelar")]
    public MonoBehaviour[] aiScripts; 
    public NavMeshAgent agent;
    public Animator animator;
    [SerializeField]
    private ActionGhosts _actionGhost;

    [Header("Feedback visual (opcional)")]
    public Renderer[] renderers;
    public Color freezeTint = new Color(0.5f, 0.8f, 1f, 1f);
    public GameObject iceVfxPrefab;

    bool frozen;
    float unfreezeAt;

    public void ApplyFreeze(float seconds)
    {
        if (seconds <= 0f) return;

        Debug.Log(seconds);

        if (!frozen)
        {
            StartCoroutine(FreezeRoutine(seconds));
        }
        else
        {
            
            unfreezeAt = Mathf.Max(unfreezeAt, Time.time + seconds);
        }
    }

    IEnumerator FreezeRoutine(float seconds)
    {
        _actionGhost._isPersuing = false;
        IsFrozen = true;
        unfreezeAt = Time.time + seconds;

        // para movimento/IA
        if (agent) { agent.isStopped = true; agent.ResetPath(); agent.velocity = Vector3.zero;}

        if (animator) animator.speed = 0f;
        foreach (var s in aiScripts) if (s) s.enabled = false;

        // feedback
        if (iceVfxPrefab) Instantiate(iceVfxPrefab, transform.position, Quaternion.identity, transform);
        Tint(true);

        while (Time.time < unfreezeAt) yield return null;

        // volta ao normal
        if (agent) agent.isStopped = false;
        if (animator) animator.speed = 1f;
        foreach (var s in aiScripts) if (s) s.enabled = true;
        Tint(false);

        _actionGhost._isPersuing = true;
        _actionGhost._agenteFantasma.SetDestination(_actionGhost.player.position);

        if (_actionGhost.IsPlayerInRange())
        {
            animator.SetBool("Chase", true);
        }
        else
            animator.SetBool("Chase", false);

        IsFrozen = false;
    }

    void Tint(bool on)
    {
        if (renderers == null || renderers.Length == 0) renderers = GetComponentsInChildren<Renderer>();
        foreach (var r in renderers)
        {
            foreach (var m in r.materials)
            {
                if (m.HasProperty("_Color"))
                {
                    var c = m.color;
                    m.color = on ? Color.Lerp(c, freezeTint, 0.6f) : new Color(c.r, c.g, c.b, c.a);
                }
            }
        }
    }
    public bool IsFrozen { get; private set; }
}
