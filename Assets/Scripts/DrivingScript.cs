using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour
{
    public WheelScript[] wheels;
    public float torque = 200f;
    public float maxSteerAngle = 30f;
    public float maxBrakeTorque = 500f;
    public float maxSpeed = 150f;
    public Rigidbody rb;
    public float currentSpeed;

    public GameObject brakeLights;

    public AudioSource audioSource;
    public float previousSpeed = 0f;
    public int currentGear = 1;
    const float GEAR_FACTOR = 0.05f;

    private void Start()
    {
        brakeLights.SetActive(false);
    }

    public void Drive(float accel, float brake, float steer)
    {
        accel = Mathf.Clamp(accel, -1f, 1f);
        steer = Mathf.Clamp(steer, -1f, 1f) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0f, 1f) * maxBrakeTorque;

        brakeLights.SetActive(IsCarBreaking(accel));

        currentSpeed = rb.velocity.magnitude * 3;
        float thrustTorque = 0f;
        if (currentSpeed < maxSpeed)
            thrustTorque = accel * torque;

        foreach(WheelScript wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = thrustTorque;
            if (wheel.isFrontWheel)
                wheel.wheelCollider.steerAngle = steer;
            else
                wheel.wheelCollider.brakeTorque = brake;

            Quaternion quat;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out quat);
            wheel.wheelModel.transform.position = position;
            wheel.wheelModel.transform.rotation = quat;
        }
    }

    private bool IsCarBreaking(float accel)
    {
        return IsCarMovingForward() && accel < 0f;
    }

    private bool IsCarMovingForward()
    {
        Vector3 currentMoveDirection = rb.velocity.normalized;
        Vector3 forwardDirection = rb.transform.forward.normalized;
        return Vector3.Dot(currentMoveDirection, forwardDirection) > 0 && currentSpeed >= 0.1f;
    }

    public void EngineSound()
    {
        if (audioSource.pitch >= 1f && previousSpeed < currentSpeed)
            currentGear++;
        else if (audioSource.pitch <= 0.8f && previousSpeed > currentSpeed && currentGear > 1)
            currentGear--;

        float speedPerc = Mathf.InverseLerp(0f, maxSpeed * currentGear * GEAR_FACTOR, currentSpeed);
        float pitch = Mathf.Lerp(0.3f, 1f, speedPerc);

        audioSource.pitch = pitch;

        previousSpeed = currentSpeed;
    }
}
