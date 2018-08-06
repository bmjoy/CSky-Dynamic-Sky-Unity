///////////////////////////////////////////////////////////
/// CSky.
/// Name: DateTime.
/// Description: Utility for day/night cycles, clocks,etc.
/// 
///////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.Events;

namespace AC.CSky
{

    [ExecuteInEditMode]
    public class CSky_DateTime : MonoBehaviour
    {


        #region |Fields|Time|Timeline|

        [SerializeField] protected bool m_ProgressTime = true;  // The time progress.

        [SerializeField] protected float m_Timeline = 7.50f;    // Timeline.
        const float k_TimelineLength = 24f;                     // Timeline length.

        #endregion

        #region |Fields|Time|Length|

        [SerializeField] protected bool m_UseDayNightLength     = true;                    // Use to set the length of the day and night.
        [SerializeField] protected Vector2 m_DayRange           = new Vector2(6.0f, 19f);  // Start and finish hour of the day.
        [SerializeField] protected float m_DayLengthInMinutes   = 15f;                     // Day in real minutes.
        [SerializeField] protected float m_NightLengthInMinutes = 7.5f;                    // Night in real minutes.

        #endregion

      

        #region |Fields|Date|
       
        [SerializeField, Range(1, 31)]   protected int m_Day   = 1;    // Day of the year.
        [SerializeField, Range(1, 12)]   protected int m_Month = 10;   // Month of the year.
        [SerializeField, Range(1, 9999)] protected int m_Year  = 2017; // Year.

        #endregion



        #region |Fields|Options|

        [SerializeField] protected bool m_SyncWithSystem = false; // Synchronize with the system date time.

        #endregion



        #region |Properties|Time|Cycle|

        /// <summary>
        /// DateTime/System DateTime.
        /// </summary>
        public System.DateTime DateTime
        {
            get { return GetDateTime(); }
        }

        /// <summary>
        /// It is day?
        /// </summary>
        public virtual bool IsDay
        {

            get
            {
                return (m_Timeline >= m_DayRange.x && m_Timeline < m_DayRange.y);
            }
        }
    
        /// <summary>
        /// Cycle duration in real minutes.
        /// </summary>
        public float DurationCycle
        {
            get
            {
                if (m_UseDayNightLength)
       
                    return IsDay ? 60 * m_DayLengthInMinutes * 2 : 60 * m_NightLengthInMinutes * 2;

    

                return m_DayLengthInMinutes * 60;
            }
        }

        #endregion

        #region |Instance|

        public static CSky_DateTime Instance { get; private set; }
        protected CSky_DateTime() { Instance = this; }

        #endregion


        #region |Events|

        // It will be triggered if a value sent by the user is invalid.
        [SerializeField] protected UnityEvent OnInvalidValue;

        // They are triggered when the values of time and date change.
        [SerializeField] protected UnityEvent OnHour;
        [SerializeField] protected UnityEvent OnMinute;
        [SerializeField] protected UnityEvent OnDay;
        [SerializeField] protected UnityEvent OnMonth;
        [SerializeField] protected UnityEvent OnYear;

        // They are used to trigger events
        protected float
            m_LastHour, m_LastMinute,
            m_LastDay, m_LastMonth,
            m_LastYear;

        #endregion

     
        #region |Methods|Initialize|

        protected virtual void Awake()
        {

            // Initialize Timeline.
            m_Timeline = m_SyncWithSystem ? (float)DateTime.TimeOfDay.TotalHours : m_Timeline;

            // Initialize Time.
            Hour   = DateTime.Hour;
            Minute = DateTime.Minute;
            Second = DateTime.Second;
            
          
            // Initialize Last date and time.
            m_LastYear   = DateTime.Year;
            m_LastMonth  = DateTime.Month;
            m_LastDay    = DateTime.Day;
            m_LastHour   = DateTime.Hour;
            m_LastMinute = DateTime.Minute;
            
        }

        #endregion

        #region |Methods|Update|

        protected virtual void Update()
        {

            if(m_SyncWithSystem)
            {

                // Set Timeline.
                if (m_ProgressTime)
                {
                    m_Timeline = (float)DateTime.TimeOfDay.TotalHours;
                }

                m_Year  = DateTime.Year;  // Set Year.
                m_Month = DateTime.Month; // Set Month.
                m_Day   = DateTime.Day;   // Set Day.

            }
            else
            {

                RepeatDateTime();  // Repeat full date cycle.

                // Add time to timeline.
                if (m_ProgressTime)
                {
                    CSky_DateTimeHelper.AddTimeToTimeline(ref m_Timeline, DurationCycle, k_TimelineLength);
                }

            }

            UpdateEvents();  // Update unity events.
        }


        private System.DateTime GetDateTime()
        {

            if (!m_SyncWithSystem)
            {
                // Create new DateTime.
                System.DateTime dateTime = new System.DateTime(0, System.DateTimeKind.Utc);

                // Add date and time in DateTime.
                dateTime = dateTime.AddYears(m_Year - 1).AddMonths(m_Month - 1).AddDays(m_Day - 1).AddHours(m_Timeline);

                // Set date.
                m_Year  = dateTime.Year;
                m_Month = dateTime.Month;
                m_Day   = dateTime.Day;

                // Set timeline.
                m_Timeline = CSky_DateTimeHelper.TimeToFloat(dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);

                return dateTime;
            }

            return System.DateTime.Now; // System date time.
        }

        #endregion

        #region |Methods|Cycle|
      
