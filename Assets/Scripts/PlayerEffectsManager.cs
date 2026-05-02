using UnityEngine;
using System.Collections.Generic;
using System.Transactions;

public class PlayerEffectsManager : MonoBehaviour
{
    [Header("Referências")]
    public PlayerHealth health;              
    public BottleEffect _bottle;
    public Movement  movementScript;     
    public string moveSpeedFieldName = "moveSpeed"; // nome do campo float no  controller
    [SerializeField]
    private Material _catMat, _catMatInvisible;

    [Header("Invisibilidade")]
    public bool useLayerSwapForInvisibility = true;
    public string invisibleLayerName = "PlayerInvisible";
    public bool _isInvisible = false;

    [Header("Speed Boost")]
    [SerializeField]
    private float _initialSpeed = 6.58f;

    int originalLayer;
    Renderer renderers;
    public bool debugAutoApplyAtStart = false;
    public ItemSO debugItemAtStart;

    void Start()
    {
        if (debugAutoApplyAtStart && debugItemAtStart != null)
        {
            Debug.Log("[PEM] AutoApply: " + debugItemAtStart.displayName);
            Apply(debugItemAtStart);
        }
    }

    class Active
    {
        public ItemSO item;
        public float endTime;
        public int stacks = 1;
    }

    readonly Dictionary<ItemEffectType, Active> actives = new();
    readonly Dictionary<ItemEffectType, System.Action> endCallbacks = new();

   
    public System.Action<ItemSO> OnItemApplied;
    public System.Action<ItemEffectType, bool> OnEffectToggled; // ligado/desligado

    void Awake()
    {
        if (health == null) health = GetComponent<PlayerHealth>();
        GameObject catMat = transform.GetChild(0).GetChild(1).gameObject;
        renderers = catMat.GetComponent<Renderer>();
        originalLayer = gameObject.layer;
    }

    void Update()
    {
        if (actives.Count == 0) return;

        List<ItemEffectType> toStop = null;
        foreach (var kv in actives)
        {
            var type = kv.Key; var active = kv.Value;
            if (active.item.duration <= 0) continue;
            if (Time.time >= active.endTime) (toStop ??= new()).Add(type);
        }
        if (toStop != null) foreach (var t in toStop) StopEffect(t);
    }

    public void Apply(ItemSO item)
    {
        Debug.Log($"[PEM] Apply {item.type}  dur={item.duration}  val={item.value}");

        switch (item.type)
        {
            case ItemEffectType.Heal:
                ApplyHeal(item);
                break;

            case ItemEffectType.Invisibility:
                /*StartOrStackTimed(item,
                    onStart: () => SetInvisible(true),
                    onEnd: () => SetInvisible(false));*/
                Invisible();
                break;

            case ItemEffectType.SpeedBoost:
                /*StartOrStackTimed(item,
                    onStart: () => ModifyMoveSpeed(item.value),          // multiplica
                    onEnd: () => ModifyMoveSpeed(1.5f / item.value)); */   // desfaz
                SpeedBoost();
                break;

            case ItemEffectType.CaptureDevice:
                var cap = GetComponent<PlayerCapture>();
                if (cap)
                    StartOrStackTimed(item,
                        onStart: () => cap.canCapture = true,
                        onEnd: () => cap.canCapture = false) ;
                break;

            default:
                Debug.LogWarning($"Tipo {item.type} não tratado.");
                break;
        }

        OnItemApplied?.Invoke(item);
    }

    // -------- Implementações de efeitos --------
    void ApplyHeal(ItemSO item)
    {
        int amount = Mathf.RoundToInt(item.value);
        if (health != null) health.Heal(amount);
        OnEffectToggled?.Invoke(ItemEffectType.Heal, true);
        Invoke("EndHealVisual", 3);
    }

    private void EndHealVisual()
    {
        OnEffectToggled?.Invoke(ItemEffectType.Heal, false);
    }

    void EffectBottle()
    {
        Debug.Log("effectbottle");
        float value = 300;
        if (_bottle != null)
        {
            _bottle.Activate(value);
            _bottle.Scape();
        }

    }

