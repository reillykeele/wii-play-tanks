using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.ButtonControllers
{
    public class AdjustValueSelectableController : ASelectableController
    {
        [SerializeField]
        protected float _incrementValue = 5f;

        protected float _value;

        public UnityEvent<string> OnValueChangedEvent;

        protected virtual void Start()
        {
            OnValueChangedEvent.Invoke("" + _value);
        }

        void Update()
        {
            if (EventSystem.current?.currentSelectedGameObject == gameObject)
            {
                bool updated = false;

                if (Keyboard.current?.leftArrowKey.wasPressedThisFrame == true ||
                    Gamepad.current?.dpad.left.wasPressedThisFrame == true)
                {
                    Decrease();
                    updated = true;
                }
                else if (Keyboard.current?.rightArrowKey.wasPressedThisFrame == true ||
                         Gamepad.current?.dpad.right.wasPressedThisFrame == true)
                {
                    Increase();
                    updated = true;
                }

                if (updated)
                    OnValueChangedEvent.Invoke("" + _value);
            }
        }

        protected virtual void Increase() { }
        protected virtual void Decrease() { }
    }
}