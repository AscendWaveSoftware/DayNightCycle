using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private TimeProfile timeProfile;

    [Header("Scene Refs")]
    [SerializeField] private Light globalLight;


    [Header("Clock")]
    [SerializeField] private int minutes;
    public int Minutes
    { get { return minutes; } set { minutes = value; OnMinutesChange(value); } }

    [SerializeField] private int hours = 5;
    public int Hours
    { get { return hours; } set { hours = value; OnHoursChange(value); } }

    [SerializeField] private int days;
    public int Days
    { get { return days; } set { days = value; } }

    [SerializeField] private float tempSecond;

    public void Update()
    {
        if (timeProfile == null) return;

        tempSecond += Time.deltaTime;

        if(tempSecond >= timeProfile.RealSecondsPerGameMinute)
        {
            Minutes += 1;
            tempSecond = 0f;
        }
    }

    private void OnMinutesChange(int _value)
    {
        if(globalLight != null)
        {
            globalLight.transform.Rotate(
                Vector3.up,
                timeProfile != null ? timeProfile.LightRotationDegreesPerGameMinute : 0f,
                Space.World
            );
        }

        if(_value >= 60)
        {
            Hours++;
            minutes = 0;
        }
        if (Hours >= 24)
        {
            Hours = 0;
            Days++;
        }
    }

    private void OnHoursChange(int _value)
    {
        if (timeProfile == null) return;

        if (_value == timeProfile.SunriseHour)
        {
            StartCoroutine(LerpSkybox(timeProfile.SkyboxNight, timeProfile.SkyboxSunrise, timeProfile.TransitionSeconds));
            StartCoroutine(LerpLight(timeProfile.GradientNightToSunrise, timeProfile.TransitionSeconds));
        }
        else if (_value == timeProfile.DayHour)
        {
            StartCoroutine(LerpSkybox(timeProfile.SkyboxSunrise, timeProfile.SkyboxDay, timeProfile.TransitionSeconds));
            StartCoroutine(LerpLight(timeProfile.GradientSunriseToDay, timeProfile.TransitionSeconds));
        }
        else if (_value == timeProfile.SunsetHour)
        {
            StartCoroutine(LerpSkybox(timeProfile.SkyboxDay, timeProfile.SkyboxSunset, timeProfile.TransitionSeconds));
            StartCoroutine(LerpLight(timeProfile.GradientDayToSunset, timeProfile.TransitionSeconds));
        }
        else if (_value == timeProfile.NightHour)
        {
            StartCoroutine(LerpSkybox(timeProfile.SkyboxSunset, timeProfile.SkyboxNight, timeProfile.TransitionSeconds));
            StartCoroutine(LerpLight(timeProfile.GradientSunsetToNight, timeProfile.TransitionSeconds));
        }
    }

    private IEnumerator LerpSkybox(Texture2D _a, Texture2D _b, float _time)
    {
        if (_a == null || _b == null) yield break;

        RenderSettings.skybox.SetTexture("_Texture1", _a);
        RenderSettings.skybox.SetTexture("_Texture2", _b);
        RenderSettings.skybox.SetFloat("_Blend", 0);

        for (float t = 0; t < _time; t += Time.deltaTime)
        {
            RenderSettings.skybox.SetFloat("_Blend", t / _time);
            yield return null;
        }
        RenderSettings.skybox.SetTexture("_Texture1", _b);
        RenderSettings.skybox.SetFloat("_Blend", 1f);
    }

    private IEnumerator LerpLight(Gradient _lightGradient, float _time)
    {
        if (globalLight == null || _lightGradient == null) yield break;

        for (float t = 0f; t < _time; t += Time.deltaTime)
        {
            var col = _lightGradient.Evaluate(t / _time);
            globalLight.color = col;
            RenderSettings.fogColor = col;
            yield return null;
        }
    }
}