using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript drivingScript;

    private void Start()
    {
        drivingScript = GetComponent<DrivingScript>();
    }
    private void Update()
    {
        float accel = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        drivingScript.Drive(accel, brake, steer);
    }
}
