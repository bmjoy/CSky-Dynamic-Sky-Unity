///////////////////////////////////////////////////
/// CSky.
/// DateTime Helper.
/// Description: Useful methods for time/timeline.
/// 
///////////////////////////////////////////////////

using UnityEngine;

namespace AC.CSky
{ 

    public sealed class CSky_DateTimeHelper : Behaviour
    {

        #region |Timeline|

        /// <summary>
        /// Add time to timeline.
        /// </summary>
        /// <param name="timeline"></param>
        /// <param name="divider"></param>
        /// <param name="lenght"></param>
        public static void AddTimeToTimeline(ref float timeline, float divider, float lenght)
        {
            timeline += (divider != 0 && Application.isPlaying) ? (Time.deltaTime / divider) * lenght : 0.0f;

        }

        /*
        public static float TimelineDelta(float divider, float length)
        {

            return (divider != 0) ? (Time.deltaTime / divider) * length : 0.0f;
        }*/

        /// <summary>
        /// Retrun time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour)
        {
            return (float)hour;
        }

        /// <summary>
        /// Return time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute)
        {
            return (float)hour + ((float)minute / 60f);
        }

        /// <summary>
        /// Return time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute, int second)
        {
            return (float)hour + ((float)minute / 60f) + ((float)second / 3600f);
        }

        /// <summary>
        /// Retrun time in float value.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <param name="millisecond"></param>
        /// <returns></returns>
        public static float TimeToFloat(int hour, int minute, int second, int millisecond)
        {
            return (float)hour + (float)minute / 60f + (float)second / 3600f + (float)millisecond / 3600000f;
        }

        /// <summary>
        /// returns the exact hour contained in the timeline
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        public static int GetTimelineHour(float timeline)
        {
            return (int)Mathf.Floor(timeline);
        }

        /// <summary>
        /// returns the exact minute contained in the timeline
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        public static int GetTimelineMinute(float timeline)
        {
            return (int)Mathf.Floor((timeline - (int)Mathf.Floor(timeline)) * 60);
        }

        #endregion

        #region |String|

        /// <summary>
        /// Hour and minute to string.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static string TimeToString(int hour, int minute)
        {
            string h = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            string m = minute < 10 ? "0" + minute.ToString() : minute.ToString();

            return h + ":" + m;
        }

        /// <summary>
        /// Hour, minute and second to string.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static string TimeToString(int hour, int minute, int second)
        {
            string h = hour < 10 ? "0" + hour.ToString() : hour.ToString();
            string m = minute < 10 ? "0" + minute.ToString() : minute.ToString();
            string s = second < 10 ? "0" + second.ToString() : second.ToString();

            return h + ":" + m + ":" + s;
        }

        #endregion
    }
}
