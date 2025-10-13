using UnityEngine;

public static class SunRotation
{
    public static Quaternion FromTime01(float _t, Vector3 _axis)
    {
        float angle = _t * 360f;
        return Quaternion.AngleAxis(angle, _axis);
    }
}
