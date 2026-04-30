using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GunPickup : MonoBehaviour
{
    public float duration = 15f; 
    public AudioClip pickupSfx;
    public GameObject pickupVfx;
    public bool autoRotate = true;
    public float rotateSpeed = 60f;
    public ProcagemPowerUps _proc;

    void Reset() { GetComponent<Collider>().isTrigger = true; }

    void Update() { if (autoRotate) transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f); }

    void OnTriggerEnter(Collider other)
    {
        var shooter = other.GetComponent<FreezeShooter>();
        if (!shooter) return;

        if (duration <= 0f) shooter.canShoot = true;
        else shooter.GrantTemporarily(duration);

        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        if (pickupVfx) Instantiate(pickupVfx, transform.position, Quaternion.identity);

        //StartCoroutine(_proc.RespawnItem(transform, this.gameObject));
        _proc.RespawnItem(transform, this.gameObject);
        //StartCoroutine(CallRespawn());
    }

    private IEnumerator CallRespawn()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        _proc.RespawnItem(transform, this.gameObject);
    }
}
