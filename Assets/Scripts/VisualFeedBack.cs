using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualFeedBack : MonoBehaviour
{
    [System.Serializable]
    public struct EffectIconMap
    {
        public ItemEffectType type;
        public Sprite icon;
    }

    public PlayerEffectsManager manager;
    [SerializeField]
    private SpriteRenderer _effectImage;
    [SerializeField]
    private EffectIconMap[] icons;
    private float _timer;
    [SerializeField]
    private Sprite _stupidGun;
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null) manager = FindObjectOfType<PlayerEffectsManager>();
        manager.OnEffectToggled += OnEffectToggled;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
    }

    public void StupidGun(bool enable)
    {
        _effectImage.sprite = _stupidGun;
        _effectImage.gameObject.SetActive(enable);
    }

    void OnEffectToggled(ItemEffectType type, bool enabled)
    {
        Debug.Log("pick up type: " + type + " is enabled: " + enabled);
        _effectImage.sprite = FindIcon(type);
        _effectImage.gameObject.SetActive(enabled);
    }

    Sprite FindIcon(ItemEffectType type)
    {
        foreach (var m in icons) if (m.type == type) return m.icon;
        return null;
    }

    void OnDestroy()
    {
        if (manager != null) manager.OnEffectToggled -= OnEffectToggled;
    }
}
