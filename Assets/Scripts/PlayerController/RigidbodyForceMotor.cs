using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyForceMotor : MonoBehaviour
{
    [SerializeField] private MovementSettings settings;
    [SerializeField] private Transform orientation;

    private Rigidbody rb;
    private Vector3 desiredDirWS;
    private bool isGrounded;

    public void SetDesiredMove(Vector3 worldDir) => desiredDirWS = worldDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position + settings.groundCheckOffset,
                                         settings.groundCheckRadius, settings.groundMask, QueryTriggerInteraction.Ignore);

        var velocity = rb.velocity;

        // Geschwindigkeit relativ zum Boden dämpfen (nur horizontal)
        var horizVel = new Vector3(velocity.x, 0f, velocity.z);
        float damping = isGrounded ? settings.groundDamping : settings.airDamping;
        horizVel = Vector3.Lerp(horizVel, Vector3.zero, damping);
        velocity = new Vector3(horizVel.x, velocity.y, horizVel.z);
        rb.velocity = velocity;

        Vector3 wish = desiredDirWS;
        if (wish.sqrMagnitude > 1f)
            wish.Normalize();

        var currentHoriz = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if(currentHoriz.magnitude > settings.maxSpeed)
        {
            var clamped = currentHoriz.normalized * settings.maxSpeed;
            rb.velocity = new Vector3(clamped.x, rb.velocity.y, clamped.z);
        }

        // AddForce als Acceleration
        float a = isGrounded ? settings.accel : settings.airAccel;
        rb.AddForce(wish * a, ForceMode.Acceleration);

        if (orientation)
        {
            var camFwd = orientation.forward;
            camFwd.y = 0f;

            if (camFwd.sqrMagnitude > 0.01f)
            {
                var targetRot = Quaternion.LookRotation(camFwd.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRot,
                    settings.orientationLerp * Time.fixedDeltaTime
                );
            }
        }
    }

    /// <summary>
    /// Hilfsfunktion, um Input von Kamera/Orientation in World-Space zu rechnen.
    /// </summary>
    /// <returns></returns>
    public Vector3 ToWorldFromOrientation(Vector2 _input)
    {
        if (!orientation)
            return new Vector3(_input.x, 0f, _input.y);
        var fwd = orientation.forward; fwd.y = 0f; fwd.Normalize();
        var right = orientation.right; right.y = 0f; right.Normalize();
        return (right * _input.x + fwd * _input.y);
    }
}
