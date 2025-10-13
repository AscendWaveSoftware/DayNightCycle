using UnityEngine;

public sealed class ClockService : ITimeSource
{
    public int Hours { get; private set; } = 5;
    public int Minutes { get; private set; }
    public int Days { get; private set; }
    public float TimeOfDay01 => (Hours * 60f + Minutes) / 1440f;

    // Events
    public event System.Action MinuteElapsed;
    public event System.Action<int> HourChanged;
    public event System.Action DayChanged;

    private float acc;

    public void Tick(float _dt, float _realSecondsPerGameMinute)
    {
        acc += _dt;
        while(acc >= _realSecondsPerGameMinute)
        {
            acc -= _realSecondsPerGameMinute;
            AdvanceOneMinute();
        }
    }

    public void SetTime(int _hours, int _minutes)
    {
        Hours = Mathf.Clamp(_hours, 0, 23);
        Minutes = Mathf.Clamp(_minutes, 0, 59);
    }

    private void AdvanceOneMinute()
    {
        Minutes++;
        MinuteElapsed?.Invoke();
        if(Minutes >= 60)
        {
            Minutes = 0;
            Hours++;
            HourChanged?.Invoke(Hours);
            if(Hours >= 24)
            {
                Hours = 0;
                Days++;
                DayChanged?.Invoke();
            }
        }
    }
}
