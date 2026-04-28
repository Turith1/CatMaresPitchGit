// DamageOnTouch.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;
    public float cooldown = 2f; // pra não dar dano toda frame
    [SerializeField]
    private float lastHitTime = -999f;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private ActionGhosts _ghost;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        lastHitTime = cooldown;
        _ghost = GetComponent<ActionGhosts>();
    }

    private void Update()
    {
        if(lastHitTime >= 0.0f)
        {
            lastHitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (lastHitTime >= 0) return;

        _anim.SetTrigger("Hit 0");

        var health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            lastHitTime = cooldown;
            health.TakeDamage(damage, transform.position);
        }
    }
}
