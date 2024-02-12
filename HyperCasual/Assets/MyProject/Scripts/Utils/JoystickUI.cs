namespace Project.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class JoystickUI : MonoBehaviour
    {
        [SerializeField]
        private Image joystickBackground;
        [SerializeField]
        private Image stick;

        private Vector2 posInput = Vector2.zero;

        public float InputHorizontal
        {
            get
            {
                if (posInput.x != 0)
                    return posInput.x;
                return Input.GetAxis("Horizontal");
            }
        }

        public float InputVertical
        {
            get
            {
                if (posInput.y != 0)
                    return posInput.y;
                return Input.GetAxis("Vertical");
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out posInput
                ))
            {
                posInput.x = posInput.x / (joystickBackground.rectTransform.sizeDelta.x);
                posInput.y = posInput.y / (joystickBackground.rectTransform.sizeDelta.y);

                if (posInput.magnitude > 1.0f)
                {
                    posInput = posInput.normalized;
                }

                stick.rectTransform.anchoredPosition = new Vector2(
                    posInput.x * (joystickBackground.rectTransform.sizeDelta.x / 4),
                    posInput.y * (joystickBackground.rectTransform.sizeDelta.y / 4)
                    );

            }
        }

        public void StartJoytick(Vector3 position)
        {
            //Set joystick active
            gameObject.SetActive(true);
            //Set joystick position
            gameObject.transform.position = position;
        }

        public void StopJoystick()
        {
            posInput = Vector2.zero;
            stick.rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}