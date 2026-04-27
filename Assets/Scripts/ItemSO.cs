using UnityEngine;

public enum ItemEffectType { Heal, Invisibility, SpeedBoost, Garrafa , CaptureDevice /* adicione tipos novos aqui */ }

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]
public class ItemSO : ScriptableObject
{
    [Header("Identidade")]
    public string id = "item.id";
    public string displayName;
    public Sprite icon;

    [Header("Efeito")]
    public ItemEffectType type;
    [Tooltip("Valor principal (ex: qtd de cura, multiplicador de velocidade).")]
    public float value = 1f;
    [Tooltip("Duração em segundos (0 = instantâneo).")]
    public float duration = 0f;

    [Header("Acúmulo")]
    public bool canStack = false;
    public int maxStacks = 1;
    public bool refreshDurationOnGain = true;

    [Header("Feedback (opcional)")]
    public AudioClip pickupSfx;
    public GameObject pickupVfxPrefab;
}
