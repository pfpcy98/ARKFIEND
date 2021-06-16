using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool is_Pressed { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        is_Pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        is_Pressed = false;
    }
}
