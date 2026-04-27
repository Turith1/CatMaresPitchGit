using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FreezeProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public LayerMask hitMask = ~0;   
    float speed;
    float freezeSeconds;
    Rigidbody rb;

    public void Init(float speed, float freezeSeconds)
    {
        this.speed = speed;
        this.freezeSeconds = freezeSeconds;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = true;
    }

    void Start()
    {
        rb.useGravity = false;
        rb.velocity = transform.forward * (speed > 0 ? speed : 20f);
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (((1 << other.gameObject.layer) & hitMask) == 0) return;

        var freezable = other.GetComponentInParent<Freezable>();
        if (freezable != null)
        {
            freezable.ApplyFreeze(freezeSeconds > 0 ? freezeSeconds : 2f);
        }

        Destroy(gameObject);
    }
}
