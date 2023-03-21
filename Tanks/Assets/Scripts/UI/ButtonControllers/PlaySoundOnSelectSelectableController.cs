using UnityEngine.EventSystems;

namespace UI.ButtonControllers
{
    public class PlaySoundOnSelectSelectableController : ASelectableController
    {
        public override void OnSelect(BaseEventData eventData) => _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Tick);
    }
}
