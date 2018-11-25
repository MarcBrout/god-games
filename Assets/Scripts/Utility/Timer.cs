using UnityEngine;
using System;
using System.Collections.Generic;

namespace GodsGame
{
    [Serializable]
    public class Timer : IEquatable<Timer>
    {
        #region Private Var
        [SerializeField]
        private TimeSpan m_Time;
        private float m_Start;
        #endregion

        #region Properties
        public bool IsRunning { get; protected set; }

        public int Hours
        {
            get
            {
                return GetTime.Hours;
            }
        }

        public int Minutes
        {
            get
            {
                return GetTime.Minutes;
            }
        }

        public int Seconds
        {
            get
            {
                return GetTime.Seconds;
            }
        }
        public int Milliseconds
        {
            get
            {
                return GetTime.Milliseconds;
            }
        }

        public TimeSpan GetTime
        {
            get
            {
                if (IsRunning)
                    m_Time = TimeSpan.FromSeconds(TimeSinceStart());
                return m_Time;
            }
        }
        #endregion

        public Timer(bool start = false)
        {
            m_Time = new TimeSpan();
            if (start)
                Start();
        }

        private float TimeSinceStart()
        {
            return Time.time - m_Start;
        }

        public void Start()
        {
            m_Start = Time.time;
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            m_Start = 0;
            IsRunning = false;
        }

        public string ToString(bool showMilliseconds = false)
        {
            if (showMilliseconds)
                return string.Format("{0:D2}:{1:D2}:{2:D2}", Minutes, Seconds, Milliseconds);
            return string.Format("{0:D2}:{1:D2}", Minutes, Seconds);
        }

        public static bool operator <(Timer a, Timer b)
        {
            return a.m_Time < b.m_Time;
        }

        public static bool operator >(Timer a, Timer b)
        {
            return a.m_Time > b.m_Time;
        }

        public static bool operator ==(Timer a, Timer b)
        {
            return a.m_Time == b.m_Time;
        }

        public static bool operator !=(Timer a, Timer b)
        {
            return a.m_Time != b.m_Time;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Timer);
        }

        public bool Equals(Timer other)
        {
            return other != null &&
                   m_Time.Equals(other.m_Time);
        }

        public override int GetHashCode()
        {
            return 378323972 + EqualityComparer<TimeSpan>.Default.GetHashCode(m_Time);
        }
    }
}
