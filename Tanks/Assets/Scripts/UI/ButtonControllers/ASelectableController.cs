using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ButtonControllers
{
    [RequireComponent(typeof(Selectable))]
    public abstract class ASelectableController : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        protected CanvasController _canvasController;
        protected CanvasAudioController _canvasAudioController;
        protected Selectable _selectable;

        protected virtual void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _canvasAudioController = GetComponentInParent<CanvasAudioController>();
            _selectable = GetComponent<Selectable>();
        }

        public virtual void Select() => _selectable.Select();

        public virtual void OnSelect(BaseEventData eventData) { }
        public virtual void OnPointerEnter(PointerEventData eventData) => Select();
    }
}