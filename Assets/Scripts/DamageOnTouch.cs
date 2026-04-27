// DamageOnTouch.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageOnTouch : MonoBehaviour
{
    public int damage = 1;
    public float cooldown = 0.5f; // pra não dar dano toda frame
    private float lastHitTime = -999f;

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
        if (Time.time - lastHitTime < cooldown) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            lastHitTime = Time.time;
            health.TakeDamage(damage, transform.position);
        }
    }
}
