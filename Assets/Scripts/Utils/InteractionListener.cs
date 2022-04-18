using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action onPointerDown;
    public event Action onPointerUp;
    public event Action onDrug;
    public event Action onPointerEnter;
    public event Action onPointerExit;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrug?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }
}