    private void Invisible()
    {
        _isInvisible = true;
        renderers.material = _catMatInvisible;
        OnEffectToggled?.Invoke(ItemEffectType.Invisibility, true);
        Invoke("Visible", 5);
    }

    private void Visible()
    {
        OnEffectToggled?.Invoke(ItemEffectType.Invisibility, false);
        _isInvisible = false;
        renderers.material = _catMat;
    }

    void StartOrStackTimed(ItemSO item, System.Action onStart, System.Action onEnd)
    {
        if (!actives.TryGetValue(item.type, out var active))
        {
            active = new Active { item = item, endTime = Time.time + item.duration, stacks = 1 };
            actives[item.type] = active;
            onStart?.Invoke();
            Debug.Log($"[PEM] START {item.type} (stacks=1)");
            endCallbacks[item.type] = onEnd;
            OnEffectToggled?.Invoke(item.type, true);
        }
        else
        {
            if (item.canStack && active.stacks < item.maxStacks)
            {
                active.stacks++;
                Debug.Log($"[PEM] STACK {item.type} -> {active.stacks}");
                if (item.type == ItemEffectType.SpeedBoost) ModifyMoveSpeed(item.value); // empilha multiplicador
            }
            if (item.refreshDurationOnGain)
                active.endTime = Time.time + item.duration;
        }
    }

    void StopEffect(ItemEffectType type)
    {
        if (!actives.TryGetValue(type, out var active)) return;

        // desfaz empilhamento (caso SpeedBoost)
        if (type == ItemEffectType.SpeedBoost)
            for (int i = 0; i < active.stacks; i++) ModifyMoveSpeed(1f / active.item.value);

        if (endCallbacks.TryGetValue(type, out var cb)) cb?.Invoke();

        actives.Remove(type);
        endCallbacks.Remove(type);
        OnEffectToggled?.Invoke(type, false);
        Debug.Log($"[PEM] STOP {type}");
    }

    // -------- Utilitários --------
    /*void SetInvisible(bool v)
    {
        if (useLayerSwapForInvisibility)
        {
            int inv = LayerMask.NameToLayer(invisibleLayerName);
            gameObject.layer = v && inv != -1 ? inv : originalLayer;
        }

        // feedback visual: reduzir alpha (certifique-se que o shader permite transparência)
        foreach (var r in renderers)
        {
            foreach (var m in r.materials)
            {
                if (m.HasProperty("_Color"))
                {
                    var c = m.color; c.a = v ? 0.35f : 1f; m.color = c;
                }
            }
        }
    }*/

    void ModifyMoveSpeed(float multiplier)
    {
        if (movementScript == null) return;
        var t = movementScript.GetType();

        // tenta campo público
        var field = t.GetField(moveSpeedFieldName);
        if (field != null && field.FieldType == typeof(float))
        {
            float current = (float)field.GetValue(movementScript);
            field.SetValue(movementScript, current * multiplier);
            return;
        }

        // tenta propriedade pública
        var prop = t.GetProperty(moveSpeedFieldName);
        if (prop != null && prop.PropertyType == typeof(float))
        {
            float current = (float)prop.GetValue(movementScript);
            prop.SetValue(movementScript, current * multiplier);
            return;
        }

        Debug.LogWarning($"'{moveSpeedFieldName}' não encontrado como float em {t.Name}.");
    }

    private void SpeedBoost()
    {
        OnEffectToggled?.Invoke(ItemEffectType.SpeedBoost, true);
        movementScript._speed *= 2;
        Invoke("EndBoost", 3);
    }

    private void EndBoost()
    {
        OnEffectToggled?.Invoke(ItemEffectType.SpeedBoost, false);
        movementScript._speed = _initialSpeed;
    }

    // Para UI ler barra/tempo
    public bool TryGetActive(ItemEffectType type, out float remaining, out int stacks, out float duration)
    {
        if (actives.TryGetValue(type, out var a))
        {
            remaining = Mathf.Max(0, a.endTime - Time.time);
            stacks = a.stacks;
            duration = a.item.duration;
            return true;
        }
        remaining = stacks = 0; duration = 0;
        return false;
    }
}
