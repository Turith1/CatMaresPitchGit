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
    public MenuManager _menuManager;

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
        if(other.CompareTag("Player"))
            TryDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
            TryDamage(other);
    }

    void TryDamage(Collider other)
    {
        if (_menuManager._isPaused)
            return;

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
