using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ButtonControllers
{
    [RequireComponent(typeof(Button))]
    public abstract class AButtonController : ASelectableController
    {
        protected Button _button;

        protected override void Awake()
        {
            base.Awake();

            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnClick);
        }

        public override void Select() => _button.Select();

        public override void OnSelect(BaseEventData eventData) { }

        public virtual void OnClick() { }
    }
}
