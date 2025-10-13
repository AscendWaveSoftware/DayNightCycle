using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
[CreateAssetMenu(fileName = "Time Profile", menuName = "Enviroment/Time Profile", order = 0)]
public class TimeProfile : ScriptableObject
{
    [Header("Skybox Textures")]
    public Texture2D SkyboxNight;
    public Texture2D SkyboxSunrise;
    public Texture2D SkyboxDay;
    public Texture2D SkyboxSunset;

    [Header("Light Gradients")]
    [FormerlySerializedAs("graddientNightToSunrise")]
    public Gradient GradientNightToSunrise;
    [FormerlySerializedAs("graddientSunriseToDay")]
    public Gradient GradientSunriseToDay;
    [FormerlySerializedAs("graddientDayToSunset")]
    public Gradient GradientDayToSunset;
    [FormerlySerializedAs("graddientSunsetToNight")]
    public Gradient GradientSunsetToNight;

    [Header("Time & Rotation")]
    [Tooltip("Echte Sekunden pro In-Game-Minute.")]
    [Min(0.01f)] public float RealSecondsPerGameMinute = 1f;

    [Header("Phase Switch (Stunden)")]
    public int SunriseHour = 6;
    public int DayHour = 8;
    public int SunsetHour = 18;
    public int NightHour = 22;

    [Header("Blend Duration")]
    [Tooltip("Sekunden für Skybox- und Light-Lerp.")]
    [Min(0.01f)] public float TransitionSeconds = 10f;

    [Header("Post-Processing (Cinematic)")]
    public Gradient PostColorFilterOverElevation;
    [FormerlySerializedAs("VignetteColorOverEvaluation")]
    public Gradient VignetteColorOverElevation;
    public AnimationCurve PostExposureEVOverElevation;
    public AnimationCurve BloomIntensityOverElevation;
    public AnimationCurve VignetteIntensityOverElevation;
    public bool UseACES = true;

    private void OnValidate()
    {
        SunriseHour = Mathf.Clamp(SunriseHour, 0, 23);
        DayHour = Mathf.Clamp(DayHour, 0, 23);
        SunsetHour = Mathf.Clamp(SunsetHour, 0, 23);
        NightHour = Mathf.Clamp(NightHour, 0, 23);
    }
}
