using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAnimationPiperPA18 : MonoBehaviour
{

    public Transform propeller;
    public float propSpeed = 100;

    public float smoothTime = .5f;
    [Header("Aileron (Roll)")]
    public Transform aileronLeft;
    public Transform aileronRight;
    public float aileronMax = 20;
    [Header("Elevator (Pitch)")]
    public Transform elevator;
    public float elevatorMax = 20;
    [Header("Rudder (Yaw)")]
    public Transform rudder;
    public float rudderMax = 20;

    [Header("Stick")]
    public Transform stick;
    public float stickMaxPitch = 20;
    public float stickMaxRoll = 20;

    // Smoothing vars
    float smoothedRoll;
    float smoothRollV;
    float smoothedPitch;
    float smoothPitchV;
    float smoothedYaw;
    float smoothYawV;

    [Header("Plane")] public MFlight.Demo.Plane plane;

    void Update()
    {
        // https://en.wikipedia.org/wiki/Aircraft_principal_axes
        propeller.Rotate(Vector3.forward * propSpeed * Time.deltaTime);

        // Roll
        float targetRoll = plane.Roll;
        smoothedRoll = Mathf.SmoothDamp(smoothedRoll, targetRoll, ref smoothRollV, Time.deltaTime * smoothTime);
        if (targetRoll != null && smoothedRoll != null)
        {
            aileronLeft.localEulerAngles = new Vector3(-smoothedRoll * aileronMax, aileronLeft.localEulerAngles.y, aileronLeft.localEulerAngles.z);
            aileronRight.localEulerAngles = new Vector3(smoothedRoll * aileronMax, aileronRight.localEulerAngles.y, aileronRight.localEulerAngles.z);
        }
        
        // Pitch
        float targetPitch = plane.Pitch;
        smoothedPitch = Mathf.SmoothDamp(smoothedPitch, targetPitch, ref smoothPitchV, Time.deltaTime * smoothTime);
        if (targetPitch != null && smoothedPitch != null)
        {
            elevator.localEulerAngles = new Vector3(-smoothedPitch * elevatorMax, elevator.localEulerAngles.y, elevator.localEulerAngles.z);
        }
        
        // Yaw
        float targetYaw = plane.Yaw;
        smoothedYaw = Mathf.SmoothDamp(smoothedYaw, targetYaw, ref smoothYawV, Time.deltaTime * smoothTime);
        if (targetYaw != null && smoothedYaw != null)
        {
            rudder.localEulerAngles = new Vector3(rudder.localEulerAngles.x, -smoothedYaw * rudderMax, rudder.localEulerAngles.z);
        }

        // Stick 
        if (smoothedPitch != null && smoothedRoll != null)
        {
            stick.localEulerAngles = new Vector3(smoothedPitch * stickMaxPitch, stick.localEulerAngles.y,
                -smoothedRoll * stickMaxRoll);
        }
    }
}