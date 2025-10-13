using UnityEngine;

public class ClockInspectorView : MonoBehaviour
{
    [SerializeField] EnviromentDirector director;

    [Header("Current Time (read-only)")]
    public int hours;
    public int minutes;
    public int seconds;
    public int days;
    [TextArea(1, 1)]
    public string formatted;

    private void Update()
    {
        if (director == null) return;
        var time = director.TimeSource;

        hours = time.Hours;
        minutes = time.Minutes;
        days = time.Days;
        formatted = $"{hours:00}:{minutes:00} (Day {days})";
    }
}
