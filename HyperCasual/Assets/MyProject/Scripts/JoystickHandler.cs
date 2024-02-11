using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private JoystickUI joystick;

    public void OnDrag(PointerEventData eventData)
    {
        joystick.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.StartJoytick(eventData.position);
        joystick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.StopJoystick();
    }
}
