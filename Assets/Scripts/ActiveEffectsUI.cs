using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveEffectsUI : MonoBehaviour
{
  
    [System.Serializable]
    public struct EffectIconMap
    {
        public ItemEffectType type;
        public Sprite icon;
    }

    public PlayerEffectsManager manager;
    public Transform container;      // normalmente o próprio EffectsPanel
    public GameObject iconPrefab;    // o prefab EffectIcon
    public EffectIconMap[] icons;    // mapeie Heal/Invisibility/SpeedBoost -> sprites

    Dictionary<ItemEffectType, GameObject> views = new();
    void Awake()
    {
        if (iconPrefab == null)
        {
            Debug.LogError("[ActiveEffectsUI] 'iconPrefab' não atribuído no objeto " + name);
            // Opcional: desative para não tentar instanciar nada
            // enabled = false;
        }
    }
    void Start()
    {
        if (manager == null) manager = FindObjectOfType<PlayerEffectsManager>();
        manager.OnEffectToggled += OnEffectToggled;

        SyncExisting();
    }
    void SyncExisting()
    {
        foreach (ItemEffectType t in Enum.GetValues(typeof(ItemEffectType)))
        {
            if (manager.TryGetActive(t, out _, out _, out _))
            {
                OnEffectToggled(t, true);
            }
        }
    }
    void OnDestroy()
    {
        if (manager != null) manager.OnEffectToggled -= OnEffectToggled;
    }

    void OnEffectToggled(ItemEffectType type, bool enabled)
    {
        Debug.Log($"[UI] Toggle {type} -> {(enabled ? "ON" : "OFF")}");
        if (enabled)
        {
            if (views.ContainsKey(type)) return;
            if (iconPrefab == null) return;
            var go = Instantiate(iconPrefab, container);
            var sp = FindIcon(type);
            var fillTr = go.transform.Find("Fill");
            var fillImg = fillTr ? fillTr.GetComponent<Image>() : null;
            var rootImg = go.GetComponent<Image>();
            if (rootImg) rootImg.enabled = false;
            // define o ícone visual
            var img = go.GetComponent<Image>();
            if (fillImg && sp != null)
            {
                fillImg.sprite = sp;
                fillImg.preserveAspect = true;          // opcional, para manter proporção
                fillImg.type = Image.Type.Filled;       // garante que é Filled
                fillImg.fillMethod = Image.FillMethod.Radial360;
                fillImg.fillAmount = 1f;                // começa cheio
            }
            else
            {
                Debug.LogError("[ActiveEffectsUI] Falta filho 'Fill' com Image no prefab, ou sprite não mapeada.");
            }
            if (img != null)
            {
                
                if (sp != null) img.sprite = sp;
            }

            views[type] = go;
        }
        else
        {
            if (views.TryGetValue(type, out var go)) { Destroy(go); views.Remove(type); }
        }

    }

    Sprite FindIcon(ItemEffectType type)
    {
        foreach (var m in icons) if (m.type == type) return m.icon;
        return null;
    }

    void Update()
    {
        foreach (var kv in views)
        {
            var type = kv.Key; var go = kv.Value;

            // atualiza barra/contador
            if (manager.TryGetActive(type, out float rem, out int stacks, out float dur))
            {
                // Fill radial (0..1)
                var fill = go.transform.Find("Fill")?.GetComponent<Image>();
                if (fill && dur > 0f) fill.fillAmount = rem / dur;

                // Texto de stacks
                var stack = go.transform.Find("Stack");
#if TMP_PRESENT
                var tmp = stack?.GetComponent<TMPro.TMP_Text>();
                if (tmp) tmp.text = stacks > 1 ? $"x{stacks}" : "";
#else
                var txt = stack?.GetComponent<UnityEngine.UI.Text>();
                if (txt) txt.text = stacks > 1 ? $"x{stacks}" : "";
#endif
            }
        }
    }
}