        /// <summary>
        /// Repeat datetime cycle.
        /// </summary>
        private void RepeatDateTime()
        {
            if (m_Year == 9999 && m_Month == 12 && m_Day == 31 && m_Timeline >= 23.999f)
            {

                m_Year  = 1;
                m_Month = 1;
                m_Day   = 1;

                m_Timeline = 0.0f;
            }

            if (m_Year == 1 && m_Month == 1 && m_Day == 1 && m_Timeline < 0.0f)
            {
               
                m_Year  = 9999;
                m_Month = 12;
                m_Day   = 31;

                m_Timeline = 23.999f;
            }
        }

        #endregion

        #region |Methods|Set|

        /// <summary>
        /// Set time to timeline.
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        private void SetTime(int hour, int minute, int second)
        {
           m_Timeline = CSky_DateTimeHelper.TimeToFloat(hour, minute, second);
        }

        #endregion

        #region |Methods|Events|
        void UpdateEvents()
        {

            if (m_LastHour != DateTime.Hour)
            {
                
                OnHour.Invoke();
                //Debug.Log("OnHour");
                m_LastHour = DateTime.Hour;
            }

            if (m_LastMinute != DateTime.Minute)
            {
                OnMinute.Invoke();
                //Debug.Log("OnMinute");
                m_LastMinute = DateTime.Minute;
            }

            if (m_LastDay != DateTime.Day)
            {
                OnDay.Invoke();
                //Debug.Log("OnDay");
                m_LastDay = DateTime.Day;
            }

            if (m_LastMonth != DateTime.Month)
            {
                OnMonth.Invoke();
                //Debug.Log("OnMonth");
                m_LastMonth = DateTime.Month;
            }

            if (m_LastYear != DateTime.Year)
            {
                OnYear.Invoke();
                //Debug.Log("OnYear");
                m_LastYear = DateTime.Year;
            }
        }

        #endregion

    
        #region |Properties|Setup|

        /// <summary>
        /// The time and date progress.
        /// </summary>
        public bool Progress
        {
            get { return this.m_ProgressTime; }
            set { this.m_ProgressTime = value; }
        }

        /// <summary>
        /// Synchronize with the system time.
        /// </summary>
        public bool SyncWithSystem
        {
            get { return this.m_SyncWithSystem; }
            set { this.m_SyncWithSystem = value; }
        }

        #endregion

        #region |Properties|Time|

        /// <summary>
        /// Hour: Range[0-24]
        /// </summary>
        public int Hour
        {
            get
            {
                return DateTime.Hour;
            }
            set
            {

                if (value >= 0 && value < 25)
                    SetTime(value, DateTime.Minute, DateTime.Second);
                else
                    OnInvalidValue.Invoke();
            }
        }

        /// <summary>
        /// Minute: Range[0-60]
        /// </summary>
        public int Minute
        {
            get
            {
                return DateTime.Minute;
            }
            set
            {

                if (value >= 0 && value < 61)
                    SetTime(DateTime.Hour, value, DateTime.Second);
                else
                    OnInvalidValue.Invoke();

            }
        }

        /// <summary>
        /// Second: Range[0-60]
        /// </summary>
        public int Second
        {
            get
            {
                return DateTime.Second;
            }
            set
            {
                if (value >= 0 && value < 61)
                    SetTime(DateTime.Hour, DateTime.Minute, value);
                else
                    OnInvalidValue.Invoke();

            }
        }

        /// <summary>
        /// Use to set the length of the day and night.
        /// </summary>
        public bool UseDayNightLength
        {
            get { return this.m_UseDayNightLength; }
            set { this.m_UseDayNightLength = value; }
        }

        /// <summary>
        /// Range between day and night.
        /// </summary>
        public Vector2 DayRange
        {
            get { return this.m_DayRange; }
            set { this.m_DayRange = value; }
        }

        /// <summary>
        /// Day in real minutes.
        /// </summary>
        public float DayLengthInMinutes
        {
            get { return this.m_DayLengthInMinutes; }
            set { this.m_DayLengthInMinutes = value; }
        }

        /// <summary>
        /// Night in real minutes.
        /// </summary>
        public float NightLengthMinutes
        {
            get { return this.m_NightLengthInMinutes; }
            set { this.m_NightLengthInMinutes = value; }
        }

        /// <summary>
        /// Timeline Range: [0-24].
        /// </summary>
        public float Timeline
        {
            get
            {
                return this.m_Timeline;
            }
            set
            {
                if (value > 0.0f && value < 24.000001f)
                    m_Timeline = value;
                else
                    OnInvalidValue.Invoke();
            }
        }

        #endregion

        #region |Properties|DateTime|

    
        /// <summary>
        /// Day Range: [1-31].
        /// </summary>
        public int Day
        {
            get
            {
                return this.m_Day;
            }
            set
            {
                if (value > 0 && value < 32)
                    m_Day = value;
                else
                    OnInvalidValue.Invoke();
            }
        }


        /// <summary>
        /// Month Range: [1-12].
        /// </summary>
        public int Month
        {
            get
            {
                return this.m_Month;
            }
            set
            {
                if (value > 0 && value < 13)
                    m_Month = value;
                else
                    OnInvalidValue.Invoke();
            }
        }

        /// <summary>
        /// Year Range: [1-9999].
        /// </summary>
        public int Year
        {
            get
            {
                return this.m_Year;
            }
            set
            {
                if (value > 0 && value < 10000)
                    m_Year = value;
                else
                    OnInvalidValue.Invoke();
            }
        }
        #endregion

       

#if UNITY_EDITOR
        [SerializeField, Range(0, 24)] internal int m_Hour = 7;
        [SerializeField, Range(0, 60)] internal int m_Minute = 30;
        [SerializeField, Range(0, 60)] internal int m_Second = 0;
#endif

    }
}
