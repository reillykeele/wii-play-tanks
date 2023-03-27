using System;
using System.Collections;
using UnityEngine;

namespace Util.Coroutine
{
    public static class CoroutineUtil
    {
        public static IEnumerator Wait(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
        }

        public static IEnumerator WaitForExecute(Action action, float delay) {
            yield return new WaitForSecondsRealtime(delay);
            action();
        }

        public static IEnumerator Sequence(params IEnumerator[] sequence)
        {
            foreach (var enumerator in sequence)
            {
                while (enumerator.MoveNext()) yield return enumerator.Current;
            }

            yield break;
        }

        public static IEnumerator CallAction(Action action)
        {
            action();
            yield break;
        }
    }
}
