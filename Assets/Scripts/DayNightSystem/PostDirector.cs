using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostDirector
{
    private readonly Volume volume;
    private readonly ColorAdjustments colorAdj;
    private readonly Bloom bloom;
    private readonly Vignette vignette;
    private readonly Tonemapping tone;

    public float exposureDamp = 3.0f;
    private float evSmoothed;
    private bool init;

    public PostDirector(Volume _v)
    {
        volume = _v;
        if (!volume || !volume.profile) return;

        volume.profile.TryGet(out colorAdj);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out tone);
    }


    /// <summary>
    /// Einziger öffentlicher Entry-Point:
    /// steuert ColorFilter, PostExposure (gedämpft), Bloom (inkl. Facing-Bonus),
    /// Vignette (Intensity + Color) und Tonemapping.
    /// </summary>
    public void ApplyCinematics(float _elev01, float _facingSun01, TimeProfile _profile)
    {
        if (_profile == null || volume == null || volume.profile == null) return;

        // --- Color Filter ---
        if (colorAdj != null && _profile.PostColorFilterOverElevation != null)
        {
            colorAdj.active = true;
            colorAdj.colorFilter.Override(_profile.PostColorFilterOverElevation.Evaluate(_elev01));
        }

        // --- Post Exposure (EV) mit sanfter Dämpfung ---
        if (colorAdj != null && _profile.PostExposureEVOverElevation != null)
        {
            float evTarget = _profile.PostExposureEVOverElevation.Evaluate(_elev01);
            if (!init) { evSmoothed = evTarget; init = true; }
            evSmoothed = Mathf.Lerp(evSmoothed, evTarget, 1f - Mathf.Exp(-exposureDamp * Time.deltaTime));
            colorAdj.postExposure.Override(evSmoothed);
        }

        // --- Bloom (Elevation + optional Facing-Bonus) ---
        if (bloom != null)
        {
            float baseBloom = _profile.BloomIntensityOverElevation != null
                ? _profile.BloomIntensityOverElevation.Evaluate(_elev01)
                : 0f;

            float facingBonus = Mathf.Lerp(0f, 0.6f, Mathf.Clamp01(_facingSun01)); // Bonus bis +0.6
            bloom.active = true;
            bloom.intensity.Override(baseBloom + facingBonus);
        }

        // --- Vignette (Intensity + Color) ---
        if (vignette != null)
        {
            vignette.active = true;

            if (_profile.VignetteIntensityOverElevation != null)
            {
                vignette.intensity.Override(_profile.VignetteIntensityOverElevation.Evaluate(_elev01));
            }

            if (_profile.VignetteColorOverElevation != null)
            {
                vignette.color.Override(_profile.VignetteColorOverElevation.Evaluate(_elev01));
            }
        }

        // --- Tonemapping ---
        if (tone != null)
        {
            tone.active = true;
            tone.mode.Override(_profile.UseACES ? TonemappingMode.ACES : TonemappingMode.None);
        }
    }

}
