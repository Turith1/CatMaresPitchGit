using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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
    private Sprite[] _stupidGun;
    [SerializeField]
    private Sprite[] _alternateHeal;
    [SerializeField]
    private Sprite[] _speedBoostSprites;
    private bool _isSpriteA;
    private Tween _feedBackTween;
    [SerializeField]
    private TextMeshProUGUI _captureText;
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

        _feedBackTween.Kill(false);

        if (!enable)
        {
            /*_feedBackTween.Kill();
            _effectImage.gameObject.SetActive(enable);
            return;*/
            KillTheTweens();
            return;
        }

        _effectImage.sprite = _stupidGun[0];
        _feedBackTween = DOTween.Sequence().AppendInterval(0.3f).AppendCallback(() => { _isSpriteA = !_isSpriteA; _effectImage.sprite = _isSpriteA ? _stupidGun[0] : _stupidGun[1]; })
            .Append(_effectImage.transform.DOPunchScale(Vector3.one * 2f, 2f))
            .SetLoops(-1);
        _effectImage.gameObject.SetActive(enable);
    }

    void OnEffectToggled(ItemEffectType type, bool enabled)
    {
        Debug.Log("pick up type: " + type + " is enabled: " + enabled);

        if(!enabled)
        {
            /*_feedBackTween.Kill(false);
            _effectImage.gameObject.SetActive(enabled);
            _effectImage.color = Color.white;
            return;*/
            KillTheTweens();
            return;
        }

        _feedBackTween.Kill(false);

        switch (type)
        {
            case ItemEffectType.Heal:
                _effectImage.sprite = FindIcon(type);
                _feedBackTween = DOTween.Sequence().AppendInterval(0.3f).AppendCallback(() => {_isSpriteA = !_isSpriteA;_effectImage.sprite = _isSpriteA ? _alternateHeal[0] : _alternateHeal[1];}).SetLoops(-1);
                _feedBackTween.Play();
                break;
            case ItemEffectType.Invisibility:
                _effectImage.sprite = FindIcon(type);
                _feedBackTween = _effectImage.DOFade(0f, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
                _feedBackTween.Play();
                break;
            case ItemEffectType.SpeedBoost:
                _effectImage.sprite = FindIcon(type);
                _feedBackTween = DOTween.Sequence().AppendInterval(0.3f).AppendCallback(() => { _isSpriteA = !_isSpriteA; _effectImage.sprite = _isSpriteA ? _speedBoostSprites[0] : _speedBoostSprites[1]; }).SetLoops(-1);
                break;
            case ItemEffectType.CaptureDevice:
                _effectImage.sprite = FindIcon(type);
                _captureText.gameObject.SetActive(true);
                Invoke("FadeText", 5);
                break;
        }
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

    private void KillTheTweens()
    {
        _feedBackTween.Kill(false);
        _effectImage.gameObject.SetActive(false);
        _effectImage.color = Color.white;
    }

    private void FadeText()
    {
        _captureText.DOFade(0f, 1f);
    }
}
