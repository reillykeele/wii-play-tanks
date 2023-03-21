using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Util.Helpers
{
    public static class UIHelper
    {
        public static IEnumerator FadeIn(CanvasGroup canvasGroup, Action after = null)
        {
            canvasGroup.alpha = 0f;

            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += 0.05f;
                yield return null;
            }

            canvasGroup.alpha = 1f;
            if (after != null) after();
        }

        public static IEnumerator FadeOut(CanvasGroup canvasGroup, Action after = null)
        {
            canvasGroup.alpha = 1f;

            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= 0.05f;
                yield return null;
            }

            canvasGroup.alpha = 0f;
            if (after != null) after();
        }

        public static IEnumerator FadeInAndEnable(UIController uiController, CanvasGroup canvasGroup, Action after = null)
        {
            uiController.Enable();
            canvasGroup.alpha = 0f;

            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += 0.01f;
                yield return null;
            }

            canvasGroup.alpha = 1f;

            if (after != null) after();
        }

        public static IEnumerator FadeOutAndDisable(UIController uiController, CanvasGroup canvasGroup, Action after = null)
        {
            canvasGroup.alpha = 1f;

            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= 0.01f;
                yield return null;
            }

            canvasGroup.alpha = 0f;
            uiController.Disable();

            if (after != null) after();
        }
    }
}