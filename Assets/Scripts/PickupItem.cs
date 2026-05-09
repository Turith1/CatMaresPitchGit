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
    [SerializeField]
    private GameObject _turnOffItem;


    void Reset() { GetComponent<Collider>().isTrigger = true; }
    void Update() { if (autoRotate) transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f); }

    void OnTriggerEnter(Collider other)
    {
        var effects = other.GetComponent<PlayerEffectsManager>();
        if (effects == null || item == null) return;

        effects.Apply(item);

        if (item.pickupSfx) AudioSource.PlayClipAtPoint(item.pickupSfx, transform.position);
        if (item.pickupVfxPrefab) Instantiate(item.pickupVfxPrefab, transform.position, Quaternion.identity);

        if(_proc != null)
            _proc.RespawnItem(transform, _turnOffItem);
    }
}
