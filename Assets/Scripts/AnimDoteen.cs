using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AnimDoteen : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _initialScale;

    private void OnEnable()
    {
        DOTween.KillAll();
    }

    private void Awake()
    {
        _initialScale = transform.localScale;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_initialScale * 2, 1f).SetEase(Ease.InElastic);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_initialScale, 1f).SetEase(Ease.OutElastic);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
