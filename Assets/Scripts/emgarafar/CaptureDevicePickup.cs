using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CaptureDevicePickup : MonoBehaviour
{
    public float duration = 0f; // 0 = permanente, >0 = temporário
    public AudioClip pickupSfx;
    public GameObject pickupVfx;

    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void OnTriggerEnter(Collider other)
    {
        var cap = other.GetComponent<PlayerCapture>();
        if (!cap) return;

        if (duration <= 0f) cap.canCapture = true;
        else cap.GrantTemporarily(duration);

        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);

        //Destroy(gameObject);
    }
}
