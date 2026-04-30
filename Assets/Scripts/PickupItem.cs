using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    public ItemSO item;
    public bool autoRotate = true;
    public float rotateSpeed = 60f;
    public bool destroyOnPickup = true;
    public ProcagemPowerUps _proc;


    void Reset() { GetComponent<Collider>().isTrigger = true; }
    void Update() { if (autoRotate) transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f); }

    void OnTriggerEnter(Collider other)
    {
        var effects = other.GetComponent<PlayerEffectsManager>();
        if (effects == null || item == null) return;

        effects.Apply(item);

        if (item.pickupSfx) AudioSource.PlayClipAtPoint(item.pickupSfx, transform.position);
        if (item.pickupVfxPrefab) Instantiate(item.pickupVfxPrefab, transform.position, Quaternion.identity);

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
