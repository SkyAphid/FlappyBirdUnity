using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickAnimator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{

    [SerializeField] private float m_AnimationSpeed = 0.15f;

    private Vector3 m_OriginalScale;

    public void Awake()
    {
        m_OriginalScale = this.gameObject.transform.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioController.PlaySFX(AudioController.Sound.Select);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameObject.transform.DOScale(m_OriginalScale * 0.75f, m_AnimationSpeed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.gameObject.transform.DOScale(m_OriginalScale, m_AnimationSpeed);
    }
}
