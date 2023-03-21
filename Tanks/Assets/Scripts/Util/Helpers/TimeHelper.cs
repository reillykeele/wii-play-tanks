using UnityEngine;

namespace Util.Helpers
{
    public static class TimeHelper
    {
        public static string FormatTime(float start, float end) => FormatTime(end - start);

        public static string FormatTime(float t)
        {
            var minutes = ((int) t / 60).ToString("00");
            var seconds = (t % 60).ToString("00");
            var millis = Mathf.FloorToInt((t % 1) * 100).ToString("00");

            return minutes + ":" + seconds + ":" + millis;
        }
    }
}
