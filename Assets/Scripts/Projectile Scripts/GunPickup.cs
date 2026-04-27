using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GunPickup : MonoBehaviour
{
    public float duration = 15f; 
    public AudioClip pickupSfx;
    public GameObject pickupVfx;

    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void OnTriggerEnter(Collider other)
    {
        var shooter = other.GetComponent<FreezeShooter>();
        if (!shooter) return;

        if (duration <= 0f) shooter.canShoot = true;
        else shooter.GrantTemporarily(duration);

        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
