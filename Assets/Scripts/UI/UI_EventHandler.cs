using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnDragAction = null;
    public Action<PointerEventData> OnPointerClickAction = null;

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragAction != null)
            OnDragAction.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerClickAction != null)
            OnPointerClickAction.Invoke(eventData);
    }
}
