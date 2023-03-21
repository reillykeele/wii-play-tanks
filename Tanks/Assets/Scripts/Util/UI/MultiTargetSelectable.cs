using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
    public class MultiTargetSelectable : Selectable
    {
        private Graphic[] graphics;

        private MultiTargetGraphic targetGraphics;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!GetGraphics())
                return;

            var targetColor = state switch
                {
                    SelectionState.Disabled => colors.disabledColor,
                    SelectionState.Highlighted => colors.highlightedColor,
                    SelectionState.Normal => colors.normalColor,
                    SelectionState.Pressed => colors.pressedColor,
                    SelectionState.Selected => colors.selectedColor,
                    _ => Color.white
                };

            foreach (var graphic in graphics)
                graphic.CrossFadeColor(targetColor, instant ? 0 : colors.fadeDuration, true, true);
        }

        private bool GetGraphics()
        {
            if (!targetGraphics) 
                targetGraphics = GetComponent<MultiTargetGraphic>();
            
            graphics = targetGraphics?.GetTargetGraphics;

            var success = graphics != null && graphics.Length > 0;
            return success;
        }
    }
}
