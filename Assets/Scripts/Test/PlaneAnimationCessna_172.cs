using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAnimationCessna172 : MonoBehaviour
{

    public Transform propeller;
    public float propSpeed = 100;

    public float smoothTime = .5f;
    [Header("Aileron (Roll)")]
    public Transform aileronLeft;
    public Transform aileronRight;
    public float aileronMax = 20;
    [Header("Elevator (Pitch)")]
    public Transform elevatorLeft;
    public Transform elevatorRight;
    public float elevatorMax = 20;
    [Header("Rudder (Yaw)")]
    public Transform rudder;
    public float rudderMax = 20;

    [Header("Yokes")]
    public Transform yokeLeft;
    public Transform yokeRight;
    public float yokesMaxPitch = 20;
    public float yokesMaxRoll = 20;

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
        aileronLeft.localEulerAngles = new Vector3(-smoothedRoll * aileronMax, aileronLeft.localEulerAngles.y, aileronLeft.localEulerAngles.z);
        aileronRight.localEulerAngles = new Vector3(smoothedRoll * aileronMax, aileronRight.localEulerAngles.y, aileronRight.localEulerAngles.z);

        // Pitch
        float targetPitch = plane.Pitch;
        smoothedPitch = Mathf.SmoothDamp(smoothedPitch, targetPitch, ref smoothPitchV, Time.deltaTime * smoothTime);
        elevatorLeft.localEulerAngles = new Vector3(-smoothedPitch * elevatorMax, elevatorLeft.localEulerAngles.y, elevatorLeft.localEulerAngles.z);
        elevatorRight.localEulerAngles = new Vector3(-smoothedPitch * elevatorMax, elevatorRight.localEulerAngles.y, elevatorRight.localEulerAngles.z);

        // Yaw
        float targetYaw = plane.Yaw;
        smoothedYaw = Mathf.SmoothDamp(smoothedYaw, targetYaw, ref smoothYawV, Time.deltaTime * smoothTime);
        rudder.localEulerAngles = new Vector3(rudder.localEulerAngles.x, -smoothedYaw * rudderMax, rudder.localEulerAngles.z);

        // Stick 
        yokeLeft.localEulerAngles = new Vector3(smoothedPitch * yokesMaxPitch, yokeLeft.localEulerAngles.y, -smoothedRoll * yokesMaxRoll);
        // yokeLeft.position = new Vector3(yokeLeft.position.x, yokeLeft.position.y, Math.Clamp(smoothedPitch * yokesMaxPitch, 0.0f, 0.2f));
        yokeRight.localEulerAngles = new Vector3(smoothedPitch * yokesMaxPitch, yokeRight.localEulerAngles.y, -smoothedRoll * yokesMaxRoll);
        // yokeRight.position = new Vector3(yokeRight.position.x, yokeRight.position.y, Math.Clamp(smoothedPitch * yokesMaxPitch, 0.0f, 0.2f));
    }
}