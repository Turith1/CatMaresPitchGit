
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHearts = 3;
    public int currentHearts;

    [Header("Invulnerabilidade")]
    public float invulnDuration = 1.0f;     // tempo intocável após levar dano
    public float blinkInterval = 0.1f;      // piscar do mesh
    private bool isInvulnerable = false;

    [Header("Knockback (opcional)")]
    public Rigidbody rb;                    // arraste o Rigidbody do player aqui
    public float knockbackForce = 8f;

    [Header("Eventos")]
    public UnityEvent onHurt;
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged; // (current, max)

    Renderer[] renderers; // pra piscar

    void Awake()
    {
        currentHearts = maxHearts;
        renderers = GetComponentsInChildren<Renderer>(true);
        if (rb == null) rb = GetComponent<Rigidbody>();
        onHealthChanged?.Invoke(currentHearts, maxHearts);
    }

    public void Heal(int amount = 1)
    {
        currentHearts = Mathf.Clamp(currentHearts + amount, 0, maxHearts);
        onHealthChanged?.Invoke(currentHearts, maxHearts);
    }

    public void TakeDamage(int amount = 1, Vector3? damageSource = null)
    {
        if (isInvulnerable || currentHearts <= 0) return;

        currentHearts = Mathf.Max(0, currentHearts - amount);
        onHealthChanged?.Invoke(currentHearts, maxHearts);
        onHurt?.Invoke();

        // Knockback opcional
        if (rb && damageSource.HasValue)
        {
            Vector3 dir = (transform.position - damageSource.Value).normalized;
            dir.y = 0.25f; // leve lift
            rb.AddForce(dir * knockbackForce, ForceMode.VelocityChange);
        }

        if (currentHearts <= 0)
        {
            // morreu
            onDeath?.Invoke();
            Invoke("OnDeath", 2);
            // Desative controles aqui, se quiser
            // GetComponent<PlayerController>().enabled = false;
            return;
        }

        // Invulnerabilidade + blink
        StopAllCoroutines();
        StartCoroutine(InvulnerabilityRoutine());
    }

    public void OnDeath()
    {
        SceneManager.LoadScene("vICTORY");
    }

    System.Collections.IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        bool visible = true;

        while (elapsed < invulnDuration)
        {
            visible = !visible;
            foreach (var r in renderers)
            {
                foreach (var m in r.materials)
                    m.SetFloat("_Surface", m.GetFloat("_Surface")); // no-op p/ URP; abaixo usa enabled
                r.enabled = visible; // forma simples de piscar
            }

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        // garante visível
        foreach (var r in renderers) r.enabled = true;
        isInvulnerable = false;
    }
}
