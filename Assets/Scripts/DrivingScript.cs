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

    public void Drive(float accel, float brake, float steer)
    {
        accel = Mathf.Clamp(accel, -1f, 1f);
        steer = Mathf.Clamp(steer, -1f, 1f) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0f, 1f) * maxBrakeTorque;

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
}
