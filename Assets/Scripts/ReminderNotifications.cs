using System.Collections;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using Unity.Notifications.Android;
#endif

public class ReminderNotifications : MonoBehaviour
{
    public static ReminderNotifications Instance { get; private set; }

    [SerializeField] float m_reminderHours = 12f; // Remind the player ever 12 hours if they haven't opened the app
    [SerializeField] float m_testDelaySeconds = 10f; // 0 = no test delay; >0 = test delay in seconds (for testing)
    [SerializeField] string m_title = "Merge & Defend";
    [SerializeField] string[] m_messages =
    {
        "Your game is waiting. Come back and play!",
        "Ready for another round? Jump back in!",
        "Trying to beat your high-score? Your game is ready!"
    };

    const string GROUP_ID = "Main";
    const string CHANNEL_ID = "reminders";

    bool m_hasPermission;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    IEnumerator Start()
    {
        RegisterChannel();

        // Ask for permission (Android 13+ shows a prompt)
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
        {
            yield return null;
        }

        m_hasPermission = request.Status == PermissionStatus.Allowed;
        Debug.Log("[Notifications] Permission granted: " + m_hasPermission);

        // Log if the app was opened by tapping a reminder
        var lastIntent = AndroidNotificationCenter.GetLastNotificationIntent();
        if (lastIntent != null)
        {
            Debug.Log("[Notifications] Opened from notification. Data: " + lastIntent.Notification.IntentData);
        }

        // The player is in the app, so nothing should be pending
        AndroidNotificationCenter.CancelAllScheduledNotifications();
    }

    void RegisterChannel()
    {
        var group = new AndroidNotificationChannelGroup()
        {
            Id = GROUP_ID,
            Name = "Main notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = "Play Reminders",
            Importance = Importance.Default,
            Description = "Reminders to come back and play",
            Group = GROUP_ID,
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    void ScheduleReminder()
    {
        if (!m_hasPermission) return;

        // Cancel the old one first so reminders never stack up
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        System.DateTime fireTime = m_testDelaySeconds > 0f
            ? System.DateTime.Now.AddSeconds(m_testDelaySeconds)
            : System.DateTime.Now.AddHours(m_reminderHours);

        var notification = new AndroidNotification
        {
            Title = m_title,
            Text = m_messages[Random.Range(0, m_messages.Length)],
            FireTime = fireTime,
            IntentData = "daily_reminder",
        };

        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
        Debug.Log("[Notifications] Reminder scheduled for " + fireTime);
    }

    void OnApplicationPause(bool paused)
    {
        if (paused) ScheduleReminder(); // player left: start the countdown
        else if (m_hasPermission) AndroidNotificationCenter.CancelAllScheduledNotifications(); // player came back
    }

    void OnApplicationQuit() => ScheduleReminder();
#else
    void Start() { }   // notifications only run in an Android build
#endif
}